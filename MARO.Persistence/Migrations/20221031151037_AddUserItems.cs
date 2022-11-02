using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARO.Persistence.Migrations
{
    public partial class AddUserItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b122a285-fb65-4ca6-b95b-4dd30aa4f7a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1b990f5-251d-4a0e-9567-cf2810176f3d");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CriterionItems",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CriterionItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => new { x.UserId, x.CriterionItemId });
                    table.ForeignKey(
                        name: "FK_UserItems_CriterionItems_CriterionItemId",
                        column: x => x.CriterionItemId,
                        principalTable: "CriterionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "595739b1-140a-4e5a-b472-fd4895e547a3", "3c060337-7c6b-4921-b82c-4c9cf8bcc1a7", "user", "USER" },
                    { "e88c2716-9cdb-4415-a358-ce61b371698e", "26797ffb-2a4b-46fb-a7f1-e81b5ba0b76c", "guest", "GUEST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CriterionItems_UserId",
                table: "CriterionItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_CriterionItemId",
                table: "UserItems",
                column: "CriterionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CriterionItems_Users_UserId",
                table: "CriterionItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriterionItems_Users_UserId",
                table: "CriterionItems");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropIndex(
                name: "IX_CriterionItems_UserId",
                table: "CriterionItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "595739b1-140a-4e5a-b472-fd4895e547a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e88c2716-9cdb-4415-a358-ce61b371698e");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CriterionItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b122a285-fb65-4ca6-b95b-4dd30aa4f7a7", "b1d35655-9634-4504-9eb6-2e15ee3424a5", "guest", "GUEST" },
                    { "b1b990f5-251d-4a0e-9567-cf2810176f3d", "8983d5ad-414e-42fe-9152-75d7d45174c0", "user", "USER" }
                });
        }
    }
}
