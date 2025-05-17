using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Service.AutoMapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile(IFileManager fileManager)
        {
            CreateMap<ProjectCard, ProjectCardDTO>().ReverseMap();
            CreateMap<ProjectCard, ProjectPageDto>().ReverseMap();
            CreateMap<ProjectCard, ShortCardDto>().ReverseMap();

            CreateMap<ProjectCardUpdateDto, ProjectCard>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && (srcMember is not string || !string.IsNullOrWhiteSpace((string)srcMember))));

            CreateMap<ProjectCardUpdateDto, ProjectCard>()
                .ForMember(dest => dest.LogoPath, opt => opt.MapFrom(
                    new FilePathResolver(
                        fileManager,
                        src => src.LogoPhoto,
                        dest => dest.LogoPath,
                        dest => $"logo_{dest.Name}")))
                .ForMember(dest => dest.PhotoPath, opt => opt.MapFrom(
                    new FilePathResolver(
                        fileManager,
                        src => src.ProjectPhoto,
                        dest => dest.PhotoPath,
                        dest => $"photo_{dest.Name}")))
                .ForMember(dest => dest.DocumentationPath, opt => opt.MapFrom(
                    new FilePathResolver(
                        fileManager,
                        src => src.Documentation,
                        dest => dest.DocumentationPath,
                        dest => $"documentation_{dest.Name}")));
        }
    }
}
