using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class SeedDataRolePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "ExpireRolePermission", "PermissionId", "RoleId" },
                values: new object[] { 1, null, 1, 2 });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "ExpireRolePermission", "PermissionId", "RoleId" },
                values: new object[] { 2, null, 1, 3 });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 6, 2, 11, 48, 1, 415, DateTimeKind.Local).AddTicks(5199));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 6, 1, 16, 45, 7, 137, DateTimeKind.Local).AddTicks(2196));
        }
    }
}
