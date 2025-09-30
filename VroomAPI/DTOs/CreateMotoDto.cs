using System.ComponentModel.DataAnnotations;
using VroomAPI.Model.Enum;

namespace VroomAPI.DTOs
{
    public class CreateMotoDto
    {
        [Required(ErrorMessage = "A placa � obrigat�ria")]
        [StringLength(8, ErrorMessage = "A placa deve ter no m�ximo 8 caracteres")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O chassi � obrigat�rio")]
        [StringLength(17, ErrorMessage = "O chassi deve ter no m�ximo 17 caracteres")]
        public string Chassi { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descri��o do problema � obrigat�ria")]
        [StringLength(500, ErrorMessage = "A descri��o deve ter no m�ximo 500 caracteres")]
        public string DescricaoProblema { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto � obrigat�rio")]
        public ModeloMoto ModeloMoto { get; set; }

        [Required(ErrorMessage = "A categoria do problema � obrigat�ria")]
        public CategoriaProblema CategoriaProblema { get; set; }

        [Required(ErrorMessage = "O ID da tag � obrigat�rio")]
        public int TagId { get; set; }
    }
}