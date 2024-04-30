using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class mode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Sites_SiteID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_SiteID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "SiteID",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewSite",
                table: "Reviews",
                column: "ReviewSite");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Sites_ReviewSite",
                table: "Reviews",
                column: "ReviewSite",
                principalTable: "Sites",
                principalColumn: "SiteID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Sites_ReviewSite",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewSite",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "SiteID",
                table: "Reviews",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SiteID",
                table: "Reviews",
                column: "SiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Sites_SiteID",
                table: "Reviews",
                column: "SiteID",
                principalTable: "Sites",
                principalColumn: "SiteID");
        }
    }
}
