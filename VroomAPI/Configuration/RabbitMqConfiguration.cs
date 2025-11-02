namespace VroomAPI.Configuration
{
    /// <summary>
    /// Configuração para conexão com RabbitMQ
    /// </summary>
    public class RabbitMqConfiguration
    {
        public const string SectionName = "RabbitMQ";
        
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string EventoIotQueueName { get; set; } = "evento_iot_queue";
        public string EventoIotExchangeName { get; set; } = "evento_iot_exchange";
        public string EventoIotRoutingKey { get; set; } = "evento_iot";
    }
}