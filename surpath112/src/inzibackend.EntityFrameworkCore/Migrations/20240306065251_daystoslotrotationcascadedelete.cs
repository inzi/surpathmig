using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class daystoslotrotationcascadedelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotAvailableDays_RotationSlots_RotationSlotId",
                table: "SlotAvailableDays");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotRotationDays_RotationSlots_RotationSlotId",
                table: "SlotRotationDays");

            migrationBuilder.AddForeignKey(
                name: "FK_SlotAvailableDays_RotationSlots_RotationSlotId",
                table: "SlotAvailableDays",
                column: "RotationSlotId",
                principalTable: "RotationSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SlotRotationDays_RotationSlots_RotationSlotId",
                table: "SlotRotationDays",
                column: "RotationSlotId",
                principalTable: "RotationSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlotAvailableDays_RotationSlots_RotationSlotId",
                table: "SlotAvailableDays");

            migrationBuilder.DropForeignKey(
                name: "FK_SlotRotationDays_RotationSlots_RotationSlotId",
                table: "SlotRotationDays");

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
    }
}
