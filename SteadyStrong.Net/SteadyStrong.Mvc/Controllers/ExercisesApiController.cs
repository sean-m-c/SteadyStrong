using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteadyStrong.Mvc.Data;
using SteadyStrong.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Controllers
{
    [Produces("application/json")]
    [Route("api/exercises")]
    [Authorize]
    public class ExercisesApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesApiController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/exercises
        [HttpGet]
        public IEnumerable<Exercise> GetExercise()
        {
            return _context.Exercises;
        }

        // GET: api/exercises/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExercise([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exercise = await _context.Exercises.SingleOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        // PUT: api/exercises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise([FromRoute] Guid id, [FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exercise.Id)
            {
                return BadRequest();
            }

            _context.Entry(exercise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/exercises
        [HttpPost]
        public async Task<IActionResult> PostExercise([FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercise", new { id = exercise.Id }, exercise);
        }

        // DELETE: api/exercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exercise = await _context.Exercises.SingleOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return Ok(exercise);
        }

        private bool ExerciseExists(Guid id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}