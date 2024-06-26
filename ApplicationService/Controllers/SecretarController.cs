﻿using ApplicationService.BLL.Models;
using ApplicationService.BLL.Services;
using ApplicationService.DAL.Contexts;
using ApplicationService.StaticObjekts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Web;

namespace ApplicationService.Controllers
{
    public class StatementModel
    {
        public int statementId { get; set; }
    }

    public class ExamModel
    {
        public int examId { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class SecretarController : ControllerBase
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IGlobalExamService _globalExamService;
        public SecretarController(AppDbContext appContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IGlobalExamService globalExamService)
        {
            _appContext = appContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _globalExamService = globalExamService;
        }

        [Authorize(Roles = ("SECRETAR"))]
        [HttpGet]
        [Route("GetAllStatementsBydepartment")]

        public async Task<IActionResult> GetAllStatements()
        {
            try
            {
                var currentUSerID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (currentUSerID == null)
                    return NotFound("Secretar not found");
                var secretar = await _userManager.FindByIdAsync(currentUSerID);
                if (secretar == null)
                    return NotFound("Secretar not found");

                var statements = await _appContext.Statements.Where(u => u.DepartmentId == secretar.DepartmentId && u.IsAccespted == false).ToListAsync();

                //var statements = await _userManager.Users.Where(u => u.DepartmentId == secretar.DepartmentId).ToListAsync();
                return Ok(statements);
            }
            catch (Exception ex)
            {
                // Возврат ошибки сервера с текстом ошибки
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }


        [Authorize(Roles = ("SECRETAR"))]
        [HttpGet]
        [Route("GetAllGlobalExamsBydepartment")]

        public async Task<IActionResult> GetAllGlobalExamsBydepartment()
        {
            try
            {
                var currentUSerID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (currentUSerID == null)
                    return NotFound("Secretar not found");
                var secretar = await _userManager.FindByIdAsync(currentUSerID);
                if (secretar == null)
                    return NotFound("Secretar not found");

                var globalExams = await _appContext.GlobalExams.Where(u => u.DepartmentId == secretar.DepartmentId).ToListAsync();

                //var statements = await _userManager.Users.Where(u => u.DepartmentId == secretar.DepartmentId).ToListAsync();
                return Ok(globalExams);
            }
            catch (Exception ex)
            {
                // Возврат ошибки сервера с текстом ошибки
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        [Authorize(Roles = ("SECRETAR"))]
        [HttpGet]
        [Route("GetAllStudentsByGlobalExamId")]
        public async Task<IActionResult> GetAllStudentsByGlobalExamId([FromQuery] int id)
        {
            try
            {

                var exam = await _appContext.GlobalExams.Where(u=>u.Id==id).FirstOrDefaultAsync();
                if (exam is null)
                    return BadRequest("Exam not found");
                

                var user = await _userManager.GetUsersInRoleAsync("STUDENT");
                var usersInDepartment =  user.Where(u => u.DepartmentId == exam.DepartmentId).ToList();
                    

                return Ok(usersInDepartment);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    


        [Authorize(Roles =("SECRETAR"))]
        [HttpPost]
        [Route("AccesptStatement")]
        public async Task<IActionResult> AccesptStatrement([FromBody] StatementModel model)
        {
            try
            {
                var statement = await _appContext.Statements.Where(u => u.Id == model.statementId).FirstOrDefaultAsync();
                if (statement == null)
                    return NotFound("this statement not found");
                var user = new ApplicationUser
                {
                    FirstName = statement.FirstName,
                    LastName = statement.LastName,
                    UserName = statement.Email,
                    Email = statement.Email,
                    PhoneNumber = statement.Phone,
                    IsMagistr = statement.IsMagistr,
                    DoB = statement.DoB,
                    PassportFrontImage = statement.PassportFrontImage,
                    PassportBackImage = statement.PassportBackImage,
                    DiplomImage = statement.DiplomImage,
                    DepartmentId = statement.DepartmentId,


                };
                Guid guid = Guid.NewGuid();
                string guidString = guid.ToString("N"); // Преобразовать Guid в строку без разделителей
                string password = guidString.Substring(0, 8); // Взять первые 8 символов строки
                var createUserResult = await _userManager.CreateAsync(user, password);
                if (!createUserResult.Succeeded)
                {
                    var errorString = "Users Creation Failed Beacause: ";
                    foreach (var error in createUserResult.Errors)
                    {
                        errorString += " # " + error.Description;
                    }
                    return BadRequest(errorString);


                }
                await _userManager.AddToRoleAsync(user, StaticUserRole.STUDENT);
                await SendConfirmEmail(user.Email, password);
                _appContext.Statements.Remove(statement);
                await _appContext.SaveChangesAsync();
                return Ok("STUDENT Created Successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [Authorize(Roles = ("SECRETAR"))]
        [HttpPost]
        [Route("NotAccesptStatement")]
        public async Task<IActionResult> NotAccesptStatrement([FromBody] StatementModel model)
        {
            try
            {
                var statement = await _appContext.Statements.Where(u => u.Id == model.statementId).FirstOrDefaultAsync();
                if (statement == null)
                    return NotFound("this statement not found");
                await SendNotConfirmEmail(statement.Email);
                _appContext.Statements.Remove(statement);
                await _appContext.SaveChangesAsync();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        private async Task SendConfirmEmail(string email, string password)
        {
            var emailBody = $"Congratulations, you application is accespted!!!You password - {password} ";

            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = email;
            mailRequest.Subject = "Sub";
            mailRequest.Body = emailBody;
            await _emailService.SendEmailAsync(mailRequest);
        } 
        private async Task SendNotConfirmEmail(string email)
        {
            var emailBody = $" Your Application not accepted";

            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = email;
            mailRequest.Subject = "Sub";
            mailRequest.Body = emailBody;
            await _emailService.SendEmailAsync(mailRequest);
        }


    }
}
