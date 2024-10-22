using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataAnnotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Trainings",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trainings",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Sets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercise",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exercise");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Exercise",
                type: "time",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exercise",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trainings");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90);

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exercise");

            migrationBuilder.AlterColumn<float>(
                name: "Duration",
                table: "Exercise",
                type: "real",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Duration",
                table: "Exercise",
                type: "real",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }
    }
}
