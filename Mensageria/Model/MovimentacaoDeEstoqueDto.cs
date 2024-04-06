using Domain.Pkg.Enum;

namespace Mensageria.Model;

public class MovimentacaoDeEstoqueDto
{
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public decimal Quantidade { get; set; }
    public string? Observacao { get; set; }
    public TipoMovimentacaoDeProduto TipoMovimentacaoDeProduto { get; set; }
}
