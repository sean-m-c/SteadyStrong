using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{
    /// <summary>
    /// An instance of an exercise performed at a certain weight for a number of repetitions.
    /// </summary>
    public class ExerciseSet
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You must have performed at least one rep.")]
        public int Repetitions { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Weight must be greater than zero.")]
        public int Weight { get; set; }
    }
}
