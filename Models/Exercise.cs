namespace WorkoutTracker.Api.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ExerciseType ExerciseType { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public Equipment Equipment { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }

    public enum ExerciseType
    {
        Strength,
        Cardio,
        Isometric
    }

    public enum MuscleGroup
    {
        FullBody,
        Chest,
        Back,
        Shoulders,
        Arms,
        Biceps,
        Triceps,
        Legs,
        Quads,
        Hamstrings,
        Glutes,
        Calves,
        Core,
        Abs,
        LowerBack,
        Neck
    }

    public enum Equipment
    {
        None,
        Barbell,
        Dumbbell,
        Kettlebell,
        ResistanceBand,
        PullUpBar,
        DipBars,
        Bench,
        Machine,
        Cable,
        MedicineBall,
        TRX,
        JumpRope,
        Treadmill,
        RowingMachine,
        Bike,
        Other
    }

    public enum DifficultyLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }
}

