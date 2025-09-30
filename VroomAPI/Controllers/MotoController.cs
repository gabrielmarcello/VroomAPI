using Microsoft.AspNetCore.Mvc;
using VroomAPI.DTOs;
using VroomAPI.Interface;
using System.ComponentModel.DataAnnotations;
using VroomAPI.Helpers;

namespace VroomAPI.Controllers {

    [ApiController]
    [Route("[controller]")]
    [Tags("Motos")]
    [Produces("application/json")]
    public class MotoController : ControllerBase {

        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService) {
            _motoService = motoService;
        }

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
            
            moto.AddSelfLink(baseUrl, "Moto", moto.Id);
            moto.AddCollectionLink(baseUrl, "Moto");
        }

        private void AddCollectionLinks(PagedResponse<MotoDto> response, int page, int pageSize)
        {
            var baseUrl = HateoasHelper.GetBaseUrl(HttpContext);
            
            response.AddSelfLink($"{baseUrl}/Moto?page={page}&pageSize={pageSize}");
            
            if (response.HasNext)
                response.AddLink($"{baseUrl}/Moto?page={page + 1}&pageSize={pageSize}", "next");
            
            if (response.HasPrevious)
                response.AddLink($"{baseUrl}/Moto?page={page - 1}&pageSize={pageSize}", "prev");
        }
    }
}