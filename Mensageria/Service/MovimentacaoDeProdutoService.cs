using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using Mensageria.Infra.Interfaces;
using Mensageria.Interfaces;
using Mensageria.Model;

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

    public async Task MovimentarProdutosAsync(IList<MovimentacaoDeEstoqueDto> movimentacaoDeEstoqueDtos, string referer)
    {
        var date = DateTime.Now;
        var produtoIds = movimentacaoDeEstoqueDtos
            .DistinctBy(x => x.ProdutoId)
            .Select(x => x.ProdutoId)
            .ToList();

        var estoques = await _estoqueRepository.GetAllEstoquesAsync(produtoIds, referer);

        var movimntosDeProdutos = movimentacaoDeEstoqueDtos
            .Select(x =>
                new MovimentacaoDeProduto(
                    Guid.NewGuid(),
                    date,
                    date,
                    0,
                    x.Quantidade,
                    x.TipoMovimentacaoDeProduto,
                    x.ProdutoId,
                    x.TamanhoId,
                    x.PesoId,
                    x.Observacao))
            .ToList();

        await _movimentacaoDeProdutoRepository.AddMovimentacaoDeProdutosAsync(movimntosDeProdutos, referer);

        var newEstoques = new List<Estoque>();
        foreach (var item in movimentacaoDeEstoqueDtos)
        {
            bool where(Estoque x) => x.ProdutoId == item?.ProdutoId &&
                x.TamanhoId == item.TamanhoId &&
                x.PesoId == item.PesoId;

            ProcessarEstoqueItemPedido(estoques, item, newEstoques, date, where, item.TipoMovimentacaoDeProduto);
        }

        await _estoqueRepository.UpdateEstoqueAsync(estoques, referer);
        await _estoqueRepository.AddEstoqueAsync(newEstoques, referer);
    }

    private static void ProcessarEstoqueItemPedido(
        IList<Estoque> estoques,
        MovimentacaoDeEstoqueDto item,
        List<Estoque> newEstoques,
        DateTime date,
        Func<Estoque, bool> where,
        TipoMovimentacaoDeProduto tipoMovimentacaoDeProduto)
    {
        var estoque = estoques
                .FirstOrDefault(where);

        if (estoque != null)
        {
            estoque.UpdateEstoque(item.Quantidade, tipoMovimentacaoDeProduto);
            return;
        }

        estoque = newEstoques.FirstOrDefault(where);

        if (estoque != null)
        {
            estoque.UpdateEstoque(item.Quantidade, tipoMovimentacaoDeProduto);
            return;
        }

        var newQuantidade = tipoMovimentacaoDeProduto == TipoMovimentacaoDeProduto.Entrada ?
            item.Quantidade : -item.Quantidade;

        estoque = new Estoque(
            Guid.NewGuid(),
            date,
            date,
            0,
            item.ProdutoId,
            newQuantidade,
            item.TamanhoId,
            item.PesoId);

        newEstoques.Add(estoque);
    }
}
