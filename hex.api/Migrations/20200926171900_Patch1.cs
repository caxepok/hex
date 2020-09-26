using Microsoft.EntityFrameworkCore.Migrations;

namespace hex.api.Migrations
{
    public partial class Patch1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContainerPlaces_Containers_WarehouseId",
                table: "ContainerPlaces");

            migrationBuilder.AddForeignKey(
                name: "FK_ContainerPlaces_Warehouses_WarehouseId",
                table: "ContainerPlaces",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContainerPlaces_Warehouses_WarehouseId",
                table: "ContainerPlaces");

            migrationBuilder.AddForeignKey(
                name: "FK_ContainerPlaces_Containers_WarehouseId",
                table: "ContainerPlaces",
                column: "WarehouseId",
                principalTable: "Containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
