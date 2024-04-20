using ApplicationService.BLL.Models.AuthModels;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using ApplicationService.StaticObjekts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _dbContext;

        public TestController(RoleManager<IdentityRole> roleManager, AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetRoles()
        {

            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        //[HttpGet]
        //[Route("Seed-Roles")]
        //public async Task<IActionResult> SeedRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.STUDENT });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.SUPERADMIN });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.BOLUMBASKAN });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.SECRETAR });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.COMISSION });
        //    return Ok();
        //}

        [HttpPost]
        [Route("AddDepartments")]
        public async Task<IActionResult> AddDepartments([FromBody] string departmentName)
        {
            var isExists = await _dbContext.Departments.Where(u=>u.Name == departmentName).FirstOrDefaultAsync();
            if (isExists is not null)
                return BadRequest("This department name already exist");

            var department = new Department { Name = departmentName };
            await _dbContext.AddAsync(department);
            await _dbContext.SaveChangesAsync();    
            return Ok();

        }

        [HttpGet]
        [Route("GetAllDepartmenst")]
        public async Task<IActionResult> GetAllDeportments()
        {
            var departments =  await _dbContext.Departments.ToListAsync();
            return Ok(departments);
        }

        [HttpPost]
        [Route("CreateBolumBashkanUser")]
        public async Task<IActionResult> CreateBolumBashkanUser([FromBody]LoginModel login)
        {
            var user = new ApplicationUser
            {
                Email = login.Email,
                UserName = login.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DepartmentId = 1
            };
            var createUserResult = await _userManager.CreateAsync(user, login.Password);
            if (!createUserResult.Succeeded)
            {
                var errorString = "Users Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return BadRequest(errorString);
               
             
            }
            await _userManager.AddToRoleAsync(user, StaticUserRole.BOLUMBASKAN);
            return Ok("BolumBashkan Created Successfully");
        }


        [HttpPost]
        [Route("CreateComissionUser")]
        public async Task<IActionResult> CreateComissionUser([FromBody] LoginModel login)
        {
            var user = new ApplicationUser
            {
                Email = login.Email,
                UserName = login.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DepartmentId = 1
            };
            var createUserResult = await _userManager.CreateAsync(user, login.Password);
            if (!createUserResult.Succeeded)
            {
                var errorString = "Users Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return BadRequest(errorString);


            }
            await _userManager.AddToRoleAsync(user, StaticUserRole.COMISSION);
            return Ok("COMISSION Created Successfully");
        }


        [HttpPost]
        [Route("CreateSecretarUser")]
        public async Task<IActionResult> CreateSecretarUser([FromBody] LoginModel login)
        {
            var user = new ApplicationUser
            {
                Email = login.Email,
                UserName = login.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DepartmentId = 1
            };
            var createUserResult = await _userManager.CreateAsync(user, login.Password);
            if (!createUserResult.Succeeded)
            {
                var errorString = "Users Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return BadRequest(errorString);


            }
            await _userManager.AddToRoleAsync(user, StaticUserRole.SECRETAR);
            return Ok("SECRETAR Created Successfully");
        }



        [HttpPost]
        [Route("CreateStudentUser")]
        public async Task<IActionResult> CreateStudentUser([FromBody] LoginModel login)
        {
            var user = new ApplicationUser
            {
                Email = login.Email,
                UserName = login.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                DepartmentId = 1
            };
            var createUserResult = await _userManager.CreateAsync(user, login.Password);
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
            return Ok("STUDENT Created Successfully");
        }


    }
}
