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
        public DbSet<WorkoutMeta> BodyParts { get; set; }
        public DbSet<WorkoutMeta> ExerciseCategories { get; set; }
        public DbSet<WorkoutMeta> WorkoutMetas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BodyPart>()
                .HasIndex(b => b.CommonName)
                .IsUnique();

            builder.Entity<BodyPart>()
                .HasIndex(b => b.ScientificName)
                .IsUnique();



            builder.Entity<Exercise>()
                .HasIndex(b => b.Name)
                .IsUnique();


            builder.Entity<UserExercise>()
                .HasKey(ue => new { ue.User, ue.Exercise });
        }
    }
}
