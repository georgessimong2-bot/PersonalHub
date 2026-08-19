using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubFundCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "SubFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubFunds_CurrencyId",
                table: "SubFunds",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubFunds_Currencies_CurrencyId",
                table: "SubFunds",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubFunds_Currencies_CurrencyId",
                table: "SubFunds");

            migrationBuilder.DropIndex(
                name: "IX_SubFunds_CurrencyId",
                table: "SubFunds");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "SubFunds");
        }
    }
}
