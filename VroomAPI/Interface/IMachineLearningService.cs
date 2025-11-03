using VroomAPI.Abstractions;
using VroomAPI.DTOs;
using VroomAPI.ML.Models;

namespace VroomAPI.Interface
{
    /// <summary>
    /// Interface para serviço de Machine Learning de eventos IoT
    /// </summary>
    public interface IMachineLearningService
    {
        /// <summary>
        /// Treina o modelo de ML com dados históricos
        /// </summary>
        /// <returns>Resultado da operação de treinamento</returns>
        Task<Result> TrainModelAsync();

        /// <summary>
        /// Prediz a categoria de um problema baseado em dados do evento
        /// </summary>
        /// <param name="predictDto">Dados para predição</param>
        /// <returns>Resultado com a predição</returns>
        Task<Result<ProblemaPrediction>> PredictCategoriaAsync(PredictProblemaDto predictDto);

        /// <summary>
        /// Verifica se o modelo está treinado
        /// </summary>
        /// <returns>True se o modelo existe, false caso contrário</returns>
        bool IsModelTrained();

        /// <summary>
        /// Obtém métricas do modelo treinado
        /// </summary>
        /// <returns>Resultado com as métricas do modelo</returns>
        Task<Result<ModelMetrics>> GetModelMetricsAsync();
    }
}
