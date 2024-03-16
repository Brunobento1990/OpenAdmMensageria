using Domain.Pkg.Entities;
using Mensageria.Domain.Interfaces;
using Mensageria.Interfaces;

namespace Mensageria.Service;

public sealed class TopUsuarioService : ITopUsuarioService
{
    private readonly ITopUsuarioRepository _topUsuarioRepository;

    public TopUsuarioService(ITopUsuarioRepository topUsuarioRepository)
    {
        _topUsuarioRepository = topUsuarioRepository;
    }

    public async Task AddOrUpdateTopUsuarioAsync(Pedido pedido, string referer)
    {
        var topUsuario = await _topUsuarioRepository.GetTopUsuarioByUsuarioIdAsync(pedido.UsuarioId, referer);

        if(topUsuario != null)
        {
            topUsuario.Update(pedido.ValorTotal, 1);
            await _topUsuarioRepository.UpdateTopUsuarioAsync(topUsuario, referer);
            return;
        }

        var date = DateTime.Now;

        topUsuario = new TopUsuarios(
            Guid.NewGuid(),
            date,
            date,
            0,
            pedido.ValorTotal,
            1,
            pedido.UsuarioId,
            pedido.Usuario.Nome);

        await _topUsuarioRepository.AddTopUsuarioAsync(topUsuario, referer);
    }
}
