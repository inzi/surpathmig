using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class fromPIDtoPid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPids_PIDTypes_PIDTypeId",
                table: "UserPids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PIDTypes",
                table: "PIDTypes");

            migrationBuilder.RenameTable(
                name: "PIDTypes",
                newName: "PidTypes");

            migrationBuilder.RenameColumn(
                name: "PIDTypeId",
                table: "UserPids",
                newName: "PidTypeId");

            migrationBuilder.RenameColumn(
                name: "PID",
                table: "UserPids",
                newName: "Pid");

            migrationBuilder.RenameIndex(
                name: "IX_UserPids_PIDTypeId",
                table: "UserPids",
                newName: "IX_UserPids_PidTypeId");

            migrationBuilder.RenameColumn(
                name: "PIDRegex",
                table: "PidTypes",
                newName: "PidRegex");

            migrationBuilder.RenameIndex(
                name: "IX_PIDTypes_TenantId",
                table: "PidTypes",
                newName: "IX_PidTypes_TenantId");

            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "PidTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PidTypes",
                table: "PidTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPids_PidTypes_PidTypeId",
                table: "UserPids",
                column: "PidTypeId",
                principalTable: "PidTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPids_PidTypes_PidTypeId",
                table: "UserPids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PidTypes",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "Required",
                table: "PidTypes");

            migrationBuilder.RenameTable(
                name: "PidTypes",
                newName: "PIDTypes");

            migrationBuilder.RenameColumn(
                name: "PidTypeId",
                table: "UserPids",
                newName: "PIDTypeId");

            migrationBuilder.RenameColumn(
                name: "Pid",
                table: "UserPids",
                newName: "PID");

            migrationBuilder.RenameIndex(
                name: "IX_UserPids_PidTypeId",
                table: "UserPids",
                newName: "IX_UserPids_PIDTypeId");

            migrationBuilder.RenameColumn(
                name: "PidRegex",
                table: "PIDTypes",
                newName: "PIDRegex");

            migrationBuilder.RenameIndex(
                name: "IX_PidTypes_TenantId",
                table: "PIDTypes",
                newName: "IX_PIDTypes_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PIDTypes",
                table: "PIDTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPids_PIDTypes_PIDTypeId",
                table: "UserPids",
                column: "PIDTypeId",
                principalTable: "PIDTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
