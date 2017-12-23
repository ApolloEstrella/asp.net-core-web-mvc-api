using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Login.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Data
{
    public class LoginDbContext : IdentityDbContext<ApplicationUser>
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options)
        {

        }

        public DbSet<UserFood> UserFood { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("server=(localdb)\\ProjectsV13;Database=LoginDb;Integrated Security=True;MultipleActiveResultSets=true;");
        //}
    }
}
