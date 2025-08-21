using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class DurationMinutesNameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimatedDurationMinutes",
                table: "TrainingSessions",
                newName: "DurationMinutes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                table: "TrainingSessions",
                newName: "EstimatedDurationMinutes");
        }
    }
}
