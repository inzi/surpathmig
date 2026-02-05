using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class FKtoSlotdays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RotationSlotId",
                table: "SlotRotationDays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RotationSlotId",
                table: "SlotAvailableDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SlotRotationDays_RotationSlotId",
                table: "SlotRotationDays",
                column: "RotationSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_SlotAvailableDays_RotationSlotId",
                table: "SlotAvailableDays",
                column: "RotationSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotAvailableDays_RotationSlots_RotationSlotId",
                table: "SlotAvailableDays",
                column: "RotationSlotId",
                principalTable: "RotationSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotRotationDays_RotationSlots_RotationSlotId",
                table: "SlotRotationDays",
                column: "RotationSlotId",
                principalTable: "RotationSlots",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotAvailableDays_RotationSlots_RotationSlotId",
                table: "SlotAvailableDays");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotRotationDays_RotationSlots_RotationSlotId",
                table: "SlotRotationDays");

            migrationBuilder.DropIndex(
                name: "IX_SlotRotationDays_RotationSlotId",
                table: "SlotRotationDays");

            migrationBuilder.DropIndex(
                name: "IX_SlotAvailableDays_RotationSlotId",
                table: "SlotAvailableDays");

            migrationBuilder.DropColumn(
                name: "RotationSlotId",
                table: "SlotRotationDays");

            migrationBuilder.DropColumn(
                name: "RotationSlotId",
                table: "SlotAvailableDays");
        }
    }
}
