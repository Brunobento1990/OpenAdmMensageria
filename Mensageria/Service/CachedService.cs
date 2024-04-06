using Mensageria.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Mensageria.Service;

public class CachedService : ICachedService
{
    private readonly IDistributedCache _distributedCache;

    public CachedService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task RemoveCachedAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
