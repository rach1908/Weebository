using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class NewFriendModelWithBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequestAccepted",
                table: "FriendEntry",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestAccepted",
                table: "FriendEntry");
        }
    }
}
