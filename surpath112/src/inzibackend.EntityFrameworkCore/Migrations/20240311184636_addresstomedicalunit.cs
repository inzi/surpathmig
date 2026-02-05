using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class addresstomedicalunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "MedicalUnits",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "MedicalUnits",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "MedicalUnits",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "MedicalUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "MedicalUnits",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MedicalUnits",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "MedicalUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "MedicalUnits",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "City",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "State",
                table: "MedicalUnits");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "MedicalUnits");
        }
    }
}
