using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class EnumStringConversionsRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DifficultyRating",
                table: "TrainingSessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseMuscleGroupLinks",
                table: "ExerciseMuscleGroupLinks");

            migrationBuilder.AlterColumn<int>(
                name: "MuscleGroup",
                table: "ExerciseMuscleGroupLinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseMuscleGroupLinks",
                table: "ExerciseMuscleGroupLinks",
                columns: new[] { "ExerciseDefinitionId", "MuscleGroup" });

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseType",
                table: "ExerciseDefinitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Equipment",
                table: "ExerciseDefinitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyLevel",
                table: "ExerciseDefinitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DifficultyRating",
                table: "TrainingSessions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseMuscleGroupLinks",
                table: "ExerciseMuscleGroupLinks");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleGroup",
                table: "ExerciseMuscleGroupLinks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseMuscleGroupLinks",
                table: "ExerciseMuscleGroupLinks",
                columns: new[] { "ExerciseDefinitionId", "MuscleGroup" });

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseType",
                table: "ExerciseDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Equipment",
                table: "ExerciseDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DifficultyLevel",
                table: "ExerciseDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
