using System.ComponentModel.DataAnnotations;
using VroomAPI.Model.Enum;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para atualização de uma moto existente
    /// </summary>
    public class UpdateMotoDto
    {
        /// <summary>
        /// Placa da moto
        /// </summary>
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres")]
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// Número do chassi da moto
        /// </summary>
        [Required(ErrorMessage = "O chassi é obrigatório")]
        [StringLength(17, ErrorMessage = "O chassi deve ter no máximo 17 caracteres")]
        public string Chassi { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do problema identificado na moto
        /// </summary>
        [Required(ErrorMessage = "A descrição do problema é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string DescricaoProblema { get; set; } = string.Empty;

        /// <summary>
        /// Modelo da moto
        /// </summary>
        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public ModeloMoto ModeloMoto { get; set; }

        /// <summary>
        /// Categoria do problema identificado
        /// </summary>
        [Required(ErrorMessage = "A categoria do problema é obrigatória")]
        public CategoriaProblema CategoriaProblema { get; set; }

        /// <summary>
        /// ID da tag de localização associada
        /// </summary>
        [Required(ErrorMessage = "O ID da tag é obrigatório")]
        public int TagId { get; set; }
    }
}