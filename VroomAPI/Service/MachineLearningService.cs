using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using VroomAPI.Abstractions;
using VroomAPI.Data;
using VroomAPI.DTOs;
using VroomAPI.Interface;
using VroomAPI.ML.Models;
using VroomAPI.Model.Enum;

namespace VroomAPI.Service
{
    /// <summary>
    /// Serviço de Machine Learning para classificação de problemas em eventos IoT
    /// </summary>
    public class MachineLearningService : IMachineLearningService
    {
        private readonly AppDbContext _dbContext;
        private readonly MLContext _mlContext;
        private readonly string _modelPath;
        private ITransformer? _trainedModel;

        public MachineLearningService(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _mlContext = new MLContext(seed: 0);
            _modelPath = Path.Combine(environment.ContentRootPath, "ML", "Models", "problema_model.zip");
            
            // Carregar modelo se existir
            if (File.Exists(_modelPath))
            {
                _trainedModel = _mlContext.Model.Load(_modelPath, out _);
            }
        }

        public bool IsModelTrained()
        {
            return _trainedModel != null || File.Exists(_modelPath);
        }

        public async Task<Result> TrainModelAsync()
        {
            try
            {
                // Buscar dados históricos
                var eventos = await _dbContext.eventos.ToListAsync();

                if (eventos.Count < 10)
                {
                    return Result.Failure(new Error("INSUFFICIENT_DATA", 
                        "Dados insuficientes para treinamento. São necessários pelo menos 10 eventos."));
                }

                // Preparar dados para treinamento
                var trainingData = eventos.Select(e => new EventoIotData
                {
                    Cor = e.Cor,
                    LedOn = e.LedOn ? 1.0f : 0.0f,
                    ProblemaLength = string.IsNullOrEmpty(e.Problema) ? 0 : e.Problema.Length,
                    HourOfDay = ExtractHourFromTimestamp(e.Timestamp),
                    CategoriaProblema = ClassifyProblem(e)
                }).ToList();

                var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // Dividir dados em treino e teste
                var trainTestSplit = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Pipeline de treinamento
                var pipeline = _mlContext.Transforms.Conversion
                    .MapValueToKey("Label", "Label")
                    .Append(_mlContext.Transforms.Concatenate("Features", 
                        nameof(EventoIotData.Cor), 
                        nameof(EventoIotData.LedOn),
                        nameof(EventoIotData.ProblemaLength),
                        nameof(EventoIotData.HourOfDay)))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                // Treinar modelo
                _trainedModel = pipeline.Fit(trainTestSplit.TrainSet);

                // Criar diretório se não existir
                var modelDir = Path.GetDirectoryName(_modelPath);
                if (modelDir != null && !Directory.Exists(modelDir))
                {
                    Directory.CreateDirectory(modelDir);
                }

                // Salvar modelo
                _mlContext.Model.Save(_trainedModel, dataView.Schema, _modelPath);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("TRAINING_FAILED", 
                    $"Falha ao treinar modelo: {ex.Message}"));
            }
        }

        public async Task<Result<ProblemaPrediction>> PredictCategoriaAsync(PredictProblemaDto predictDto)
        {
            try
            {
                if (_trainedModel == null)
                {
                    if (File.Exists(_modelPath))
                    {
                        _trainedModel = _mlContext.Model.Load(_modelPath, out _);
                    }
                    else
                    {
                        return Result<ProblemaPrediction>.Failure(
                            new Error("MODEL_NOT_TRAINED", 
                                "Modelo não treinado. Execute o treinamento primeiro."));
                    }
                }

                var predictionEngine = _mlContext.Model
                    .CreatePredictionEngine<EventoIotData, ProblemaPrediction>(_trainedModel);

                var input = new EventoIotData
                {
                    Cor = predictDto.Cor,
                    LedOn = predictDto.LedOn ? 1.0f : 0.0f,
                    ProblemaLength = string.IsNullOrEmpty(predictDto.Problema) ? 0 : predictDto.Problema.Length,
                    HourOfDay = ExtractHourFromTimestamp(predictDto.Timestamp)
                };

                var prediction = predictionEngine.Predict(input);
                
                // Calcular confiança
                var maxScore = prediction.Score.Max();
                prediction.Confidence = maxScore;
                prediction.CategoryName = GetCategoryName(prediction.PredictedCategory);

                return await Task.FromResult(Result<ProblemaPrediction>.Success(prediction));
            }
            catch (Exception ex)
            {
                return Result<ProblemaPrediction>.Failure(
                    new Error("PREDICTION_FAILED", $"Falha ao realizar predição: {ex.Message}"));
            }
        }

        public async Task<Result<ModelMetrics>> GetModelMetricsAsync()
        {
            try
            {
                if (_trainedModel == null)
                {
                    if (File.Exists(_modelPath))
                    {
                        _trainedModel = _mlContext.Model.Load(_modelPath, out _);
                    }
                    else
                    {
                        return Result<ModelMetrics>.Failure(
                            new Error("MODEL_NOT_TRAINED", "Modelo não treinado."));
                    }
                }

                var eventos = await _dbContext.eventos.ToListAsync();
                
                var testData = eventos.Select(e => new EventoIotData
                {
                    Cor = e.Cor,
                    LedOn = e.LedOn ? 1.0f : 0.0f,
                    ProblemaLength = string.IsNullOrEmpty(e.Problema) ? 0 : e.Problema.Length,
                    HourOfDay = ExtractHourFromTimestamp(e.Timestamp),
                    CategoriaProblema = ClassifyProblem(e)
                }).ToList();

                var dataView = _mlContext.Data.LoadFromEnumerable(testData);
                var predictions = _trainedModel.Transform(dataView);
                
                var metrics = _mlContext.MulticlassClassification.Evaluate(predictions);

                var modelMetrics = new ModelMetrics
                {
                    MacroAccuracy = metrics.MacroAccuracy,
                    MicroAccuracy = metrics.MicroAccuracy,
                    LogLoss = metrics.LogLoss,
                    LogLossReduction = metrics.LogLossReduction,
                    TrainingSamples = eventos.Count
                };

                return Result<ModelMetrics>.Success(modelMetrics);
            }
            catch (Exception ex)
            {
                return Result<ModelMetrics>.Failure(
                    new Error("METRICS_FAILED", $"Falha ao obter métricas: {ex.Message}"));
            }
        }

        /// <summary>
        /// Classifica o problema com base em palavras-chave e características
        /// </summary>
        private uint ClassifyProblem(Model.EventoIot evento)
        {
            if (string.IsNullOrEmpty(evento.Problema))
                return (uint)CategoriaProblema.CONFORME;

            var problema = evento.Problema.ToLower();

            // Classificação baseada em palavras-chave
            if (problema.Contains("motor") || problema.Contains("freio") || problema.Contains("suspensão") || 
                problema.Contains("transmissão") || problema.Contains("corrente"))
                return (uint)CategoriaProblema.MECANICO;

            if (problema.Contains("bateria") || problema.Contains("elétrico") || problema.Contains("luz") || 
                problema.Contains("farol") || problema.Contains("fiação"))
                return (uint)CategoriaProblema.ELETRICO;

            if (problema.Contains("documento") || problema.Contains("licença") || problema.Contains("registro") || 
                problema.Contains("placa"))
                return (uint)CategoriaProblema.DOCUMENTACAO;

            if (problema.Contains("pintura") || problema.Contains("arranhão") || problema.Contains("estético") || 
                problema.Contains("aparência"))
                return (uint)CategoriaProblema.ESTETICO;

            if (problema.Contains("capacete") || problema.Contains("segurança") || problema.Contains("extintor"))
                return (uint)CategoriaProblema.SEGURANCA;

            // Se LED está ligado, provavelmente indica algum problema
            if (evento.LedOn && evento.Cor < 100)
                return (uint)CategoriaProblema.MULTIPLO;

            return (uint)CategoriaProblema.CONFORME;
        }

        /// <summary>
        /// Extrai a hora do timestamp
        /// </summary>
        private float ExtractHourFromTimestamp(string timestamp)
        {
            try
            {
                if (DateTime.TryParse(timestamp, out DateTime dt))
                {
                    return dt.Hour;
                }
                return 12; // Valor padrão
            }
            catch
            {
                return 12;
            }
        }

        /// <summary>
        /// Obtém o nome da categoria
        /// </summary>
        private string GetCategoryName(uint categoryId)
        {
            return categoryId switch
            {
                0 => "MECÂNICO",
                1 => "ELÉTRICO",
                2 => "DOCUMENTAÇÃO",
                3 => "ESTÉTICO",
                4 => "SEGURANÇA",
                5 => "MÚLTIPLO",
                6 => "CONFORME",
                _ => "DESCONHECIDO"
            };
        }
    }
}
