# EShop

Модульное e-commerce приложение, разработанное на **C# / ASP.NET Core** с использованием **Modular Monolith Architecture**, **Vertical Slice Architecture**, **CQRS** и **Wolverine**.

Проект построен вокруг независимых бизнес-модулей, изолированных баз данных, асинхронного обмена сообщениями и надёжных распределённых бизнес-процессов.

---

## Архитектура

Приложение построено в стиле **Modular Monolith**.

Все основные бизнес-модули работают внутри одного ASP.NET Core процесса, однако каждый модуль владеет собственной базой данных и своей бизнес-логикой.

Взаимодействие между модулями происходит через сообщения и события, а не через прямой доступ к базам данных друг друга.

```text
                         ┌─────────────────────┐
                         │      EShop API      │
                         │                     │
                         │   Modular Monolith  │
                         └──────────┬──────────┘
                                    │
          ┌─────────────────────────┼─────────────────────────┐
          │                         │                         │
          ▼                         ▼                         ▼
     ┌──────────┐             ┌──────────┐              ┌──────────┐
     │ Identity │             │ Catalog  │              │   Cart   │
     └────┬─────┘             └────┬─────┘              └────┬─────┘
          │                        │                         │
          ▼                        ▼                         ▼
     Identity DB              Catalog DB                 Cart DB


          ┌─────────────────────────┼─────────────────────────┐
          │                         │                         │
          ▼                         ▼                         ▼
     ┌──────────┐             ┌──────────┐              ┌──────────┐
     │Inventory │             │  Orders  │              │ Payments │
     └────┬─────┘             └────┬─────┘              └────┬─────┘
          │                        │                         │
          ▼                        ▼                         ▼
    Inventory DB              Orders DB                 Payments DB


                              RabbitMQ
                                 │
                                 ▼
                            EmailWorker
```

Основная цель архитектуры — сохранить чёткие границы между модулями, не усложняя проект полноценной микросервисной инфраструктурой.

---

# Модули

## Identity

Отвечает за:

* регистрацию пользователей;
* подтверждение email;
* аутентификацию;
* пользовательские процессы;
* временное состояние регистрации.

Identity владеет собственной PostgreSQL базой данных.

---

## Catalog

Отвечает за:

* товары;
* информацию о товарах;
* цены;
* активность товаров;
* изображения товаров;
* загрузку изображений;
* генерацию thumbnails;
* запросы, связанные с каталогом.

Catalog владеет собственной PostgreSQL базой данных.

Catalog не имеет прямого доступа к базам других модулей.

### Работа с изображениями

На текущем этапе обработка изображений выполняется непосредственно внутри Catalog.

Pipeline выглядит следующим образом:

```text
Client
   │
   ▼
Create Product
   │
   ▼
Catalog
   │
   ├── validate image
   │
   ├── detect actual content type
   │
   ├── validate file size
   │
   ├── upload original → S3 / MinIO
   │
   ├── resize image
   │
   └── upload thumbnail → S3 / MinIO
```

Для проверки реального типа файла используется `MimeDetective`.

Для обработки изображений используется `ImageSharp`.

На текущем этапе поддерживаются:

```text
JPEG
PNG
```

Максимальный размер изображения:

```text
6 MB
```

Thumbnail генерируется размером:

```text
200 × 200
```

и сохраняется в формате JPEG.

В PostgreSQL сохраняются относительные object keys, а не полные URL.

Например:

```text
products/{productId}/{imageId}.jpg
products/{productId}/{thumbnailId}_thumb.jpg
```

Публичный URL формируется через `MediaUrlService`.

Таким образом, Catalog не хранит бинарные данные изображений в PostgreSQL.

В дальнейшем при увеличении нагрузки или усложнении image processing обработка может быть вынесена в отдельный worker без изменения ответственности Catalog за данные изображений.

---

## Cart

Отвечает за:

* корзины пользователей;
* товары в корзине;
* добавление и удаление товаров;
* очистку корзины;
* получение корзины для checkout.

Cart владеет собственной PostgreSQL базой данных.

---

## Inventory

Отвечает за:

* складские остатки;
* доступное количество товара;
* зарезервированное количество;
* резервирование товара;
* освобождение резерва;
* подтверждение резерва.

Inventory владеет собственной PostgreSQL базой данных.

Inventory является источником истины для складских остатков.

---

## Orders

Отвечает за:

* создание заказов;
* позиции заказа;
* состояние заказа;
* checkout;
* жизненный цикл заказа.

Orders владеет собственной PostgreSQL базой данных.

В заказ сохраняется snapshot информации о товаре на момент покупки:

* `ProductId`;
* название товара;
* цена;
* количество.

Таким образом, Orders не зависит от текущего состояния Catalog.

---

## Payments

Отвечает за:

* создание платежей;
* состояние платежей;
* обработку платежей;
* интеграцию с платёжным провайдером.

Payments владеет собственной PostgreSQL базой данных.

Платёжный провайдер скрыт за интерфейсом:

```csharp
IPaymentGateway
```

Это позволяет использовать fake-провайдер во время разработки и заменить его на реальный без изменения бизнес-логики обработки заказа.

---

# Messaging

Для messaging используется **Wolverine**.

RabbitMQ используется в качестве транспорта для сообщений, которым необходимо пересекать границу процессов.

В проекте используются три основных типа сообщений:

* команды;
* события;
* запросы.

---

## Commands

Команда выражает намерение выполнить действие.

Например:

```text
ProcessPaymentCommand
ReleaseInventoryCommand
CommitInventoryCommand
ClearCartCommand
```

Команда обычно отправляется конкретному обработчику:

```csharp
await bus.SendAsync(
    new ProcessPaymentCommand(...));
```

---

## Events

Событие описывает факт, который уже произошёл.

Например:

```text
OrderCreated
InventoryReservedEvent
InventoryReservationFailedEvent
PaymentSucceededEvent
PaymentFailedEvent
InventoryCommittedEvent
```

События публикуются:

```csharp
await bus.PublishAsync(
    new PaymentSucceededEvent(...));
```

---

## Queries

Запрос используется, когда обработчику необходимо получить данные.

Например:

```csharp
var cartResult =
    await bus.InvokeAsync<Result<CartForCheckoutResponse>>(
        new GetCartForCheckoutQuery(userId),
        ct);
```

---

# Messaging Architecture

Wolverine выступает в качестве основной messaging abstraction, а RabbitMQ используется там, где требуется взаимодействие между процессами.

```text
Application
    │
    ▼
 Wolverine
    │
    ├── Local messaging
    │
    └── RabbitMQ
             │
             └── EmailWorker
```

Важно: `PublishAsync()` сам по себе не означает, что сообщение обязательно будет отправлено в RabbitMQ.

Конкретный транспорт определяется конфигурацией Wolverine.

Это позволяет внутренним модулям взаимодействовать локально, а отдельным Worker-процессам — через RabbitMQ.

---

# Архитектура баз данных

Каждый бизнес-модуль владеет собственной PostgreSQL базой.

```text
PostgreSQL
│
├── IdentityDatabase
├── CatalogDatabase
├── CartDatabase
├── InventoryDatabase
├── OrderDatabase
└── PaymentDatabase
```

Модули не используют общие таблицы.

Например:

```text
Orders
   │
   └── не обращается напрямую к Inventory DB

Inventory
   │
   └── не обращается напрямую к Orders DB
```

Вместо этого используются сообщения и события.

Такой подход сохраняет чёткие границы владения данными и в будущем позволяет при необходимости вынести отдельный модуль в самостоятельный сервис.

---

# CQRS

Проект использует облегчённый подход к CQRS.

Команды изменяют состояние:

```text
CreateOrderCommand
ProcessPaymentCommand
ReleaseInventoryCommand
```

Запросы получают данные:

```text
GetCartForCheckoutQuery
GetProductsForOrderQuery
```

При этом проект не требует обязательного разделения read/write баз данных.

В данном проекте CQRS прежде всего означает разделение:

* бизнес-намерений;
* чтения;
* изменения состояния;
* ответственности обработчиков.

---

# Vertical Slice Architecture

Функциональность организована вокруг бизнес-возможностей, а не вокруг технических слоёв.

Например:

```text
Modules/
└── Orders/
    ├── Domain/
    ├── Features/
    │   ├── Checkout/
    │   │   ├── CheckoutCommand.cs
    │   │   └── CheckoutHandler.cs
    │   │
    │   └── IntegrationEvents/
    │       ├── InventoryReservedHandler.cs
    │       ├── PaymentSucceededHandler.cs
    │       └── PaymentFailedHandler.cs
    │
    ├── DTOs/
    ├── Errors/
    ├── Infrastructure/
    │   └── Persistence/
    └── DependencyInjection.cs
```

Цель — держать код, относящийся к конкретному use case, максимально близко друг к другу.

---

# Checkout

Checkout представляет собой распределённый workflow между несколькими модулями.

Глобальной транзакции между базами данных нет.

Каждый модуль выполняет собственную локальную транзакцию и передаёт управление следующему этапу через сообщения.

```text
Checkout
   │
   ▼
Create Order
   │
   ▼
OrderCreated
   │
   ▼
Reserve Inventory
   │
   ├──────────────► InventoryReservationFailed
   │                         │
   │                         ▼
   │                    Order.Cancelled
   │
   ▼
InventoryReserved
   │
   ▼
ProcessPaymentCommand
   │
   ├──────────────► PaymentFailed
   │                         │
   │                         ▼
   │                    Order.Cancelled
   │                         │
   │                         ▼
   │                  ReleaseInventory
   │
   ▼
PaymentSucceeded
   │
   ▼
Order.Paid
   │
   ▼
CommitInventory
   │
   ▼
InventoryCommitted
   │
   ▼
Order.Completed
   │
   ▼
ClearCart
```

Данный workflow является **choreography-based Saga**.

Отдельного класса `Saga` нет.

Каждый модуль реагирует на события и выполняет свою часть процесса.

---

# Резервирование Inventory

При создании заказа физический остаток товара не уменьшается сразу.

Используется понятие доступного количества:

```text
Available = Quantity - ReservedQuantity
```

При резервировании:

```text
ReservedQuantity += Quantity
```

Если оплата не прошла:

```text
ReservedQuantity -= Quantity
```

Если оплата прошла и резерв подтверждается:

```text
Quantity -= Quantity
ReservedQuantity -= Quantity
```

Таким образом неоплаченный заказ не забирает товар из фактического остатка.

---

# Inventory Reservation

Резервирование товара отслеживается отдельно.

Резерв содержит:

```text
OrderId
ProductId
Quantity
Status
CreatedAt
CompletedAt
```

Возможные состояния:

```text
Reserved
Committed
Released
```

Это позволяет сделать операции идемпотентными.

Например, если `CommitInventoryCommand` будет доставлена дважды:

```text
Reserved
   │
   ▼
Committed
   │
   ▼
повторное сообщение → ничего не делать
```

То же самое относится к освобождению резерва.

---

# Idempotency

Messaging предполагает возможность повторной доставки сообщений.

Поэтому обработчики должны быть безопасными при повторном выполнении.

Например:

```csharp
if (order.Status != OrderStatus.InventoryReserved)
    return;
```

Если `PaymentSucceededEvent` будет доставлен повторно, обработчик не должен повторно изменять состояние заказа.

Такой подход используется для:

* резервирования Inventory;
* освобождения Inventory;
* подтверждения Inventory;
* обработки платежей;
* изменения статуса заказа;
* очистки корзины.

---

# Transactions

Каждый модуль управляет собственной транзакцией.

Например:

```csharp
[Transactional(typeof(OrderDbContext))]
public sealed class PaymentSucceededHandler(...)
{
    public async Task Handle(
        PaymentSucceededEvent message,
        CancellationToken ct)
    {
        // изменение Order

        await bus.SendAsync(
            new CommitInventoryCommand(...));
    }
}
```

В обработчиках не требуется вручную вызывать:

```csharp
await context.SaveChangesAsync(ct);
```

Wolverine transaction middleware управляет сохранением и commit транзакции.

Поскольку у каждого модуля собственная база, глобальной транзакции между несколькими базами данных нет.

---

# Outbox и Durable Messaging

Для надёжной доставки сообщений используется durable messaging с PostgreSQL.

Это особенно важно, когда изменение состояния в базе и отправка сообщения являются частью одной бизнес-операции.

Упрощённо процесс выглядит так:

```text
Database Transaction
       │
       ├── изменение состояния
       │
       └── сохранение outgoing message
              │
              ▼
         transaction commit
              │
              ▼
         доставка сообщения
```

Таким образом уменьшается вероятность ситуации:

```text
Database успешно обновлена
        +
процесс упал
        ↓
сообщение потеряно
```

---

# EmailWorker

Отправка email вынесена в отдельный процесс.

Например, Identity публикует:

```text
UserStartRegistrationEvent
```

Далее:

```text
Identity
   │
   ▼
RabbitMQ
   │
   ▼
EmailWorker
   │
   ▼
Email Provider
```

EmailWorker отвечает за:

* получение сообщений;
* формирование email;
* отправку;
* retry при временных ошибках.

API не выполняет отправку email непосредственно во время HTTP-запроса.

---

# Работа с изображениями

На текущем этапе отдельный `ImageWorker` не используется.

Изображения обрабатываются непосредственно в `Catalog`.

Основной pipeline:

```text
Client
   │
   ▼
POST /products
   │
   ▼
Catalog Endpoint
   │
   ▼
CreateProductCommand
   │
   ▼
CreateProductHandler
   │
   ├── validate product data
   │
   ├── validate image
   │
   ├── detect actual MIME type
   │
   ├── upload original → S3 / MinIO
   │
   ├── generate thumbnail
   │
   └── upload thumbnail → S3 / MinIO
   │
   ▼
ProductImage
```

Для загрузки используется `multipart/form-data`.

Изображение передаётся в Catalog как `IFormFile`, после чего преобразуется в application-level `UploadFile`.

Например:

```csharp
public sealed class UploadFile : IDisposable
{
    public string FileName { get; init; } = null!;

    public string ContentType { get; init; } = null!;

    public long Length { get; init; }

    public Stream Stream { get; init; } = null!;
}
```

Проверка содержимого выполняется независимо от значения HTTP `Content-Type`.

Для этого используется `MimeDetective`.

Обработка изображений выполняется через `ImageSharp`.

Текущая реализация генерирует thumbnail размером:

```text
200 × 200
```

с JPEG quality:

```text
45
```

Исходное изображение и thumbnail сохраняются в S3-compatible object storage.

---

# Object Storage

Изображения хранятся в object storage, а не непосредственно в PostgreSQL.

Текущая реализация использует S3-compatible API.

В production можно использовать любой S3-compatible storage.

Для локальной разработки может использоваться MinIO.

В базе данных хранятся относительные object keys.

Например:

```text
products/
  {productId}/
    {imageId}.jpg
    {thumbnailId}_thumb.jpg
```

Публичный URL формируется отдельно через `MediaUrlService`.

Таким образом, доменная модель не зависит от конкретного S3 endpoint или CDN.

Основная abstraction:

```csharp
IPublicStorage
```

а конкретная реализация работает через AWS S3 SDK.

---

# Payments

Платёжная интеграция скрыта за интерфейсом:

```csharp
public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(
        PaymentRequest request,
        CancellationToken ct);
}
```

Для разработки используется:

```csharp
services.AddScoped<IPaymentGateway, FakePaymentGateway>();
```

Это позволяет разрабатывать checkout workflow без подключения к реальному платёжному провайдеру.

В будущем `FakePaymentGateway` может быть заменён на реализацию конкретного платёжного провайдера.

Для реальных payment providers обработка должна быть построена с учётом webhook'ов и асинхронного подтверждения платежа, а не вокруг долгой DB-транзакции во время HTTP-запроса к внешнему API.

---

# Структура проекта

Упрощённая структура:

```text
src/
│
├── EShop.Api/
│
├── EShop.Contracts/
│
├── EShop.Shared/
│
├── Modules/
│   │
│   ├── Identity/
│   │   ├── Domain/
│   │   ├── Features/
│   │   ├── Infrastructure/
│   │   └── DependencyInjection.cs
│   │
│   ├── Catalog/
│   │   ├── Domain/
│   │   ├── Features/
│   │   ├── Application/
│   │   ├── Infrastructure/
│   │   └── DependencyInjection.cs
│   │
│   ├── Cart/
│   │   ├── Domain/
│   │   ├── Features/
│   │   ├── Infrastructure/
│   │   └── DependencyInjection.cs
│   │
│   ├── Inventory/
│   │   ├── Domain/
│   │   ├── Features/
│   │   ├── Infrastructure/
│   │   └── DependencyInjection.cs
│   │
│   ├── Orders/
│   │   ├── Domain/
│   │   ├── Features/
│   │   ├── Infrastructure/
│   │   └── DependencyInjection.cs
│   │
│   └── Payments/
│       ├── Domain/
│       ├── Features/
│       ├── Infrastructure/
│       └── DependencyInjection.cs
│
└── Workers/
    │
    └── EmailWorker/
```

---

# Технологический стек

## Backend

* C#
* ASP.NET Core
* Entity Framework Core

## Архитектура

* Modular Monolith
* Vertical Slice Architecture
* CQRS
* Domain-oriented modules
* Event-driven architecture
* Choreography-based Saga

## Messaging

* Wolverine
* RabbitMQ

## Databases

* PostgreSQL
* Entity Framework Core

## Cache / Temporary Storage

* Redis

## Object Storage

* S3-compatible storage
* MinIO для локальной разработки

## Image Processing

* ImageSharp
* MimeDetective

## Workers

* .NET Worker Services
* EmailWorker

## Infrastructure

* Docker
* Docker Compose

---

# Локальная разработка

## Требования

Для запуска проекта необходимо установить:

* Docker;
* Docker Compose.

---

# Infrastructure

Для локальной разработки используются контейнеры:

```text
PostgreSQL
RabbitMQ
Redis
MinIO
```

Запуск:

```bash
docker compose up -d
```

Проверка контейнеров:

```bash
docker compose ps
```

Остановка:

```bash
docker compose down
```

---

# Configuration

Connection strings и другие настройки задаются через configuration.

Пример:

```json
{
  "ConnectionStrings": {
    "IdentityDatabase": "...",
    "CatalogDatabase": "...",
    "CartDatabase": "...",
    "InventoryDatabase": "...",
    "OrderDatabase": "...",
    "PaymentDatabase": "...",
    "WolverineDatabase": "...",
    "RabbitMq": "...",
    "Redis": "...",
    "S3": "..."
  }
}
```

Секреты не должны храниться в Git.

Для локальной разработки рекомендуется использовать:

* User Secrets;
* environment variables;
* Docker secrets.

В production следует использовать специализированное хранилище секретов.

---

# Database Migrations

Каждый модуль владеет собственным `DbContext` и своими migrations.

Например:

```text
Identity
    └── IdentityDbContext

Catalog
    └── CatalogDbContext

Cart
    └── CartDbContext

Inventory
    └── InventoryDbContext

Orders
    └── OrderDbContext

Payments
    └── PaymentDbContext
```

Migrations применяются независимо для каждого модуля.

---

# Запуск приложения

Сначала запускаем инфраструктуру:

```bash
docker compose up -d
```

Запуск API:

```bash
dotnet run --project src/EShop.Api
```

Запуск EmailWorker:

```bash
dotnet run --project src/Workers/EmailWorker
```

---

# Основные архитектурные принципы

## Владение модулем

Каждый модуль владеет:

* своей бизнес-логикой;
* своей базой данных;
* своей persistence layer;
* своими бизнес-правилами.

Модуль не должен напрямую обращаться к persistence layer другого модуля.

---

## Явное взаимодействие

Для взаимодействия между модулями используются:

* Commands;
* Events;
* Queries.

Вместо прямого доступа к базе данных другого модуля.

---

## Локальные транзакции

Транзакция принадлежит одному модулю.

Например, нет одной транзакции одновременно для:

```text
Orders DB
Inventory DB
Payments DB
Cart DB
```

Каждая база фиксирует изменения независимо.

---

## Eventual Consistency

Из-за независимых баз некоторые изменения происходят не мгновенно.

Например:

```text
Order.Paid
      │
      ▼
CommitInventoryCommand
      │
      ▼
InventoryCommitted
      │
      ▼
Order.Completed
```

Order становится `Completed` только после подтверждения Inventory.

---

## Idempotency

Обработчики должны быть готовы к повторной доставке сообщений.

Для этого используются:

* проверки текущего состояния;
* уникальные идентификаторы;
* reservation records;
* явные состояния процессов.

---

## Компенсирующие операции

В распределённых workflow ошибки являются частью нормального сценария.

Например:

```text
PaymentFailed
      │
      ▼
Order.Cancelled
      │
      ▼
ReleaseInventory
```

Таким образом система компенсирует уже выполненную операцию резервирования.

---

# Почему Modular Monolith?

Проект начинается с Modular Monolith вместо полноценной системы микросервисов.

Это позволяет получить:

* более простой deployment;
* более простой local development;
* меньше инфраструктурной сложности;
* быстрые локальные вызовы;
* явные границы модулей;
* независимые базы данных;
* возможность в будущем вынести отдельные модули в сервисы.

При этом границы модулей сохраняются достаточно строгими, чтобы дальнейшая декомпозиция была возможна без полной переработки бизнес-логики.

---

# Почему Wolverine?

Wolverine используется как messaging abstraction и предоставляет:

* обработку сообщений;
* Commands;
* Events;
* локальное messaging;
* RabbitMQ integration;
* transaction middleware;
* durable messaging;
* интеграцию с Entity Framework Core;
* возможности Outbox / Inbox.

Это позволяет использовать единый подход к messaging как внутри приложения, так и между отдельными процессами.

---

# Почему RabbitMQ?

RabbitMQ используется преимущественно на границах процессов.

Например:

```text
EShop API
    │
    ▼
RabbitMQ
    │
    ▼
EmailWorker
```

Это позволяет вынести долгие или ресурсоёмкие операции из HTTP request lifecycle.

---

# Почему отдельная база данных для каждого модуля?

Каждый модуль должен иметь чёткое владение своими данными.

Например:

```text
Catalog    → Catalog DB
Orders     → Orders DB
Inventory  → Inventory DB
Payments   → Payments DB
```

Это предотвращает скрытую связанность через общие таблицы.

Также такая архитектура приближает проект к модели микросервисов, сохраняя простоту Modular Monolith.

---

# Обработка ошибок

Ошибки являются частью бизнес-процесса и представлены явно.

Например:

```text
InventoryReservationFailed
PaymentFailed
```

Для image processing ошибки на текущем этапе обрабатываются непосредственно в Catalog.

Временные инфраструктурные ошибки могут обрабатываться через retry.

Постоянные ошибки должны переводить процесс в соответствующее состояние, а не просто теряться.

---

# Надёжность

Архитектура предполагает, что:

* сообщения могут быть доставлены несколько раз;
* процесс может завершиться аварийно;
* внешние сервисы могут быть недоступны;
* одна база может успешно сохранить изменения, пока следующий этап workflow завершился ошибкой;
* между независимыми базами нет общей ACID-транзакции.

Поэтому используются:

* idempotent handlers;
* локальные транзакции;
* durable messaging;
* Outbox;
* compensating commands;
* явные состояния бизнес-процессов.

Для object storage учитывается отдельная граница согласованности между PostgreSQL и S3-compatible storage: они не участвуют в одной ACID-транзакции.

---

# Roadmap

* [ ] Интеграция с реальным payment provider
* [ ] Payment webhooks
* [ ] Улучшить image processing
* [ ] Поддержка дополнительных форматов изображений
* [ ] Конвертация изображений в WebP / AVIF
* [ ] Генерация дополнительных размеров изображений
* [ ] Модуль Reviews
* [ ] Изображения в отзывах
* [ ] Улучшить concurrency control в Inventory
* [ ] Integration tests для checkout Saga
* [ ] End-to-end тесты checkout
* [ ] Health checks
* [ ] Metrics
* [ ] Distributed tracing
* [ ] CI/CD
* [ ] Production deployment

---

# Состояния заказа

Текущий lifecycle заказа:

```text
                 ┌─────────────────────┐
                 │       Pending       │
                 └──────────┬──────────┘
                            │
                     InventoryReserved
                            │
                            ▼
                 ┌─────────────────────┐
                 │ InventoryReserved   │
                 └──────────┬──────────┘
                            │
                     PaymentSucceeded
                            │
                            ▼
                 ┌─────────────────────┐
                 │        Paid         │
                 └──────────┬──────────┘
                            │
                     InventoryCommitted
                            │
                            ▼
                 ┌─────────────────────┐
                 │     Completed       │
                 └─────────────────────┘


Pending ───────────────► Cancelled
        InventoryFailed

InventoryReserved ─────► Cancelled
        PaymentFailed
```

---

# License

Проект предназначен для учебных и демонстрационных целей.
