using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    public class LedCommandDto
    {
        [Required(ErrorMessage = "O id da tag é obrigatório")]
        public int IdTag { get; set; }

        [Required(ErrorMessage = "A cor da tag é obrigatória")]
        public int Color { get; set; }
    }
}
