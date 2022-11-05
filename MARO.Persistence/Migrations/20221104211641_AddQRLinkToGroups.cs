using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARO.Persistence.Migrations
{
    public partial class AddQRLinkToGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    QRLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetRoles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.InsertData(
                table: "CriterionItems",
                columns: new[] { "Id", "CriterionId", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "free", null },
                    { 2, 1, "fee", null },
                    { 3, 1, "walking", null },
                    { 4, 1, "electrobus", null },
                    { 5, 1, "more_people", null },
                    { 6, 1, "less_people", null },
                    { 7, 1, "street", null },
                    { 8, 1, "room", null },
                    { 9, 1, "limited_health", null },
                    { 10, 2, "museums_permanentExhibits", null },
                    { 11, 2, "museums_temporaryExhibits", null },
                    { 12, 2, "museums_excursions", null },
                    { 13, 2, "entertaining_festivals", null },
                    { 14, 2, "entertaining_concerts", null },
                    { 15, 2, "entertaining_attractions", null },
                    { 16, 2, "educational_masterClasses", null },
                    { 17, 2, "educational_lectures", null },
                    { 18, 2, "sports_masterClasses", null },
                    { 19, 2, "sports_races", null },
                    { 20, 2, "gastronomic_festivals", null },
                    { 21, 2, "business_expo", null },
                    { 22, 2, "eating_cafe", null },
                    { 23, 2, "eating_restaurant", null },
                    { 24, 2, "eating_streetfood", null },
                    { 25, 2, "walking_fountains", null },
                    { 26, 2, "walking_rocket", null },
                    { 27, 2, "walking_architecture", null },
                    { 28, 2, "walking_ponds", null },
                    { 29, 2, "walking_botanicalGarden", null },
                    { 30, 2, "walking_infoCenter", null },
                    { 31, 2, "walking_mothersRoom", null },
                    { 32, 2, "walking_toilets", null },
                    { 33, 2, "sports_periodicEvents", null }
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
    }
}
