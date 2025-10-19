using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VroomAPI.Authentication;
using VroomAPI.DTOs;
using VroomAPI.Helpers;
using VroomAPI.Interface;

namespace VroomAPI.Controllers {

    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("1.0", Deprecated = true)]
    [Tags("Motos")]
    [Produces("application/json")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class MotoController : ControllerBase {

        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService) {
            _motoService = motoService;
        }

        /// <summary>
        /// Cria uma nova moto no sistema
        /// </summary>
        /// <param name="createMotoDto">Dados para criação da moto incluindo placa, modelo, ano</param>
        /// <returns>Moto criada com sucesso</returns>
        /// <response code="201">Moto criada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMoto([FromBody] CreateMotoDto createMotoDto) {
            var result = await _motoService.CreateMoto(createMotoDto);
            
            if (result.IsFailure) { 
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return CreatedAtAction(nameof(GetMotoById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Busca uma moto específica pelo ID
        /// </summary>
        /// <param name="id">ID único da moto a ser buscada</param>
        /// <returns>Moto encontrada</returns>
        /// <response code="200">Moto encontrada com sucesso</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMotoById([FromRoute] [Required] int id) {
            var result = await _motoService.GetMotoById(id);
            
            if (result.IsFailure) { 
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return Ok(result.Value);
        }

        /// <summary>
        /// Lista todas as motos com paginação
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
        /// <returns>Lista paginada de motos</returns>
        /// <response code="200">Lista de motos retornada com sucesso</response>
        /// <response code="400">Parâmetros de paginação inválidos</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<MotoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMotos(int page = 1, int pageSize = 10) {
            var result = await _motoService.GetAllMotosPaged(page, pageSize);
            
            if (result.IsFailure) { 
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            var response = CreatePagedResponse(result.Value, page, pageSize);
            AddCollectionLinks(response, page, pageSize);

            return Ok(response);
        }
        [HttpGet]
        [MapToApiVersion(1.0)]
        public async Task<IActionResult> GetAllMotosV1()
        {
            var result = await _motoService.GetAllMotos();

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Atualiza uma moto existente
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada</param>
        /// <param name="updateMotoDto">Dados atualizados da moto</param>
        /// <returns>Moto atualizada</returns>
        /// <response code="200">Moto atualizada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMoto([FromRoute] [Required] int id, [FromBody] UpdateMotoDto updateMotoDto) {
            var result = await _motoService.UpdateMoto(id, updateMotoDto);
            
            if (result.IsFailure) { 
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return Ok(result.Value);
        }

        /// <summary>
        /// Remove uma moto do sistema
        /// </summary>
        /// <param name="id">ID da moto a ser removida</param>
        /// <returns>Confirmação da remoção</returns>
        /// <response code="204">Moto removida com sucesso</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMoto([FromRoute] [Required] int id) {
            var result = await _motoService.DeleteMoto(id);
            
            if (result.IsFailure) { 
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return NoContent();
        }

        private PagedResponse<MotoDto> CreatePagedResponse(PagedList<MotoDto> pagedList, int page, int pageSize)
        {
            var response = new PagedResponse<MotoDto>
            {
                Data = pagedList.Items,
                CurrentPage = pagedList.Page,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                TotalCount = pagedList.TotalCount,
                HasNext = pagedList.hasNextPage,
                HasPrevious = pagedList.hasPreviousPage
            };

            foreach (var moto in response.Data) { 
                AddHateoasLinks(moto);
            }

            return response;
        }

        private void AddHateoasLinks(MotoDto moto)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            
            moto.AddSelfLink(baseUrl, $"v{version}/Moto", moto.Id);
            moto.AddCollectionLink(baseUrl, $"v{version}/Moto");
        }

        private void AddCollectionLinks(PagedResponse<MotoDto> response, int page, int pageSize)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            
            response.AddSelfLink($"{baseUrl}/v{version}/Moto?page={page}&pageSize={pageSize}");
            
            if (response.HasNext)
                response.AddLink($"{baseUrl}/v{version}/Moto?page={page + 1}&pageSize={pageSize}", "next");
            
            if (response.HasPrevious)
                response.AddLink($"{baseUrl}/v{version}/Moto?page={page - 1}&pageSize={pageSize}", "prev");
        }
    }
}