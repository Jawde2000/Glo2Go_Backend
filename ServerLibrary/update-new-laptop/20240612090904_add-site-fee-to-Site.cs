using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.updatenewlaptop
{
    /// <inheritdoc />
    public partial class addsitefeetoSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteFee",
                table: "Sites",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteFee",
                table: "Sites");
        }
    }
}
