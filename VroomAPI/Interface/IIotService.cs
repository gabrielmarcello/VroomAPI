using VroomAPI.Abstractions;
using VroomAPI.DTOs;
using VroomAPI.Helpers;
using VroomAPI.Model;

namespace VroomAPI.Interface
{
    /// <summary>
    /// Interface para serviços de eventos IoT
    /// </summary>
    public interface IIotService
    {
        /// <summary>
        /// Cria um novo evento IoT
        /// </summary>
        /// <param name="createEventoDto">Dados para criação do evento</param>
        /// <returns>Resultado da operação com o evento criado</returns>
        Task<Result<EventoIotDto>> CreateEvento(CreateEventoIotDto createEventoDto);
        
        /// <summary>
        /// Cria um novo evento IoT diretamente no banco de dados (usado pelo consumer RabbitMQ)
        /// </summary>
        /// <param name="createEventoDto">Dados para criação do evento</param>
        /// <returns>Resultado da operação com o evento criado</returns>
        Task<Result<EventoIotDto>> CreateEventoDirectly(CreateEventoIotDto createEventoDto);
        
        Task<Result> SendCommandAsync(LedCommandDto command);
        Task<Result<PagedList<EventoIotDto>>> GetAllEventosPaged(int page, int pageSize);
    }
}