using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    /// <summary>
    /// Fixes NULL values in IsArchived column that may occur after database restore
    /// or incomplete migration from IsCurrentDocument to IsArchived
    /// </summary>
    public partial class fixisarchivednulls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update any NULL IsArchived values to false (0)
            // This ensures all existing records are considered "current" by default
            migrationBuilder.Sql(@"
                UPDATE RecordStates 
                SET IsArchived = 0 
                WHERE IsArchived IS NULL
            ");

            // Ensure the column is NOT NULL with default value false
            // This prevents future NULL values
            migrationBuilder.AlterColumn<bool>(
                name: "IsArchived",
                table: "RecordStates",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to nullable if rolling back
            migrationBuilder.AlterColumn<bool>(
                name: "IsArchived",
                table: "RecordStates",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: false,
                oldDefaultValue: false);
        }
    }
}