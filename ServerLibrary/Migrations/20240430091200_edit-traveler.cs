using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class edittraveler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRoles");

            migrationBuilder.AddColumn<string>(
                name: "TravelerEmail",
                table: "UserRoles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelerEmail",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
