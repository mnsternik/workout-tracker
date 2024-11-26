using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Data;
using WorkoutTracker.Models;
using WorkoutTracker.Models.ViewModels;
using WorkoutTracker.Services.Mappers;

namespace WorkoutTracker.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TrainingMapper _trainingMapper; 

        public TrainingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, TrainingMapper trainingMapper)
        {
            _context = context;
            _userManager = userManager;
            _trainingMapper = trainingMapper;
        }

        // GET: Trainings
        public async Task<IActionResult> Index(string search)
        {
            var userId = _userManager.GetUserId(User);

            var trainings = await _context.Trainings
                .Where(t => t.UserId == userId).
                ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                trainings = trainings.Where(t => t.Name!.Contains(search, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            var trainingsListViewModel = new TrainingsListViewModel
            {
                Trainings = trainings,
                SearchString = search
            };

            return View(trainingsListViewModel);
        }

        // GET: Discover/
        public async Task<IActionResult> Discover(string search)
        {
            var trainings = await _context.Trainings.
                Include(t => t.User).
                Include(t => t.Exercises).
                ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                trainings = trainings.Where(t => t.Name!.Contains(search, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            var trainingsListViewModel = new TrainingsListViewModel
            {
                Trainings = trainings,
                SearchString = search
            };

            return View(trainingsListViewModel);
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .Include(t => t.Exercises)
                .ThenInclude(e => e.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Trainings/Create
        public IActionResult Create()
        {
            return View(new TrainingViewModel());
        }

        // POST: Trainings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var training = _trainingMapper.MapToTraining(model, _userManager.GetUserId(User));
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .Include(t => t.Exercises)
                .ThenInclude(e => e.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            var model = _trainingMapper.MapToViewModel(training);
            return View(model);
        }

        // POST: Trainings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Description,Exercises")] TrainingViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .Include(t => t.Exercises)
                .ThenInclude(e => e.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            if (training.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                training = _trainingMapper.MapToTraining(model, training);

                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.Id))
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
            return View(model);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .FirstOrDefaultAsync(m => m.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            if (training != null)
            {
                _context.Trainings.Remove(training);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return _context.Trainings.Any(e => e.Id == id);
        }
    }
}
