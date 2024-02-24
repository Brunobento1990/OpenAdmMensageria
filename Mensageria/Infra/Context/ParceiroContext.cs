using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mensageria.Infra.Context;

public class ParceiroContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Peso> Pesos { get; set; }
    public DbSet<Tamanho> Tamanhos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<PesosProdutos> PesosProdutos { get; set; }
    public DbSet<TamanhosProdutos> TamanhosProdutos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItensPedido> ItensPedidos { get; set; }
    public DbSet<TabelaDePreco> TabelaDePreco { get; set; }
    public DbSet<ItensTabelaDePreco> ItensTabelaDePreco { get; set; }
    public DbSet<ProdutosMaisVendidos> ProdutosMaisVendidos { get; set; }
    public DbSet<ConfiguracaoDeEmail> ConfiguracoesDeEmail { get; set; }
    public DbSet<ConfiguracoesDePedido> ConfiguracoesDePedidos { get; set; }
}
