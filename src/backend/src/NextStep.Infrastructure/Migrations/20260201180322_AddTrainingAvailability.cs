using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextStep.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilityFriday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityMonday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilitySaturday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilitySunday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityThursday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityTuesday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityWednesday",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilityFriday",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AvailabilityMonday",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AvailabilitySaturday",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AvailabilitySunday",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AvailabilityThursday",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AvailabilityTuesday",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "AvailabilityWednesday",
                table: "Athletes");
        }
    }
}
