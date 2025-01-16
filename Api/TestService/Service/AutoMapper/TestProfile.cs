using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Service.AutoMapper
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Test, TestDto>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore()) // Исключаем Questions из маппинга
                .ReverseMap()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<UserTest, UserTestResultDto>()
                .ForMember(dest => dest.MaxPoints, opt
                    => opt.MapFrom(src => src.Test != null ? (float?)src.Test.MaxPoints : null)).ReverseMap();

            CreateMap<UserTest, ShortTestDto>()
           .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.TestId))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Test.Name))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Test.Description))
           .ForMember(dest => dest.Theme, opt => opt.MapFrom(src => src.Test.Theme))
           .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Test.Difficulty))
           .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Test.OwnerId))
           .ForMember(dest => dest.StartTestTime, opt => opt.MapFrom(src => src.Test.StartTestTime))
           .ForMember(dest => dest.EndTestTime, opt => opt.MapFrom(src => src.Test.EndTestTime))
           .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
           .ForMember(dest => dest.AttemptNumber, opt => opt.MapFrom(src => src.AttemptNumber))
           .ForMember(dest => dest.MaxPoints, opt => opt.MapFrom(src => src.Test.MaxPoints))
           .ForMember(dest => dest.LeftAttemptsCount, opt => opt.MapFrom(src => src.LeftAttemptsCount))
           .ForMember(dest => dest.ScoredPoints, opt => opt.MapFrom(src => src.ScoredPoints))
           .ForMember(dest => dest.PassTestTime, opt => opt.MapFrom(src => src.Test.PassTestTime))
           .ForMember(dest => dest.LeftTestTime, opt => opt.MapFrom(src => src.LeftTestTime))
           .ForMember(dest => dest.IsChecked, opt => opt.MapFrom(src => src.IsChecked));
        }
    }
}
