using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteadyStrong.Mvc.Data;
using SteadyStrong.Mvc.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Controllers
{
    public class DemoDescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DemoDescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DemoDescriptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.DemoDescriptions.ToListAsync());
        }

        // GET: DemoDescriptions/Details/5
        public async Task<IActionResult> Details(string id, string previousUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demoDescription = await _context.DemoDescriptions
                .SingleOrDefaultAsync(m => id == m.PagePath);
            if (demoDescription == null)
            {
                return NotFound();
            }

            ViewBag.PreviousUrl = previousUrl;

            return View(demoDescription);
        }

        // GET: DemoDescriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DemoDescriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PagePath,Description")] DemoDescription demoDescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(demoDescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(demoDescription);
        }

        // GET: DemoDescriptions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demoDescription = await _context.DemoDescriptions.SingleOrDefaultAsync(m => m.PagePath == id);
            if (demoDescription == null)
            {
                return NotFound();
            }
            return View(demoDescription);
        }

        // POST: DemoDescriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PagePath,Description")] DemoDescription demoDescription)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                id.ToLower() != demoDescription.PagePath.TrimStart('/').ToLower())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demoDescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemoDescriptionExists(demoDescription.PagePath))
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
            return View(demoDescription);
        }

        // GET: DemoDescriptions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demoDescription = await _context.DemoDescriptions
                .SingleOrDefaultAsync(m => m.PagePath == id);
            if (demoDescription == null)
            {
                return NotFound();
            }

            return View(demoDescription);
        }

        // POST: DemoDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var demoDescription = await _context.DemoDescriptions.SingleOrDefaultAsync(m => m.PagePath == id);
            _context.DemoDescriptions.Remove(demoDescription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemoDescriptionExists(string id)
        {
            return _context.DemoDescriptions.Any(e => e.PagePath == id);
        }

    }
}
