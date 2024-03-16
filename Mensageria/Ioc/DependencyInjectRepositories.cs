using Mensageria.Domain.Interfaces;
using Mensageria.Infra.Factories;
using Mensageria.Infra.Interfaces;
using Mensageria.Infra.Repositories;

namespace Mensageria.Ioc;

public static class DependencyInjectRepositories
{
    public static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IConfiguracaoParceiroRepository, ConfiguracaoParceiroRepository>();
        services.AddScoped<IFactoryParceiroContext, FactoryParceiroContext>();
        services.AddScoped<IProdutosMaisVendidosRepository, ProdutosMaisVendidosRepository>();
        services.AddScoped<IMovimentacaoDeProdutoRepository, MovimentacaoDeProdutoRepository>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<ITopUsuarioRepository, TopUsuarioRepository>();
    }
}
