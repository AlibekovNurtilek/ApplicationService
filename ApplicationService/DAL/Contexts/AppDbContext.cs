﻿using Microsoft.EntityFrameworkCore;

namespace ApplicationService.DAL.Contexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
