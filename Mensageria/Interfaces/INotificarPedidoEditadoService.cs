using Mensageria.Model;

namespace Mensageria.Interfaces;

public interface INotificarPedidoEditadoService
{
    Task NotificarAsync(NotificarPedidoEditadoModel notificarPedidoEditadoModel);
}
