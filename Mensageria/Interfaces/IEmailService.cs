using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface IEmailService
{
    bool SendEmail(EnvioEmailModel envioEmailModel);
}
