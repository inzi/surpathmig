using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class removefkrecordrequirement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordRequirements_TenantSurpathServices_TenantSurpathServic~",
                table: "RecordRequirements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_RecordRequirements_TenantSurpathServices_TenantSurpathServic~",
                table: "RecordRequirements",
                column: "TenantSurpathServiceId",
                principalTable: "TenantSurpathServices",
                principalColumn: "Id");
        }
    }
}
