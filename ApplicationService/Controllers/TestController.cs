﻿using ApplicationService.BLL.Models.AuthModels;
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
        private readonly AppDbContext _dbContext;

        public TestController(RoleManager<IdentityRole> roleManager, AppDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetRoles()
        {

            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet]
        [Route("Seed-Roles")]
        public async Task<IActionResult> SeedRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.STUDENT });
            await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.SUPERADMIN });
            await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.BOLUMBASKAN });
            await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.SECRETAR });
            await _roleManager.CreateAsync(new IdentityRole { Name = StaticUserRole.COMISSION });
            return Ok();
        }

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
    }
}
