using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class medicalunitstorotationslots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalUnitId",
                table: "RotationSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RotationSlots_MedicalUnitId",
                table: "RotationSlots",
                column: "MedicalUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_RotationSlots_MedicalUnits_MedicalUnitId",
                table: "RotationSlots",
                column: "MedicalUnitId",
                principalTable: "MedicalUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RotationSlots_MedicalUnits_MedicalUnitId",
                table: "RotationSlots");

            migrationBuilder.DropIndex(
                name: "IX_RotationSlots_MedicalUnitId",
                table: "RotationSlots");

            migrationBuilder.DropColumn(
                name: "MedicalUnitId",
                table: "RotationSlots");
        }
    }
}
