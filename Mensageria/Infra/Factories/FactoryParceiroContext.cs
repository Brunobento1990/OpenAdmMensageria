using Mensageria.Domain.Interfaces;
using Mensageria.Infra.Context;
using Mensageria.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Factories;

public class FactoryParceiroContext : IFactoryParceiroContext
{
    private readonly IConfiguracaoParceiroRepository _configuracaoParceiroRepository;

    public FactoryParceiroContext(IConfiguracaoParceiroRepository configuracaoParceiroRepository)
    {
        _configuracaoParceiroRepository = configuracaoParceiroRepository;
    }

    public async Task<ParceiroContext> CreateParceiroContextAsync(string referer)
    {
        var connectionString = await _configuracaoParceiroRepository.GetConnectionStringParceiroAsync(referer);

        var optionsBuilderParceiro = new DbContextOptionsBuilder<ParceiroContext>();

        optionsBuilderParceiro.UseNpgsql(connectionString);

        return new ParceiroContext(optionsBuilderParceiro.Options);
    }
}
