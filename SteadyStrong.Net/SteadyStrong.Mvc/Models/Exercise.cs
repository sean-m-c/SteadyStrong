using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{
    /// <summary>
    /// A specific exercise, for example, "benchpress".
    /// </summary>
    public class Exercise
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength=3, ErrorMessage = "The {0} must be at least {2} characters.")]
        public string Name { get; set; }
    }
}
