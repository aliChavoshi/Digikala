using Microsoft.EntityFrameworkCore.Migrations;

namespace Digikala.DataAccessLayer.Migrations
{
    public partial class AddIdentityTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatorUser",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ModifierUser",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatorUser",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ModifierUser",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_CreatorUser",
                table: "User",
                column: "CreatorUser");

            migrationBuilder.CreateIndex(
                name: "IX_User_ModifierUser",
                table: "User",
                column: "ModifierUser");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatorUser",
                table: "User",
                column: "CreatorUser",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ModifierUser",
                table: "User",
                column: "ModifierUser",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
