using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    public class CreateEventoIotDto
    {
        [Required(ErrorMessage = "O ID da tag � obrigat�rio")]
        public int IdTag { get; set; }

        [Required(ErrorMessage = "O timestamp � obrigat�rio")]
        public string Timestamp { get; set; } = string.Empty;

        public bool LedOn { get; set; }

        [StringLength(500, ErrorMessage = "A descri��o do problema deve ter no m�ximo 500 caracteres")]
        public string Problema { get; set; } = string.Empty;

        [Range(0, 255, ErrorMessage = "O c�digo da cor deve estar entre 0 e 255")]
        public int Cor { get; set; }
    }
}