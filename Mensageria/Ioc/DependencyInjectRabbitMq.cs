using Mensageria.Mensageria;
using RabbitMQ.Client;

namespace Mensageria.Ioc;

public static class DependencyInjectRabbitMq
{
    public static void InjectConnectionRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IConnection>(s => ConnectionBase.InitConnection());
        services.AddTransient<IModel>(s => s.GetRequiredService<IConnection>().CreateModel());
    }
}
