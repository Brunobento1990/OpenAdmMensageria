using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Mensageria.Model;
using Mensageria.Interfaces;

namespace Mensageria.Mensageria.Consumers;

public class PedidoCreatePdfConsumer : BackgroundService
{
    private readonly IModel _channel;
    private const string ExchangeName = "pedido-create";
    private readonly string _queueName;
    private readonly IServiceProvider _provider;

    public PedidoCreatePdfConsumer(IModel channel, IServiceProvider provider)
    {
        _provider = provider;
        _channel = channel;
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(_queueName, ExchangeName, "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, e) =>
        {
            var headers = e.BasicProperties.Headers;

            if (headers.ContainsKey("Referer"))
            {
                var refererHeaderValue = Encoding.UTF8.GetString((byte[])headers["Referer"]);
                Console.WriteLine($"Referer: {refererHeaderValue}");
            }
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var pedido = JsonSerializer.Deserialize<PedidoCreateModel>(message);

            if (pedido != null)
            {
                using var scope = _provider.CreateScope();

                try
                {
                    var pedidoService = scope.ServiceProvider.GetRequiredService<IPedidoPdfService>();
                    pedidoService.GeneratePdfAsync(pedido);
                    var cached = scope.ServiceProvider.GetRequiredService<ICachedService>();
                    await cached.RemoveCachedAsync(pedido.Pedido.UsuarioId.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro rabbitMq adicionar pedido : {ex.Message}");
                    throw new Exception(ex.Message);
                }
            }

            _channel.BasicAck(e.DeliveryTag, false);
        };

        _channel.BasicConsume(_queueName, false, consumer);

        return Task.CompletedTask;
    }
}
