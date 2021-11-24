using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class HotFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarAccessories_TeslaCars_CarId",
                table: "CarAccessories");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "CarAccessories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CarAccessories_TeslaCars_CarId",
                table: "CarAccessories",
                column: "CarId",
                principalTable: "TeslaCars",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarAccessories_TeslaCars_CarId",
                table: "CarAccessories");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "CarAccessories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarAccessories_TeslaCars_CarId",
                table: "CarAccessories",
                column: "CarId",
                principalTable: "TeslaCars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
