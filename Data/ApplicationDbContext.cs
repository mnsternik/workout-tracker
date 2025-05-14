using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainingSession>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.UserId).IsRequired();

                entity.HasMany(e => e.Exercises)
                    .WithOne(e => e.TrainingSession)
                    .HasForeignKey(e => e.TrainingSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TrainingExercise>(entity =>
            {
                entity.Property(e => e.OrderInSession).IsRequired();

                entity.HasOne(e => e.Exercise)
                  .WithMany()
                  .HasForeignKey(e => e.ExerciseId);

                entity.HasMany(e => e.Sets)
                  .WithOne(s => s.TrainingExercise)
                  .HasForeignKey(s => s.TrainingExerciseId)
                  .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<TrainingSet>(entity =>
            {
                entity.Property(e => e.OrderInExercise).IsRequired();
                entity.Property(e => e.WeightKg).HasColumnType("decimal(6,2)");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ExerciseType)
                      .HasConversion<string>(); 

                entity.Property(e => e.Equipment)
                      .HasConversion<string>(); 

                entity.Property(e => e.DifficultyLevel)
                      .HasConversion<string>();

                // TODO: Fix it
                //entity.HasMany(e => e.MuscleGroups) 
                //      .WithMany();
            });
        }

        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<TrainingExercise> TrainingExercises { get; set; }
        public DbSet<TrainingSet> TrainingSets { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<UserRefreshToken> RefreshTokens { get; set; }
    }
}
