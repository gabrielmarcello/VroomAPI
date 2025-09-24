using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para criação de uma nova tag
    /// </summary>
    public class CreateTagDto
    {
        /// <summary>
        /// Coordenadas geográficas da localização (latitude,longitude)
        /// </summary>
        /// <example>-23.5505,-46.6333</example>
        [Required(ErrorMessage = "A coordenada é obrigatória")]
        [StringLength(50, ErrorMessage = "A coordenada deve ter no máximo 50 caracteres")]
        public string Coordenada { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a tag está disponível (1) ou indisponível (0)
        /// </summary>
        /// <example>1</example>
        [Range(0, 1, ErrorMessage = "O valor deve ser 0 (indisponível) ou 1 (disponível)")]
        public byte Disponivel { get; set; }
    }
}