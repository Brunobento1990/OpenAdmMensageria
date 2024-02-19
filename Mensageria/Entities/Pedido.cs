namespace Mensageria.Entities;

public sealed class Pedido : BaseEntity
{
    public Pedido(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusPedido statusPedido,
        Guid usuarioId)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        StatusPedido = statusPedido;
        UsuarioId = usuarioId;
    }

    public StatusPedido StatusPedido { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public decimal ValorTotal { get { return ItensPedido.Sum(x => x.ValorTotal); } }
    public IList<ItensPedido> ItensPedido { get; set; } = new List<ItensPedido>();
}

public enum StatusPedido
{
    Aberto,
    Faturado,
    RotaDeEntrega,
    Entregue
}
