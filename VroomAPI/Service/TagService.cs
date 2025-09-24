using Microsoft.EntityFrameworkCore;
using VroomAPI.Abstractions;
using VroomAPI.Data;
using VroomAPI.Helpers;
using VroomAPI.Interface;
using VroomAPI.Model;
using VroomAPI.DTOs;
using AutoMapper;

namespace VroomAPI.Service {
    public class TagService : ITagService{

        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TagService(AppDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<TagDto>> CreateTag(CreateTagDto createTagDto) {
            try {
                var tag = _mapper.Map<Tag>(createTagDto);
                _dbContext.tags.Add(tag);

                await _dbContext.SaveChangesAsync();
                
                var tagDto = _mapper.Map<TagDto>(tag);
                return Result<TagDto>.Success(tagDto);
            }
            catch (Exception) {
                return Result<TagDto>.Failure(new Error("Falha ao criar tag"));
            }
        }

        public async Task<Result<TagDto>> GetTagById(int id) {
            try {
                var tag = await _dbContext.tags.FindAsync(id);
                if (tag == null) {
                    return Result<TagDto>.Failure(new Error("Tag não encontrada"));
                }

                var tagDto = _mapper.Map<TagDto>(tag);

                return Result<TagDto>.Success(tagDto);
            }
            catch (Exception) {
                return Result<TagDto>.Failure(new Error("Falha ao buscar tag"));
            }
        }

        public async Task<Result<PagedList<TagDto>>> GetAllTagsPaged(int page, int pageSize) {
            try {
                var pagedTags = await PagedList<Tag>.createAsync(_dbContext.tags, page, pageSize);

                var tagsDto = _mapper.Map<List<TagDto>>(pagedTags.Items);
                var pagedTagsDto = new PagedList<TagDto>(tagsDto, pagedTags.Page, pagedTags.PageSize, pagedTags.TotalCount);

                return Result<PagedList<TagDto>>.Success(pagedTagsDto);
            }
            catch (Exception) {
                return Result<PagedList<TagDto>>.Failure(new Error("Falha ao buscar todas as tags"));
            }
        }

        public async Task<Result<TagDto>> UpdateTag(int id, UpdateTagDto updateTagDto) {
            try {
                var existingTag = await _dbContext.tags.FindAsync(id);
                if (existingTag == null) {
                    return Result<TagDto>.Failure(new Error("Tag não encontrada"));
                }

                _mapper.Map(updateTagDto, existingTag);
                await _dbContext.SaveChangesAsync();
                
                var tagDto = _mapper.Map<TagDto>(existingTag);
                return Result<TagDto>.Success(tagDto);
            }
            catch (Exception) {
                return Result<TagDto>.Failure(new Error("Falha ao atualizar tag"));
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
