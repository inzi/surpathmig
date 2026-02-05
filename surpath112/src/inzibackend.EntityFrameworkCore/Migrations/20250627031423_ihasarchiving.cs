using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class ihasarchiving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCurrentDocument",
                table: "RecordStates",
                newName: "IsArchived");

            migrationBuilder.AddColumn<long>(
                name: "ArchivedByUserId",
                table: "RecordStates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedTime",
                table: "RecordStates",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchivedByUserId",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "ArchivedTime",
                table: "RecordStates");

            migrationBuilder.RenameColumn(
                name: "IsArchived",
                table: "RecordStates",
                newName: "IsCurrentDocument");
        }
    }
}
