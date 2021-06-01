using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 5, 29, 14, 5, 45, 463, DateTimeKind.Local).AddTicks(6351));
        }
    }
}
