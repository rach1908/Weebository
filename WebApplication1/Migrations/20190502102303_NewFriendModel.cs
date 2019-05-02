using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class NewFriendModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RequestAccepted",
                table: "FriendEntry");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_FriendEntry_FriendID_UserID",
                table: "FriendEntry",
                columns: new[] { "FriendID", "UserID" });

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendEntry_User_FriendID",
                table: "FriendEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendEntry_User_UserID",
                table: "FriendEntry");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_FriendEntry_FriendID_UserID",
                table: "FriendEntry");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequestAccepted",
                table: "FriendEntry",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserId",
                table: "User",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UserId",
                table: "User",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
