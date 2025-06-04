namespace WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession
{
    public class TrainingSessionQueryParameters
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value < 1 ? 1 : value);
        }

        public int PageNumber { get; set; } = 1;

        public string? UserId { get; set; }
        public string? DisplayName { get; set; } 
        public List<string>? ExerciseNames { get; set; } 
        public List<string>? MuscleGroups { get; set; } 
        public int? MinDifficulty { get; set; }
        public int? MaxDifficulty { get; set; }
        public int? MinDurationMinutes { get; set; }
        public int? MaxDurationMinutes { get; set; }
        public string? SortBy { get; set; } // e.g. "createdAt", "name", "difficultyRating"
        public string? SortOrder { get; set; } = "desc"; // "asc" or "desc" TODO: change it to bool
    }
}
