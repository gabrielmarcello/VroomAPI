using Microsoft.EntityFrameworkCore;
using VroomAPI.Abstractions;
using VroomAPI.Data;
using VroomAPI.Helpers;
using VroomAPI.Interface;
using VroomAPI.Model;

namespace VroomAPI.Service {
    public class TagService : ITagService{

        private readonly AppDbContext _dbContext;

        public TagService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Result<Tag>> CreateTag(Tag tag) {
            try {
                _dbContext.tags.Add(tag);
                await _dbContext.SaveChangesAsync();
                return Result<Tag>.Success(tag);
            }
            catch (Exception) {
                return Result<Tag>.Failure(new Error("Falha ao criar tag"));
            }
        }

        public async Task<Result<Tag>> GetTagById(int id) {
            try {
                var tag = await _dbContext.tags.FindAsync(id);
                if (tag == null) {
                    return Result<Tag>.Failure(new Error("Tag não encontrada"));
                }
                return Result<Tag>.Success(tag);
            }
            catch (Exception) {
                return Result<Tag>.Failure(new Error("Falha ao buscar tag"));
            }
        }

        public async Task<Result<PagedList<Tag>>> GetAllTagsPaged(int page, int pageSize) {
            try {
                var pagedTags = await PagedList<Tag>.createAsync(_dbContext.tags, page, pageSize);
                return Result<PagedList<Tag>>.Success(pagedTags);
            }
            catch (Exception) {
                return Result<PagedList<Tag>>.Failure(new Error("Falha ao buscar todas as tags"));
            }
        }

        public async Task<Result<Tag>> UpdateTag(Tag tag) {
            try {
                var existingTag = await _dbContext.tags.FindAsync(tag.Id);
                if (existingTag == null) {
                    return Result<Tag>.Failure(new Error("Tag não encontrada"));
                }

                existingTag.Coordenada = tag.Coordenada;
                existingTag.Disponivel = tag.Disponivel;

                await _dbContext.SaveChangesAsync();
                return Result<Tag>.Success(existingTag);
            }
            catch (Exception) {
                return Result<Tag>.Failure(new Error("Falha ao atualizar tag"));
            }
        }

        public async Task<Result> DeleteTag(int id) {
            try {
                var tag = await _dbContext.tags.FindAsync(id);
                if (tag == null) {
                    return Result.Failure(new Error("Tag não encontrada"));
                }

                _dbContext.tags.Remove(tag);
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception) {
                return Result.Failure(new Error("Falha ao deletar tag"));
            }
        }
    }
}
