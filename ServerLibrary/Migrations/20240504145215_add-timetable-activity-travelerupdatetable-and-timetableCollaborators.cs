using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addtimetableactivitytravelerupdatetableandtimetableCollaborators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Timetables",
                columns: table => new
                {
                    TimelineID = table.Column<string>(type: "text", nullable: false),
                    TimelineTitle = table.Column<string>(type: "text", nullable: true),
                    TimelineStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TimelineEndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.TimelineID);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityID = table.Column<string>(type: "text", nullable: false),
                    ActivityTitle = table.Column<string>(type: "text", nullable: true),
                    ActivityDuration = table.Column<double>(type: "double precision", nullable: true),
                    ActivityType = table.Column<string>(type: "text", nullable: true),
                    TimelineID = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ActivityDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_Activities_Timetables_TimelineID",
                        column: x => x.TimelineID,
                        principalTable: "Timetables",
                        principalColumn: "TimelineID");
                });

            migrationBuilder.CreateTable(
                name: "TimetableCollaborators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TimelineID = table.Column<string>(type: "text", nullable: true),
                    TravelerEmail = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableCollaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableCollaborators_Timetables_TimelineID",
                        column: x => x.TimelineID,
                        principalTable: "Timetables",
                        principalColumn: "TimelineID");
                    table.ForeignKey(
                        name: "FK_TimetableCollaborators_Travelers_TravelerEmail",
                        column: x => x.TravelerEmail,
                        principalTable: "Travelers",
                        principalColumn: "TravelerEmail");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TimelineID",
                table: "Activities",
                column: "TimelineID");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableCollaborators_TimelineID",
                table: "TimetableCollaborators",
                column: "TimelineID");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableCollaborators_TravelerEmail",
                table: "TimetableCollaborators",
                column: "TravelerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "TimetableCollaborators");

            migrationBuilder.DropTable(
                name: "Timetables");
        }
    }
}
