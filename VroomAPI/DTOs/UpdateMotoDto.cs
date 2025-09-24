using System.ComponentModel.DataAnnotations;
using VroomAPI.Model.Enum;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para atualiza��o de uma moto existente
    /// </summary>
    public class UpdateMotoDto
    {
        /// <summary>
        /// Placa da moto
        /// </summary>
        [Required(ErrorMessage = "A placa � obrigat�ria")]
        [StringLength(8, ErrorMessage = "A placa deve ter no m�ximo 8 caracteres")]
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// N�mero do chassi da moto
        /// </summary>
        [Required(ErrorMessage = "O chassi � obrigat�rio")]
        [StringLength(17, ErrorMessage = "O chassi deve ter no m�ximo 17 caracteres")]
        public string Chassi { get; set; } = string.Empty;

        /// <summary>
        /// Descri��o do problema identificado na moto
        /// </summary>
        [Required(ErrorMessage = "A descri��o do problema � obrigat�ria")]
        [StringLength(500, ErrorMessage = "A descri��o deve ter no m�ximo 500 caracteres")]
        public string DescricaoProblema { get; set; } = string.Empty;

        /// <summary>
        /// Modelo da moto
        /// </summary>
        [Required(ErrorMessage = "O modelo da moto � obrigat�rio")]
        public ModeloMoto ModeloMoto { get; set; }

        /// <summary>
        /// Categoria do problema identificado
        /// </summary>
        [Required(ErrorMessage = "A categoria do problema � obrigat�ria")]
        public CategoriaProblema CategoriaProblema { get; set; }

        /// <summary>
        /// ID da tag de localiza��o associada
        /// </summary>
        [Required(ErrorMessage = "O ID da tag � obrigat�rio")]
        public int TagId { get; set; }
    }
}