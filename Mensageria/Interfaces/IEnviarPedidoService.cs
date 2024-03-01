using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface IEnviarPedidoService
{
    void EnviarPdf(PedidoCreateModel pedido);
}
