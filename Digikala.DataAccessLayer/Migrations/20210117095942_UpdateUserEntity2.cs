using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class UpdateUserEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEnail",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmEmail",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEmail",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmEnail",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
