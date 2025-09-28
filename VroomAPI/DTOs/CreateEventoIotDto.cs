using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para cria��o de um novo evento IoT
    /// </summary>
    public class CreateEventoIotDto
    {
        /// <summary>
        /// Identificador da tag associada ao evento
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "O ID da tag � obrigat�rio")]
        public int IdTag { get; set; }

        /// <summary>
        /// Timestamp do evento
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        [Required(ErrorMessage = "O timestamp � obrigat�rio")]
        public string Timestamp { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o LED est� ligado
        /// </summary>
        /// <example>true</example>
        public bool LedOn { get; set; }

        /// <summary>
        /// Descri��o do problema detectado
        /// </summary>
        /// <example>Temperatura alta detectada</example>
        [StringLength(500, ErrorMessage = "A descri��o do problema deve ter no m�ximo 500 caracteres")]
        public string Problema { get; set; } = string.Empty;

        /// <summary>
        /// C�digo da cor do LED
        /// </summary>
        /// <example>1</example>
        [Range(0, 255, ErrorMessage = "O c�digo da cor deve estar entre 0 e 255")]
        public int Cor { get; set; }
    }
}