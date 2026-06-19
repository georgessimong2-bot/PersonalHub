using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyToBenchmark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "Benchmarks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Benchmarks_CurrencyId",
                table: "Benchmarks",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Benchmarks_Currencies_CurrencyId",
                table: "Benchmarks",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Benchmarks_Currencies_CurrencyId",
                table: "Benchmarks");

            migrationBuilder.DropIndex(
                name: "IX_Benchmarks_CurrencyId",
                table: "Benchmarks");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Benchmarks");
        }
    }
}
