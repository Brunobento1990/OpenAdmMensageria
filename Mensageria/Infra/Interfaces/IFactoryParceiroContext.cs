using Mensageria.Infra.Context;

namespace Mensageria.Infra.Interfaces;

public interface IFactoryParceiroContext
{
    Task<ParceiroContext> CreateParceiroContextAsync(string referer);
}
