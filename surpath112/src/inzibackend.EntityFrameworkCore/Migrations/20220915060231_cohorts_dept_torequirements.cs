using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class cohorts_dept_torequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CohortId",
                table: "RecordRequirements",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantDepartmentId",
                table: "RecordRequirements",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecordRequirements_CohortId",
                table: "RecordRequirements",
                column: "CohortId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordRequirements_TenantDepartmentId",
                table: "RecordRequirements",
                column: "TenantDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordRequirements_Cohorts_CohortId",
                table: "RecordRequirements",
                column: "CohortId",
                principalTable: "Cohorts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordRequirements_TenantDepartments_TenantDepartmentId",
                table: "RecordRequirements",
                column: "TenantDepartmentId",
                principalTable: "TenantDepartments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordRequirements_Cohorts_CohortId",
                table: "RecordRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordRequirements_TenantDepartments_TenantDepartmentId",
                table: "RecordRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecordRequirements_CohortId",
                table: "RecordRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecordRequirements_TenantDepartmentId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "CohortId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "TenantDepartmentId",
                table: "RecordRequirements");
        }
    }
}
