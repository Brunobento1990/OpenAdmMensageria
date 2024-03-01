using Domain.Pkg.Pdfs.Services;
using Mensageria.Interfaces;
using Mensageria.Model;

namespace Mensageria.Service;

public class EnviarPedidoService : IEnviarPedidoService
{
    private readonly IEmailService _emailService;

    public EnviarPedidoService(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public void EnviarPdf(PedidoCreateModel pedidoCreateModel)
    {
        try
        {
            var pdf = PedidoPdfService.GeneratePdfAsync(pedidoCreateModel.Pedido);

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
