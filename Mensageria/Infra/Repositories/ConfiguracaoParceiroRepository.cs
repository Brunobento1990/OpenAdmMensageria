using Mensageria.Domain.Interfaces;
using Mensageria.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Repositories;

public class ConfiguracaoParceiroRepository : IConfiguracaoParceiroRepository
{
    private readonly OpenAdmContext _openAdmContext;

    public ConfiguracaoParceiroRepository(OpenAdmContext openAdmContext)
    {
        _openAdmContext = openAdmContext;
    }

    public async Task<string> GetConnectionStringParceiroAsync(string referer)
    {
        return await _openAdmContext
            .ConfiguracoesParceiro
            .AsNoTracking()
            .Where(x => x.DominioSiteAdm == referer || x.DominioSiteEcommerce == referer)
            .Select(x => x.ConexaoDb)
            .FirstOrDefaultAsync()
             ?? throw new Exception("Conexão do parceiro não foi localizada!");
    }
}
