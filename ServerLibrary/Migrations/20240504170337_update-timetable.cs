using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updatetimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TravelerEmail",
                table: "Timetables",
                type: "text",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_Travelers_TravelerEmail",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_TravelerEmail",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "TravelerEmail",
                table: "Timetables");
        }
    }
}
