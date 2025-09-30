using VroomAPI.Models;

namespace VroomAPI.DTOs
{
    public class TagDto : HateoasResource
    {
        public int Id { get; set; }
        public string Coordenada { get; set; } = string.Empty;
        public byte Disponivel { get; set; }
    }
}