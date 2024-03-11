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
        var date = DateTime.Now;
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
                    date,
                    date,
                    0,
                    x.Quantidade,
                    TipoMovimentacaoDeProduto.Saida,
                    x.ProdutoId,
                    x.TamanhoId,
                    x.PesoId))
            .ToList();

        await _movimentacaoDeProdutoRepository.AddMovimentacaoDeProdutosAsync(movimntosDeProdutos, referer);


        var newEstoques = new List<Estoque>();
        foreach (var item in pedido.ItensPedido)
        {
            bool where(Estoque x) => x.ProdutoId == item.ProdutoId &&
                x.TamanhoId == item.TamanhoId &&
                x.PesoId == item.PesoId;

            ProcessarEstoqueItemPedido(estoques, item, newEstoques, date, where);
        }

        await _estoqueRepository.UpdateEstoqueAsync(estoques, referer);
        await _estoqueRepository.AddEstoqueAsync(newEstoques, referer);
    }

    private static void ProcessarEstoqueItemPedido(
        IList<Estoque> estoques,
        ItensPedido item,
        List<Estoque> newEstoques,
        DateTime date,
        Func<Estoque, bool> where)
    {
        var estoque = estoques
                .FirstOrDefault(where);

        if (estoque != null)
        {
            estoque.UpdateEstoque(item.Quantidade, TipoMovimentacaoDeProduto.Saida);
            return;
        }

        estoque = newEstoques.FirstOrDefault(where);

        if (estoque != null)
        {
            estoque.UpdateEstoque(item.Quantidade, TipoMovimentacaoDeProduto.Saida);
            return;
        }

        estoque = new Estoque(
            Guid.NewGuid(),
            date,
            date,
            0,
            item.ProdutoId,
            -item.Quantidade,
            item.TamanhoId,
            item.PesoId);

        newEstoques.Add(estoque);
    }
}
