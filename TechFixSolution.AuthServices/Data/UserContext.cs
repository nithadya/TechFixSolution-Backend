using System.Collections.Generic;
using TechFixSolution.AuthServices.Models;
using Microsoft.EntityFrameworkCore;

namespace TechFixSolution.AuthServices.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}