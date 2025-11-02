using VroomAPI.Abstractions;
using VroomAPI.DTOs;
using VroomAPI.Helpers;

namespace VroomAPI.Interface {
    public interface ITagService {
        Task<Result<TagDto>> CreateTag(CreateTagDto createTagDto);
        Task<Result<TagDto>> GetTagById(int id);
        Task<Result<PagedList<TagDto>>> GetAllTagsPaged(int page, int pageSize);
        Task<Result<IEnumerable<TagDto>>> GetAllTags();
        Task<Result<TagDto>> UpdateTag(int id, UpdateTagDto updateTagDto);
        Task<Result> DeleteTag(int id);
    }
}
