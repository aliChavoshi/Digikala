using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class UpdateIdentityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Role");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "User",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Role",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Role");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Role",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
