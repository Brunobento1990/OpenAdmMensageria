using Domain.Pkg.Entities;

namespace Mensageria.Interfaces;

public interface IPrecessarProdutosMaisVendidosService
{
    Task ProcessarAsync(Pedido pedido, string referer);
}
