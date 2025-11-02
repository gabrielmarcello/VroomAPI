using Asp.Versioning;
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
    [Tags("Tags")]
    [Produces("application/json")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class TagController : ControllerBase {

        private readonly ITagService _tagService;

        public TagController(ITagService tagService) {
            _tagService = tagService;
        }

        /// <summary>
        /// Cria uma nova tag no sistema
        /// </summary>
        /// <param name="tag">Dados para criação da tag contendo coordenada e disponibilidade</param>
        /// <returns>Tag criada com sucesso</returns>
        /// <response code="201">Tag criada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto tag) {
            var result = await _tagService.CreateTag(tag);
            
            if (result.IsFailure) { 
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return CreatedAtAction(nameof(GetTagById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Busca uma tag específica pelo ID
        /// </summary>
        /// <param name="id">ID único da tag a ser buscada</param>
        /// <returns>Tag encontrada</returns>
        /// <response code="200">Tag encontrada com sucesso</response>
        /// <response code="404">Tag não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTagById([FromRoute] [Required] int id) {
            var result = await _tagService.GetTagById(id);
            
            if (result.IsFailure) { 
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return Ok(result.Value);
        }

        /// <summary>
        /// Lista todas as tags com paginação
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
        /// <returns>Lista paginada de tags</returns>
        /// <response code="200">Lista de tags retornada com sucesso</response>
        /// <response code="400">Parâmetros de paginação inválidos</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<TagDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTags(int page = 1, int pageSize = 10) {
            var result = await _tagService.GetAllTagsPaged(page, pageSize);
            
            if (result.IsFailure) { 
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }
            var response = CreatePagedResponse(result.Value, page, pageSize);
            AddCollectionLinks(response, page, pageSize);

            return Ok(response);
        }

        [HttpGet]
        [MapToApiVersion(1.0)]
        public async Task<IActionResult> GetAllTagsV1()
        {
            var result = await _tagService.GetAllTags();

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Atualiza uma tag existente
        /// </summary>
        /// <param name="id">ID da tag a ser atualizada</param>
        /// <param name="tag">Dados atualizados da tag</param>
        /// <returns>Tag atualizada</returns>
        /// <response code="200">Tag atualizada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Tag não encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTag([FromRoute] [Required] int id, [FromBody] UpdateTagDto tag) {
            var result = await _tagService.UpdateTag(id, tag);
            
            if (result.IsFailure) { 
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            AddHateoasLinks(result.Value);
            return Ok(result.Value);
        }

        /// <summary>
        /// Remove uma tag do sistema
        /// </summary>
        /// <param name="id">ID da tag a ser removida</param>
        /// <returns>Confirmação da remoção</returns>
        /// <response code="204">Tag removida com sucesso</response>
        /// <response code="404">Tag não encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTag([FromRoute] [Required] int id) {
            var result = await _tagService.DeleteTag(id);
            
            if (result.IsFailure) { 
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return NoContent();
        }

        private PagedResponse<TagDto> CreatePagedResponse(PagedList<TagDto> pagedList, int page, int pageSize)
        {
            var response = new PagedResponse<TagDto>
            {
                Data = pagedList.Items,
                CurrentPage = pagedList.Page,
                PageSize = pagedList.PageSize,
                TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                TotalCount = pagedList.TotalCount,
                HasNext = pagedList.hasNextPage,
                HasPrevious = pagedList.hasPreviousPage
            };

            foreach (var tag in response.Data) { 
                AddHateoasLinks(tag);
            }

            return response;
        }

        private void AddHateoasLinks(TagDto tag)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            
            tag.AddSelfLink(baseUrl, $"v{version}/Tag", tag.Id);
            tag.AddCollectionLink(baseUrl, $"v{version}/Tag");
        }

        private void AddCollectionLinks(PagedResponse<TagDto> response, int page, int pageSize)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "2.0";
            
            response.AddSelfLink($"{baseUrl}/v{version}/Tag?page={page}&pageSize={pageSize}");
            
            if (response.HasNext) { 
                response.AddLink($"{baseUrl}/v{version}/Tag?page={page + 1}&pageSize={pageSize}", "next");
            }

            if (response.HasPrevious) { 
                response.AddLink($"{baseUrl}/v{version}/Tag?page={page - 1}&pageSize={pageSize}", "prev");
            }
        }
    }
}
