using Domain.Pkg.Enum;
using Mensageria.Interfaces;
using Mensageria.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Mensageria.Mensageria.Consumers;

public class PedidoEntregueConsumer : BackgroundService
{
    private readonly IModel _channel;
    private const string ExchangeName = "pedido-entregue";
    private readonly string _queueName;
    private readonly IServiceProvider _provider;
    private const string _mensagemDeMovimentacaoEstoque = "Entrega de pedido!";

    public PedidoEntregueConsumer(IModel channel, IServiceProvider provider)
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
                var pedidoCreateModel = JsonSerializer.Deserialize<PedidoCreateModel>(message);

                if (pedidoCreateModel != null)
                {
                    using var scope = _provider.CreateScope();

                    try
                    {
                        var service = scope.ServiceProvider.GetRequiredService<IMovimentacaoDeProdutoService>();
                        var itensMovimentos = pedidoCreateModel
                            .Pedido
                            .ItensPedido
                            .Select(x => new MovimentacaoDeEstoqueDto()
                            {
                                TipoMovimentacaoDeProduto = TipoMovimentacaoDeProduto.Saida,
                                Observacao = _mensagemDeMovimentacaoEstoque,
                                PesoId = x.PesoId,
                                ProdutoId = x.ProdutoId,
                                Quantidade = x.Quantidade,
                                TamanhoId = x.TamanhoId
                            }).ToList();
                        await service.MovimentarProdutosAsync(itensMovimentos, referer);
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
