using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Service.AutoMapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectCard, ProjectCardDTO>().ReverseMap();
            CreateMap<ProjectCard, ProjectPageDto>().ReverseMap();
            CreateMap<ProjectCard, ShortCardDto>().ReverseMap();
        } 
    }
}
