using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class tenantsurpathserviceisenabledtoIsPricingOverrideEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "TenantSurpathServices",
                newName: "IsPricingOverrideEnabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPricingOverrideEnabled",
                table: "TenantSurpathServices",
                newName: "IsEnabled");
        }
    }
}
