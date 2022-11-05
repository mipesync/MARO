using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MARO.Persistence.Migrations
{
    public partial class SmallUpdateItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "595739b1-140a-4e5a-b472-fd4895e547a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e88c2716-9cdb-4415-a358-ce61b371698e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "442b043d-dce9-43b7-bbf9-df4b95776fcb", "75ca3528-041f-4115-ba81-dcf5c8add3d7", "guest", "GUEST" },
                    { "5234f114-4ab3-4c02-97f3-74a991160685", "909a8ffb-9234-4755-8a85-19e8f6d66192", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "CriterionItems",
                columns: new[] { "Id", "CriterionId", "Name", "UserId" },
                values: new object[] { 33, 2, "sports_periodicEvents", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "442b043d-dce9-43b7-bbf9-df4b95776fcb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5234f114-4ab3-4c02-97f3-74a991160685");

            migrationBuilder.DeleteData(
                table: "CriterionItems",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "595739b1-140a-4e5a-b472-fd4895e547a3", "3c060337-7c6b-4921-b82c-4c9cf8bcc1a7", "user", "USER" },
                    { "e88c2716-9cdb-4415-a358-ce61b371698e", "26797ffb-2a4b-46fb-a7f1-e81b5ba0b76c", "guest", "GUEST" }
                });
        }
    }
}
