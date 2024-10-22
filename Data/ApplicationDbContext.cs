using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;

namespace WorkoutTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Training> Trainings { get; set; }
        public DbSet<CardioExercise> CardioExercises { get; set; }
        public DbSet<StrengthExercise> StrengthExercises { get; set; }
        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Exercise>()
                .HasDiscriminator<string>("ExerciseType")
                .HasValue<CardioExercise>("Cardio")
                .HasValue<StrengthExercise>("Strength");
        }
    }


}
