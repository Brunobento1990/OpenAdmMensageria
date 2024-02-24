using Mensageria.Dtos.ProdutosMaisVendidos;

namespace Mensageria.Interfaces;

public interface IPrecessarProdutosMaisVendidos
{
    Task ProcessarAsync(IList<AddOrUpdateProdutosMaisVendidosDto> addOrUpdateProdutosMaisVendidosDtos, string referer);
}
