using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class ledgercashproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AvailableUserBalance",
                table: "LedgerEntries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PaidAmount",
                table: "LedgerEntries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PaidInCash",
                table: "LedgerEntries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableUserBalance",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "PaidInCash",
                table: "LedgerEntries");
        }
    }
}
