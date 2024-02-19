using RabbitMQ.Client;

namespace Mensageria.Mensageria;

public class ConnectionBase
{
    public static IConnection InitConnection()
    {
        ConnectionFactory factory = new()
        {
            Uri = new Uri(VariaveisDeAmbiente.GetVariavel("MENSAGERIA_URI"))
        };

        return factory.CreateConnection();
    }
}
