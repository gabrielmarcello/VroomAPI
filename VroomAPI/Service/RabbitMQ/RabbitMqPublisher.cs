using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using VroomAPI.Configuration;
using VroomAPI.DTOs;

namespace VroomAPI.Service.RabbitMQ
{
    /// <summary>
    /// Interface para publicação de mensagens no RabbitMQ
    /// </summary>
    public interface IRabbitMqPublisher
    {
        Task PublishEventoIotAsync(CreateEventoIotDto eventoDto);
    }

    /// <summary>
    /// Serviço para publicação de mensagens no RabbitMQ
    /// </summary>
    public class RabbitMqPublisher : IRabbitMqPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly RabbitMqConfiguration _config;

        public RabbitMqPublisher(RabbitMqConfiguration config)
        {
            _config = config;
            
            var factory = new ConnectionFactory()
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(_config.EventoIotExchangeName, ExchangeType.Direct, durable: true).GetAwaiter().GetResult();
            _channel.QueueDeclareAsync(_config.EventoIotQueueName, durable: true, exclusive: false, autoDelete: false).GetAwaiter().GetResult();
            _channel.QueueBindAsync(_config.EventoIotQueueName, _config.EventoIotExchangeName, _config.EventoIotRoutingKey).GetAwaiter().GetResult();
        }

        public async Task PublishEventoIotAsync(CreateEventoIotDto eventoDto)
        {
            var message = JsonSerializer.Serialize(eventoDto);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await _channel.BasicPublishAsync(
                exchange: _config.EventoIotExchangeName,
                routingKey: _config.EventoIotRoutingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}