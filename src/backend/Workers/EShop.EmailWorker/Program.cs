using EShop.EmailWorker.Abstractions;
using EShop.EmailWorker.Email;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection(nameof(EmailOptions)));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();

builder.UseWolverine(opt =>
{
    opt.UseRabbitMq(
            builder.Configuration.GetConnectionString("RabbitMq")!)
        .AutoProvision();

    opt.ListenToRabbitQueue("eshop-email")
        .ConfigureQueue(q =>
        {
            q.IsDurable = true;
            q.AutoDelete = false;
            q.IsExclusive = false;
        });

    opt.Discovery.IncludeAssembly(
        typeof(EmailWorkerMarker).Assembly);
});

var host = builder.Build();

await host.RunAsync();

public sealed class EmailWorkerMarker;
