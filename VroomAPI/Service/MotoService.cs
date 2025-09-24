using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VroomAPI.Abstractions;
using VroomAPI.Data;
using VroomAPI.DTOs;
using VroomAPI.Helpers;
using VroomAPI.Interface;
using VroomAPI.Model;

namespace VroomAPI.Service {
    public class MotoService : IMotoService {

        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public MotoService(AppDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<MotoDto>> CreateMoto(CreateMotoDto createMotoDto) {
            try {
                var existingTag = await _dbContext.tags.FindAsync(createMotoDto.TagId);
                if (existingTag == null) {
                    return Result<MotoDto>.Failure(new Error("Tag não encontrada", "A Tag especificada não existe"));
                }

                var moto = _mapper.Map<Moto>(createMotoDto);
                
                _dbContext.motos.Add(moto);
                await _dbContext.SaveChangesAsync();
                
                await _dbContext.Entry(moto)
                    .Reference(m => m.Tag)
                    .LoadAsync();
                
                var motoDto = _mapper.Map<MotoDto>(moto);
                return Result<MotoDto>.Success(motoDto);
            }
            catch (Exception) {
                return Result<MotoDto>.Failure(new Error("Falha ao criar moto"));
            }
        }

        public async Task<Result<MotoDto>> GetMotoById(int id) {
            try {
                var moto = await _dbContext.motos
                    .Include(m => m.Tag)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (moto == null) {
                    return Result<MotoDto>.Failure(new Error("Moto não encontrada"));
                }
                
                var motoDto = _mapper.Map<MotoDto>(moto);
                return Result<MotoDto>.Success(motoDto);
            }
            catch (Exception) {
                return Result<MotoDto>.Failure(new Error("Falha ao buscar moto"));
            }
        }

        public async Task<Result<PagedList<MotoDto>>> GetAllMotosPaged(int page, int pageSize) {
            try {
                var query = _dbContext.motos.Include(m => m.Tag);
                var pagedMotos = await PagedList<Moto>.createAsync(query, page, pageSize);
                
                var motosDto = _mapper.Map<List<MotoDto>>(pagedMotos.Items);
                var pagedMotosDto = new PagedList<MotoDto>(motosDto, pagedMotos.Page, pagedMotos.PageSize, pagedMotos.TotalCount);
                
                return Result<PagedList<MotoDto>>.Success(pagedMotosDto);
            }
            catch (Exception) {
                return Result<PagedList<MotoDto>>.Failure(new Error("Falha ao buscar todas as motos"));
            }
        }

        public async Task<Result<MotoDto>> UpdateMoto(int id, UpdateMotoDto updateMotoDto) {
            try {
                var existingMoto = await _dbContext.motos.FindAsync(id);
                if (existingMoto == null) {
                    return Result<MotoDto>.Failure(new Error("Moto não encontrada"));
                }

                var existingTag = await _dbContext.tags.FindAsync(updateMotoDto.TagId);
                if (existingTag == null) {
                    return Result<MotoDto>.Failure(new Error("Tag não encontrada", "A Tag especificada não existe"));
                }

                _mapper.Map(updateMotoDto, existingMoto);

                await _dbContext.SaveChangesAsync();
                
                await _dbContext.Entry(existingMoto)
                    .Reference(m => m.Tag)
                    .LoadAsync();
                
                var motoDto = _mapper.Map<MotoDto>(existingMoto);
                return Result<MotoDto>.Success(motoDto);
            }
            catch (Exception) {
                return Result<MotoDto>.Failure(new Error("Falha ao atualizar moto"));
            }
        }

        public async Task<Result> DeleteMoto(int id) {
            try {
                var moto = await _dbContext.motos.FindAsync(id);
                if (moto == null) {
                    return Result.Failure(new Error("Moto não encontrada", "None"));
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