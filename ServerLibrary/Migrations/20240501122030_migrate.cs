using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "RefreshTokenInfos");

            migrationBuilder.AddColumn<string>(
                name: "TravelerEmail",
                table: "RefreshTokenInfos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelerEmail",
                table: "RefreshTokenInfos");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "RefreshTokenInfos",
                type: "integer",
                nullable: true);
        }
    }
}
