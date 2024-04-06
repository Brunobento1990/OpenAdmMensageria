using Domain.Pkg.Interfaces;
using Domain.Pkg.Model;
using Mensageria.Interfaces;
using Mensageria.Model;

namespace Mensageria.Service;

public sealed class NotificarPedidoEditadoService : INotificarPedidoEditadoService
{
    private readonly IEmailService _emailService;
    private readonly FromEnvioEmailModel _fromEmailModel;

    public NotificarPedidoEditadoService(IEmailService emailService)
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

    public async Task NotificarAsync(NotificarPedidoEditadoModel notificarPedidoEditadoModel)
    {
        var message = $"Pedido editado!\nN. do pedido : {notificarPedidoEditadoModel.Pedido.Numero}";
        var assunto = "Pedido editado!";

        var emailModel = new ToEnvioEmailModel()
        {
            Assunto = assunto,
            Email = notificarPedidoEditadoModel.EmailEnvio,
            Mensagem = message
        };

        await _emailService.SendEmail(emailModel, _fromEmailModel);
    }
}
