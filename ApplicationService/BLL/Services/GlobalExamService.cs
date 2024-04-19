using ApplicationService.BLL.Models.GlobalExamModels;
using ApplicationService.BLL.Models.Responses;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.BLL.Services
{
    public interface IGlobalExamService
    {
        Task<ApiResponse> CreateGlobalExam(int depId, GlobalExamRequest model);
        Task<ApiResponse> UpdateGlobalExam(int id, GlobalExamRequest model);

        Task<ApiResponse> DeleteGlobalExam(int id);
        Task<GlobalExamResponse> GetSingleGlobalExam(int id);
        Task<List<GlobalExamResponse>> GetAllGlobalExam();
    }
    public class GlobalExamService : IGlobalExamService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GlobalExamService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateGlobalExam(int depId, GlobalExamRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            //if (await CheckName(model.Name!))
            //{
            //    return new ApiResponse() { Message = "GlobalExam added allready", Success = false };
            //}
            var result = _mapper.Map<GlobalExam>(model);
            result.DepartmentId = depId;
            _context.GlobalExams.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "GlobalExam successfully added :)" };
        }

        public async Task<ApiResponse> DeleteGlobalExam(int id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleGlobalExam(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "GlobalExam successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<GlobalExamResponse>> GetAllGlobalExam()
        {
            var result = await _context.GlobalExams.ToListAsync();
            return _mapper.Map<List<GlobalExamResponse>>(result);
        }

        public async Task<GlobalExamResponse> GetSingleGlobalExam(int id)
        {
            var result = await _context.GlobalExams.SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<GlobalExamResponse>(result);
        }

        public async Task<ApiResponse> UpdateGlobalExam(int id, GlobalExamRequest model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleGlobalExam(id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(id, model)) { return new ApiResponse() { Message = "Updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        //private async Task<bool> CheckName(string name)
        //{
        //    var DoesExist = await _context.GlobalExams.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
        //    return DoesExist == null ? false : true;
        //}
        private async Task<bool> Delete(int id)
        {
            var result = _context.GlobalExams.FirstOrDefault(h => h.Id == id);
            _context.GlobalExams.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(int id, GlobalExamRequest model)
        {
            var result = await _context.GlobalExams.FirstOrDefaultAsync(u => u.Id == id);
            if (result == null) { return false; }
            result.Name = model.Name;
            result.ClassRoom = model.ClassRoom;
            result.StartTime = model.StartTime;
            result.EndTime = model.EndTime;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
