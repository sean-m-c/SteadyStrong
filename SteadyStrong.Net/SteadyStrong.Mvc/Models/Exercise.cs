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
        protected readonly int _weightMultiplier;
        

        public enum ExerciseType
        {
            Bilateral,
            Unilateral,
            Bodyweight,
            Alternating
        };
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} characters.")]
        public string Name { get; set; }


        [Required]
        public List<BodyPart> BodyParts { get; set; }



        public int WeightMultiplier {
            get
            {
                return _weightMultiplier;
            }
        }

    }


    public class Alternating : Exercise
    {

    }

    public class BilateralExercise : Exercise
    {

    }

    public class BodyweightExercise : Exercise
    {

    }

    public class UnilateralExercise : Exercise
    {

    }

    public class DumbbellExercise : Exercise
    {

    }

}
