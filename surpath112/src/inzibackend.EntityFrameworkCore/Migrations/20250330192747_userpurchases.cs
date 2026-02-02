using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class userpurchases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserPurchaseId",
                table: "LedgerEntryDetails",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsRefund",
                table: "LedgerEntries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "LedgerEntries",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "LedgerEntries",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "UserPurchaseId",
                table: "LedgerEntries",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "UserPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OriginalPrice = table.Column<double>(type: "double", nullable: false),
                    AdjustedPrice = table.Column<double>(type: "double", nullable: false),
                    DiscountAmount = table.Column<double>(type: "double", nullable: false),
                    AmountPaid = table.Column<double>(type: "double", nullable: false),
                    PaymentPeriodType = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsRecurring = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    SurpathServiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TenantSurpathServiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CohortId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPurchases_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPurchases_Cohorts_CohortId",
                        column: x => x.CohortId,
                        principalTable: "Cohorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPurchases_SurpathServices_SurpathServiceId",
                        column: x => x.SurpathServiceId,
                        principalTable: "SurpathServices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPurchases_TenantSurpathServices_TenantSurpathServiceId",
                        column: x => x.TenantSurpathServiceId,
                        principalTable: "TenantSurpathServices",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntryDetails_UserPurchaseId",
                table: "LedgerEntryDetails",
                column: "UserPurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_UserPurchaseId",
                table: "LedgerEntries",
                column: "UserPurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPurchases_CohortId",
                table: "UserPurchases",
                column: "CohortId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPurchases_SurpathServiceId",
                table: "UserPurchases",
                column: "SurpathServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPurchases_TenantId",
                table: "UserPurchases",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPurchases_TenantSurpathServiceId",
                table: "UserPurchases",
                column: "TenantSurpathServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPurchases_UserId",
                table: "UserPurchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_UserPurchases_UserPurchaseId",
                table: "LedgerEntries",
                column: "UserPurchaseId",
                principalTable: "UserPurchases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntryDetails_UserPurchases_UserPurchaseId",
                table: "LedgerEntryDetails",
                column: "UserPurchaseId",
                principalTable: "UserPurchases",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_UserPurchases_UserPurchaseId",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntryDetails_UserPurchases_UserPurchaseId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropTable(
                name: "UserPurchases");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntryDetails_UserPurchaseId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_UserPurchaseId",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "UserPurchaseId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "IsRefund",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "UserPurchaseId",
                table: "LedgerEntries");
        }
    }
}
