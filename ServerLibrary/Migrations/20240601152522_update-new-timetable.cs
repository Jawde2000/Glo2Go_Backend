using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_Travelers_TravelerEmail",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_TravelerEmail",
                table: "Timetables");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Timetables",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimetableRegions",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimelineID = table.Column<string>(type: "text", nullable: true),
                    DateStart = table.Column<DateOnly>(type: "date", nullable: true),
                    DateEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeStart = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    TimeEnd = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    RegionName = table.Column<string>(type: "text", nullable: true),
                    TimetableTimelineID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableRegions", x => x.RegionID);
                    table.ForeignKey(
                        name: "FK_TimetableRegions_Timetables_TimetableTimelineID",
                        column: x => x.TimetableTimelineID,
                        principalTable: "Timetables",
                        principalColumn: "TimelineID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableRegions_TimetableTimelineID",
                table: "TimetableRegions",
                column: "TimetableTimelineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimetableRegions");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Timetables");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_TravelerEmail",
                table: "Timetables",
                column: "TravelerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_Travelers_TravelerEmail",
                table: "Timetables",
                column: "TravelerEmail",
                principalTable: "Travelers",
                principalColumn: "TravelerEmail");
        }
    }
}
