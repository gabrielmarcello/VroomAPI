namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para representação de um evento IoT
    /// </summary>
    public class EventoIotDto
    {
        /// <summary>
        /// Identificador único do evento
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da tag associada ao evento
        /// </summary>
        /// <example>1</example>
        public int IdTag { get; set; }

        /// <summary>
        /// Timestamp do evento
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        public string Timestamp { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o LED está ligado
        /// </summary>
        /// <example>true</example>
        public bool LedOn { get; set; }

        /// <summary>
        /// Descrição do problema detectado
        /// </summary>
        /// <example>Temperatura alta detectada</example>
        public string Problema { get; set; } = string.Empty;

        /// <summary>
        /// Código da cor do LED
        /// </summary>
        /// <example>1</example>
        public int Cor { get; set; }
    }
}