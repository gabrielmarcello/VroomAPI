using VroomAPI.Model;

namespace VroomAPI.DTOs
{
    public class EventoIotDto : HateoasResource
    {
        public int Id { get; set; }
        public string IdTag { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public bool LedOn { get; set; }
        public string Problema { get; set; } = string.Empty;
        public int Cor { get; set; }
    }
}