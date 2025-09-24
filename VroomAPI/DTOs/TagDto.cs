namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para dados da tag
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Identificador �nico da tag
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Coordenadas geogr�ficas da localiza��o
        /// </summary>
        public string Coordenada { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a tag est� dispon�vel
        /// </summary>
        public byte Disponivel { get; set; }
    }
}