using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class AddMigrationAuditLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MigrationAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MigrationId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MigrationType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CohortId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CohortName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SourceDepartmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SourceDepartmentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TargetDepartmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TargetDepartmentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsNewDepartment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AffectedUsersCount = table.Column<int>(type: "int", nullable: false),
                    AffectedRecordsCount = table.Column<int>(type: "int", nullable: false),
                    RequirementCategoriesCount = table.Column<int>(type: "int", nullable: false),
                    MappingDecisionsJson = table.Column<string>(type: "LONGTEXT", nullable: true),
                    BeforeStateJson = table.Column<string>(type: "LONGTEXT", nullable: true),
                    AfterStateJson = table.Column<string>(type: "LONGTEXT", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true),
                    ErrorMessage = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorDetails = table.Column<string>(type: "LONGTEXT", nullable: true),
                    MetadataJson = table.Column<string>(type: "LONGTEXT", nullable: true),
                    CanRollback = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRolledBack = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RolledBackAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RolledBackByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RollbackReason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MigrationAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MigrationAuditLogs_Cohorts_CohortId",
                        column: x => x.CohortId,
                        principalTable: "Cohorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MigrationAuditLogs_TenantDepartments_SourceDepartmentId",
                        column: x => x.SourceDepartmentId,
                        principalTable: "TenantDepartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MigrationAuditLogs_TenantDepartments_TargetDepartmentId",
                        column: x => x.TargetDepartmentId,
                        principalTable: "TenantDepartments",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_CohortId",
                table: "MigrationAuditLogs",
                column: "CohortId");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_CompletedAt",
                table: "MigrationAuditLogs",
                column: "CompletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_MigrationId",
                table: "MigrationAuditLogs",
                column: "MigrationId");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_SourceDepartmentId",
                table: "MigrationAuditLogs",
                column: "SourceDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_StartedAt",
                table: "MigrationAuditLogs",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_Status",
                table: "MigrationAuditLogs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_TargetDepartmentId",
                table: "MigrationAuditLogs",
                column: "TargetDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationAuditLogs_TenantId",
                table: "MigrationAuditLogs",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MigrationAuditLogs");
        }
    }
}