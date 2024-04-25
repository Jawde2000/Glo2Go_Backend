using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Context.Migrations
{
    /// <inheritdoc />
    public partial class siteUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteAddress",
                table: "Sites",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SiteCountry",
                table: "Sites",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "SitePics",
                table: "Sites",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteAddress",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "SiteCountry",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "SitePics",
                table: "Sites");
        }
    }
}
