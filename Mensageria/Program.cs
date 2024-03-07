using dotenv.net;
using Mensageria;
using Mensageria.Infra.Context;
using Mensageria.Interfaces;
using Mensageria.Mensageria;
using Mensageria.Mensageria.Consumers;
using Mensageria.Service;
using QuestPDF.Infrastructure;
using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
using Mensageria.Domain.Interfaces;
using Mensageria.Infra.Repositories;
using Mensageria.Infra.Interfaces;
using Mensageria.Infra.Factories;

var builder = Host.CreateApplicationBuilder(args);

DotEnv.Load();

QuestPDF.Settings.License = LicenseType.Community;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
string connectionString = VariaveisDeAmbiente.GetVariavel("STRING_CONNECTION");
builder.Services.AddDbContext<OpenAdmContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = VariaveisDeAmbiente.GetVariavel("REDIS_URL");
});

builder.Services.AddSingleton<IConnection>(s => ConnectionBase.InitConnection());
builder.Services.AddTransient<IModel>(s => s.GetRequiredService<IConnection>().CreateModel());

builder.Services.AddScoped<IEnviarPedidoService, EnviarPedidoService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICachedService, CachedService>();
builder.Services.AddScoped<IConfiguracaoParceiroRepository, ConfiguracaoParceiroRepository>();
builder.Services.AddScoped<IFactoryParceiroContext, FactoryParceiroContext>();
builder.Services.AddScoped<IProdutosMaisVendidosRepository, ProdutosMaisVendidosRepository>();
builder.Services.AddScoped<IPrecessarProdutosMaisVendidos, PrecessarProdutosMaisVendidos>();
builder.Services.AddScoped<IMovimentacaoDeProdutoRepository, MovimentacaoDeProdutoRepository>();
builder.Services.AddScoped<IEstoqueRepository, EstoqueRepository>();
builder.Services.AddScoped<IMovimentacaoDeProdutoService, MovimentacaoDeProdutoService>();

builder.Services.AddHostedService<PedidoCreatePdfConsumer>();
builder.Services.AddHostedService<ProdutosMaisVendidosConsumer>();
builder.Services.AddHostedService<MovimentacaoDeProdutosConsumer>();

var host = builder.Build();
host.Run();
