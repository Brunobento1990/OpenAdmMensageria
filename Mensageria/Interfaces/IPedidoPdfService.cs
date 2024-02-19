using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface IPedidoPdfService
{
    void GeneratePdfAsync(PedidoCreateModel pedido);
}
