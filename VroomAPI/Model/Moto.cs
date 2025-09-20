using VroomAPI.Model.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VroomAPI.Model {
    /// <summary>
    /// Representa uma moto no sistema
    /// </summary>
    public class Moto {

        /// <summary>
        /// Identificador único da moto
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Placa da moto
        /// </summary>
        /// <example>BRA2E19</example>
        [Required(ErrorMessage = "A placa é obrigatória")]
        [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres")]
        public string Placa { get; set; } = string.Empty;
        
        /// <summary>
        /// Número do chassi da moto
        /// </summary>
        /// <example>9BWZZZ377VT004251</example>
        [Required(ErrorMessage = "O chassi é obrigatório")]
        [StringLength(17, ErrorMessage = "O chassi deve ter no máximo 17 caracteres")]
        public string Chassi { get; set; } = string.Empty;
        
        /// <summary>
        /// Descrição do problema identificado na moto
        /// </summary>
        /// <example>Motor fazendo ruído estranho ao acelerar</example>
        [Required(ErrorMessage = "A descrição do problema é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string DescricaoProblema { get; set; } = string.Empty;
        
        /// <summary>
        /// Modelo da moto
        /// </summary>
        /// <example>0</example>
        [Required(ErrorMessage = "O modelo da moto é obrigatório")]
        public ModeloMoto ModeloMoto { get; set; }
        
        /// <summary>
        /// Categoria do problema identificado
        /// </summary>
        /// <example>0</example>
        [Required(ErrorMessage = "A categoria do problema é obrigatória")]
        public CategoriaProblema CategoriaProblema { get; set; }
        
        /// <summary>
        /// ID da tag de localização associada
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "O ID da tag é obrigatório")]
        public int TagId { get; set; }
        
        /// <summary>
        /// Tag de localização associada à moto
        /// </summary>
        public Tag? Tag { get; set; }
    }
}
