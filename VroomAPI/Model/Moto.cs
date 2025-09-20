using VroomAPI.Model.Enum;

namespace VroomAPI.Model {
    public class Moto {

        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string DescricaoProblema { get; set; } = string.Empty;
        public ModeloMoto ModeloMoto { get; set; }
        public CategoriaProblema CategoriaProblema { get; set; }
        public Tag Tag { get; set; }
    }
}
