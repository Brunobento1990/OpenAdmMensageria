using Domain.Pkg.Entities;
using Mensageria.Domain.Interfaces;
using Mensageria.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Repositories;

public class ProdutosMaisVendidosRepository : IProdutosMaisVendidosRepository
{
    private readonly IFactoryParceiroContext _factoryParceiroContext;

    public ProdutosMaisVendidosRepository(IFactoryParceiroContext factoryParceiroContext)
    {
        _factoryParceiroContext = factoryParceiroContext;
    }

    public async Task AddRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);
        await context.AddRangeAsync(produtosMaisVendidos);
        await context.SaveChangesAsync();
    }

    public async Task<IList<ProdutosMaisVendidos>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);

        return await context
            .ProdutosMaisVendidos
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }

    public async Task UpdateRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);
        context.UpdateRange(produtosMaisVendidos);
        await context.SaveChangesAsync();
    }
}
