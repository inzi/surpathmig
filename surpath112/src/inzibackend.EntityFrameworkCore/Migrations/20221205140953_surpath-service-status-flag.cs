using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class surpathservicestatusflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecordCategoryRuleId",
                table: "TenantSurpathServices",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "RecordCategoryRuleId",
                table: "SurpathServices",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsSurpathServiceStatus",
                table: "RecordStatuses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSurpathServices_RecordCategoryRuleId",
                table: "TenantSurpathServices",
                column: "RecordCategoryRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SurpathServices_RecordCategoryRuleId",
                table: "SurpathServices",
                column: "RecordCategoryRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurpathServices_RecordCategoryRules_RecordCategoryRuleId",
                table: "SurpathServices",
                column: "RecordCategoryRuleId",
                principalTable: "RecordCategoryRules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSurpathServices_RecordCategoryRules_RecordCategoryRule~",
                table: "TenantSurpathServices",
                column: "RecordCategoryRuleId",
                principalTable: "RecordCategoryRules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurpathServices_RecordCategoryRules_RecordCategoryRuleId",
                table: "SurpathServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantSurpathServices_RecordCategoryRules_RecordCategoryRule~",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantSurpathServices_RecordCategoryRuleId",
                table: "TenantSurpathServices");

            migrationBuilder.DropIndex(
                name: "IX_SurpathServices_RecordCategoryRuleId",
                table: "SurpathServices");

            migrationBuilder.DropColumn(
                name: "RecordCategoryRuleId",
                table: "TenantSurpathServices");

            migrationBuilder.DropColumn(
                name: "RecordCategoryRuleId",
                table: "SurpathServices");

            migrationBuilder.DropColumn(
                name: "IsSurpathServiceStatus",
                table: "RecordStatuses");
        }
    }
}
