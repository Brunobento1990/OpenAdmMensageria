using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Mensageria.Interfaces;
using Mensageria.Model;

namespace Mensageria.Mensageria.Consumers;

public class EditItemPedidoConsumer : BackgroundService
{
    private readonly IModel _channel;
    private const string ExchangeName = "pedido-editado";
    private readonly string _queueName;
    private readonly IServiceProvider _provider;

    public EditItemPedidoConsumer(IModel channel, IServiceProvider provider)
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

            if (headers.TryGetValue("Referer", out object? value))
            {
                var referer = Encoding.UTF8.GetString((byte[])value);

                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var notificacao = JsonSerializer.Deserialize<NotificarPedidoEditadoModel>(message);

                if (notificacao != null)
                {
                    using var scope = _provider.CreateScope();

                    try
                    {
                        var service = scope.ServiceProvider.GetRequiredService<INotificarPedidoEditadoService>();
                        await service.NotificarAsync(notificacao);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro rabbitMq movimentar o estoque: {ex.Message}");
                        throw new Exception(ex.Message);
                    }
                }
            }

            _channel.BasicAck(e.DeliveryTag, false);
        };

        _channel.BasicConsume(_queueName, false, consumer);

        return Task.CompletedTask;
    }
}
