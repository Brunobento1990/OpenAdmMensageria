using Domain.Pkg.Entities;

namespace Mensageria.Domain.Interfaces;

public interface ITopUsuarioRepository
{
    Task<TopUsuarios?> GetTopUsuarioByUsuarioIdAsync(Guid usuarioId, string referer);
    Task AddTopUsuarioAsync(TopUsuarios topUsuario, string referer);
    Task UpdateTopUsuarioAsync(TopUsuarios topUsuario, string referer);
}
