using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class uplifttenantsurpathservices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CohortId",
                table: "TenantSurpathServices",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TenantSurpathServices",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TenantSurpathServices",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "TenantSurpathServices",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantDepartmentId",
                table: "TenantSurpathServices",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "TenantSurpathServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSurpathServices_CohortId",
                table: "TenantSurpathServices",
                column: "CohortId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSurpathServices_TenantDepartmentId",
                table: "TenantSurpathServices",
                column: "TenantDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSurpathServices_UserId",
                table: "TenantSurpathServices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSurpathServices_AbpUsers_UserId",
                table: "TenantSurpathServices",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSurpathServices_Cohorts_CohortId",
                table: "TenantSurpathServices",
                column: "CohortId",
                principalTable: "Cohorts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSurpathServices_TenantDepartments_TenantDepartmentId",
                table: "TenantSurpathServices",
                column: "TenantDepartmentId",
                principalTable: "TenantDepartments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSurpathServices_AbpUsers_UserId",
                table: "TenantSurpathServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantSurpathServices_Cohorts_CohortId",
                table: "TenantSurpathServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantSurpathServices_TenantDepartments_TenantDepartmentId",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantSurpathServices_CohortId",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantSurpathServices_TenantDepartmentId",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantSurpathServices_UserId",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "CohortId",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "TenantDepartmentId",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TenantSurpathServices");
        }
    }
}
