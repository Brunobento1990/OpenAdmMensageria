using Mensageria.Mensageria.Consumers;

namespace Mensageria.Ioc;

public static class DependencyInjectConsumer
{
    public static void InjectConsumer(this IServiceCollection services)
    {
        services.AddHostedService<PedidoCreatePdfConsumer>();
        services.AddHostedService<ProdutosMaisVendidosConsumer>();
        services.AddHostedService<MovimentacaoDeProdutosConsumer>();
        services.AddHostedService<TopUsuariosConsumer>();
    }
}
