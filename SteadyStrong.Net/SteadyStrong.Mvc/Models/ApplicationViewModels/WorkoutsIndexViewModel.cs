using Microsoft.Extensions.FileProviders;
using SteadyStrong.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SteadyStrong.Mvc.Models.ApplicationViewModels
{
    public class WorkoutsIndexViewModel
    {
        public IList<WorkoutMeta> Workouts { get; set; }

        public string Username { get; set; }
    }
}
