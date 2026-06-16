using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFundMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseCurrency",
                table: "Funds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Funds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomicileCountry",
                table: "Funds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FundCode",
                table: "Funds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Funds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LaunchDate",
                table: "Funds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "Funds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubFund",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestmentObjective = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benchmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaunchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubFund_Funds_FundId",
                        column: x => x.FundId,
                        principalTable: "Funds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShareClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubFundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hedged = table.Column<bool>(type: "bit", nullable: false),
                    Distributing = table.Column<bool>(type: "bit", nullable: false),
                    InvestorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagementFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareClass_SubFund_SubFundId",
                        column: x => x.SubFundId,
                        principalTable: "SubFund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareClass_SubFundId",
                table: "ShareClass",
                column: "SubFundId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFund_FundId",
                table: "SubFund",
                column: "FundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareClass");

            migrationBuilder.DropTable(
                name: "SubFund");

            migrationBuilder.DropColumn(
                name: "BaseCurrency",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "DomicileCountry",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "FundCode",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "LaunchDate",
                table: "Funds");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "Funds");
        }
    }
}
