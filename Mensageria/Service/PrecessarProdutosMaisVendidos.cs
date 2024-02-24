using Domain.Pkg.Entities;
using Mensageria.Domain.Interfaces;
using Mensageria.Interfaces;

namespace Mensageria.Service;

public class PrecessarProdutosMaisVendidos : IPrecessarProdutosMaisVendidos
{
    private readonly IProdutosMaisVendidosRepository _produtosMaisVendidosRepository;

    public PrecessarProdutosMaisVendidos(IProdutosMaisVendidosRepository produtosMaisVendidosRepository)
    {
        _produtosMaisVendidosRepository = produtosMaisVendidosRepository;
    }

    public async Task ProcessarAsync(Pedido pedido, string referer)
    {
        var produtosIds = pedido.ItensPedido
            .Select(x => x.ProdutoId)
            .ToList();

        var produtosMaisVendidos = await _produtosMaisVendidosRepository.GetProdutosMaisVendidosAsync(produtosIds, referer);

        var add = new List<ProdutosMaisVendidos>();
        var updates = new List<ProdutosMaisVendidos>();
        var date = DateTime.Now;

        foreach (var itens in pedido.ItensPedido)
        {
            var addOrUpdate = produtosMaisVendidos
                .FirstOrDefault(x => x.ProdutoId == itens.ProdutoId);

            if (addOrUpdate == null)
            {
                var insert = add.FirstOrDefault(x => x.ProdutoId == itens.ProdutoId);

                if (insert == null)
                {
                    addOrUpdate = new ProdutosMaisVendidos(
                        Guid.NewGuid(),
                        date,
                        date,
                        0,
                        itens.Quantidade,
                        1,
                        itens.ProdutoId);

                    add.Add(addOrUpdate);
                }
                else
                {
                    insert.UpdateQuantidadeProdutos(itens.Quantidade);
                }

            }
            else
            {
                var update = updates.FirstOrDefault(x => x.ProdutoId == itens.ProdutoId);

                if (update == null)
                {
                    addOrUpdate.UpdateQuantidadeProdutos(itens.Quantidade);
                    updates.Add(addOrUpdate);
                }
                else
                {
                    update.UpdateQuantidadeProdutos(itens.Quantidade);
                }

            }
        }

        await _produtosMaisVendidosRepository.AddRangeAsync(add, referer);
        await _produtosMaisVendidosRepository.UpdateRangeAsync(updates, referer);
    }
}
