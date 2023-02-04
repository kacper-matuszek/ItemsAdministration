using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemsAdministration.Infrastructure.EF.PostgreSql.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_colors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    annotations = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "colors",
                columns: new[] { "id", "createdat", "name", "updatedat", "version" },
                values: new object[,]
                {
                    { new Guid("69e93e75-8f18-456c-820c-e6db63bbffba"), new DateTime(2023, 2, 4, 11, 34, 25, 565, DateTimeKind.Utc).AddTicks(7652), "Czarny", null, 1L },
                    { new Guid("9f13d328-75d9-473c-81af-c41111418fed"), new DateTime(2023, 2, 4, 11, 34, 25, 565, DateTimeKind.Utc).AddTicks(7654), "Czerwony", null, 1L },
                    { new Guid("e418be2b-08d4-4ed9-95a0-549e587d933e"), new DateTime(2023, 2, 4, 11, 34, 25, 565, DateTimeKind.Utc).AddTicks(7655), "Zielony", null, 1L }
                });

            migrationBuilder.CreateIndex(
                name: "ix_colors_name",
                table: "colors",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_items_code",
                table: "items",
                column: "code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "colors");

            migrationBuilder.DropTable(
                name: "items");
        }
    }
}
