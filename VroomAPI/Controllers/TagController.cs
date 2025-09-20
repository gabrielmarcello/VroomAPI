using Microsoft.AspNetCore.Mvc;
using VroomAPI.Interface;
using VroomAPI.Model;

namespace VroomAPI.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase {

        private readonly ITagService _tagService;

        public TagController(ITagService tagService) {
            _tagService = tagService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] Tag tag) {
            var result = await _tagService.CreateTag(tag);
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return CreatedAtAction(nameof(GetTagById), new { id = result.Value.Id }, result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById([FromRoute] int id) {
            var result = await _tagService.GetTagById(id);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags() {
            var result = await _tagService.GetAllTags();
            
            if (result.IsFailure) {
                return BadRequest(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateTag([FromRoute] int id, [FromBody] Tag tag) {
            if (id != tag.Id) {
                return BadRequest(new { error = "Os ids não coencidem", message = "O id fornecido não é o mesmo da tag fornecida" });
            }

            var result = await _tagService.UpdateTag(tag);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return Ok(result.Value);
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteTag([FromRoute] int id) {
            var result = await _tagService.DeleteTag(id);
            
            if (result.IsFailure) {
                return NotFound(new { error = result.Error.Code, message = result.Error.Description });
            }

            return NoContent();
        }
    }
}
