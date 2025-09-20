using Microsoft.AspNetCore.Mvc;
using VroomAPI.Interface;
using VroomAPI.Model;
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
        /// <param name="moto">Dados da moto a ser criada</param>
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
        [ProducesResponseType(typeof(Moto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMoto([FromBody] Moto moto) {
            var result = await _motoService.CreateMoto(moto);
            
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
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(IEnumerable<Moto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMotos() {
            var result = await _motoService.GetAllMotos();
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada</param>
        /// <param name="moto">Novos dados da moto</param>
        /// <returns>Retorna a moto atualizada</returns>
        /// <response code="200">Moto atualizada com sucesso</response>
        /// <response code="400">ID da rota não coincide com ID da moto</response>
        /// <response code="404">Moto não encontrada</response>
        /// <example>
        /// Exemplo de payload:
        /// {
        ///   "id": 1,
        ///   "placa": "XYZ-5678",
        ///   "chassi": "9BWZZZ377VT004251",
        ///   "descricaoProblema": "Problema no freio traseiro",
        ///   "modeloMoto": 1,
        ///   "categoriaProblema": 4,
        ///   "tagId": 2
        /// }
        /// </example>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Moto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMoto([FromRoute] [Required] int id, [FromBody] Moto moto) {
            if (id != moto.Id) {
                return BadRequest(new { error = "Os ids não coencidem", message = "O id fornecido não é o mesmo da moto fornecida" });
            }

            var result = await _motoService.UpdateMoto(moto);
            
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