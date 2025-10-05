namespace WorkoutTracker.Api.Models
{
    public class ExerciseDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public Equipment Equipment { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<ExerciseMuscleGroupLink> MuscleGroupsLinks { get; set; } = [];
    }

    public enum ExerciseType
    {
        Strength = 1,
        Cardio = 2,
        Isometric = 3
    }

    public enum MuscleGroup
    {
        FullBody = 1,
        Chest = 2,
        Back = 3,
        Shoulders = 4,
        Arms = 5,
        Biceps = 6,
        Triceps = 7,
        Legs = 8,
        Quads = 9,
        Hamstrings = 10,
        Glutes = 11,
        Calves = 12,
        Core = 13,
        Abs = 14,
        LowerBack = 15,
        Neck = 16
    }

    public enum Equipment
    {
        None = 1,
        Barbell = 2,
        Dumbbell = 3,
        Kettlebell = 4,
        ResistanceBand = 5,
        PullUpBar = 6,
        DipBars = 7,
        Bench = 8,
        Machine = 9,
        Cable = 10,
        MedicineBall = 11,
        TRX = 12,
        JumpRope = 13,
        Treadmill = 14,
        RowingMachine = 15,
        Bike = 16,
        Other = 17
    }

    public enum DifficultyLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }
}

