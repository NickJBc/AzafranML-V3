using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzafranML_V3.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cattle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeightInKg = table.Column<double>(type: "float", nullable: false),
                    FeedType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cattle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyProduction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyProduction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CattleDailyProduction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CattleId = table.Column<int>(type: "int", nullable: false),
                    DailyProductionId = table.Column<int>(type: "int", nullable: false),
                    AmountProduced = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CattleDailyProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CattleDailyProduction_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CattleDailyProduction_DailyProduction_DailyProductionId",
                        column: x => x.DailyProductionId,
                        principalTable: "DailyProduction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CattleDailyProduction_CattleId",
                table: "CattleDailyProduction",
                column: "CattleId");

            migrationBuilder.CreateIndex(
                name: "IX_CattleDailyProduction_DailyProductionId",
                table: "CattleDailyProduction",
                column: "DailyProductionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CattleDailyProduction");

            migrationBuilder.DropTable(
                name: "Cattle");

            migrationBuilder.DropTable(
                name: "DailyProduction");
        }
    }
}
