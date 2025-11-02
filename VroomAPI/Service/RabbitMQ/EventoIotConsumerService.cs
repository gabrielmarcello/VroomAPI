using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using VroomAPI.Configuration;
using VroomAPI.DTOs;
using VroomAPI.Interface;

namespace VroomAPI.Service.RabbitMQ
{
    /// <summary>
    /// Serviço em background para consumir mensagens do RabbitMQ e processar eventos IoT
    /// </summary>
    public class EventoIotConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConfiguration _config;
        private readonly ILogger<EventoIotConsumerService> _logger;
        private IConnection _connection;
        private IChannel _channel;

        public EventoIotConsumerService(
            IServiceProvider serviceProvider,
            RabbitMqConfiguration config,
            ILogger<EventoIotConsumerService> logger)
        {
            _serviceProvider = serviceProvider;
            _config = config;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMqAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventoDto = JsonSerializer.Deserialize<CreateEventoIotDto>(message);

                    if (eventoDto != null)
                    {
                        await ProcessEventoIotAsync(eventoDto);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                        _logger.LogInformation("Evento IoT processado com sucesso: {IdTag}", eventoDto.IdTag);
                    }
                    else
                    {
                        _logger.LogWarning("Mensagem recebida é nula ou inválida");
                        await _channel.BasicRejectAsync(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem do RabbitMQ");
                    await _channel.BasicRejectAsync(ea.DeliveryTag, true);
                }
            };

            await _channel.BasicConsumeAsync(_config.EventoIotQueueName, false, consumer);

            _logger.LogInformation("EventoIotConsumerService iniciado e aguardando mensagens...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task InitializeRabbitMqAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(_config.EventoIotExchangeName, ExchangeType.Direct, durable: true);
            await _channel.QueueDeclareAsync(_config.EventoIotQueueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(_config.EventoIotQueueName, _config.EventoIotExchangeName, _config.EventoIotRoutingKey);

            await _channel.BasicQosAsync(0, 1, false);
        }

        private async Task ProcessEventoIotAsync(CreateEventoIotDto eventoDto)
        {
            using var scope = _serviceProvider.CreateScope();
            var iotService = scope.ServiceProvider.GetRequiredService<IIotService>();

            var result = await iotService.CreateEventoDirectly(eventoDto);
            
            if (result.IsFailure)
            {
                _logger.LogError("Falha ao salvar evento IoT no banco de dados: {Error}", result.Error.Description);
                throw new Exception($"Falha ao salvar evento IoT: {result.Error.Description}");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventoIotConsumerService parando...");
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}