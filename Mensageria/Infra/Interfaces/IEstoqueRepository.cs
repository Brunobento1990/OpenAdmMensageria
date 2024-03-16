using Domain.Pkg.Entities;

namespace Mensageria.Infra.Interfaces;

public interface IEstoqueRepository
{
    Task AddEstoqueAsync(IList<Estoque> estoques, string referer);
    Task UpdateEstoqueAsync(IList<Estoque> estoques, string referer);
    Task<IList<Estoque>> GetAllEstoquesAsync(IList<Guid> produtosIds, string referer);
}
