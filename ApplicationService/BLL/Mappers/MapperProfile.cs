using ApplicationService.BLL.Models.ExamModels;
using ApplicationService.DAL.Entities;
using AutoMapper;

namespace ApplicationService.BLL.Mappers
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<ExamRequest, Exam>();
            CreateMap<Exam, ExamResponse>();
            
        }
    }
}
