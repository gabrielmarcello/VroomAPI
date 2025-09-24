namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para dados da tag
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Identificador único da tag
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Coordenadas geográficas da localização
        /// </summary>
        public string Coordenada { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a tag está disponível
        /// </summary>
        public byte Disponivel { get; set; }
    }
}