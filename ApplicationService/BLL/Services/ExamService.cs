using Amazon.Auth.AccessControlPolicy;
using Amazon.Runtime;
using ApplicationService.BLL.Models.ExamModels;
using ApplicationService.BLL.Models.Responses;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.BLL.Services
{
    public interface IExamService
    {
        Task<ApiResponse> CreateExam(string empId,ExamRequest model);
        Task<ApiResponse> UpdateExam(int id,ExamRequest model);

        Task<ApiResponse> DeleteExam(int id);
        Task<ExamResponse> GetSingleExam(int id);
        Task<List<ExamResponse>> GetAllExam();
        Task<ExamResponse> GetSingleExam(string id);
    }
    public class ExamService : IExamService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAmazonService _amazonService;

        public ExamService(AppDbContext context, IMapper mapper, IAmazonService amazonService)
        {
            _context = context;
            _mapper = mapper;
            _amazonService = amazonService;

        }

        public async Task<ApiResponse> CreateExam(string empId,ExamRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            //if (await CheckName(model.Name!))
            //{
            //    return new ApiResponse() { Message = "Exam added allready", Success = false };
            //}
            var imageList = new List<ExamImage>();
            foreach(var item in model.ExamImages)
            {
                var passportFrontBase64 = await ConvertToBase64(item);
                var image = new ExamImage();
                image.url =  await _amazonService.UploadImage(passportFrontBase64);
                imageList.Add(image);
            }
            var exam = new Exam();
            exam.ApplicationUserStudId = model.ApplicationUserStudId;
            exam.ApplicationUserEmpId = empId;
            exam.GlobalExamId = model.GlobalExamId;
            exam.ExamImages = imageList;
            //var result = _mapper.Map<Exam>(model);
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "Exam successfully added :)" };
        }

        public async Task<ApiResponse> DeleteExam(int id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleExam(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "Exam successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<ExamResponse>> GetAllExam()
        {
            var result = await _context.Exams.ToListAsync();
            return _mapper.Map<List<ExamResponse>>(result);
        }

        public async Task<ExamResponse> GetSingleExam(int id)
        {
            var result = await _context.Exams.SingleOrDefaultAsync(u=>u.Id==id);
            if (result == null) return null;
            return _mapper.Map<ExamResponse>(result);
        }
        public async Task<ExamResponse> GetSingleExam(string id)
        {
            var result = await _context.Exams.SingleOrDefaultAsync(u => u.ApplicationUserStudId == id);
            if (result == null) return null;
            return _mapper.Map<ExamResponse>(result);
        }

        public async Task<ApiResponse> UpdateExam(int id,ExamRequest model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleExam(id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(id,model)) { return new ApiResponse() { Message = "Updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        //private async Task<bool> CheckName(string name)
        //{
        //    var DoesExist = await _context.Exams.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
        //    return DoesExist == null ? false : true;
        //}
        private async Task<bool> Delete(int id)
        {
            var result = _context.Exams.FirstOrDefault(h => h.Id == id);
            _context.Exams.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(int id,ExamRequest model)
        {
            var result = await _context.Exams.FirstOrDefaultAsync(u => u.Id == id);
            if (result == null) { return false; }
            result.ApplicationUserStudId = model.ApplicationUserStudId;
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<string> ConvertToBase64(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("Фото не может быть пустым");
            }

            using (var ms = new MemoryStream())
            {
                await photo.CopyToAsync(ms);
                byte[] photoBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(photoBytes);
                return $"data:{photo.ContentType};base64,{base64String}";
            }
        }
    }
}
