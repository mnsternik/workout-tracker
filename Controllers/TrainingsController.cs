using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public TrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trainings.ToListAsync());
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
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            var strengthExercises = training.Exercises
                .OfType<StrengthExercise>()
                .ToList();

            foreach (var strengthExercise in strengthExercises)
            {
                await _context.Entry(strengthExercise)
                    .Collection(se => se.Sets)
                    .LoadAsync();
            }

            var trainingViewModel = new TrainingViewModel
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description,
                Date = training.Date,
                Exercises = new List<ExerciseViewModel> { }
            };

            foreach (var se in strengthExercises)
            {
                trainingViewModel.Exercises.Add(new ExerciseViewModel
                {
                    Id = se.Id,
                    Name = se.Name,
                    Description = se.Description,
                    Sets = se.Sets.Select(set => new SetViewModel
                    {
                        Id = set.Id,
                        Repetitions = set.Repetitions,
                        Weight = set.Weight
                    }).ToList()
                });
            }

            return View(trainingViewModel);
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
                    Name = model.Name,
                    Description = model.Description,
                    Date = model.Date,
                    Exercises = new List<Exercise> { }
                };

                foreach (var exercise in model.Exercises)
                {
                    var strengthExercise = new StrengthExercise
                    {
                        Name = exercise.Name,
                        Description = exercise.Description,
                        Sets = exercise.Sets.Select(set => new Set
                        {
                            Repetitions = set.Repetitions,
                            Weight = set.Weight
                        }).ToList()
                    };

                    training.Exercises.Add(strengthExercise);
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
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null)
            {
                return NotFound();
            }

            var strengthExercises = training.Exercises
                   .OfType<StrengthExercise>()
                   .ToList();

            foreach (var strengthExercise in strengthExercises)
            {
                await _context.Entry(strengthExercise)
                    .Collection(se => se.Sets)
                    .LoadAsync();
            }

            var trainingViewModel = new TrainingViewModel
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description,
                Date = training.Date,
                Exercises = new List<ExerciseViewModel> { }
            };

            foreach (var se in strengthExercises)
            {
                trainingViewModel.Exercises.Add(new ExerciseViewModel
                {
                    Id = se.Id,
                    Name = se.Name,
                    Description = se.Description,
                    Sets = se.Sets.Select(set => new SetViewModel
                    {
                        Id = set.Id,
                        Repetitions = set.Repetitions,
                        Weight = set.Weight
                    }).ToList()
                });
            }

            return View(trainingViewModel);
        }

        // POST: Trainings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Description,Exercises")] TrainingViewModel trainingViewModel)
        {
            if (id != trainingViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var training = await _context.Trainings
                    .Include(t => t.Exercises)
                    .ThenInclude(e => (e as StrengthExercise).Sets)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (training == null)
                {
                    return NotFound();
                }

                training.Name = trainingViewModel.Name;
                training.Description = trainingViewModel.Description;
                training.Exercises = new List<Exercise>{};

                foreach (var exercise in trainingViewModel.Exercises)
                {
                    var strengthExercise = new StrengthExercise
                    {
                        Name = exercise.Name,
                        Description = exercise.Description,
                        Sets = exercise.Sets.Select(set => new Set
                        {
                            Repetitions = set.Repetitions,
                            Weight = set.Weight
                        }).ToList()
                    };
                    training.Exercises.Add(strengthExercise);
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
            return View(trainingViewModel);
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
