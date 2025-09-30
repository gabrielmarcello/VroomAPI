using VroomAPI.Model.Enum;
using VroomAPI.Models;

namespace VroomAPI.DTOs
{
    public class MotoDto : HateoasResource
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string DescricaoProblema { get; set; } = string.Empty;
        public ModeloMoto ModeloMoto { get; set; }
        public CategoriaProblema CategoriaProblema { get; set; }
        public int TagId { get; set; }
        public TagDto? Tag { get; set; }
    }
}