using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class UpdateStoreEntityVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vote",
                table: "Stores",
                newName: "VotePositive");

            migrationBuilder.AddColumn<int>(
                name: "VoteNegative",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 7, 12, 14, 22, 44, 524, DateTimeKind.Local).AddTicks(7183));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteNegative",
                table: "Stores");

            migrationBuilder.RenameColumn(
                name: "VotePositive",
                table: "Stores",
                newName: "Vote");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2021, 7, 12, 14, 15, 39, 0, DateTimeKind.Local).AddTicks(8118));
        }
    }
}
