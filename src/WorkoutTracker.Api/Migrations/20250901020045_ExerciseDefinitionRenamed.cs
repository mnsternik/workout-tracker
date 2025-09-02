using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseDefinitionRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseMuscleGroupLinks_Exercises_ExerciseId",
                table: "ExerciseMuscleGroupLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingExercises_Exercises_ExerciseId",
                table: "TrainingExercises");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "TrainingExercises",
                newName: "ExerciseDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingExercises_ExerciseId",
                table: "TrainingExercises",
                newName: "IX_TrainingExercises_ExerciseDefinitionId");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "ExerciseMuscleGroupLinks",
                newName: "ExerciseDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseMuscleGroupLinks_Exercises_ExerciseDefinitionId",
                table: "ExerciseMuscleGroupLinks",
                column: "ExerciseDefinitionId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingExercises_Exercises_ExerciseDefinitionId",
                table: "TrainingExercises",
                column: "ExerciseDefinitionId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseMuscleGroupLinks_Exercises_ExerciseDefinitionId",
                table: "ExerciseMuscleGroupLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingExercises_Exercises_ExerciseDefinitionId",
                table: "TrainingExercises");

            migrationBuilder.RenameColumn(
                name: "ExerciseDefinitionId",
                table: "TrainingExercises",
                newName: "ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingExercises_ExerciseDefinitionId",
                table: "TrainingExercises",
                newName: "IX_TrainingExercises_ExerciseId");

            migrationBuilder.RenameColumn(
                name: "ExerciseDefinitionId",
                table: "ExerciseMuscleGroupLinks",
                newName: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseMuscleGroupLinks_Exercises_ExerciseId",
                table: "ExerciseMuscleGroupLinks",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingExercises_Exercises_ExerciseId",
                table: "TrainingExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
