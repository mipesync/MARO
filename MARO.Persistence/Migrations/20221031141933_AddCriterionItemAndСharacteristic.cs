using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MARO.Persistence.Migrations
{
    public partial class AddCriterionItemAndСharacteristic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criteria_Criteria_ParentId",
                table: "Criteria");

            migrationBuilder.DropIndex(
                name: "IX_Criteria_ParentId",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Criteria");

            migrationBuilder.CreateTable(
                name: "Сharacteristics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ages = table.Column<List<int>>(type: "integer[]", nullable: true),
                    ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Place = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CriterionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Сharacteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Сharacteristics_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Сharacteristics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CriterionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CriterionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriterionItems_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b122a285-fb65-4ca6-b95b-4dd30aa4f7a7", "b1d35655-9634-4504-9eb6-2e15ee3424a5", "guest", "GUEST" },
                    { "b1b990f5-251d-4a0e-9567-cf2810176f3d", "8983d5ad-414e-42fe-9152-75d7d45174c0", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Criteria",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "wishes" },
                    { 2, "pastime" },
                    { 3, "characteristics" }
                });

            migrationBuilder.InsertData(
                table: "CriterionItems",
                columns: new[] { "Id", "CriterionId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "free" },
                    { 2, 1, "fee" },
                    { 3, 1, "walking" },
                    { 4, 1, "electrobus" },
                    { 5, 1, "more_people" },
                    { 6, 1, "less_people" },
                    { 7, 1, "street" },
                    { 8, 1, "room" },
                    { 9, 1, "limited_health" },
                    { 10, 2, "museums_permanentExhibits" },
                    { 11, 2, "museums_temporaryExhibits" },
                    { 12, 2, "museums_excursions" },
                    { 13, 2, "entertaining_festivals" },
                    { 14, 2, "entertaining_concerts" },
                    { 15, 2, "entertaining_attractions" },
                    { 16, 2, "educational_masterClasses" },
                    { 17, 2, "educational_lectures" },
                    { 18, 2, "sports_masterClasses" },
                    { 19, 2, "sports_races" },
                    { 20, 2, "gastronomic_festivals" },
                    { 21, 2, "business_expo" },
                    { 22, 2, "eating_cafe" },
                    { 23, 2, "eating_restaurant" },
                    { 24, 2, "eating_streetfood" },
                    { 25, 2, "walking_fountains" },
                    { 26, 2, "walking_rocket" },
                    { 27, 2, "walking_architecture" },
                    { 28, 2, "walking_ponds" },
                    { 29, 2, "walking_botanicalGarden" },
                    { 30, 2, "walking_infoCenter" },
                    { 31, 2, "walking_mothersRoom" },
                    { 32, 2, "walking_toilets" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Сharacteristics_CriterionId",
                table: "Сharacteristics",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_Сharacteristics_UserId",
                table: "Сharacteristics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CriterionItems_CriterionId",
                table: "CriterionItems",
                column: "CriterionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Сharacteristics");

            migrationBuilder.DropTable(
                name: "CriterionItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b122a285-fb65-4ca6-b95b-4dd30aa4f7a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1b990f5-251d-4a0e-9567-cf2810176f3d");

            migrationBuilder.DeleteData(
                table: "Criteria",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Criteria",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Criteria",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Criteria",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_ParentId",
                table: "Criteria",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criteria_Criteria_ParentId",
                table: "Criteria",
                column: "ParentId",
                principalTable: "Criteria",
                principalColumn: "Id");
        }
    }
}
