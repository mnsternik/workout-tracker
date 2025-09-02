namespace WorkoutTracker.Api.DTOs.ExerciseDefinition
{
    public class ExerciseDefinitionQueryParameters
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

        // Exercise properties
        public string Name { get; set; } = string.Empty;
        public string ExerciseType { get; set; } = string.Empty;
        public string MuscleGroup { get; set; } = string.Empty;
        public string Equipment { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;

        // Sorting
        public string? SortBy { get; set; } // e.g. "name", "difficultyLevel"
        public string? SortOrder { get; set; } = "desc"; // "asc" or "desc"

    }
}
