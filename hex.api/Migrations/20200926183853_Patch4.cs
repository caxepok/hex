using Microsoft.EntityFrameworkCore.Migrations;

namespace hex.api.Migrations
{
    public partial class Patch4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpenSky",
                table: "Warehouses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PlaceCount",
                table: "Warehouses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpenSky",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "PlaceCount",
                table: "Warehouses");
        }
    }
}
