using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARO.Persistence.Migrations
{
    public partial class AddGroupRoleIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupRoleId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupRoleId",
                table: "Users",
                column: "GroupRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetRoles_GroupRoleId",
                table: "Users",
                column: "GroupRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetRoles_GroupRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupRoleId",
                table: "Users");
        }
    }
}
