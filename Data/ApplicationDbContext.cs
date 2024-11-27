using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;

namespace WorkoutTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Training> Trainings { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Set>()
                .Property(s => s.Weight)
                .HasPrecision(10, 2); 

            modelBuilder.Entity<Set>()
                .Property(s => s.Distance)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Sets)
                .WithOne(s => s.Exercise)
                .HasForeignKey(s => s.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Training>()
                .HasMany(t => t.Exercises)
                .WithOne(e => e.Training)
                .HasForeignKey(e => e.TrainingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
