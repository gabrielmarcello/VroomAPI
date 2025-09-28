using VroomAPI.Abstractions;
using VroomAPI.DTOs;
using VroomAPI.Helpers;

namespace VroomAPI.Interface
{
    /// <summary>
    /// Interface para servi�os de eventos IoT
    /// </summary>
    public interface IEventoService
    {
        /// <summary>
        /// Cria um novo evento IoT
        /// </summary>
        /// <param name="createEventoDto">Dados para cria��o do evento</param>
        /// <returns>Resultado da opera��o com o evento criado</returns>
        Task<Result<EventoIotDto>> CreateEvento(CreateEventoIotDto createEventoDto);
        Task<Result<PagedList<EventoIotDto>>> GetAllEventosPaged(int page, int pageSize);
    }
}