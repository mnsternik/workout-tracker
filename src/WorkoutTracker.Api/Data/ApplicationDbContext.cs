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

        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<PerformedExercise> PerformedExercises { get; set; }
        public DbSet<PerformedSet> PerformedSets { get; set; }
        public DbSet<ExerciseDefinition> ExerciseDefinitions { get; set; }
        public DbSet<ExerciseMuscleGroupLink> ExerciseMuscleGroupLinks { get; set; }
        public DbSet<UserRefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainingSession>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Note)
                    .HasMaxLength(1000);

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.HasMany(e => e.PerformedExercises)
                    .WithOne(e => e.TrainingSession)
                    .HasForeignKey(e => e.TrainingSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PerformedExercise>(entity =>
            {
                entity.Property(e => e.OrderInSession)i
                    .IsRequired();

                entity.HasOne(e => e.ExerciseDefinition)
                    .WithMany()
                    .HasForeignKey(e => e.ExerciseDefinitionId);

                entity.HasMany(e => e.Sets)
                    .WithOne(s => s.PerformedExercise)
                    .HasForeignKey(s => s.PerformedExerciseId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<PerformedSet>(entity =>
            {
                entity.Property(e => e.OrderInExercise)
                    .IsRequired();

                entity.Property(e => e.WeightKg)
                    .HasColumnType("decimal(6,2)");
            });

            modelBuilder.Entity<ExerciseDefinition>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .IsRequired();
            });

            modelBuilder.Entity<ExerciseMuscleGroupLink>(entity =>
            {
                // Composite primary key
                entity.HasKey(e => new { e.ExerciseDefinitionId, e.MuscleGroup });

                entity.HasOne(e => e.ExerciseDefinition)
                    .WithMany(e => e.MuscleGroupsLinks)
                    .HasForeignKey(e => e.ExerciseDefinitionId);
            });
        }
    }
}
