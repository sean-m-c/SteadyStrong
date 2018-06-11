using SteadyStrong.Mvc.Models;
using System;
using System.Linq;

namespace SteadyStrong.Mvc.Data
{    
    // Initalize database with default data.
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Exercises.Any())
            {
                return;   // DB has been seeded
            }

            var exercises = new Exercise[]
            {
                new Exercise{Id = Guid.NewGuid(), Name="Deadlift"},
                new Exercise{Id = Guid.NewGuid(), Name="Benchpress"},
                new Exercise{Id = Guid.NewGuid(), Name="Ab crunch"},
                new Exercise{Id = Guid.NewGuid(), Name="Overhead press"},
                new Exercise{Id = Guid.NewGuid(), Name="Squat"},
                new Exercise{Id = Guid.NewGuid(), Name="Barbell row"},
                new Exercise{Id = Guid.NewGuid(), Name="Bicep curl"},
                new Exercise{Id = Guid.NewGuid(), Name="Tricep extension"}
            };

            foreach (Exercise exercise in exercises)
            {
                context.Exercises.Add(exercise);
            }

            context.SaveChanges();
        }
    }
}