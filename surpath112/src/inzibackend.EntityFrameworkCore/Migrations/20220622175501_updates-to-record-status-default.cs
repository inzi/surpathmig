using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class updatestorecordstatusdefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "RecordStatuses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantDepartmentId",
                table: "RecordStatuses",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecordStatuses_TenantDepartmentId",
                table: "RecordStatuses",
                column: "TenantDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordStatuses_TenantDepartments_TenantDepartmentId",
                table: "RecordStatuses",
                column: "TenantDepartmentId",
                principalTable: "TenantDepartments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordStatuses_TenantDepartments_TenantDepartmentId",
                table: "RecordStatuses");

            migrationBuilder.DropIndex(
                name: "IX_RecordStatuses_TenantDepartmentId",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "Default",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "TenantDepartmentId",
                table: "RecordStatuses");
        }
    }
}
