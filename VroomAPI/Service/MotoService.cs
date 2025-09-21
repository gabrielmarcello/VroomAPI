using Microsoft.EntityFrameworkCore;
using VroomAPI.Abstractions;
using VroomAPI.Data;
using VroomAPI.Helpers;
using VroomAPI.Interface;
using VroomAPI.Model;

namespace VroomAPI.Service {
    public class MotoService : IMotoService {

        private readonly AppDbContext _dbContext;

        public MotoService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Result<Moto>> CreateMoto(Moto moto) {
            try {
                var existingTag = await _dbContext.tags.FindAsync(moto.TagId);
                if (existingTag == null) {
                    return Result<Moto>.Failure(new Error("Tag não encontrada", "A Tag especificada não existe"));
                }

                moto.Tag = null;
                
                _dbContext.motos.Add(moto);
                await _dbContext.SaveChangesAsync();
                
                await _dbContext.Entry(moto)
                    .Reference(m => m.Tag)
                    .LoadAsync();
                
                return Result<Moto>.Success(moto);
            }
            catch (Exception) {
                return Result<Moto>.Failure(new Error("Falha ao criar moto"));
            }
        }

        public async Task<Result<Moto>> GetMotoById(int id) {
            try {
                var moto = await _dbContext.motos
                    .Include(m => m.Tag)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (moto == null) {
                    return Result<Moto>.Failure(new Error("Moto não encontrada"));
                }
                return Result<Moto>.Success(moto);
            }
            catch (Exception) {
                return Result<Moto>.Failure(new Error("Falha ao buscar moto"));
            }
        }

        public async Task<Result<PagedList<Moto>>> GetAllMotosPaged(int page, int pageSize) {
            try {
                var query = _dbContext.motos.Include(m => m.Tag);
                var pagedMotos = await PagedList<Moto>.createAsync(query, page, pageSize);
                return Result<PagedList<Moto>>.Success(pagedMotos);
            }
            catch (Exception) {
                return Result<PagedList<Moto>>.Failure(new Error("Falha ao buscar todas as motos"));
            }
        }

        public async Task<Result<Moto>> UpdateMoto(Moto moto) {
            try {
                var existingMoto = await _dbContext.motos.FindAsync(moto.Id);
                if (existingMoto == null) {
                    return Result<Moto>.Failure(new Error("Moto não encontrada"));
                }

                var existingTag = await _dbContext.tags.FindAsync(moto.TagId);
                if (existingTag == null) {
                    return Result<Moto>.Failure(new Error("Tag não encontrada", "A Tag especificada não existe"));
                }

                existingMoto.Placa = moto.Placa;
                existingMoto.Chassi = moto.Chassi;
                existingMoto.DescricaoProblema = moto.DescricaoProblema;
                existingMoto.ModeloMoto = moto.ModeloMoto;
                existingMoto.CategoriaProblema = moto.CategoriaProblema;
                existingMoto.TagId = moto.TagId;

                await _dbContext.SaveChangesAsync();
                
                await _dbContext.Entry(existingMoto)
                    .Reference(m => m.Tag)
                    .LoadAsync();
                
                return Result<Moto>.Success(existingMoto);
            }
            catch (Exception) {
                return Result<Moto>.Failure(new Error("Falha ao atualizar moto"));
            }
        }

        public async Task<Result> DeleteMoto(int id) {
            try {
                var moto = await _dbContext.motos.FindAsync(id);
                if (moto == null) {
                    return Result.Failure(new Error("Moto não encontrada"));
                }

                _dbContext.motos.Remove(moto);
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception) {
                return Result.Failure(new Error("Falha ao deletar moto"));
            }
        }
    }
}