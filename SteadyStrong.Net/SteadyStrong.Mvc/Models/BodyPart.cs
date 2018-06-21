using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{
    /// <summary>
    /// A body part, e.g "deltoids." Can belong to other body parts, e.g "rear deltoids" could belong to "deltoids".
    /// </summary>
    public class BodyPart
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CommonName { get; set; }

        public string ScientificName { get; set; }

        public BodyPart ParentBodyPart { get; set; }

        public List<BodyPart> ChildBodyParts { get; set; }

    }
}
