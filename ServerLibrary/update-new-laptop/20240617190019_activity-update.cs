using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.updatenewlaptop
{
    /// <inheritdoc />
    public partial class activityupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Timetables_TimelineID",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_TimelineID",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityDate",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityDuration",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "TimelineID",
                table: "Activities",
                newName: "TimetableID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Activities",
                newName: "ActivityRegion");

            migrationBuilder.AddColumn<string>(
                name: "ActivityDescription",
                table: "Activities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivityEnd",
                table: "Activities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivityStart",
                table: "Activities",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityDescription",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityEnd",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityStart",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "TimetableID",
                table: "Activities",
                newName: "TimelineID");

            migrationBuilder.RenameColumn(
                name: "ActivityRegion",
                table: "Activities",
                newName: "Description");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ActivityDate",
                table: "Activities",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ActivityDuration",
                table: "Activities",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TimelineID",
                table: "Activities",
                column: "TimelineID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Timetables_TimelineID",
                table: "Activities",
                column: "TimelineID",
                principalTable: "Timetables",
                principalColumn: "TimelineID");
        }
    }
}
