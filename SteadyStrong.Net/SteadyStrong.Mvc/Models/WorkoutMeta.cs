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
    public class WorkoutMeta
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime CreatedTimestamp { get; set; }

        [Required]
        [Display(Name="Workout Name")]
        public string Name { get; set; }

        [Required]
        public String Username { get; set; }
    }

}
