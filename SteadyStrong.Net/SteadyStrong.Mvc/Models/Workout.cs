using SteadyStrong.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{

    /// <summary>
    /// A entire workout, consisting of a series of exercise sets.
    /// </summary>
    public class Workout
    {
        public class ExerciseInstance
        {
            [Required]
            public string ExerciseName { get; set; }

            [Required]
            public Guid ExerciseId { get; set; }

            [Required]
            public List<ExerciseSet> ExerciseSets { get; set; }
        }

        [Required]
        public DateTime CreatedTimestamp { get; set; }

        [Required]
        public List<ExerciseInstance> ExerciseInstances { get; set; }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

}
