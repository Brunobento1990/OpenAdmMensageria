using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface IEnviarPedidoService
{
    Task EnviarPdfAsync(PedidoCreateModel pedido);
}
