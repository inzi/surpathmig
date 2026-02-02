using inzibackend.Surpath;
using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using inzibackend.Authorization.Delegation;
using inzibackend.Authorization.Roles;
using inzibackend.Authorization.Users;
using inzibackend.Chat;
using inzibackend.Editions;
using inzibackend.Friendships;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Accounting;
using inzibackend.MultiTenancy.Payments;
using inzibackend.Storage;
using System.Threading;
using System.Threading.Tasks;
using inzibackend.Surpath.OUs;
using System;

namespace inzibackend.EntityFrameworkCore
{
    public class inzibackendDbContext : AbpZeroDbContext<Tenant, Role, User, inzibackendDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<TenantDepartmentOrganizationUnit> TenantDepartmentOrganizationUnits { get; set; }

        public virtual DbSet<MedicalUnit> MedicalUnits { get; set; }

        public virtual DbSet<RotationSlot> RotationSlots { get; set; }

        public virtual DbSet<SlotAvailableDay> SlotAvailableDays { get; set; }
        public virtual DbSet<SlotRotationDay> SlotRotationDays { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }

        public virtual DbSet<UserPurchase> UserPurchases { get; set; }

        public virtual DbSet<Welcomemessage> Welcomemessages { get; set; }

        public virtual DbSet<TenantSurpathService> TenantSurpathServices { get; set; }

        public virtual DbSet<LedgerEntryDetail> LedgerEntryDetails { get; set; }

        public virtual DbSet<SurpathService> SurpathServices { get; set; }

        public virtual DbSet<LedgerEntry> LedgerEntries { get; set; }

        public virtual DbSet<PidType> PidTypes { get; set; }

        public virtual DbSet<UserPid> UserPids { get; set; }

        public virtual DbSet<TenantDocument> TenantDocuments { get; set; }

        public virtual DbSet<TenantDepartmentUser> TenantDepartmentUsers { get; set; }

        public virtual DbSet<RecordStatus> RecordStatuses { get; set; }

        public virtual DbSet<RecordState> RecordStates { get; set; }

        public virtual DbSet<TenantDocumentCategory> TenantDocumentCategories { get; set; }

        public virtual DbSet<DrugTestCategory> DrugTestCategories { get; set; }

        public virtual DbSet<DeptCode> DeptCodes { get; set; }

        public virtual DbSet<DepartmentUser> DepartmentUsers { get; set; }

        public virtual DbSet<CohortUser> CohortUsers { get; set; }

        public virtual DbSet<Cohort> Cohorts { get; set; }

        public virtual DbSet<TenantDepartment> TenantDepartments { get; set; }

        public virtual DbSet<ConfirmationValue> ConfirmationValues { get; set; }

        public virtual DbSet<CodeType> CodeTypes { get; set; }

        public virtual DbSet<RecordNote> RecordNotes { get; set; }

        public virtual DbSet<RecordCategory> RecordCategories { get; set; }

        public virtual DbSet<RecordRequirement> RecordRequirements { get; set; }

        public virtual DbSet<RecordCategoryRule> RecordCategoryRules { get; set; }

        public virtual DbSet<Record> Records { get; set; }

        public virtual DbSet<DrugPanel> DrugPanels { get; set; }

        public virtual DbSet<Panel> Panels { get; set; }

        public virtual DbSet<TestCategory> TestCategories { get; set; }

        public virtual DbSet<Drug> Drugs { get; set; }

        public virtual DbSet<LegalDocument> LegalDocuments { get; set; }

        public virtual DbSet<MigrationAuditLog> MigrationAuditLogs { get; set; }
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public virtual DbSet<ProcessedTransactions> ProcessedTransactions { get; set; }

        public inzibackendDbContext(DbContextOptions<inzibackendDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RotationSlot>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDepartmentOrganizationUnit>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<MedicalUnit>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Hospital>(h =>
            {
                h.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RotationSlot>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<MedicalUnit>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RotationSlot>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Hospital>(h =>
            {
                h.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategoryRule>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntryDetail>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntry>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntryDetail>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategoryRule>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordStatus>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntryDetail>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntry>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntryDetail>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntry>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategory>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordRequirement>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantSurpathService>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Welcomemessage>(w =>
            {
                w.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordStatus>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<SurpathService>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantSurpathService>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategory>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordRequirement>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<SurpathService>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantSurpathService>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntryDetail>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntry>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntryDetail>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<SurpathService>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LedgerEntry>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategory>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordStatus>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<UserPid>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<PidType>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<UserPid>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategory>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordRequirement>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocument>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CohortUser>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordStatus>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDepartmentUser>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDepartment>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDepartmentUser>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordNote>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordState>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordStatus>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordNote>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordState>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDocumentCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<DeptCode>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<DepartmentUser>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CohortUser>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Cohort>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordNote>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TenantDepartment>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CodeType>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordNote>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategory>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordRequirement>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<RecordCategoryRule>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Record>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<UserPurchase>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
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

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.Entity<RotationSlot>()
                   .HasMany(r => r.SlotAvailableDays) // RotationSlot has many AvailableDays
                   .WithOne(a => a.RotationSlotFK) // AvailableDay has one RotationSlot
                   .OnDelete(DeleteBehavior.Cascade); // Configure cascade delete

            modelBuilder.Entity<RotationSlot>()
                   .HasMany(r => r.SlotRotationDays) // RotationSlot has many AvailableDays
                   .WithOne(a => a.RotationSlotFK) // AvailableDay has one RotationSlot
                   .OnDelete(DeleteBehavior.Cascade); // Configure cascade delete

            modelBuilder.Entity<LegalDocument>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
                l.HasIndex(e => new { e.Type });
            });

            modelBuilder.Entity<MigrationAuditLog>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
                m.HasIndex(e => new { e.MigrationId });
                m.HasIndex(e => new { e.Status });
                m.HasIndex(e => new { e.CohortId });
                m.HasIndex(e => new { e.SourceDepartmentId });
                m.HasIndex(e => new { e.TargetDepartmentId });
                m.HasIndex(e => new { e.StartedAt });
                m.HasIndex(e => new { e.CompletedAt });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }

        public override int SaveChanges()
        {
            ApplyArchivingConcepts();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyArchivingConcepts();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyArchivingConcepts()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IHasArchiving && entry.State == EntityState.Modified)
                {
                    var entity = entry.Entity as IHasArchiving;

                    // Check if IsArchived changed from false to true
                    var isArchivedProperty = entry.Property(nameof(IHasArchiving.IsArchived));
                    if (isArchivedProperty.IsModified &&
                        isArchivedProperty.OriginalValue.Equals(false) &&
                        isArchivedProperty.CurrentValue.Equals(true))
                    {
                        entity.ArchivedTime = DateTime.UtcNow;
                        entity.ArchivedByUserId = AbpSession?.UserId;
                    }
                    // If un-archiving, clear the archive audit fields
                    else if (isArchivedProperty.IsModified &&
                             isArchivedProperty.OriginalValue.Equals(true) &&
                             isArchivedProperty.CurrentValue.Equals(false))
                    {
                        entity.ArchivedTime = null;
                        entity.ArchivedByUserId = null;
                    }
                }
            }
        }
    }
}