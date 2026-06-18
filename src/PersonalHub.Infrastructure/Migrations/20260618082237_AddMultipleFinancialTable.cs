using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleFinancialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareClass_SubFund_SubFundId",
                table: "ShareClass");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFund_Funds_FundId",
                table: "SubFund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubFund",
                table: "SubFund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareClass",
                table: "ShareClass");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "SubFund");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ShareClass");

            migrationBuilder.DropColumn(
                name: "InvestorType",
                table: "ShareClass");

            migrationBuilder.RenameTable(
                name: "SubFund",
                newName: "SubFunds");

            migrationBuilder.RenameTable(
                name: "ShareClass",
                newName: "ShareClasses");

            migrationBuilder.RenameColumn(
                name: "Benchmark",
                table: "SubFunds",
                newName: "SectorFocus");

            migrationBuilder.RenameIndex(
                name: "IX_SubFund_FundId",
                table: "SubFunds",
                newName: "IX_SubFunds_FundId");

            migrationBuilder.RenameColumn(
                name: "Hedged",
                table: "ShareClasses",
                newName: "IsInstitutional");

            migrationBuilder.RenameColumn(
                name: "Distributing",
                table: "ShareClasses",
                newName: "IsHedged");

            migrationBuilder.RenameIndex(
                name: "IX_ShareClass_SubFundId",
                table: "ShareClasses",
                newName: "IX_ShareClasses_SubFundId");

            migrationBuilder.AddColumn<Guid>(
                name: "AssetClassId",
                table: "SubFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BenchmarkId",
                table: "SubFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeographicFocus",
                table: "SubFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalCode",
                table: "SubFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvestmentPolicy",
                table: "SubFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnboardingDate",
                table: "SubFunds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskProfile",
                table: "SubFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SfdrClassificationId",
                table: "SubFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "ShareClasses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDistribution",
                table: "ShareClasses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LaunchDate",
                table: "ShareClasses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerformanceFee",
                table: "ShareClasses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubFunds",
                table: "SubFunds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareClasses",
                table: "ShareClasses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssetClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Benchmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloombergTicker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReutersCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benchmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SfdrClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SfdrClassifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubFunds_AssetClassId",
                table: "SubFunds",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFunds_BenchmarkId",
                table: "SubFunds",
                column: "BenchmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFunds_SfdrClassificationId",
                table: "SubFunds",
                column: "SfdrClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareClasses_CurrencyId",
                table: "ShareClasses",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareClasses_Currencies_CurrencyId",
                table: "ShareClasses",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareClasses_SubFunds_SubFundId",
                table: "ShareClasses",
                column: "SubFundId",
                principalTable: "SubFunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_AssetClasses_AssetClassId",
                table: "SubFunds",
                column: "AssetClassId",
                principalTable: "AssetClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_Benchmarks_BenchmarkId",
                table: "SubFunds",
                column: "BenchmarkId",
                principalTable: "Benchmarks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_Funds_FundId",
                table: "SubFunds",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_SfdrClassifications_SfdrClassificationId",
                table: "SubFunds",
                column: "SfdrClassificationId",
                principalTable: "SfdrClassifications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareClasses_Currencies_CurrencyId",
                table: "ShareClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareClasses_SubFunds_SubFundId",
                table: "ShareClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_AssetClasses_AssetClassId",
                table: "SubFunds");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_Benchmarks_BenchmarkId",
                table: "SubFunds");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_Funds_FundId",
                table: "SubFunds");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_SfdrClassifications_SfdrClassificationId",
                table: "SubFunds");

            migrationBuilder.DropTable(
                name: "AssetClasses");

            migrationBuilder.DropTable(
                name: "Benchmarks");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "SfdrClassifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubFunds",
                table: "SubFunds");

            migrationBuilder.DropIndex(
                name: "IX_SubFunds_AssetClassId",
                table: "SubFunds");

            migrationBuilder.DropIndex(
                name: "IX_SubFunds_BenchmarkId",
                table: "SubFunds");

            migrationBuilder.DropIndex(
                name: "IX_SubFunds_SfdrClassificationId",
                table: "SubFunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareClasses",
                table: "ShareClasses");

            migrationBuilder.DropIndex(
                name: "IX_ShareClasses_CurrencyId",
                table: "ShareClasses");

            migrationBuilder.DropColumn(
                name: "AssetClassId",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "BenchmarkId",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "GeographicFocus",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "InvestmentPolicy",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "OnboardingDate",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "RiskProfile",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "SfdrClassificationId",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ShareClasses");

            migrationBuilder.DropColumn(
                name: "IsDistribution",
                table: "ShareClasses");

            migrationBuilder.DropColumn(
                name: "LaunchDate",
                table: "ShareClasses");

            migrationBuilder.DropColumn(
                name: "PerformanceFee",
                table: "ShareClasses");

            migrationBuilder.RenameTable(
                name: "SubFunds",
                newName: "SubFund");

            migrationBuilder.RenameTable(
                name: "ShareClasses",
                newName: "ShareClass");

            migrationBuilder.RenameColumn(
                name: "SectorFocus",
                table: "SubFund",
                newName: "Benchmark");

            migrationBuilder.RenameIndex(
                name: "IX_SubFunds_FundId",
                table: "SubFund",
                newName: "IX_SubFund_FundId");

            migrationBuilder.RenameColumn(
                name: "IsInstitutional",
                table: "ShareClass",
                newName: "Hedged");

            migrationBuilder.RenameColumn(
                name: "IsHedged",
                table: "ShareClass",
                newName: "Distributing");

            migrationBuilder.RenameIndex(
                name: "IX_ShareClasses_SubFundId",
                table: "ShareClass",
                newName: "IX_ShareClass_SubFundId");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "SubFund",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ShareClass",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvestorType",
                table: "ShareClass",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubFund",
                table: "SubFund",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareClass",
                table: "ShareClass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareClass_SubFund_SubFundId",
                table: "ShareClass",
                column: "SubFundId",
                principalTable: "SubFund",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubFund_Funds_FundId",
                table: "SubFund",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
