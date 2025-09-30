using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    public class CreateTagDto
    {
        [Required(ErrorMessage = "A coordenada � obrigat�ria")]
        [StringLength(50, ErrorMessage = "A coordenada deve ter no m�ximo 50 caracteres")]
        public string Coordenada { get; set; } = string.Empty;

        [Range(0, 1, ErrorMessage = "O valor deve ser 0 (indispon�vel) ou 1 (dispon�vel)")]
        public byte Disponivel { get; set; }
    }
}