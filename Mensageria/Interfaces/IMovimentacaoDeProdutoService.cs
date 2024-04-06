using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface IMovimentacaoDeProdutoService
{
    Task MovimentarProdutosAsync(IList<MovimentacaoDeEstoqueDto> movimentacaoDeEstoqueDtos, string referer);
}
