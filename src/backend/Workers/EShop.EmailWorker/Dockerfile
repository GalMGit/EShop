FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/Workers/EShop.EmailWorker/EShop.EmailWorker.csproj", "src/backend/Workers/EShop.EmailWorker/"]
RUN dotnet restore "src/backend/Workers/EShop.EmailWorker/EShop.EmailWorker.csproj"
COPY . .
WORKDIR "/src/src/backend/Workers/EShop.EmailWorker"
RUN dotnet build "./EShop.EmailWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShop.EmailWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.EmailWorker.dll"]
