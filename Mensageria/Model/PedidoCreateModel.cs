using Mensageria.Entities;

namespace Mensageria.Model;

public class PedidoCreateModel
{
    public string EmailEnvio { get; set; } = string.Empty;
    public Pedido Pedido { get; set; } = null!;
}
