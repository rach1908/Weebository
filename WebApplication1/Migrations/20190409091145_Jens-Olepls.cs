using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class JensOlepls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Merchandise_MerchandiseID",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "MerchandiseID",
                table: "Transaction",
                newName: "MerchandiseId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_MerchandiseID",
                table: "Transaction",
                newName: "IX_Transaction_MerchandiseId");

            migrationBuilder.AlterColumn<int>(
                name: "MerchandiseId",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Merchandise_MerchandiseId",
                table: "Transaction",
                column: "MerchandiseId",
                principalTable: "Merchandise",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Merchandise_MerchandiseId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "MerchandiseId",
                table: "Transaction",
                newName: "MerchandiseID");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_MerchandiseId",
                table: "Transaction",
                newName: "IX_Transaction_MerchandiseID");

            migrationBuilder.AlterColumn<int>(
                name: "MerchandiseID",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Merchandise_MerchandiseID",
                table: "Transaction",
                column: "MerchandiseID",
                principalTable: "Merchandise",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
