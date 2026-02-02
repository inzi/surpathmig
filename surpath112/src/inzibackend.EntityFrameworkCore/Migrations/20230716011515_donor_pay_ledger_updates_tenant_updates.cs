using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class donor_pay_ledger_updates_tenant_updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountPaid",
                table: "LedgerEntryDetails",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePaidOn",
                table: "LedgerEntryDetails",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BalanceForward",
                table: "LedgerEntries",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DonorPayPromptDelayUntil",
                table: "AbpUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeferDonorPerpetualPay",
                table: "AbpTenants",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "DatePaidOn",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "BalanceForward",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "DonorPayPromptDelayUntil",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsDeferDonorPerpetualPay",
                table: "AbpTenants");
        }
    }
}
