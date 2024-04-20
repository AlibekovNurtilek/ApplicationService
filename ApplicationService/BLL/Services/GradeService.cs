using ApplicationService.BLL.Models.GradeModels;
using ApplicationService.BLL.Models.Responses;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.BLL.Services
{
    public interface IGradeService
    {
        Task<ApiResponse> CreateGrade(string empId, GradeRequest model);
        Task<ApiResponse> UpdateGrade(int id, GradeRequest model);

        Task<ApiResponse> DeleteGrade(int id);
        Task<GradeResponse> GetSingleGrade(int id);
        Task<List<GradeResponse>> GetAllGrade();
    }
    public class GradeService : IGradeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAmazonService _amazonService;

        public GradeService(AppDbContext context, IMapper mapper, IAmazonService amazonService)
        {
            _context = context;
            _mapper = mapper;
            _amazonService = amazonService;

        }

        public async Task<ApiResponse> CreateGrade(string empId, GradeRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            //if (await CheckName(model.Name!))
            //{
            //    return new ApiResponse() { Message = "Grade added allready", Success = false };
            //}
            
            var result = _mapper.Map<Grade>(model);
            result.ApplicationUserEmpId= empId;
            _context.Grades.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "Grade successfully added :)" };
        }

        public async Task<ApiResponse> DeleteGrade(int id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleGrade(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "Grade successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<GradeResponse>> GetAllGrade()
        {
            var result = await _context.Grades.ToListAsync();
            return _mapper.Map<List<GradeResponse>>(result);
        }

        public async Task<GradeResponse> GetSingleGrade(int id)
        {
            var result = await _context.Grades.SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<GradeResponse>(result);
        }

        public async Task<ApiResponse> UpdateGrade(int id, GradeRequest model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleGrade(id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(id, model)) { return new ApiResponse() { Message = "Updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        //private async Task<bool> CheckName(string name)
        //{
        //    var DoesExist = await _context.Grades.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
        //    return DoesExist == null ? false : true;
        //}
        private async Task<bool> Delete(int id)
        {
            var result = _context.Grades.FirstOrDefault(h => h.Id == id);
            _context.Grades.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(int id, GradeRequest model)
        {
            var result = await _context.Grades.FirstOrDefaultAsync(u => u.Id == id);
            if (result == null) { return false; }
            result.ApplicationUserStudId = model.ApplicationUserStudId;
            result.StudGrade=model.StudGrade;
            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}
