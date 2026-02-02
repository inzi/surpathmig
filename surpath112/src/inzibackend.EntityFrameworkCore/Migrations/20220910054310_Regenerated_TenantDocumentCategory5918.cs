using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class Regenerated_TenantDocumentCategory5918 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantDocumentCategories_AbpUsers_OwnerUserId",
                table: "TenantDocumentCategories");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "TenantDocumentCategories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantDocumentCategories_OwnerUserId",
                table: "TenantDocumentCategories",
                newName: "IX_TenantDocumentCategories_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantDocumentCategories_AbpUsers_UserId",
                table: "TenantDocumentCategories",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantDocumentCategories_AbpUsers_UserId",
                table: "TenantDocumentCategories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TenantDocumentCategories",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantDocumentCategories_UserId",
                table: "TenantDocumentCategories",
                newName: "IX_TenantDocumentCategories_OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantDocumentCategories_AbpUsers_OwnerUserId",
                table: "TenantDocumentCategories",
                column: "OwnerUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }
    }
}
