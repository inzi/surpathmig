using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class Added_ExpiredStatus_To_RecordCategoryRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExpiredStatusId",
                table: "RecordCategoryRules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecordCategoryRules_ExpiredStatusId",
                table: "RecordCategoryRules",
                column: "ExpiredStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_ExpiredStatusId",
                table: "RecordCategoryRules",
                column: "ExpiredStatusId",
                principalTable: "RecordStatuses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordCategoryRules_RecordStatuses_ExpiredStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropIndex(
                name: "IX_RecordCategoryRules_ExpiredStatusId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "ExpiredStatusId",
                table: "RecordCategoryRules");
        }
    }
}
