using Domain.Pkg.Interfaces;
using Domain.Pkg.Services;
using Mensageria.Interfaces;
using Mensageria.Service;

namespace Mensageria.Ioc;

public static class DependencyInjectServices
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IMovimentacaoDeProdutoService, MovimentacaoDeProdutoService>();
        services.AddScoped<IPrecessarProdutosMaisVendidosService, PrecessarProdutosMaisVendidosService>();
        services.AddScoped<IEnviarPedidoService, EnviarPedidoService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICachedService, CachedService>();
        services.AddScoped<ITopUsuarioService, TopUsuarioService>();
    }
}
