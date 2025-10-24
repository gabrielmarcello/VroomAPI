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
        private readonly IEventoService _eventoService;
        private readonly IHttpClientFactory _httpClientFactory;

        public IotController(IEventoService eventoService, IHttpClientFactory httpClientFactory)
        {
            _eventoService = eventoService;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Recebe e registra eventos IoT no histórico do sistema
        /// </summary>
        /// <param name="createEventoDto">Dados do evento IoT contendo informações da tag e coordenadas</param>
        /// <returns>Evento IoT registrado com sucesso</returns>
        /// <response code="200">Evento IoT registrado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos ou erro de validação</response>
        /// <response code="404">Tag especificada não foi encontrada</response>
        [HttpPost("historico")]
        [ProducesResponseType(typeof(EventoIotDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RecebeIot([FromBody] CreateEventoIotDto createEventoDto)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            var result = await _eventoService.CreateEvento(createEventoDto);

            if (result.IsFailure) {
                return result.Error.Code == "TAG_NOT_FOUND" 
                    ? NotFound(new { message = result.Error.Description })
                    : BadRequest(new { message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return Ok(result.Value);
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

            var response = CreatePagedResponse(result.Value, page, pageSize);
            AddCollectionLinks(response, page, pageSize);

            return Ok(response);
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetLed([FromBody] LedCommand command)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var nodeRedUrl = "http://localhost:1880/led";
            var response = await client.PostAsync(nodeRedUrl, content);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erro ao enviar comando");

            return Ok("Comando enviado!");
        }

        private PagedResponse<EventoIotDto> CreatePagedResponse(PagedList<EventoIotDto> pagedList, int page, int pageSize)
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
