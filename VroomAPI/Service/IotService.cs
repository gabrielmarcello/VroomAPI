using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    public class IotService : IIotService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public IotService(AppDbContext dbContext, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
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
                return Result<EventoIotDto>.Failure(new Error("CREATE_EVENTO_FAILED", "Falha ao criar evento"));
            }
        }

        public async Task<Result> SendCommandAsync(LedCommandDto command)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var nodeRedUrl = "http://localhost:1880/led";
            var response = await client.PostAsync(nodeRedUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure(new Error("SEND_EVENTO_FAILED", "Falha ao enviar evento"));
            }

            return Result.Success();
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
