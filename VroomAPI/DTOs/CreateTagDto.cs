using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para cria��o de uma nova tag
    /// </summary>
    public class CreateTagDto
    {
        /// <summary>
        /// Coordenadas geogr�ficas da localiza��o (latitude,longitude)
        /// </summary>
        /// <example>-23.5505,-46.6333</example>
        [Required(ErrorMessage = "A coordenada � obrigat�ria")]
        [StringLength(50, ErrorMessage = "A coordenada deve ter no m�ximo 50 caracteres")]
        public string Coordenada { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a tag est� dispon�vel (1) ou indispon�vel (0)
        /// </summary>
        /// <example>1</example>
        [Range(0, 1, ErrorMessage = "O valor deve ser 0 (indispon�vel) ou 1 (dispon�vel)")]
        public byte Disponivel { get; set; }
    }
}