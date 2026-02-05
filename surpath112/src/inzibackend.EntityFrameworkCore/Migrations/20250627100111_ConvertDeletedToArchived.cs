using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class ConvertDeletedToArchived : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert all soft-deleted records to archived records
            // This preserves the historical documents that were marked as deleted when new versions were uploaded
            migrationBuilder.Sql(@"
                UPDATE RecordStates 
                SET IsArchived = 1, 
                    IsDeleted = 0 
                WHERE IsDeleted = 1;
            ");

            // Copy the deletion audit fields to archiving audit fields
            migrationBuilder.Sql(@"
                UPDATE RecordStates 
                SET ArchivedByUserId = DeleterUserId, 
                    ArchivedTime = DeletionTime 
                WHERE IsArchived = 1;
            ");

            // Ensure all non-archived records have IsArchived set to 0 (handle any NULL values)
            migrationBuilder.Sql(@"
                UPDATE RecordStates 
                SET IsArchived = 0 
                WHERE IsArchived IS NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert archived records back to soft-deleted
            // This restores the original state before the archiving system was introduced
            migrationBuilder.Sql(@"
                UPDATE RecordStates 
                SET IsDeleted = 1,
                    DeleterUserId = ArchivedByUserId,
                    DeletionTime = ArchivedTime
                WHERE IsArchived = 1;
            ");

            // Clear the archiving fields
            migrationBuilder.Sql(@"
                UPDATE RecordStates 
                SET IsArchived = 0,
                    ArchivedByUserId = NULL,
                    ArchivedTime = NULL
                WHERE IsArchived = 1;
            ");
        }
    }
}