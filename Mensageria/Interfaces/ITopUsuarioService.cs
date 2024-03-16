using Domain.Pkg.Entities;

namespace Mensageria.Interfaces;

public interface ITopUsuarioService
{
    Task AddOrUpdateTopUsuarioAsync(Pedido pedido, string referer);
}
