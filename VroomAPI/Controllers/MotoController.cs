using Microsoft.AspNetCore.Mvc;
using VroomAPI.DTOs;
using VroomAPI.Interface;
using System.ComponentModel.DataAnnotations;

namespace VroomAPI.Controllers {

    /// <summary>
    /// Controller responsável pelo gerenciamento de motos
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MotoController : ControllerBase {

        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService) {
            _motoService = motoService;
        }

        /// <summary>
        /// Cria uma nova moto no sistema
        /// </summary>
        /// <param name="createMotoDto">Dados da moto a ser criada</param>
        /// <returns>Retorna a moto criada com o ID gerado</returns>
        /// <response code="201">Moto criada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <example>
        /// Exemplo de payload:
        /// {
        ///   "placa": "ABC-1234",
        ///   "chassi": "9BWZZZ377VT004251",
        ///   "descricaoProblema": "Motor fazendo ruído estranho",
        ///   "modeloMoto": 0,
        ///   "categoriaProblema": 0,
        ///   "tagId": 1
        /// }
        /// </example>
        [HttpPost]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMoto([FromBody] CreateMotoDto createMotoDto) {
            var result = await _motoService.CreateMoto(createMotoDto);
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return CreatedAtAction(nameof(GetMotoById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Busca uma moto específica pelo ID
        /// </summary>
        /// <param name="id">ID único da moto</param>
        /// <returns>Retorna os dados da moto encontrada</returns>
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

            return Ok(result.Value);
        }

        /// <summary>
        /// Retorna todas as motos cadastradas no sistema
        /// </summary>
        /// <returns>Lista com todas as motos</returns>
        /// <response code="200">Lista de motos retornada com sucesso</response>
        /// <response code="400">Erro ao buscar motos</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMotos(int page = 1, int pageSize = 10) {
            var result = await _motoService.GetAllMotosPaged(page, pageSize);
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada</param>
        /// <param name="updateMotoDto">Novos dados da moto</param>
        /// <returns>Retorna a moto atualizada</returns>
        /// <response code="200">Moto atualizada com sucesso</response>
        /// <response code="404">Moto não encontrada</response>
        /// <example>
        /// Exemplo de payload:
        /// {
        ///   "placa": "XYZ-5678",
        ///   "chassi": "9BWZZZ377VT004251",
        ///   "descricaoProblema": "Problema no freio traseiro",
        ///   "modeloMoto": 1,
        ///   "categoriaProblema": 4,
        ///   "tagId": 2
        /// }
        /// </example>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMoto([FromRoute] [Required] int id, [FromBody] UpdateMotoDto updateMotoDto) {
            var result = await _motoService.UpdateMoto(id, updateMotoDto);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Remove uma moto do sistema
        /// </summary>
        /// <param name="id">ID da moto a ser removida</param>
        /// <returns>Confirmação de remoção</returns>
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
    }
}