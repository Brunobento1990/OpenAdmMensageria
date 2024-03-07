using Domain.Pkg.Entities;

namespace Mensageria.Infra.Interfaces;

public interface IMovimentacaoDeProdutoRepository
{
    Task AddMovimentacaoDeProdutosAsync(IList<MovimentacaoDeProduto> movimentacoesDeProdutos, string referer);
}
