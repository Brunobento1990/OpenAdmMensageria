using Mensageria.Mensageria.Consumers;

namespace Mensageria.Ioc;

public static class DependencyInjectConsumer
{
    public static void InjectConsumer(this IServiceCollection services)
    {
        services.AddHostedService<PedidoCreatePdfConsumer>();
        services.AddHostedService<ProdutosMaisVendidosConsumer>();
        services.AddHostedService<PedidoEntregueConsumer>();
        services.AddHostedService<TopUsuariosConsumer>();
        services.AddHostedService<EditItemPedidoConsumer>();
    }
}
