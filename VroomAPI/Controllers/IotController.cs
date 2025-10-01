using Microsoft.AspNetCore.Mvc;
using VroomAPI.DTOs;
using VroomAPI.Interface;
using VroomAPI.Helpers;

namespace VroomAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Tags("IoT")]
    public class IotController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public IotController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        /// <summary>
        /// Recebe e registra eventos IoT no histórico do sistema
        /// </summary>
        /// <param name="createEventoDto">Dados do evento IoT contendo informações da tag e coordenadas</param>
        /// <returns>Evento IoT registrado com sucesso</returns>
        /// <response code="200">Evento IoT registrado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos ou erro de validação</response>
        /// <response code="404">Tag especificada não foi encontrada</response>
        [HttpPost("/historico")]
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
            
            evento.AddSelfLink(baseUrl, "historico", evento.Id);
            evento.AddCollectionLink(baseUrl, "Iot");
        }

        private void AddCollectionLinks(PagedResponse<EventoIotDto> response, int page, int pageSize)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            
            response.AddSelfLink($"{baseUrl}/Iot?page={page}&pageSize={pageSize}");
            
            if (response.HasNext)
                response.AddLink($"{baseUrl}/Iot?page={page + 1}&pageSize={pageSize}", "next");
            
            if (response.HasPrevious)
                response.AddLink($"{baseUrl}/Iot?page={page - 1}&pageSize={pageSize}", "prev");
        }
    }
}
