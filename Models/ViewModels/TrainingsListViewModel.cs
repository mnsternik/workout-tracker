using WorkoutTracker.Utilities;

namespace WorkoutTracker.Models.ViewModels
{
    public class TrainingsListViewModel
    {
        public PaginatedList<Training>? Trainings { get; set; }

        public string? SearchString { get; set; } 
    }
}
