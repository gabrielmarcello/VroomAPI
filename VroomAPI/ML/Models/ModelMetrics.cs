namespace VroomAPI.ML.Models
{
    /// <summary>
    /// Métricas do modelo de ML
    /// </summary>
    public class ModelMetrics
    {
        public double MacroAccuracy { get; set; }
        public double MicroAccuracy { get; set; }
        public double LogLoss { get; set; }
        public double LogLossReduction { get; set; }
        public int TrainingSamples { get; set; }
    }
}
