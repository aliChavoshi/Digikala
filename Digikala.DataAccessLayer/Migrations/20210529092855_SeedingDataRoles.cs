using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class SeedingDataRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "IsDeleted", "Title" },
                values: new object[] { 1, false, "کاربر عادی" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "IsDeleted", "Title" },
                values: new object[] { 2, false, "فروشندگان" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "IsDeleted", "Title" },
                values: new object[] { 3, false, "ادمین" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
