using Microsoft.EntityFrameworkCore.Migrations;

namespace Animerch.Migrations
{
    public partial class FriendEntryRequestSentBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RequestAccepted",
                table: "FriendEntry",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequestSent",
                table: "FriendEntry",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestSent",
                table: "FriendEntry");

            migrationBuilder.AlterColumn<bool>(
                name: "RequestAccepted",
                table: "FriendEntry",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }
    }
}
