using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Context.Migrations
{
    /// <inheritdoc />
    public partial class addgenderToTraveler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Travelers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Travelers");
        }
    }
}
