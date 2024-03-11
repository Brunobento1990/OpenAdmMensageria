using Domain.Pkg.Entities;
using Mensageria.Domain.Interfaces;
using Mensageria.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Repositories;

public class TopUsuarioRepository : ITopUsuarioRepository
{
    private readonly IFactoryParceiroContext _factoryParceiroContext;

    public TopUsuarioRepository(IFactoryParceiroContext factoryParceiroContext)
    {
        _factoryParceiroContext = factoryParceiroContext;
    }

    public async Task AddTopUsuarioAsync(TopUsuarios topUsuario, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);
        context.Attach(topUsuario);
        await context.AddAsync(topUsuario);
        await context.SaveChangesAsync();
    }

    public async Task<TopUsuarios?> GetTopUsuarioByUsuarioIdAsync(Guid usuarioId, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);

        return await context
            .TopUsuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
    }

    public async Task UpdateTopUsuarioAsync(TopUsuarios topUsuario, string referer)
    {
        var context = await _factoryParceiroContext.CreateParceiroContextAsync(referer);
        context.Attach(topUsuario);
        context.Update(topUsuario);
        await context.SaveChangesAsync();
    }
}
