namespace VroomAPI.DTOs
{
    /// <summary>
    /// DTO de resposta com a predição de categoria de problema
    /// </summary>
    public class PredictProblemaResponseDto
    {
        public string PredictedCategory { get; set; } = string.Empty;
        public uint CategoryId { get; set; }
        public float Confidence { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
