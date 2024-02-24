using Domain.Pkg.Entities;

namespace Mensageria.Interfaces;

public interface IPrecessarProdutosMaisVendidos
{
    Task ProcessarAsync(Pedido pedido, string referer);
}
