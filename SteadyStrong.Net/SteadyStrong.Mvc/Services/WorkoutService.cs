using SteadyStrong.Mvc.Data;
using SteadyStrong.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteadyStrong.Mvc.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkoutDataFileRepository _workoutDataFileRepository;

        public WorkoutService(
            ApplicationDbContext context,
            IWorkoutDataFileRepository workoutDataFileRepository
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _workoutDataFileRepository = workoutDataFileRepository ?? throw new ArgumentNullException(nameof(workoutDataFileRepository));
        }


        void IWorkoutService.Create(Workout workout, string username)
        {
            if (workout == null) throw new ArgumentNullException(nameof(workout));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));

            _workoutDataFileRepository.Create(workout);

            WorkoutMeta workoutMeta = new WorkoutMeta
            {
                CreatedTimestamp = workout.CreatedTimestamp,
                Id = workout.Id,
                Name = workout.Name,
                Username = username
            };

            _context.WorkoutMetas.Add(workoutMeta);
            _context.SaveChanges();
        }

        WorkoutMeta IWorkoutService.FindMeta(Guid id)
        {
            if (id == null) return null;

            return _context.WorkoutMetas.Find(id);
        }

        WorkoutMeta IWorkoutService.FindMeta(Guid id, string username)
        {
            if (id == null) return null;
            if (string.IsNullOrWhiteSpace(username)) return null;

            return _context.WorkoutMetas.SingleOrDefault(wm => wm.Username.ToLowerInvariant() == username.ToLowerInvariant() && wm.Id == id);
        }

        IList<WorkoutMeta> IWorkoutService.FindAll(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return new List<WorkoutMeta>();

            return _context.WorkoutMetas.Where(w => w.Username.ToLowerInvariant() == username.ToLowerInvariant()).ToList();
        }


    }
}
