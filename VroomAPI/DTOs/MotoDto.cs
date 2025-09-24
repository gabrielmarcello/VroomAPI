using VroomAPI.Model.Enum;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para retorno de dados da moto
    /// </summary>
    public class MotoDto
    {
        /// <summary>
        /// Identificador �nico da moto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Placa da moto
        /// </summary>
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// N�mero do chassi da moto
        /// </summary>
        public string Chassi { get; set; } = string.Empty;

        /// <summary>
        /// Descri��o do problema identificado na moto
        /// </summary>
        public string DescricaoProblema { get; set; } = string.Empty;

        /// <summary>
        /// Modelo da moto
        /// </summary>
        public ModeloMoto ModeloMoto { get; set; }

        /// <summary>
        /// Categoria do problema identificado
        /// </summary>
        public CategoriaProblema CategoriaProblema { get; set; }

        /// <summary>
        /// ID da tag de localiza��o associada
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Dados da tag associada
        /// </summary>
        public TagDto? Tag { get; set; }
    }
}