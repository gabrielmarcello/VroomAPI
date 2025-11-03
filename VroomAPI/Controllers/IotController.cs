using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using VroomAPI.Authentication;
using VroomAPI.DTOs;
using VroomAPI.Model;
using VroomAPI.Helpers;
using VroomAPI.Interface;

namespace VroomAPI.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("1.0", Deprecated = true)]
    [Tags("IoT")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class IotController : ControllerBase
    {
        private readonly IIotService _eventoService;
        private readonly IMachineLearningService _mlService;

        public IotController(IIotService eventoService, IMachineLearningService mlService)
        {
            _eventoService = eventoService;
            _mlService = mlService;
        }

        /// <summary>
        /// Recebe e registra eventos IoT no histórico do sistema de forma assíncrona usando RabbitMQ
        /// </summary>
        /// <param name="createEventoDto">Dados do evento IoT contendo informações da tag e coordenadas</param>
        /// <returns>Confirmação de que o evento foi aceito para processamento</returns>
        /// <response code="202">Evento IoT aceito para processamento assíncrono</response>
        /// <response code="400">Dados inválidos fornecidos ou erro de validação</response>
        [HttpPost("historico")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RecebeIot([FromBody] CreateEventoIotDto createEventoDto)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            var result = await _eventoService.CreateEvento(createEventoDto);

            if (result.IsFailure) {
                return BadRequest(new { message = result.Error.Description });
            }

            return Accepted(new { 
                message = "Evento IoT aceito e enviado para processamento assíncrono",
                idTag = createEventoDto.IdTag,
                timestamp = createEventoDto.Timestamp
            });
        }

        /// <summary>
        /// Lista todos os eventos IoT registrados no sistema com paginação
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
        /// <returns>Lista paginada de eventos IoT</returns>
        /// <response code="200">Lista de eventos IoT retornada com sucesso</response>
        /// <response code="400">Parâmetros de paginação inválidos</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<EventoIotDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllEventos(int page = 1, int pageSize = 10)
        {
            var result = await _eventoService.GetAllEventosPaged(page, pageSize);

            if (result.IsFailure) { 
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            var response = CreatePagedResponse(result.Value);
            AddCollectionLinks(response, page, pageSize);

            return Ok(response);
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetLed([FromBody] LedCommandDto command)
        {
            var result = await _eventoService.SendCommandAsync(command);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok("Comando enviado!");
        }

        /// <summary>
        /// Treina o modelo de Machine Learning com dados históricos de eventos IoT
        /// </summary>
        /// <returns>Confirmação do treinamento do modelo</returns>
        /// <response code="200">Modelo treinado com sucesso</response>
        /// <response code="400">Erro durante o treinamento ou dados insuficientes</response>
        [HttpPost("ml/train")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> TrainModel()
        {
            var result = await _mlService.TrainModelAsync();

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(new { message = "Modelo treinado com sucesso!" });
        }

        /// <summary>
        /// Prediz a categoria de um problema usando ML.NET baseado em dados do evento IoT
        /// </summary>
        /// <param name="predictDto">Dados do evento para predição</param>
        /// <returns>Categoria predita e nível de confiança</returns>
        /// <response code="200">Predição realizada com sucesso</response>
        /// <response code="400">Erro na predição ou modelo não treinado</response>
        [HttpPost("ml/predict")]
        [ProducesResponseType(typeof(PredictProblemaResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PredictCategoria([FromBody] PredictProblemaDto predictDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mlService.PredictCategoriaAsync(predictDto);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            var prediction = result.Value;
            var response = new PredictProblemaResponseDto
            {
                PredictedCategory = prediction.CategoryName,
                CategoryId = prediction.PredictedCategory,
                Confidence = prediction.Confidence,
                Message = $"Categoria predita: {prediction.CategoryName} com {prediction.Confidence:P2} de confiança"
            };

            return Ok(response);
        }

        /// <summary>
        /// Obtém métricas do modelo de Machine Learning treinado
        /// </summary>
        /// <returns>Métricas de performance do modelo</returns>
        /// <response code="200">Métricas retornadas com sucesso</response>
        /// <response code="400">Modelo não treinado ou erro ao obter métricas</response>
        [HttpGet("ml/metrics")]
        [ProducesResponseType(typeof(ModelMetrics), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetModelMetrics()
        {
            var result = await _mlService.GetModelMetricsAsync();

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Verifica o status do modelo de Machine Learning
        /// </summary>
        /// <returns>Status indicando se o modelo está treinado</returns>
        /// <response code="200">Status do modelo retornado</response>
        [HttpGet("ml/status")]
        [ProducesResponseType(200)]
        public IActionResult GetModelStatus()
        {
            var isTrained = _mlService.IsModelTrained();
            
            return Ok(new 
            { 
                modelTrained = isTrained,
                message = isTrained 
                    ? "Modelo está treinado e pronto para uso" 
                    : "Modelo não treinado. Execute POST /ml/train primeiro"
            });
        }

        private PagedResponse<EventoIotDto> CreatePagedResponse(PagedList<EventoIotDto> pagedList)
        {
            var response = new PagedResponse<EventoIotDto>
            {
                Data = pagedList.Items,
                CurrentPage = pagedList.Page,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                TotalCount = pagedList.TotalCount,
                HasNext = pagedList.hasNextPage,
                HasPrevious = pagedList.hasPreviousPage
            };

            foreach (var evento in response.Data)
                AddHateoasLinks(evento);

            return response;
        }

        private void AddHateoasLinks(EventoIotDto evento)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            
            evento.AddSelfLink(baseUrl, $"v{version}/historico", evento.Id);
            evento.AddCollectionLink(baseUrl, $"v{version}/Iot");
        }

        private void AddCollectionLinks(PagedResponse<EventoIotDto> response, int page, int pageSize)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            
            response.AddSelfLink($"{baseUrl}/v{version}/Iot?page={page}&pageSize={pageSize}");
            
            if (response.HasNext)
                response.AddLink($"{baseUrl}/v{version}/Iot?page={page + 1}&pageSize={pageSize}", "next");
            
            if (response.HasPrevious)
                response.AddLink($"{baseUrl}/v{version}/Iot?page={page - 1}&pageSize={pageSize}", "prev");
        }
    }
}
