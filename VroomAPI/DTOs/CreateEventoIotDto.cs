using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para criação de um novo evento IoT
    /// </summary>
    public class CreateEventoIotDto
    {
        /// <summary>
        /// Identificador da tag associada ao evento
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "O ID da tag é obrigatório")]
        public int IdTag { get; set; }

        /// <summary>
        /// Timestamp do evento
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [Required(ErrorMessage = "O timestamp é obrigatório")]
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
        [StringLength(500, ErrorMessage = "A descrição do problema deve ter no máximo 500 caracteres")]
        public string Problema { get; set; } = string.Empty;

        /// <summary>
        /// Código da cor do LED
        /// </summary>
        /// <example>1</example>
        [Range(0, 255, ErrorMessage = "O código da cor deve estar entre 0 e 255")]
        public int Cor { get; set; }
    }
}