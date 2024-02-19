using Mensageria.Interfaces;
using Mensageria.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Mensageria.Service;

public class PedidoPdfService : IPedidoPdfService
{
    private readonly IList<string> _colunsName;
    private readonly IEmailService _emailService;
    private readonly IList<int> _colunsWidt;

    public PedidoPdfService(IEmailService emailService)
    {
        _colunsName = new List<string>()
        {
            "REF",
            "Descrição",
            "Tamanho/Peso",
            "Quantidade",
            "Valor unitário",
            "Total"
        };
        _colunsWidt = new List<int>()
        {
            60,150,80,70,90,50
        };
        _emailService = emailService;
        //_imageData = Convert.FromBase64String(_imageString);
    }
    public void GeneratePdfAsync(PedidoCreateModel pedidoCreateModel)
    {
        void HeaderCustom(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();
            var titleStyle2 = TextStyle.Default.FontSize(10).SemiBold();
            var titleStyleName = TextStyle.Default.FontSize(10);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"#Iscas Lune").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Data de emissão: ").SemiBold().FontSize(14);
                        text.Span(pedidoCreateModel.Pedido.DataDeCriacao.ToString("dd/MM/yyyy"));
                    });

                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("Cliente: ").Style(titleStyle2);
                        text.Span(pedidoCreateModel.Pedido.Usuario.Nome).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Email: ").Style(titleStyle2);
                        text.Span(pedidoCreateModel.Pedido.Usuario.Email).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Telefone: ").Style(titleStyle2);
                        text.Span(pedidoCreateModel.Pedido.Usuario.Telefone).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("CNPJ: ").Style(titleStyle2);
                        text.Span(pedidoCreateModel.Pedido.Usuario.Cnpj).Style(titleStyleName);
                    });
                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("Número: ").Style(titleStyle2);
                        text.Span(pedidoCreateModel.Pedido.Numero.ToString()).Style(titleStyleName);
                    });
                });
                //row.ConstantItem(50).Height(50).Image(_imageData);

            });
        }

        static IContainer CellStyleHeaderTable(IContainer container)
        {
            return container
                .DefaultTextStyle(x => x.SemiBold())
                .PaddingVertical(5)
                .BorderBottom(1)
                .BorderColor(Colors.Black);
        }

        static IContainer CellTableStyle(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5);
        }

        try
        {
            var pdf = Document
                .Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Configurar();
                        page.Header().Element(HeaderCustom);
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var columnsWidth in _colunsWidt)
                                {
                                    columns.ConstantColumn(columnsWidth);
                                }
                            });

                            table.Header(header =>
                            {
                                foreach (var columnsName in _colunsName)
                                {
                                    header
                                        .Cell()
                                        .Element(CellStyleHeaderTable)
                                        .Text(columnsName)
                                        .FontSize(10);
                                }
                            });

                            pedidoCreateModel.Pedido.ItensPedido = [.. pedidoCreateModel.Pedido.ItensPedido.OrderBy(x => x.Numero)];
                            var itemsPedidosGroup = pedidoCreateModel.Pedido.ItensPedido.GroupBy(x => x.ProdutoId);

                            foreach (var itensGroup in itemsPedidosGroup)
                            {
                                foreach (var item in itensGroup)
                                {
                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Produto.Referencia)
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Produto.Descricao)
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Tamanho?.Descricao ?? item.Peso?.Descricao)
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.Quantidade.ToString().Replace(".", ","))
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.ValorUnitario.ToString().Replace(".", ","))
                                    .FontSize(8);

                                    table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text(item.ValorTotal.ToString().Replace(".", ","))
                                    .FontSize(8);
                                }
                            }

                            var tamamhosItens = pedidoCreateModel.Pedido
                                .ItensPedido
                                .OrderBy(x => x.Tamanho?.Numero)
                                .Select(x => x.Tamanho)
                                .ToList()
                                .GroupBy(x => x?.Id);

                            table
                                .Cell()
                                .Element(CellStyleHeaderTable)
                                .Text($"Quantidades")
                                .FontSize(10);

                            foreach (var tamanhoGroup in tamamhosItens)
                            {
                                table.Cell();
                                table.Cell();
                                table.Cell();
                                table.Cell();
                                table.Cell();
                                var itemPedido = pedidoCreateModel.Pedido
                                    .ItensPedido
                                    .FirstOrDefault(x => x.TamanhoId == tamanhoGroup.Key);

                                var totalQuantidade = pedidoCreateModel.Pedido
                                    .ItensPedido
                                    .Where(x => x.TamanhoId == tamanhoGroup.Key)
                                    .ToList()
                                    .Sum(x => x.Quantidade);

                                table
                                    .Cell()
                                    .Element(CellTableStyle)
                                    .Text($"{itemPedido?.Tamanho?.Descricao} : {totalQuantidade.ToString().Replace(".", ",")}")
                                    .FontSize(8);
                            }
                        });
                        page.FooterCustom();
                    });
                }).GeneratePdf();

            var message = $"Que ótima noticia, mais um pedido!\nN. do pedido : {pedidoCreateModel.Pedido.Numero}";
            var assunto = "Novo pedido";
            var emailModel = new EnvioEmailModel()
            {
                Assunto = assunto,
                Email = pedidoCreateModel.EmailEnvio,
                Mensagem = message,
                Arquivo = pdf,
                NomeDoArquivo = $"pedido-{pedidoCreateModel.Pedido.Numero}",
                TipoDoArquivo = "application/pdf"
            };

            _emailService.SendEmail(emailModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ao gerar pdf : {ex.Message}");
        }
    }
}
