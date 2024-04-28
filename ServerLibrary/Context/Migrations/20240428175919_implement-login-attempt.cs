using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Context.Migrations
{
    /// <inheritdoc />
    public partial class implementloginattempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempt",
                table: "Travelers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Travelers",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedLoginAttempt",
                table: "Travelers");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Travelers");
        }
    }
}
