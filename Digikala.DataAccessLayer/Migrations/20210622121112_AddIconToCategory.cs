using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class AddIconToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Category",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 6, 22, 16, 41, 11, 347, DateTimeKind.Local).AddTicks(2497));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Category");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 6, 22, 16, 34, 11, 143, DateTimeKind.Local).AddTicks(994));
        }
    }
}
