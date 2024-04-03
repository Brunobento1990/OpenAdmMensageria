using dotenv.net;
using Mensageria;
using Domain.Pkg.Cryptography;
using Mensageria.Ioc;
using Domain.Pkg.Pdfs.Configure;

var builder = Host.CreateApplicationBuilder(args);

DotEnv.Load();

ConfigurePdfQuest.ConfigureQuest();

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
