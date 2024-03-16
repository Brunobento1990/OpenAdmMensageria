using Domain.Pkg.Entities;

namespace Mensageria.Model;

public class PedidoCreateModel
{
    public string EmailEnvio { get; set; } = string.Empty;
    public byte[]? Logo { get; set; }
    public Pedido Pedido { get; set; } = null!;
}
