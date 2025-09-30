using System.ComponentModel.DataAnnotations;
using VroomAPI.Model.Enum;

namespace VroomAPI.DTOs
{
    public class CreateMotoDto
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O chassi é obrigatório")]
        [StringLength(17, ErrorMessage = "O chassi deve ter no máximo 17 caracteres")]
        public string Chassi { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do problema é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string DescricaoProblema { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public ModeloMoto ModeloMoto { get; set; }

        [Required(ErrorMessage = "A categoria do problema é obrigatória")]
        public CategoriaProblema CategoriaProblema { get; set; }

        [Required(ErrorMessage = "O ID da tag é obrigatório")]
        public int TagId { get; set; }
    }
}