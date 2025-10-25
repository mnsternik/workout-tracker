using FluentAssertions;
using Microsoft.OpenApi.Validations;
using WorkoutTracker.Api.DTOs.PerformedExercise;
using WorkoutTracker.Api.DTOs.PerformedSet;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class UpdateTrainingSessionAsyncTests : TrainingSessionsServiceTestsBase
    {
        [Fact]
        public async Task UpdateTrainingSession_SuccessfullyUpdatesValues()
        {
            // Arrange
            int sessionId = 1;
            var user = new ApplicationUserBuilder().BuildDomain();
            var orginalSession = new TrainingSessionBuilder()
                .WithId(sessionId)
                .WithName("Old session name")
                .WithNote("Old session note")
                .WithDifficultyRating(DifficultyRating.VeryEasy)
                .WithDurationMinutes(45)
                .WithUser(user)
                .WithDefaultExercises(0)
                .BuildDomain();
            var sessionUpdateDto = new TrainingSessionBuilder()
                .WithId(sessionId)
                .WithName("Updated session name")
                .WithNote("Updated session note")
                .WithDifficultyRating(DifficultyRating.Extreme)
                .WithDurationMinutes(120)
                .WithUser(user)
                .WithDefaultExercises(0)
                .BuildUpdateDto();

            Context.TrainingSessions.Add(orginalSession);
            Context.SaveChanges();

            // Act
            await TsService.UpdateTrainingSessionAsync(sessionId, user.Id, sessionUpdateDto);

            // Assert
            var updatedSession = Context.TrainingSessions.Find(sessionId);
            updatedSession.Should().BeOfType<TrainingSession>();
            updatedSession.Name.Should().Be(updatedSession.Name);
            updatedSession.Note.Should().Be(updatedSession.Note);
            updatedSession.DifficultyRating.Should().Be(updatedSession.DifficultyRating);
            updatedSession.DurationMinutes.Should().Be(updatedSession.DurationMinutes);
        }

        [Fact]
        public async Task UpdateTrainingSession_SuccessfullyUpdatesExercisesAndSets()
        {
            // Arrange
            var user = new ApplicationUserBuilder().BuildDomain();
            int updatedWeight = 20;
            int updatedReps = 9;

            // By default TrainingSession has 3 PerformedExercises each with 3 PerformedSets
            // In update DTO changes are:
            // - delete first exercise
            // - in rest of exercises change its order by -1
            // - in sets: add +10 to WeightKg and + 5 to Reps
            var orginalSession = new TrainingSessionBuilder().WithUser(user).BuildDomain();
            var updateSessionDto = new TrainingSessionUpdateDto
            {
                Id = orginalSession.Id,
                Name = orginalSession.Name,
                Note = orginalSession.Note,
                DifficultyRating = orginalSession.DifficultyRating,
                DurationMinutes = orginalSession.DurationMinutes,
                Exercises = orginalSession.PerformedExercises.Select(pe => new PerformedExerciseUpdateDto
                {
                    Id = pe.Id,
                    OrderInSession = pe.OrderInSession - 1,
                    ExerciseDefinitionId = pe.ExerciseDefinitionId,
                    Sets = pe.Sets.Select(set => new PerformedSetUpdateDto
                    {
                        WeightKg = updatedWeight,
                        Reps = updatedReps,
                    }).ToList(),
                }).ToList()
            };

            updateSessionDto.Exercises.RemoveAt(0);

            Context.TrainingSessions.Add(orginalSession);
            Context.SaveChanges();

            // Act
            await TsService.UpdateTrainingSessionAsync(orginalSession.Id, user.Id, updateSessionDto);

            // Assert
            var updatedSession = Context.TrainingSessions.Find(orginalSession.Id); 
            updatedSession.Should().BeOfType<TrainingSession>();
            updatedSession.PerformedExercises.Should().HaveCount(2);

            updatedSession.PerformedExercises.ToList()[0].OrderInSession.Should().Be(1);
            updatedSession.PerformedExercises.ToList()[1].OrderInSession.Should().Be(2);

            updatedSession.PerformedExercises.ToList()[0].Sets.ToList()[0].WeightKg.Should().Be(updatedWeight);
            updatedSession.PerformedExercises.ToList()[0].Sets.ToList()[0].Reps.Should().Be(updatedReps);
        }
    }
}

