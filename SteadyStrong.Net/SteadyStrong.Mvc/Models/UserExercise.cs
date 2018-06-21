using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{
    public class UserExercise
    {
        public ApplicationUser User { get; set; }
        public double OneRepetitionMax { get; set; }
        public Exercise Exercise { get; set; }
    }
}
