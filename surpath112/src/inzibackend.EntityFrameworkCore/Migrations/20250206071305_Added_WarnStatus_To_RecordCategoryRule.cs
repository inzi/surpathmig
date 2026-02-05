using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class Added_WarnStatus_To_RecordCategoryRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FinalWarnStatusId",
                table: "RecordCategoryRules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "FirstWarnStatusId",
                table: "RecordCategoryRules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "SecondWarnStatusId",
                table: "RecordCategoryRules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecordCategoryRules_FinalWarnStatusId",
                table: "RecordCategoryRules",
                column: "FinalWarnStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordCategoryRules_FirstWarnStatusId",
                table: "RecordCategoryRules",
                column: "FirstWarnStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordCategoryRules_SecondWarnStatusId",
                table: "RecordCategoryRules",
                column: "SecondWarnStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_FinalWarnStatusId",
                table: "RecordCategoryRules",
                column: "FinalWarnStatusId",
                principalTable: "RecordStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_FirstWarnStatusId",
                table: "RecordCategoryRules",
                column: "FirstWarnStatusId",
                principalTable: "RecordStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_SecondWarnStatusId",
                table: "RecordCategoryRules",
                column: "SecondWarnStatusId",
                principalTable: "RecordStatuses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_FinalWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_FirstWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_SecondWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropIndex(
                name: "IX_RecordCategoryRules_FinalWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropIndex(
                name: "IX_RecordCategoryRules_FirstWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropIndex(
                name: "IX_RecordCategoryRules_SecondWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "FinalWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "FirstWarnStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "SecondWarnStatusId",
                table: "RecordCategoryRules");
        }
    }
}
