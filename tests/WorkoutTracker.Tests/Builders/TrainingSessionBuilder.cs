using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class TrainingSessionBuilder
    {
        private int _id = 1;
        private string _name = "My training session";
        private string? _note = "Notes to my trainig session";
        private DateTime _createdAt = DateTime.UtcNow;
        private DifficultyRating? _difficultyRating = DifficultyRating.Easy;
        private int? _durationMinutes = 45;
        private string _userId = new Guid().ToString();
        private ApplicationUser User = new ApplicationUser();
        private ICollection<PerformedExercise> PerformedExercises = [];
        public TrainingSessionBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public TrainingSessionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TrainingSessionBuilder WithNote(string note)
        {
            _note = note;
            return this;
        }

        public TrainingSessionBuilder WithCreateDate(DateTime date)
        {
            _createdAt = date;
            return this;
        }

        public TrainingSessionBuilder WithDifficultyRating(DifficultyRating difficulty)
        {
            _difficultyRating = difficulty;
            return this;
        }

        public TrainingSessionBuilder WithDurationMinutes(int duration)
        {
            _durationMinutes = duration;
            return this;
        }

        public TrainingSessionBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }


    }
}
