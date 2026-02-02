using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class addedsurpathonlytorequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSurpathOnly",
                table: "RecordRequirements",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SurpathServiceId",
                table: "RecordRequirements",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecordRequirements_SurpathServiceId",
                table: "RecordRequirements",
                column: "SurpathServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordRequirements_SurpathServices_SurpathServiceId",
                table: "RecordRequirements",
                column: "SurpathServiceId",
                principalTable: "SurpathServices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordRequirements_SurpathServices_SurpathServiceId",
                table: "RecordRequirements");

            migrationBuilder.DropIndex(
                name: "IX_RecordRequirements_SurpathServiceId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "IsSurpathOnly",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "SurpathServiceId",
                table: "RecordRequirements");
        }
    }
}
