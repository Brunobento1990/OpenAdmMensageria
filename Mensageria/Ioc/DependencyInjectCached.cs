namespace Mensageria.Ioc;

public static class DependencyInjectCached
{
    public static void InjectCached(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = VariaveisDeAmbiente.GetVariavel("REDIS_URL");
        });
    }
}
