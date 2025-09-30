using Microsoft.AspNetCore.Mvc;
using VroomAPI.Interface;
using System.ComponentModel.DataAnnotations;
using VroomAPI.DTOs;
using VroomAPI.Helpers;

namespace VroomAPI.Controllers {

    [ApiController]
    [Route("[controller]")]
    [Tags("Tags")]
    [Produces("application/json")]
    public class TagController : ControllerBase {

        private readonly ITagService _tagService;

        public TagController(ITagService tagService) {
            _tagService = tagService;
        }

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
            
            tag.AddSelfLink(baseUrl, "Tag", tag.Id);
            tag.AddCollectionLink(baseUrl, "Tag");
        }

        private void AddCollectionLinks(PagedResponse<TagDto> response, int page, int pageSize)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            
            response.AddSelfLink($"{baseUrl}/Tag?page={page}&pageSize={pageSize}");
            
            if (response.HasNext) { 
                response.AddLink($"{baseUrl}/Tag?page={page + 1}&pageSize={pageSize}", "next");
            }

            if (response.HasPrevious) { 
                response.AddLink($"{baseUrl}/Tag?page={page - 1}&pageSize={pageSize}", "prev");
            }
        }
    }
}
