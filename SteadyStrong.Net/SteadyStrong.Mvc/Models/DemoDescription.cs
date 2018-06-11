using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Models
{
    public class DemoDescription
    {
        [Required]
        [Key]
        public string PagePath { get; set; }


        [Required]
        public string Description { get; set; }


        public static string GetPagePathUrlFriendlyFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            return url.Trim().ToLower().TrimStart('/').TrimEnd('/').Replace('/', '-');
        }
    }
}
