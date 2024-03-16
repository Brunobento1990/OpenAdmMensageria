﻿using Domain.Pkg.Entities;
using Mensageria.Infra.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Infra.EntityConfiguration;

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
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<MovimentacaoDeProduto> MovimentacoesDeProdutos { get; set; }
    public DbSet<TopUsuarios> TopUsuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new PesoConfiguration());
        modelBuilder.ApplyConfiguration(new PesosProdutosConfiguration());
        modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        modelBuilder.ApplyConfiguration(new TamanhoConfiguration());
        modelBuilder.ApplyConfiguration(new TamanhosProdutosConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        modelBuilder.ApplyConfiguration(new ItensPedidoConfiguration());
        modelBuilder.ApplyConfiguration(new FuncionarioConfiguration());
        modelBuilder.ApplyConfiguration(new TabelaDePrecoConfiguration());
        modelBuilder.ApplyConfiguration(new ItensTabelaDePrecoConfiguration());
        modelBuilder.ApplyConfiguration(new ProdutosMaisVendidosConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDeEmailConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracoesDePedidoConfiguration());
        modelBuilder.ApplyConfiguration(new EstoqueConfiguration());
        modelBuilder.ApplyConfiguration(new MovimentacaoDeProdutoConfiguration());
        modelBuilder.ApplyConfiguration(new TopUsuariosConfiguration());
    }
}
