using Microsoft.EntityFrameworkCore;
using VroomAPI.Abstractions;
using VroomAPI.Data;
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
                _dbContext.motos.Add(moto);
                await _dbContext.SaveChangesAsync();
                return Result<Moto>.Success(moto);
            }
            catch (Exception ex) {
                return Result<Moto>.Failure(new Error("Falha ao criar moto", ex.Message));
            }
        }

        public async Task<Result<Moto>> GetMotoById(int id) {
            try {
                var moto = await _dbContext.motos
                    .Include(m => m.Tag)
                    .FirstOrDefaultAsync(m => m.Id == id);
                
                if (moto == null) {
                    return Result<Moto>.Failure(new Error("Moto não encontrada", $"Com com o {id} não encontrada"));
                }
                return Result<Moto>.Success(moto);
            }
            catch (Exception ex) {
                return Result<Moto>.Failure(new Error("Falha ao buscar moto", ex.Message));
            }
        }

        public async Task<Result<IEnumerable<Moto>>> GetAllMotos() {
            try {
                var motos = await _dbContext.motos
                    .Include(m => m.Tag)
                    .ToListAsync();
                return Result<IEnumerable<Moto>>.Success(motos);
            }
            catch (Exception ex) {
                return Result<IEnumerable<Moto>>.Failure(new Error("Falha ao buscar todas as motos", ex.Message));
            }
        }

        public async Task<Result<Moto>> UpdateMoto(Moto moto) {
            try {
                var existingMoto = await _dbContext.motos.FindAsync(moto.Id);
                if (existingMoto == null) {
                    return Result<Moto>.Failure(new Error("Moto não encontrada", $"Moto com o {moto.Id} não encontrada"));
                }

                existingMoto.Placa = moto.Placa;
                existingMoto.Chassi = moto.Chassi;
                existingMoto.DescricaoProblema = moto.DescricaoProblema;
                existingMoto.ModeloMoto = moto.ModeloMoto;
                existingMoto.CategoriaProblema = moto.CategoriaProblema;
                existingMoto.Tag = moto.Tag;

                await _dbContext.SaveChangesAsync();
                return Result<Moto>.Success(existingMoto);
            }
            catch (Exception ex) {
                return Result<Moto>.Failure(new Error("Falha ao atualizar moto", ex.Message));
            }
        }

        public async Task<Result> DeleteMoto(int id) {
            try {
                var moto = await _dbContext.motos.FindAsync(id);
                if (moto == null) {
                    return Result.Failure(new Error("Moto não encontrada", $"Moto com o {id} não encontrada"));
                }

                _dbContext.motos.Remove(moto);
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex) {
                return Result.Failure(new Error("Falha ao deletar moto", ex.Message));
            }
        }
    }
}