using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class fixledgerentrydetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "LedgerEntryDetails",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "LedgerEntryDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "LedgerEntryDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "LedgerEntryDetails",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LedgerEntryDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "LedgerEntryDetails",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "LedgerEntryDetails",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "LedgerEntryDetails");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "LedgerEntryDetails");
        }
    }
}
