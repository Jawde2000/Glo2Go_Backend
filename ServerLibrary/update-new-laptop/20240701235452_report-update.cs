using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.updatenewlaptop
{
    /// <inheritdoc />
    public partial class reportupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportEmail",
                table: "Reports",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportEmail",
                table: "Reports");
        }
    }
}
