using AutoMapper;
using Domain.DTO.Questions;
using Domain.Entities;

namespace Service.AutoMapper;

public class QuestionsProfile : Profile
{
    public QuestionsProfile() 
    { 
        CreateMap<QuestionCompliance, QuestionComplianceDto>().ReverseMap();
        CreateMap<QuestionFile, QuestionFileDto>().ReverseMap();
        CreateMap<QuestionOpen, QuestionOpenDto>().ReverseMap();
        CreateMap<QuestionVariant, QuestionVariantDto>().ReverseMap();
    }
}