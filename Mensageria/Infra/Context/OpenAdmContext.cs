using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Context;

public class OpenAdmContext(DbContextOptions<OpenAdmContext> options)
    : DbContext(options)
{
    public DbSet<Parceiro> Parceiros { get; set; }
    public DbSet<ConfiguracaoParceiro> ConfiguracoesParceiro { get; set; }
}
