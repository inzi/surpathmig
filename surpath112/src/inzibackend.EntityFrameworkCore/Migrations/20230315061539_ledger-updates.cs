using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class ledgerupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantSurpathServiceId",
                table: "LedgerEntryDetails",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "CardNameOnCard",
                table: "LedgerEntries",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardZipCode",
                table: "LedgerEntries",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntryDetails_TenantSurpathServiceId",
                table: "LedgerEntryDetails",
                column: "TenantSurpathServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntryDetails_TenantSurpathServices_TenantSurpathServic~",
                table: "LedgerEntryDetails",
                column: "TenantSurpathServiceId",
                principalTable: "TenantSurpathServices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntryDetails_TenantSurpathServices_TenantSurpathServic~",
                table: "LedgerEntryDetails");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntryDetails_TenantSurpathServiceId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "TenantSurpathServiceId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "CardNameOnCard",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "CardZipCode",
                table: "LedgerEntries");
        }
    }
}
