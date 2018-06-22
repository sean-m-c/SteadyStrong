using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteadyStrong.Mvc.Data;
using SteadyStrong.Mvc.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExercisesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(
                await _context.Exercises.
                        Include(e => e.UserExercises.
                            Where(ue => ue.User == user)
                        ).
                        OrderBy(i => i.Name).
                        ToListAsync()
            );
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var exercise = await _context.Exercises.
                                    Include(e => e.UserExercises.
                                        Where(ue => ue.User == user)
                                    )
                                    .SingleOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            return View(new Exercise());
        }

        // POST: Exercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                exercise.Id = Guid.NewGuid();

                _context.Add(exercise);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.SingleOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .SingleOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var exercise = await _context.Exercises.SingleOrDefaultAsync(m => m.Id == id);

            _context.Exercises.Remove(exercise);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(Guid id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
