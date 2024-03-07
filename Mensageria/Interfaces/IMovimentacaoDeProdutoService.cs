using Domain.Pkg.Entities;

namespace Mensageria.Interfaces;

public interface IMovimentacaoDeProdutoService
{
    Task MovimentarProdutosAsync(Pedido pedido, string referer);
}
