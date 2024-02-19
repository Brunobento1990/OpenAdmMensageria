using Mensageria.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Mensageria.Service;

public class CachedService : ICachedService
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _options;
    private static readonly double _absolutExpiration = 60;
    private static readonly double _slidingExpiration = 30;

    public CachedService(IDistributedCache distributedCache)
    {
        _options = new DistributedCacheEntryOptions()
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_absolutExpiration))
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_slidingExpiration));

        _distributedCache = distributedCache;
    }
    public async Task RemoveCachedAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
