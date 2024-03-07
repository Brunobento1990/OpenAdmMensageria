using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using Mensageria.Infra.Interfaces;
using Mensageria.Interfaces;

namespace Mensageria.Service;

public class MovimentacaoDeProdutoService : IMovimentacaoDeProdutoService
{
    private readonly IMovimentacaoDeProdutoRepository _movimentacaoDeProdutoRepository;
    private readonly IEstoqueRepository _estoqueRepository;

    public MovimentacaoDeProdutoService(
        IMovimentacaoDeProdutoRepository movimentacaoDeProdutoRepository,
        IEstoqueRepository estoqueRepository)
    {
        _movimentacaoDeProdutoRepository = movimentacaoDeProdutoRepository;
        _estoqueRepository = estoqueRepository;
    }

    public async Task MovimentarProdutosAsync(Pedido pedido, string referer)
    {
        var produtoIds = pedido
            .ItensPedido
            .DistinctBy(x => x.ProdutoId)
            .Select(x => x.ProdutoId)
            .ToList();

        var estoques = await _estoqueRepository.GetAllEstoquesAsync(produtoIds, referer);

        var movimntosDeProdutos = pedido.ItensPedido
            .Select(x =>
                new MovimentacaoDeProduto(
                    Guid.NewGuid(),
                    pedido.DataDeCriacao,
                    pedido.DataDeAtualizacao,
                    0,
                    x.Quantidade,
                    TipoMovimentacaoDeProduto.Saida,
                    x.ProdutoId))
            .ToList();

        await _movimentacaoDeProdutoRepository.AddMovimentacaoDeProdutosAsync(movimntosDeProdutos, referer);

        var newEstoques = new List<Estoque>();

        foreach (var item in pedido.ItensPedido)
        {
            var estoque = estoques
                .FirstOrDefault(x => x.ProdutoId == item.ProdutoId);

            if (estoque == null)
            {
                estoque = new Estoque(
                        Guid.NewGuid(),
                        pedido.DataDeCriacao,
                        pedido.DataDeAtualizacao,
                        0,
                        item.ProdutoId,
                        item.Quantidade);

                newEstoques.Add(estoque);
            }
            else
            {
                estoque.UpdateEstoque(item.Quantidade, TipoMovimentacaoDeProduto.Saida);
            }
        }

        await _estoqueRepository.UpdateEstoqueAsync(estoques, referer);
        await _estoqueRepository.AddEstoqueAsync(newEstoques, referer);
    }
}
