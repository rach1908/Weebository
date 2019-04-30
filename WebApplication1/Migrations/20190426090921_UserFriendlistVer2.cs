using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class UserFriendlistVer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FriendEntry_FriendID",
                table: "FriendEntry",
                column: "FriendID");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendEntry_User_FriendID",
                table: "FriendEntry",
                column: "FriendID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendEntry_User_UserID",
                table: "FriendEntry",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendEntry_User_FriendID",
                table: "FriendEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendEntry_User_UserID",
                table: "FriendEntry");

            migrationBuilder.DropIndex(
                name: "IX_FriendEntry_FriendID",
                table: "FriendEntry");
        }
    }
}
