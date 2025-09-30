using VroomAPI.Models;

namespace VroomAPI.DTOs
{
    public class EventoIotDto : HateoasResource
    {
        public int Id { get; set; }
        public int IdTag { get; set; }
        public string Timestamp { get; set; } = string.Empty;
        public bool LedOn { get; set; }
        public string Problema { get; set; } = string.Empty;
        public int Cor { get; set; }
    }
}