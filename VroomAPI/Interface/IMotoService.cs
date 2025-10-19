using VroomAPI.Abstractions;
using VroomAPI.DTOs;
using VroomAPI.Helpers;

namespace VroomAPI.Interface {
    public interface IMotoService {
        Task<Result<MotoDto>> CreateMoto(CreateMotoDto createMotoDto);
        Task<Result<MotoDto>> GetMotoById(int id);
        Task<Result<PagedList<MotoDto>>> GetAllMotosPaged(int page, int pageSize);
        Task<Result<IEnumerable<MotoDto>>> GetAllMotos();
        Task<Result<MotoDto>> UpdateMoto(int id, UpdateMotoDto updateMotoDto);
        Task<Result> DeleteMoto(int id);
    }
}