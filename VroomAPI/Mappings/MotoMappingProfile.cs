using AutoMapper;
using VroomAPI.DTOs;
using VroomAPI.Model;

namespace VroomAPI.Mappings
{
    /// <summary>
    /// Profile de mapeamento do AutoMapper para entidades
    /// </summary>
    public class MotoMappingProfile : Profile
    {
        public MotoMappingProfile()
        {
            CreateMap<Moto, MotoDto>();
            CreateMap<CreateMotoDto, Moto>();
            CreateMap<UpdateMotoDto, Moto>();
            
            CreateMap<Tag, TagDto>();
            CreateMap<CreateTagDto, Tag>();
            CreateMap<UpdateTagDto, Tag>();
        }
    }
}