using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARO.Persistence.Migrations
{
    public partial class AddGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "442b043d-dce9-43b7-bbf9-df4b95776fcb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5234f114-4ab3-4c02-97f3-74a991160685");

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserGroups_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
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
                    { "267ef025-ca21-45fa-aa2d-3319a8c173ac", "fd7b774b-a3f2-41bf-80ce-23236a0c4c80", "group_member", "GROUP_MEMBER" },
                    { "93d4b5ec-ee06-4fe0-b27e-d1a63561b0ee", "5485b591-e04a-4464-9688-64218ff902ec", "group_admin", "GROUP_ADMIN" },
                    { "9aadc3a7-30bf-47ff-a745-a597bed571f9", "3eb2f22a-574a-4d8b-96e7-d61460cbbfb4", "user", "USER" },
                    { "da33f196-16b6-4822-aebb-89fc27d84eab", "8893547c-927c-4f31-852b-cfb1ba75c258", "guest", "GUEST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_RoleId",
                table: "UserGroups",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "267ef025-ca21-45fa-aa2d-3319a8c173ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93d4b5ec-ee06-4fe0-b27e-d1a63561b0ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aadc3a7-30bf-47ff-a745-a597bed571f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da33f196-16b6-4822-aebb-89fc27d84eab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "442b043d-dce9-43b7-bbf9-df4b95776fcb", "75ca3528-041f-4115-ba81-dcf5c8add3d7", "guest", "GUEST" },
                    { "5234f114-4ab3-4c02-97f3-74a991160685", "909a8ffb-9234-4755-8a85-19e8f6d66192", "user", "USER" }
                });
        }
    }
}
