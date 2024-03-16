using Domain.Pkg.Entities;
using Mensageria.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Repositories;

public class EstoqueRepository : IEstoqueRepository
{
    private readonly IFactoryParceiroContext _factoryParceiroContext;

    public EstoqueRepository(IFactoryParceiroContext factoryParceiroContext)
    {
        _factoryParceiroContext = factoryParceiroContext;
    }

    public async Task AddEstoqueAsync(IList<Estoque> estoques, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);

        await context.AddRangeAsync(estoques);
        await context.SaveChangesAsync();
    }

    public async Task<IList<Estoque>> GetAllEstoquesAsync(IList<Guid> produtosIds, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);

        return await context
            .Estoques
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }

    public async Task UpdateEstoqueAsync(IList<Estoque> estoques, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);

        foreach (var estoque in estoques)
        {
            context.Attach(estoque);
        }
        
        context.UpdateRange(estoques);
        await context.SaveChangesAsync();
    }
}
