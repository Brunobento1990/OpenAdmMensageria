using dotenv.net;
using Mensageria;
using Mensageria.Interfaces;
using Mensageria.Mensageria;
using Mensageria.Mensageria.Consumers;
using Mensageria.Service;
using QuestPDF.Infrastructure;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

DotEnv.Load();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = VariaveisDeAmbiente.GetVariavel("REDIS_URL");
});

builder.Services.AddSingleton<IConnection>(s => ConnectionBase.InitConnection());
builder.Services.AddTransient<IModel>(s => s.GetRequiredService<IConnection>().CreateModel());

builder.Services.AddScoped<IPedidoPdfService, PedidoPdfService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICachedService, CachedService>();

builder.Services.AddHostedService<PedidoCreatePdfConsumer>();

var host = builder.Build();
host.Run();
