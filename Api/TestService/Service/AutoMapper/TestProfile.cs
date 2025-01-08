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
            
            CreateMap<UserTest, UserTestResultDto>().ReverseMap();
        }
    }
}
