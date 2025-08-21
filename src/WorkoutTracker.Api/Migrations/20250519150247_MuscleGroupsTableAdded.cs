using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class MuscleGroupsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TrainingSets");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "TrainingSets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TrainingExercises");

            migrationBuilder.RenameColumn(
                name: "EstimatedDurationTimeMinutes",
                table: "TrainingSessions",
                newName: "EstimatedDurationMinutes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TrainingExercises",
                newName: "OrderInSession");

            migrationBuilder.AlterColumn<int>(
                name: "DistanceMeters",
                table: "TrainingSets",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationSeconds",
                table: "TrainingSets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderInExercise",
                table: "TrainingSets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightKg",
                table: "TrainingSets",
                type: "decimal(6,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainingSessions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TrainingSessions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TrainingSessions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "TrainingExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExerciseType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equipment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseMuscleGroupLinks",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscleGroupLinks", x => new { x.ExerciseId, x.MuscleGroup });
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleGroupLinks_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_UserId",
                table: "TrainingSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingExercises_ExerciseId",
                table: "TrainingExercises",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingExercises_Exercises_ExerciseId",
                table: "TrainingExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSessions_AspNetUsers_UserId",
                table: "TrainingSessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingExercises_Exercises_ExerciseId",
                table: "TrainingExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSessions_AspNetUsers_UserId",
                table: "TrainingSessions");

            migrationBuilder.DropTable(
                name: "ExerciseMuscleGroupLinks");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_TrainingSessions_UserId",
                table: "TrainingSessions");

            migrationBuilder.DropIndex(
                name: "IX_TrainingExercises_ExerciseId",
                table: "TrainingExercises");

            migrationBuilder.DropColumn(
                name: "DurationSeconds",
                table: "TrainingSets");

            migrationBuilder.DropColumn(
                name: "OrderInExercise",
                table: "TrainingSets");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "TrainingSets");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "TrainingExercises");

            migrationBuilder.RenameColumn(
                name: "EstimatedDurationMinutes",
                table: "TrainingSessions",
                newName: "EstimatedDurationTimeMinutes");

            migrationBuilder.RenameColumn(
                name: "OrderInSession",
                table: "TrainingExercises",
                newName: "Type");

            migrationBuilder.AlterColumn<double>(
                name: "DistanceMeters",
                table: "TrainingSets",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Duration",
                table: "TrainingSets",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "TrainingSets",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainingSessions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TrainingSessions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TrainingSessions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TrainingExercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
