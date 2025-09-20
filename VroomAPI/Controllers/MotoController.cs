using Microsoft.AspNetCore.Mvc;
using VroomAPI.Interface;
using VroomAPI.Model;

namespace VroomAPI.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class MotoController : ControllerBase {

        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService) {
            _motoService = motoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMoto([FromBody] Moto moto) {
            var result = await _motoService.CreateMoto(moto);
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return CreatedAtAction(nameof(GetMotoById), new { id = result.Value.Id }, result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotoById([FromRoute] int id) {
            var result = await _motoService.GetMotoById(id);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMotos() {
            var result = await _motoService.GetAllMotos();
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMoto([FromRoute] int id, [FromBody] Moto moto) {
            if (id != moto.Id) {
                return BadRequest(new { error = "Os ids não coencidem", message = "O id fornecido não é o mesmo da moto fornecida" });
            }

            var result = await _motoService.UpdateMoto(moto);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto([FromRoute] int id) {
            var result = await _motoService.DeleteMoto(id);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return NoContent();
        }
    }
}