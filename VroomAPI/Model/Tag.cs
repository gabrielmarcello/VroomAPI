using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VroomAPI.Model {
    public class Tag {

        public int Id { get; set; }
        
        [Required(ErrorMessage = "A coordenada é obrigatória")]
        [StringLength(50, ErrorMessage = "A coordenada deve ter no máximo 50 caracteres")]
        public string Coordenada { get; set; } = string.Empty;

        [Range(0, 1, ErrorMessage = "O valor deve ser 0 (indisponível) ou 1 (disponível)")]
        public byte Disponivel { get; set; }

    }
}
