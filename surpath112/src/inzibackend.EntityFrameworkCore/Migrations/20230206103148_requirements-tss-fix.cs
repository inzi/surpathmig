using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class requirementstssfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantSurpathServiceId",
                table: "RecordRequirements",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecordRequirements_TenantSurpathServiceId",
                table: "RecordRequirements",
                column: "TenantSurpathServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordRequirements_TenantSurpathServices_TenantSurpathServic~",
                table: "RecordRequirements",
                column: "TenantSurpathServiceId",
                principalTable: "TenantSurpathServices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordRequirements_TenantSurpathServices_TenantSurpathServic~",
                table: "RecordRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecordRequirements_TenantSurpathServiceId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "TenantSurpathServiceId",
                table: "RecordRequirements");
        }
    }
}
