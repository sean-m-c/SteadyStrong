using SteadyStrong.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SteadyStrong.Mvc.Services
{
    public interface IWorkoutService
    {
        IList<WorkoutMeta> FindAll(string username);
        void Create(Workout workout, string username);
        WorkoutMeta FindMeta(Guid id);
        WorkoutMeta FindMeta(Guid id, string username);
    }
}
