using System.ComponentModel.DataAnnotations;

namespace VroomAPI.Model
{
    public class EventoIot
    {
        [Key]
        public int Id { get; set; }
        
        public string IdTag { get; set; }
        
        public string Timestamp { get; set; }
        public bool LedOn { get; set; }
        public string Problema { get; set; }
        public int Cor { get; set; }
    }
}
