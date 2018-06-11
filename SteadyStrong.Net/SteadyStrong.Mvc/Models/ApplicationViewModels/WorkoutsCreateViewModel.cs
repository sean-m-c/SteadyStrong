using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SteadyStrong.Mvc.Models.Workout;

namespace SteadyStrong.Mvc.Models.ApplicationViewModels
{
    public class WorkoutsCreateViewModel
    {
        [Required]
        [Display(Name = "Date")]
        public DateTime CreatedTimestamp { get; set; }

        [Required]
        public List<ExerciseInstance> ExerciseInstances { get; set; }


        [Required]
        [Display(Name = "Workout Name")]
        public string Name { get; set; }
    }
}
