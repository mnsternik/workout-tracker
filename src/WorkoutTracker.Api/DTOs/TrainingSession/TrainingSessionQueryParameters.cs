namespace WorkoutTracker.Api.DTOs.TrainingSession
{
    public class TrainingSessionQueryParameters
    {
        // Pagination
        private const int MaxPageSize = 50;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
        }

        public int PageNumber { get; set; } = 1;

        // Training session properties
        public string? TrainingName { get; set; }
        public string? UserId { get; set; }
        public string? UserDisplayName { get; set; }
        public List<string>? ExerciseNames { get; set; } 
        public List<string>? MuscleGroups { get; set; } 
        public string? DifficultyRating { get; set; } // e.g "easy", "hard"
        public int? MinDurationMinutes { get; set; }
        public int? MaxDurationMinutes { get; set; }
        
        // Sorting
        public string? SortBy { get; set; } // e.g. "createdAt", "name", "difficultyRating"
        public string? SortOrder { get; set; } = "desc"; // "asc" or "desc"
    }
}
