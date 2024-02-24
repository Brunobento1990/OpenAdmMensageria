using Domain.Pkg.Entities;

namespace Mensageria.Domain.Interfaces;

public interface IProdutosMaisVendidosRepository
{
    Task AddRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos, string referer);
    Task UpdateRangeAsync(IList<ProdutosMaisVendidos> produtosMaisVendidos, string referer);
    Task<IList<ProdutosMaisVendidos>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds, string referer);
}
