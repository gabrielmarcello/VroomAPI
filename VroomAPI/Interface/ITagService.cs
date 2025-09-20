using VroomAPI.Abstractions;
using VroomAPI.Model;

namespace VroomAPI.Interface {
    public interface ITagService {
        Task<Result<Tag>> CreateTag(Tag tag);
        Task<Result<Tag>> GetTagById(int id);
        Task<Result<IEnumerable<Tag>>> GetAllTags();
        Task<Result<Tag>> UpdateTag(Tag tag);
        Task<Result> DeleteTag(int id);
    }
}
