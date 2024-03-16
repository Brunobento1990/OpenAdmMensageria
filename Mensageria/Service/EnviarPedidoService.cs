using Domain.Pkg.Interfaces;
using Domain.Pkg.Model;
using Domain.Pkg.Pdfs.Services;
using Mensageria.Interfaces;
using Mensageria.Model;
using System.Text;

namespace Mensageria.Service;

public class EnviarPedidoService : IEnviarPedidoService
{
    private readonly IEmailService _emailService;
    private readonly FromEnvioEmailModel _fromEmailModel;

    public EnviarPedidoService(IEmailService emailService)
    {
        _fromEmailModel = new FromEnvioEmailModel()
        {
            Email = VariaveisDeAmbiente.GetVariavel("EMAIL"),
            Servidor = VariaveisDeAmbiente.GetVariavel("SERVER"),
            Senha = VariaveisDeAmbiente.GetVariavel("SENHA"),
            Porta = int.Parse(VariaveisDeAmbiente.GetVariavel("PORT"))
        };
        _emailService = emailService;
    }
    public async Task EnviarPdfAsync(PedidoCreateModel pedidoCreateModel)
    {
        try
        {
            var logo = pedidoCreateModel.Logo != null ? Encoding.UTF8.GetString(pedidoCreateModel.Logo) : null;
            var pdf = PedidoPdfService.GeneratePdfAsync(pedidoCreateModel.Pedido, logo);

            var message = $"Que ótima noticia, mais um pedido!\nN. do pedido : {pedidoCreateModel.Pedido.Numero}";
            var assunto = "Novo pedido";

            var emailModel = new ToEnvioEmailModel()
            {
                Assunto = assunto,
                Email = pedidoCreateModel.EmailEnvio,
                Mensagem = message,
                Arquivo = pdf,
                NomeDoArquivo = $"pedido-{pedidoCreateModel.Pedido.Numero}",
                TipoDoArquivo = "application/pdf"
            };

            await _emailService.SendEmail(emailModel, _fromEmailModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ao gerar pdf : {ex.Message}");
        }
    }
}
