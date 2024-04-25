using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServerLibrary.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AdminEmail = table.Column<string>(type: "text", nullable: true),
                    AdminPass = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<string>(type: "text", nullable: false),
                    ReviewTraveler = table.Column<string>(type: "text", nullable: false),
                    ReviewRating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    SiteID = table.Column<string>(type: "text", nullable: false),
                    SiteName = table.Column<string>(type: "text", nullable: false),
                    SiteDesc = table.Column<string>(type: "text", nullable: false),
                    SiteOperatingHour = table.Column<string>(type: "text", nullable: false),
                    SiteRating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.SiteID);
                });

            migrationBuilder.CreateTable(
                name: "Travelers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    ProfilePic = table.Column<string>(type: "text", nullable: true),
                    TravelerEmail = table.Column<string>(type: "text", nullable: true),
                    TravelerPass = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    TravelAddress = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travelers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "Travelers");
        }
    }
}
