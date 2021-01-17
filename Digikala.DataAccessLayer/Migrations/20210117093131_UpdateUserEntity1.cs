using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class UpdateUserEntity1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmActiveCode",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmEnail",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmMobile",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEnail",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConfirmMobile",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmActiveCode",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
