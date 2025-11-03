using Microsoft.ML.Data;

namespace VroomAPI.ML.Models
{
    /// <summary>
    /// Modelo de dados de entrada para treinamento e predição ML
    /// </summary>
    public class EventoIotData
    {
        [LoadColumn(0)]
        public float Cor { get; set; }

        [LoadColumn(1)]
        public float LedOn { get; set; }

        [LoadColumn(2)]
        public float ProblemaLength { get; set; }

        [LoadColumn(3)]
        public float HourOfDay { get; set; }

        [LoadColumn(4)]
        [ColumnName("Label")]
        public uint CategoriaProblema { get; set; }
    }
}
