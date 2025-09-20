using VroomAPI.Abstractions;
using VroomAPI.Model;

namespace VroomAPI.Interface {
    public interface IMotoService {
        Task<Result<Moto>> CreateMoto(Moto moto);
        Task<Result<Moto>> GetMotoById(int id);
        Task<Result<IEnumerable<Moto>>> GetAllMotos();
        Task<Result<Moto>> UpdateMoto(Moto moto);
        Task<Result> DeleteMoto(int id);
    }
}