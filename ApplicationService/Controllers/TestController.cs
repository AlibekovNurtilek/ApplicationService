﻿using ApplicationService.BLL.Models.AuthModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public string Get([FromForm]RegisterModel registerModel)
        {
            
            return "Hello guys Team Best coder";
        }
    }
}