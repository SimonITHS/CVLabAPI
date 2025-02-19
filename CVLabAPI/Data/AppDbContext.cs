using CVLabAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CVLabAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
