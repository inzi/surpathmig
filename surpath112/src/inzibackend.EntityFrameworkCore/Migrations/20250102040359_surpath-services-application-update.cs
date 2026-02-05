using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class surpathservicesapplicationupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CohortUserId",
                table: "TenantSurpathServices",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<long>(
                name: "OrganizationUnitId",
                table: "TenantSurpathServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSurpathServices_CohortUserId",
                table: "TenantSurpathServices",
                column: "CohortUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSurpathServices_OrganizationUnitId",
                table: "TenantSurpathServices",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSurpathServices_AbpOrganizationUnits_OrganizationUnitId",
                table: "TenantSurpathServices",
                column: "OrganizationUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSurpathServices_CohortUsers_CohortUserId",
                table: "TenantSurpathServices",
                column: "CohortUserId",
                principalTable: "CohortUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSurpathServices_AbpOrganizationUnits_OrganizationUnitId",
                table: "TenantSurpathServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantSurpathServices_CohortUsers_CohortUserId",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantSurpathServices_CohortUserId",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantSurpathServices_OrganizationUnitId",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "CohortUserId",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitId",
                table: "TenantSurpathServices");
        }
    }
}
