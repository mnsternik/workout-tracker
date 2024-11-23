using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Data;
using WorkoutTracker.Models;
using WorkoutTracker.Models.ViewModels;
namespace WorkoutTracker.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                var training = new Training
                {
                    UserId = _userManager.GetUserId(User),
                    Name = model.Name,
                    Description = model.Description,
                    Date = DateTime.UtcNow,
                    Exercises = new List<Exercise> { }
                };

                foreach (var exercise in model.Exercises)
                {
                    training.Exercises.Add(new Exercise
                    {
                        Name = exercise.Name,
                        Description = exercise.Description,
                        Type = exercise.Type,
                        Sets = exercise.Sets.Select(set => new Set
                        {
                            Repetitions = set.Repetitions,
                            Weight = set.Weight,
                            Distance = set.Distance,
                            Duration = set.Duration,
                        }).ToList()
                    });
                }

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

            var model = new TrainingViewModel
            {
                Name = training.Name,
                Description = training.Description,
                Date = training.Date,
                Exercises = new List<ExerciseViewModel>()
            };

            foreach (var exercise in training.Exercises)
            {
                model.Exercises.Add(new ExerciseViewModel
                {
                    Name = exercise.Name,
                    Description = exercise.Description,
                    Type = exercise.Type,
                    Sets = exercise.Sets.Select(set => new SetViewModel
                    {
                        ExerciseType = exercise.Type,
                        Repetitions = set.Repetitions,
                        Weight = set.Weight,
                        Distance = set.Distance,
                        Duration = set.Duration,
                    }).ToList()
                });
            }

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

            if (ModelState.IsValid)
            {
                var training = await _context.Trainings
                    .Include(t => t.Exercises)
                    .ThenInclude(e => e.Sets)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (training == null)
                {
                    return NotFound();
                }

                training.Name = model.Name;
                training.Description = model.Description;
                training.Exercises = new List<Exercise>();

                foreach (var exercise in model.Exercises)
                {
                    training.Exercises.Add(new Exercise
                    {
                        Name = exercise.Name,
                        Description = exercise.Description,
                        Type = exercise.Type,
                        Sets = exercise.Sets.Select(set => new Set
                        {
                            Repetitions = set.Repetitions,
                            Weight = set.Weight,
                            Distance = set.Distance,
                            Duration = set.Duration,
                        }).ToList()
                    });
                }

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
