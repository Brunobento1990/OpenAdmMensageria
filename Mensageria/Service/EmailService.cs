using Mensageria.Interfaces;
using Mensageria.Model;
using System.Net.Mail;
using System.Net;

namespace Mensageria.Service;

public class EmailService : IEmailService
{
    private readonly string _email;
    private readonly string _server;
    private readonly string _senha;
    private readonly int _port;

    public EmailService()
    {
        _email = VariaveisDeAmbiente.GetVariavel("EMAIL");
        _server = VariaveisDeAmbiente.GetVariavel("SERVER");
        _senha = VariaveisDeAmbiente.GetVariavel("SENHA");
        _port = int.Parse(VariaveisDeAmbiente.GetVariavel("PORT"));
    }

    public bool SendEmail(EnvioEmailModel envioEmailModel)
    {
        try
        {
            var mail = new MailMessage(_email, envioEmailModel.Email)
            {
                Subject = envioEmailModel.Assunto,
                SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8"),
                BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8"),
                Body = envioEmailModel.Mensagem
            };

            if (envioEmailModel.Arquivo != null && !string.IsNullOrWhiteSpace(envioEmailModel.NomeDoArquivo) && !string.IsNullOrWhiteSpace(envioEmailModel.TipoDoArquivo))
            {
                var anexo = new Attachment(new MemoryStream(envioEmailModel.Arquivo), envioEmailModel.NomeDoArquivo, envioEmailModel.TipoDoArquivo);
                mail.Attachments.Add(anexo);
            }

            var smtp = new SmtpClient(_server, _port);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_email, _senha);
            smtp.Send(mail);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
