using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class slotDaysToFullyAudited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "SlotRotationDays",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SlotRotationDays",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SlotRotationDays",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "SlotAvailableDays",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SlotAvailableDays",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SlotAvailableDays",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "SlotRotationDays");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SlotRotationDays");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SlotRotationDays");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "SlotAvailableDays");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SlotAvailableDays");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SlotAvailableDays");
        }
    }
}
