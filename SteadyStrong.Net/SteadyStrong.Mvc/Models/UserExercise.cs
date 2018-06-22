using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{
    public class UserExercise
    {



        [Required]
        public Exercise Exercise { get; set; }

        [Required]
        public ApplicationUser User { get; set; }


        /// <summary>
        /// Gets or sets the one repetition maximum.
        /// Update when workouts are saved if user approves.
        /// </summary>
        /// <value>
        /// The one repetition maximum for the exercise in pounds.
        /// </value>
        public int OneRepetitionMaximum { get; set; }


        public string Note { get; set; }
    }
}
