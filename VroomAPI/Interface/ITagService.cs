using VroomAPI.Abstractions;
using VroomAPI.Helpers;
using VroomAPI.Model;

namespace VroomAPI.Interface {
    public interface ITagService {
        Task<Result<Tag>> CreateTag(Tag tag);
        Task<Result<Tag>> GetTagById(int id);
        Task<Result<PagedList<Tag>>> GetAllTagsPaged(int page, int pageSize);
        Task<Result<Tag>> UpdateTag(Tag tag);
        Task<Result> DeleteTag(int id);
    }
}
