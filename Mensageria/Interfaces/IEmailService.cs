using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmail(EnvioEmailModel envioEmailModel);
}
