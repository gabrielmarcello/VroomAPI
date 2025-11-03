using System.ComponentModel.DataAnnotations;

namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO para solicitação de predição de categoria de problema usando ML.NET
    /// </summary>
    public class PredictProblemaDto
    {
        [Range(0, 255, ErrorMessage = "O código da cor deve estar entre 0 e 255")]
        public int Cor { get; set; }

        public bool LedOn { get; set; }

        [StringLength(500, ErrorMessage = "A descrição do problema deve ter no máximo 500 caracteres")]
        public string Problema { get; set; } = string.Empty;

        [Required(ErrorMessage = "O timestamp é obrigatório")]
        public string Timestamp { get; set; } = string.Empty;
    }
}
