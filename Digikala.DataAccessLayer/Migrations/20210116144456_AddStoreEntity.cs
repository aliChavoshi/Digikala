using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class AddStoreEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Role");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Role",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Tel = table.Column<string>(maxLength: 50, nullable: true),
                    Mail = table.Column<string>(maxLength: 100, nullable: true),
                    Logo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Stores_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Role");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
