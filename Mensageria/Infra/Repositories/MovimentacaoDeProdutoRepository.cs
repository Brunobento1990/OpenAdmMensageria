using Domain.Pkg.Entities;
using Mensageria.Infra.Interfaces;

namespace Mensageria.Infra.Repositories;

public class MovimentacaoDeProdutoRepository : IMovimentacaoDeProdutoRepository
{
    private readonly IFactoryParceiroContext _parceiroContext;

    public MovimentacaoDeProdutoRepository(IFactoryParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task AddMovimentacaoDeProdutosAsync(IList<MovimentacaoDeProduto> movimentacoesDeProdutos, string referer)
    {
        var context = await _parceiroContext.CreateParceiroContextAsync(referer);

        await context.AddRangeAsync(movimentacoesDeProdutos);
        await context.SaveChangesAsync();
    }
}
