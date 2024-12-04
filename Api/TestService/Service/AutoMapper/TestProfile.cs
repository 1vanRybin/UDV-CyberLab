using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Service.AutoMapper
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Test, TestDto>().ReverseMap();
            CreateMap<TestResult, UserTestResultDto>().ReverseMap();
        }
    }
}
