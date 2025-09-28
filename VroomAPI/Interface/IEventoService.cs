using VroomAPI.Abstractions;
using VroomAPI.DTOs;
using VroomAPI.Helpers;

namespace VroomAPI.Interface
{
    /// <summary>
    /// Interface para serviços de eventos IoT
    /// </summary>
    public interface IEventoService
    {
        /// <summary>
        /// Cria um novo evento IoT
        /// </summary>
        /// <param name="createEventoDto">Dados para criação do evento</param>
        /// <returns>Resultado da operação com o evento criado</returns>
        Task<Result<EventoIotDto>> CreateEvento(CreateEventoIotDto createEventoDto);
        Task<Result<PagedList<EventoIotDto>>> GetAllEventosPaged(int page, int pageSize);
    }
}