using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteadyStrong.Mvc.Filters;
using SteadyStrong.Mvc.Models;
using SteadyStrong.Mvc.Models.ApplicationViewModels;
using SteadyStrong.Mvc.Services;
using System;

namespace SteadyStrong.Mvc.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(DemoDescriptionFilter))]
    public class WorkoutsController : Controller
    {
        private readonly IWorkoutService _workoutService;
        private readonly IWorkoutDataFileRepository _workoutDataFileRepository;

        public WorkoutsController(
            IWorkoutDataFileRepository workoutDataFileRepository,
            IWorkoutService workoutService
            )
        {
            _workoutDataFileRepository = workoutDataFileRepository ?? throw new ArgumentNullException(nameof(workoutDataFileRepository));
            _workoutService = workoutService ?? throw new ArgumentNullException(nameof(workoutService));
        }


        // GET: Workouts/Create
        public ActionResult Create()
        {
            return View(new WorkoutsCreateViewModel());
        }

        // POST: Workouts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkoutsCreateViewModel inputModel)
        {
            if (ModelState.IsValid)
            { 
                Workout workout = new Workout
                {
                    CreatedTimestamp = inputModel.CreatedTimestamp,
                    ExerciseInstances = inputModel.ExerciseInstances,
                    Id = Guid.NewGuid(),
                    Name = inputModel.Name
                };

                _workoutService.Create(workout,  User.Identity.Name);


                return RedirectToAction(nameof(Index));
            }

            return View(inputModel);
        }


        // GET: Workouts/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null) return BadRequest();

            WorkoutMeta workoutMeta = _workoutService.FindMeta(id, User.Identity.Name);

            // For now, only return XML data for the requesting user until privacy/sharing is implemented.
            if(workoutMeta == null) return NotFound();

            WorkoutsDetailsViewModel viewModel = new WorkoutsDetailsViewModel
            {
                DataFileUri = new Uri($"https://steadystrong.blob.core.windows.net/workouts/workout_{id}.xml"),
                WorkoutName = workoutMeta.Name
            }; 

            return View(viewModel);
        }


        // GET: Workouts
        public ActionResult Index()
        {
            WorkoutsIndexViewModel viewModel = new WorkoutsIndexViewModel 
            {
                Workouts = _workoutService.FindAll(User.Identity.Name),
                Username = User.Identity.Name
            };

            return View(viewModel);
        }


    }
}