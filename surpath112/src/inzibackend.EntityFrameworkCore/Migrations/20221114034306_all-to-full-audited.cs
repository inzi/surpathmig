using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inzibackend.Migrations
{
    public partial class alltofullaudited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "UserPids",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "UserPids",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "UserPids",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "UserPids",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserPids",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "UserPids",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "UserPids",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "TestCategories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "TestCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "TestCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "TestCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TestCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "TestCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "TestCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "TenantDocuments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "TenantDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "TenantDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "TenantDocuments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TenantDocuments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "TenantDocuments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "TenantDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "TenantDocumentCategories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "TenantDocumentCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "TenantDocumentCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "TenantDocumentCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TenantDocumentCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "TenantDocumentCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "TenantDocumentCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "TenantDepartmentUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "TenantDepartmentUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "TenantDepartmentUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "TenantDepartmentUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TenantDepartmentUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "TenantDepartmentUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "TenantDepartmentUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "TenantDepartments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "TenantDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "TenantDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "TenantDepartments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TenantDepartments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "TenantDepartments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "TenantDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecordStatuses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "RecordStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "RecordStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "RecordStatuses",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RecordStatuses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "RecordStatuses",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "RecordStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecordStates",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "RecordStates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "RecordStates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "RecordStates",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RecordStates",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "RecordStates",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "RecordStates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Records",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Records",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Records",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Records",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Records",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Records",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Records",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecordRequirements",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "RecordRequirements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "RecordRequirements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "RecordRequirements",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RecordRequirements",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "RecordRequirements",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "RecordRequirements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecordNotes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "RecordNotes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "RecordNotes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "RecordNotes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RecordNotes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "RecordNotes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "RecordNotes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecordCategoryRules",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "RecordCategoryRules",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "RecordCategoryRules",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "RecordCategoryRules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RecordCategoryRules",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "RecordCategoryRules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "RecordCategoryRules",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "RecordCategories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "RecordCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "RecordCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "RecordCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RecordCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "RecordCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "RecordCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "PidTypes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "PidTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "PidTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "PidTypes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PidTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "PidTypes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "PidTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Panels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Panels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Panels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Panels",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Panels",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Panels",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Panels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "DrugTestCategories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "DrugTestCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "DrugTestCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "DrugTestCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DrugTestCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "DrugTestCategories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "DrugTestCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "DrugPanels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "DrugPanels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "DrugPanels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "DrugPanels",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DrugPanels",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "DrugPanels",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "DrugPanels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "DeptCodes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "DeptCodes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "DeptCodes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "DeptCodes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DeptCodes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "DeptCodes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "DeptCodes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "DepartmentUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "DepartmentUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "DepartmentUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "DepartmentUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "DepartmentUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "DepartmentUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ConfirmationValues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "ConfirmationValues",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "ConfirmationValues",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ConfirmationValues",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ConfirmationValues",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ConfirmationValues",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "ConfirmationValues",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "CohortUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "CohortUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "CohortUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "CohortUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CohortUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "CohortUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "CohortUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Cohorts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Cohorts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Cohorts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Cohorts",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cohorts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Cohorts",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Cohorts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "CodeTypes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "CodeTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "CodeTypes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "CodeTypes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CodeTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "CodeTypes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "CodeTypes",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "UserPids");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "TestCategories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "TenantDocumentCategories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "TenantDepartmentUsers");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "TenantDepartments");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "RecordStatuses");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "RecordStates");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "RecordRequirements");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "RecordNotes");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "RecordCategoryRules");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "RecordCategories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "PidTypes");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Panels");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "DrugTestCategories");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "DrugPanels");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "DeptCodes");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "DepartmentUsers");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "ConfirmationValues");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "CohortUsers");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Cohorts");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "CodeTypes");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "CodeTypes");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "CodeTypes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "CodeTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CodeTypes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "CodeTypes");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "CodeTypes");
        }
    }
}
