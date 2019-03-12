using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class newDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MerchandiseID",
                table: "Transaction",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Merchandise",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Series = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchandise", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_MerchandiseID",
                table: "Transaction",
                column: "MerchandiseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Merchandise_MerchandiseID",
                table: "Transaction",
                column: "MerchandiseID",
                principalTable: "Merchandise",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Merchandise_MerchandiseID",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Merchandise");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_MerchandiseID",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "MerchandiseID",
                table: "Transaction");
        }
    }
}
