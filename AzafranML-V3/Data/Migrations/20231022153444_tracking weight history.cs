using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzafranML_V3.Data.Migrations
{
    public partial class trackingweighthistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CattleWeightHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CattleId = table.Column<int>(type: "int", nullable: false),
                    RecordedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeightInKg = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CattleWeightHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CattleWeightHistories_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CattleWeightHistories_CattleId",
                table: "CattleWeightHistories",
                column: "CattleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CattleWeightHistories");
        }
    }
}
