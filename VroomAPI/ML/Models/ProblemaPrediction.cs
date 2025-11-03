using Microsoft.ML.Data;

namespace VroomAPI.ML.Models
{
    /// <summary>
    /// Modelo de predição de categoria de problema
    /// </summary>
    public class ProblemaPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedCategory { get; set; }

        [ColumnName("Score")]
        public float[] Score { get; set; } = Array.Empty<float>();

        public string CategoryName { get; set; } = string.Empty;
        
        public float Confidence { get; set; }
    }
}
