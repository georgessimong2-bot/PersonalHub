using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAssetClassToInvestmentStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_AssetClasses_AssetClassId",
                table: "SubFunds");

            migrationBuilder.DropTable(
                name: "AssetClasses");

            migrationBuilder.RenameColumn(
                name: "AssetClassId",
                table: "SubFunds",
                newName: "InvestmentStrategyId");

            migrationBuilder.RenameIndex(
                name: "IX_SubFunds_AssetClassId",
                table: "SubFunds",
                newName: "IX_SubFunds_InvestmentStrategyId");

            migrationBuilder.CreateTable(
                name: "InvestmentStrategies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentStrategies", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_InvestmentStrategies_InvestmentStrategyId",
                table: "SubFunds",
                column: "InvestmentStrategyId",
                principalTable: "InvestmentStrategies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_InvestmentStrategies_InvestmentStrategyId",
                table: "SubFunds");

            migrationBuilder.DropTable(
                name: "InvestmentStrategies");

            migrationBuilder.RenameColumn(
                name: "InvestmentStrategyId",
                table: "SubFunds",
                newName: "AssetClassId");

            migrationBuilder.RenameIndex(
                name: "IX_SubFunds_InvestmentStrategyId",
                table: "SubFunds",
                newName: "IX_SubFunds_AssetClassId");

            migrationBuilder.CreateTable(
                name: "AssetClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClasses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_AssetClasses_AssetClassId",
                table: "SubFunds",
                column: "AssetClassId",
                principalTable: "AssetClasses",
                principalColumn: "Id");
        }
    }
}
