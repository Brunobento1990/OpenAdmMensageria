using Domain.Pkg.Entities;
using Mensageria.Domain.Interfaces;
using Mensageria.Dtos.ProdutosMaisVendidos;
using Mensageria.Interfaces;

namespace Mensageria.Service;

public class PrecessarProdutosMaisVendidos : IPrecessarProdutosMaisVendidos
{
    private readonly IProdutosMaisVendidosRepository _produtosMaisVendidosRepository;

    public PrecessarProdutosMaisVendidos(IProdutosMaisVendidosRepository produtosMaisVendidosRepository)
    {
        _produtosMaisVendidosRepository = produtosMaisVendidosRepository;
    }

    public async Task ProcessarAsync(IList<AddOrUpdateProdutosMaisVendidosDto> addOrUpdateProdutosMaisVendidosDtos, string referer)
    {
        var produtosIds = addOrUpdateProdutosMaisVendidosDtos
            .Select(x => x.ProdutoId)
            .ToList();

        var produtosMaisVendidos = await _produtosMaisVendidosRepository.GetProdutosMaisVendidosAsync(produtosIds, referer);

        var add = new List<ProdutosMaisVendidos>();
        var updates = new List<ProdutosMaisVendidos>();
        var date = DateTime.Now;

        foreach (var addOrUpdateDto in addOrUpdateProdutosMaisVendidosDtos)
        {
            var addOrUpdate = produtosMaisVendidos
                .FirstOrDefault(x => x.ProdutoId == addOrUpdateDto.ProdutoId);

            if (addOrUpdate == null)
            {
                var insert = add.FirstOrDefault(x => x.ProdutoId == addOrUpdateDto.ProdutoId);

                if (insert == null)
                {
                    addOrUpdate = new ProdutosMaisVendidos(
                        Guid.NewGuid(),
                        date,
                        date,
                        0,
                        addOrUpdateDto.QuantidadeProdutos,
                        1,
                        addOrUpdateDto.ProdutoId);

                    add.Add(addOrUpdate);
                }
                else
                {
                    insert.UpdateQuantidadeProdutos(addOrUpdateDto.QuantidadeProdutos);
                }

            }
            else
            {
                var update = updates.FirstOrDefault(x => x.ProdutoId == addOrUpdateDto.ProdutoId);

                if (update == null)
                {
                    addOrUpdate.UpdateQuantidadeProdutos(addOrUpdateDto.QuantidadeProdutos);
                    updates.Add(addOrUpdate);
                }
                else
                {
                    update.UpdateQuantidadeProdutos(addOrUpdateDto.QuantidadeProdutos);
                }

            }
        }

        await _produtosMaisVendidosRepository.AddRangeAsync(add, referer);
        await _produtosMaisVendidosRepository.UpdateRangeAsync(updates, referer);
    }
}
