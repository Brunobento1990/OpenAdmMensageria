using dotenv.net;
using Mensageria;
using QuestPDF.Infrastructure;
using Domain.Pkg.Cryptography;
using Mensageria.Ioc;

var builder = Host.CreateApplicationBuilder(args);

DotEnv.Load();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.InjectServices();
builder.Services.InjectRepositories();
builder.Services.InjectConsumer();
builder.Services.InjectContext();
builder.Services.InjectCached();
builder.Services.InjectConnectionRabbitMq();

var key = VariaveisDeAmbiente.GetVariavel("CRYP_KEY");
var iv = VariaveisDeAmbiente.GetVariavel("CRYP_IV");
CryptographyGeneric.Configure(key, iv);

var host = builder.Build();
host.Run();
