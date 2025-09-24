using Microsoft.AspNetCore.Mvc;
using VroomAPI.Interface;
using VroomAPI.Model;
using System.ComponentModel.DataAnnotations;
using VroomAPI.DTOs;

namespace VroomAPI.Controllers {

    /// <summary>
    /// Controller responsável pelo gerenciamento de tags de localização
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TagController : ControllerBase {

        private readonly ITagService _tagService;

        public TagController(ITagService tagService) {
            _tagService = tagService;
        }

        /// <summary>
        /// Cria uma nova tag no sistema
        /// </summary>
        /// <param name="tag">Dados da tag a ser criada</param>
        /// <returns>Retorna a tag criada com o ID gerado</returns>
        /// <response code="201">Tag criada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <example>
        /// Exemplo de payload:
        /// {
        ///   "coordenada": "-23.5505,-46.6333",
        ///   "disponivel": 1
        /// }
        /// </example>
        [HttpPost]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto tag) {
            var result = await _tagService.CreateTag(tag);
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return CreatedAtAction(nameof(GetTagById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Busca uma tag específica pelo ID
        /// </summary>
        /// <param name="id">ID único da tag</param>
        /// <returns>Retorna os dados da tag encontrada</returns>
        /// <response code="200">Tag encontrada com sucesso</response>
        /// <response code="404">Tag não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTagById([FromRoute] [Required] int id) {
            var result = await _tagService.GetTagById(id);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retorna todas as tags cadastradas no sistema
        /// </summary>
        /// <returns>Lista com todas as tags</returns>
        /// <response code="200">Lista de tags retornada com sucesso</response>
        /// <response code="400">Erro ao buscar tags</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Tag>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTags(int page = 1, int pageSize = 10) {
            var result = await _tagService.GetAllTagsPaged(page, pageSize);
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Atualiza os dados de uma tag existente
        /// </summary>
        /// <param name="id">ID da tag a ser atualizada</param>
        /// <param name="tag">Novos dados da tag</param>
        /// <returns>Retorna a tag atualizada</returns>
        /// <response code="200">Tag atualizada com sucesso</response>
        /// <response code="400">ID da rota não coincide com ID da tag</response>
        /// <response code="404">Tag não encontrada</response>
        /// <example>
        /// Exemplo de payload:
        /// {
        ///   "id": 1,
        ///   "coordenada": "-23.5505,-46.6333",
        ///   "disponivel": 0
        /// }
        /// </example>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTag([FromRoute] [Required] int id, [FromBody] UpdateTagDto tag) {
            var result = await _tagService.UpdateTag(id, tag);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Remove uma tag do sistema
        /// </summary>
        /// <param name="id">ID da tag a ser removida</param>
        /// <returns>Confirmação de remoção</returns>
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
    }
}
