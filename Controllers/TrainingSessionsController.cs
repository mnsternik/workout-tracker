using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainingSessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingSession>>> GetTrainingSessions()
        {
            return await _context.TrainingSessions.ToListAsync();
        }

        // GET: api/TrainingSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingSession>> GetTrainingSession(int id)
        {
            var trainingSession = await _context.TrainingSessions.FindAsync(id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            return trainingSession;
        }

        // PUT: api/TrainingSessions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingSession(int id, TrainingSession trainingSession)
        {
            if (id != trainingSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(trainingSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(id))
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

        // POST: api/TrainingSessions
        [HttpPost]
        public async Task<ActionResult<TrainingSession>> PostTrainingSession(TrainingSession trainingSession)
        {
            _context.TrainingSessions.Add(trainingSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingSession), new { id = trainingSession.Id }, trainingSession);
        }

        // DELETE: api/TrainingSessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingSession(int id)
        {
            var trainingSession = await _context.TrainingSessions.FindAsync(id);
            if (trainingSession == null)
            {
                return NotFound();
            }

            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSessions.Any(e => e.Id == id);
        }
    }
}
