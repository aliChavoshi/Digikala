using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class SeedingDataUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "ActiveCode", "ConfirmEmail", "ConfirmMobile", "CreateDate", "Description", "Email", "Fullname", "IsActive", "IsDeleted", "Mobile", "ModificationDate", "NationalCode", "Password", "RoleId", "Version" },
                values: new object[] { 1, null, true, true, new DateTime(2021, 5, 29, 14, 5, 45, 463, DateTimeKind.Local).AddTicks(6351), null, "Admin@gmail.com", "Admin", true, false, "09130242780", null, null, "nRy/sK5Z7ENvGsSwfcmLzw==", 3, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
