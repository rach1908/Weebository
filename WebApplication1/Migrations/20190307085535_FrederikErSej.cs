using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class FrederikErSej : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Transaction_TransactionID",
                table: "User");

            migrationBuilder.DropTable(
                name: "MerchVsUser");

            migrationBuilder.DropIndex(
                name: "IX_User_TransactionID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TransactionID",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                table: "Transaction",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_UserId",
                table: "Transaction",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_UserId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_UserId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "TransactionID",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MerchVsUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    TransactionID = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchVsUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MerchVsUser_Transaction_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transaction",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchVsUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_TransactionID",
                table: "User",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchVsUser_TransactionID",
                table: "MerchVsUser",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_MerchVsUser_UserId",
                table: "MerchVsUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Transaction_TransactionID",
                table: "User",
                column: "TransactionID",
                principalTable: "Transaction",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
