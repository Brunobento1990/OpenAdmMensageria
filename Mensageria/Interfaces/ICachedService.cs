namespace Mensageria.Interfaces;

public interface ICachedService
{
    Task RemoveCachedAsync(string key);
}
