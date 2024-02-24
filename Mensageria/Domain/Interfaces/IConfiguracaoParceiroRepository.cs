namespace Mensageria.Domain.Interfaces;

public interface IConfiguracaoParceiroRepository
{
    Task<string> GetConnectionStringParceiroAsync(string referer);
}
