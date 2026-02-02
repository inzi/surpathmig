using Abp.Json;
using Abp.OpenIddict.Applications;
using Abp.OpenIddict.Authorizations;
using Abp.OpenIddict.EntityFrameworkCore;
using Abp.OpenIddict.Scopes;
using Abp.OpenIddict.Tokens;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using inzibackend.Authorization.Delegation;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Chat;
using inzibackend.Editions;
using inzibackend.ExtraProperties;
using inzibackend.Friendships;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Accounting;
using inzibackend.MultiTenancy.Payments;
using inzibackend.Storage;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;

namespace inzibackend.EntityFrameworkCore;

public class inzibackendDbContext : AbpZeroDbContext<Tenant, Role, User, inzibackendDbContext>, IOpenIddictDbContext
{
    /* Define an IDbSet for each entity of the application */

    public virtual DbSet<OpenIddictApplication> Applications { get; }

    public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

    public virtual DbSet<OpenIddictScope> Scopes { get; }

    public virtual DbSet<OpenIddictToken> Tokens { get; }

    public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

    public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

    public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<UserDelegation> UserDelegations { get; set; }

    public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

	public virtual DbSet<UserAccountLink> UserAccountLinks { get; set; }

    // Surpath entities
    public virtual DbSet<CodeType> CodeTypes { get; set; }
    public virtual DbSet<Cohort> Cohorts { get; set; }
    public virtual DbSet<CohortUser> CohortUsers { get; set; }
    // DISABLED: Missing entity file - temporarily commented out
    // public virtual DbSet<ComplianceInfo> ComplianceInfos { get; set; }
    public virtual DbSet<ConfirmationValue> ConfirmationValues { get; set; }
    public virtual DbSet<DepartmentUser> DepartmentUsers { get; set; }
    public virtual DbSet<DeptCode> DeptCodes { get; set; }
    public virtual DbSet<Drug> Drugs { get; set; }
    public virtual DbSet<DrugPanel> DrugPanels { get; set; }
    public virtual DbSet<DrugTestCategory> DrugTestCategories { get; set; }
    public virtual DbSet<Hospital> Hospitals { get; set; }
    public virtual DbSet<LedgerEntry> LedgerEntries { get; set; }
    public virtual DbSet<LedgerEntryDetail> LedgerEntryDetails { get; set; }
    public virtual DbSet<LegalDocument> LegalDocuments { get; set; }
    public virtual DbSet<MedicalUnit> MedicalUnits { get; set; }
    // DISABLED: Missing entity files - temporarily commented out
    // public virtual DbSet<MetaData> MetaDatas { get; set; }
    // public virtual DbSet<MetaDataNotifications> MetaDataNotifications { get; set; }
    public virtual DbSet<MigrationAuditLog> MigrationAuditLogs { get; set; }
    public virtual DbSet<Panel> Panels { get; set; }
    // DISABLED: Depends on AuthorizeNet SDK - temporarily commented out
    // public virtual DbSet<PaymentLogHelper> PaymentLogHelpers { get; set; }
    // DISABLED: Missing entity file - temporarily commented out
    // public virtual DbSet<PIDType> PIDTypes { get; set; }
    public virtual DbSet<ProcessedTransactions> ProcessedTransactions { get; set; }
    public virtual DbSet<Record> Records { get; set; }
    public virtual DbSet<RecordCategory> RecordCategories { get; set; }
    public virtual DbSet<RecordCategoryRule> RecordCategoryRules { get; set; }
    public virtual DbSet<RecordNote> RecordNotes { get; set; }
    public virtual DbSet<RecordRequirement> RecordRequirements { get; set; }
    public virtual DbSet<RecordState> RecordStates { get; set; }
    public virtual DbSet<RecordStatus> RecordStatuses { get; set; }
    // DISABLED: Missing entity files - temporarily commented out
    // public virtual DbSet<RegistrationValidationRequest> RegistrationValidationRequests { get; set; }
    // public virtual DbSet<RegistrationValidationResult> RegistrationValidationResults { get; set; }
    public virtual DbSet<RotationSlot> RotationSlots { get; set; }
    public virtual DbSet<ServicePricingDecision> ServicePricingDecisions { get; set; }
    public virtual DbSet<SlotAvailableDay> SlotAvailableDays { get; set; }
    public virtual DbSet<SlotRotationDay> SlotRotatationDays { get; set; }
    // DISABLED: Missing AppFeatures constants - temporarily commented out
    // public virtual DbSet<SurpathOnlyRequirements> SurpathOnlyRequirements { get; set; }
    public virtual DbSet<SurpathService> SurpathServices { get; set; }
    public virtual DbSet<TenantDepartment> TenantDepartments { get; set; }
    // DISABLED: Missing entity file - temporarily commented out
    // public virtual DbSet<TenantDepartmentOrganizationUnit> TenantDepartmentOrganizationUnits { get; set; }
    public virtual DbSet<TenantDepartmentUser> TenantDepartmentUsers { get; set; }
    public virtual DbSet<TenantDocument> TenantDocuments { get; set; }
    public virtual DbSet<TenantDocumentCategory> TenantDocumentCategories { get; set; }
    // DISABLED: Missing entity file - temporarily commented out
    // public virtual DbSet<TenantRequirement> TenantRequirements { get; set; }
    public virtual DbSet<TenantSurpathService> TenantSurpathServices { get; set; }
    public virtual DbSet<TestCategory> TestCategories { get; set; }
    // DISABLED: Missing entity file - temporarily commented out
    // public virtual DbSet<UserMembership> UserMemberships { get; set; }
    public virtual DbSet<UserPid> UserPids { get; set; }
    public virtual DbSet<UserPurchase> UserPurchases { get; set; }
    public virtual DbSet<Welcomemessage> Welcomemessages { get; set; }

    public inzibackendDbContext(DbContextOptions<inzibackendDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

        modelBuilder.Entity<SubscribableEdition>(b =>
        {
            b.Property(e => e.MonthlyPrice).HasPrecision(18, 2);
            b.Property(e => e.AnnualPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<SubscriptionPayment>(x =>
        {
            x.Property(u => u.ExtraProperties)
                .HasConversion(
                    d => d.ToJsonString(false, false),
                    s => s.FromJsonString<ExtraPropertyDictionary>()
                )
                .Metadata.SetValueComparer(new ValueComparer<ExtraPropertyDictionary>(
                    (c1, c2) => c1.ToJsonString(false, false) == c2.ToJsonString(false, false),
                    c => c.ToJsonString(false, false).GetHashCode(),
                    c => c.ToJsonString(false, false).FromJsonString<ExtraPropertyDictionary>()
                ));
        });

        modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
        {
            x.Property(u => u.ExtraProperties)
                .HasConversion(
                    d => d.ToJsonString(false, false),
                    s => s.FromJsonString<ExtraPropertyDictionary>()
                )
                .Metadata.SetValueComparer(new ValueComparer<ExtraPropertyDictionary>(
                    (c1, c2) => c1.ToJsonString(false, false) == c2.ToJsonString(false, false),
                    c => c.ToJsonString(false, false).GetHashCode(),
                    c => c.ToJsonString(false, false).FromJsonString<ExtraPropertyDictionary>()
                ));

            x.Property(e => e.Amount).HasPrecision(18, 2);
            x.Property(e => e.TotalAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<ChatMessage>(b =>
        {
            b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
            b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
            b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
            b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
        });

        modelBuilder.Entity<Friendship>(b =>
        {
            b.HasIndex(e => new { e.TenantId, e.UserId });
            b.HasIndex(e => new { e.TenantId, e.FriendUserId });
            b.HasIndex(e => new { e.FriendTenantId, e.UserId });
            b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
        });

        modelBuilder.Entity<Tenant>(b =>
        {
            b.HasIndex(e => new { e.SubscriptionEndDateUtc });
            b.HasIndex(e => new { e.CreationTime });
        });

        modelBuilder.Entity<SubscriptionPayment>(b =>
        {
            b.HasIndex(e => new { e.Status, e.CreationTime });
            b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
        });

        modelBuilder.Entity<UserDelegation>(b =>
        {
            b.HasIndex(e => new { e.TenantId, e.SourceUserId });
            b.HasIndex(e => new { e.TenantId, e.TargetUserId });
        });

		modelBuilder.Entity<UserAccountLink>(b =>
		{
			b.HasIndex(e => new { e.UserAccountId, e.LinkedUserAccountId }).IsUnique();
		});

        modelBuilder.ConfigureOpenIddict();
    }
}