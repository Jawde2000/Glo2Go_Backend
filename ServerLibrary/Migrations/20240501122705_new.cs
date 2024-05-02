using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_TravelerEmail",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TravelerEmail",
                table: "Addresses",
                column: "TravelerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_TravelerEmail",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TravelerEmail",
                table: "Addresses",
                column: "TravelerEmail",
                unique: true);
        }
    }
}
