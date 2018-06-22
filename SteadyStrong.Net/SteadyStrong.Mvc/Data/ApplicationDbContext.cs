using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SteadyStrong.Mvc.Models;

namespace SteadyStrong.Mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<DemoDescription> DemoDescriptions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<UserExercise> UserExercise { get; set; }
        public DbSet<WorkoutMeta> WorkoutMetas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite key with exercise & user
            builder.Entity<UserExercise>()
                .HasKey(e => new { e.Exercise, e.User });

        }
    }
}
