using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VroomAPI.Abstractions;
using VroomAPI.Data;
using VroomAPI.DTOs;
using VroomAPI.Helpers;
using VroomAPI.Interface;
using VroomAPI.Model;

namespace VroomAPI.Service
{
    /// <summary>
    /// Serviço para gerenciamento de eventos IoT
    /// </summary>
    public class EventoService : IEventoService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventoService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um novo evento IoT
        /// </summary>
        /// <param name="createEventoDto">Dados para criação do evento</param>
        /// <returns>Resultado da operação com o evento criado</returns>
        public async Task<Result<EventoIotDto>> CreateEvento(CreateEventoIotDto createEventoDto)
        {
            try
            {
                var evento = _mapper.Map<EventoIot>(createEventoDto);
                
                _dbContext.eventos.Add(evento);
                await _dbContext.SaveChangesAsync();
                
                var eventoDto = _mapper.Map<EventoIotDto>(evento);
                return Result<EventoIotDto>.Success(eventoDto);
            }
            catch (Exception)
            {
                return Result<EventoIotDto>.Failure(new Error("CREATE_EVENTO_FAILED", $"Falha ao criar evento"));
            }
        }

        public async Task<Result<PagedList<EventoIotDto>>> GetAllEventosPaged(int page, int pageSize)
        {
            try
            {
                var pagedEventos = await PagedList<EventoIot>.createAsync(_dbContext.eventos, page, pageSize);

                var eventosDto = _mapper.Map<List<EventoIotDto>>(pagedEventos.Items);
                var pagedTagsDto = new PagedList<EventoIotDto>(eventosDto, pagedEventos.Page, pagedEventos.PageSize, pagedEventos.TotalCount);

                return Result<PagedList<EventoIotDto>>.Success(pagedTagsDto);
            }
            catch (Exception)
            {
                return Result<PagedList<EventoIotDto>>.Failure(new Error("Falha ao buscar histórico"));
            }
        }
    }
}
