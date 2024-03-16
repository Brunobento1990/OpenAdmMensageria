using Mensageria.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Ioc;

public static class DependencyInjectContext
{
    public static void InjectContext(this IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        string connectionString = VariaveisDeAmbiente.GetVariavel("STRING_CONNECTION");
        services.AddDbContext<OpenAdmContext>(opt => opt.UseNpgsql(connectionString));
    }
}
