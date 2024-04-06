using Domain.Pkg.Entities;

namespace Mensageria.Model;

public class NotificarPedidoEditadoModel
{
    public string EmailEnvio { get; set; } = string.Empty;
    public Pedido Pedido { get; set; } = null!;
}
