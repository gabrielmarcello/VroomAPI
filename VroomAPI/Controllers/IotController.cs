using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VroomAPI.DTOs;
using VroomAPI.Interface;
using VroomAPI.Model;
using VroomAPI.Service;

namespace VroomAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de eventos IoT
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class IotController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public IotController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        /// <summary>
        /// Recebe e processa um evento IoT
        /// </summary>
        /// <param name="createEventoDto">Dados do evento IoT</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Evento processado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Tag não encontrada</response>
        [HttpPost("/historico")]
        [ProducesResponseType(typeof(EventoIotDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RecebeIot([FromBody] CreateEventoIotDto createEventoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventoService.CreateEvento(createEventoDto);

            if (result.IsFailure)
            {
                if (result.Error.Code == "TAG_NOT_FOUND")
                {
                    return NotFound(new { message = result.Error.Description });
                }
                return BadRequest(new { message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventos(int page = 1, int pageSize = 10)
        {
            var result = await _eventoService.GetAllEventosPaged(page, pageSize);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }
    }
}
