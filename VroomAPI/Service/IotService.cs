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
using VroomAPI.Service.RabbitMQ;

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
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public IotService(AppDbContext dbContext, IMapper mapper, IHttpClientFactory httpClientFactory, IRabbitMqPublisher rabbitMqPublisher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        /// <summary>
        /// Cria um novo evento IoT enviando para RabbitMQ (processamento assíncrono)
        /// </summary>
        /// <param name="createEventoDto">Dados para criação do evento</param>
        /// <returns>Resultado da operação</returns>
        public async Task<Result<EventoIotDto>> CreateEvento(CreateEventoIotDto createEventoDto)
        {
            try
            {
                await _rabbitMqPublisher.PublishEventoIotAsync(createEventoDto);
                
                var eventoDto = new EventoIotDto
                {
                    Id = 0,
                    IdTag = createEventoDto.IdTag,
                    Timestamp = createEventoDto.Timestamp,
                    LedOn = createEventoDto.LedOn,
                    Problema = createEventoDto.Problema,
                    Cor = createEventoDto.Cor
                };
                
                return Result<EventoIotDto>.Success(eventoDto);
            }
            catch (Exception)
            {
                return Result<EventoIotDto>.Failure(new Error("PUBLISH_EVENTO_FAILED", $"Falha ao enviar evento para processamento"));
            }
        }

        /// <summary>
        /// Cria um novo evento IoT diretamente no banco de dados (usado pelo consumer RabbitMQ)
        /// </summary>
        /// <param name="createEventoDto">Dados para criação do evento</param>
        /// <returns>Resultado da operação com o evento criado</returns>
        public async Task<Result<EventoIotDto>> CreateEventoDirectly(CreateEventoIotDto createEventoDto)
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

        /// <summary>
        /// Envia um comando para o Node-RED
        /// </summary>
        /// <param name="command">Dados do comando a ser enviado</param>
        /// <returns>Resultado da operação</returns>
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

        /// <summary>
        /// Obtém uma lista paginada de eventos IoT
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns>Resultado com a lista paginada de eventos</returns>
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
