using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using inzibackend.Surpath;

namespace inzibackend.Tests.Surpath
{
    public class LedgerPrecision_Tests : AppTestBase
    {
        [Fact]
        public async Task Should_Persist_Full_Decimal_Precision_For_LedgerEntry()
        {
            var user = await GetCurrentUserAsync();
            Guid entryId = Guid.Empty;

            await UsingDbContextAsync(async context =>
            {
                var entry = new LedgerEntry
                {
                    TenantId = AbpSession.TenantId,
                    UserId = user.Id,
                    Name = "Precision Entry",
                    Amount = 199.99m,
                    DiscountAmount = 0.01m,
                    TotalPrice = 199.99m,
                    AmountDue = 76.54m,
                    PaidAmount = 123.45m,
                    PaidInCash = 123.45m,
                    AvailableUserBalance = 0m,
                    BalanceForward = 0m,
                    Settled = true,
                    CardNameOnCard = "Precision User",
                    CardZipCode = "12345"
                };

                context.LedgerEntries.Add(entry);
                entryId = entry.Id;
                await Task.CompletedTask;
            });

            await UsingDbContextAsync(async context =>
            {
                var entry = await context.LedgerEntries.FirstAsync(e => e.Id == entryId);

                entry.Amount.ShouldBe(199.99m);
                entry.TotalPrice.ShouldBe(199.99m);
                entry.PaidAmount.ShouldBe(123.45m);
                entry.PaidInCash.ShouldBe(123.45m);
                entry.AmountDue.ShouldBe(76.54m);
                entry.AmountDue.ShouldBe(entry.TotalPrice - entry.PaidAmount);
                entry.DiscountAmount.ShouldBe(0.01m);
            });
        }

        [Fact]
        public async Task Should_Keep_Ledger_Totals_Aligned_With_Detail_Sums()
        {
            var user = await GetCurrentUserAsync();
            Guid entryId = Guid.Empty;

            const decimal detailAAmount = 45.55m;
            const decimal detailBAmount = 30.10m;
            const decimal detailAPaid = 45.55m;
            const decimal detailBPaid = 10.00m;
            var totalPrice = detailAAmount + detailBAmount;
            var totalPaid = detailAPaid + detailBPaid;
            var amountDue = totalPrice - totalPaid;

            await UsingDbContextAsync(async context =>
            {
                var serviceA = new SurpathService
                {
                    Id = Guid.NewGuid(),
                    TenantId = AbpSession.TenantId,
                    Name = "Ledger Service A",
                    Price = 10,
                    Discount = 0
                };

                var serviceB = new SurpathService
                {
                    Id = Guid.NewGuid(),
                    TenantId = AbpSession.TenantId,
                    Name = "Ledger Service B",
                    Price = 20,
                    Discount = 0
                };

                context.SurpathServices.AddRange(serviceA, serviceB);

                var entry = new LedgerEntry
                {
                    TenantId = AbpSession.TenantId,
                    UserId = user.Id,
                    Name = "Ledger Detail Entry",
                    Amount = totalPrice,
                    DiscountAmount = 0m,
                    TotalPrice = totalPrice,
                    AmountDue = amountDue,
                    PaidAmount = totalPaid,
                    PaidInCash = detailAPaid,
                    AvailableUserBalance = 0m,
                    BalanceForward = 0m,
                    Settled = false,
                    CardNameOnCard = "Ledger Detail User",
                    CardZipCode = "67890"
                };

                context.LedgerEntries.Add(entry);

                var detailA = new LedgerEntryDetail
                {
                    TenantId = AbpSession.TenantId,
                    LedgerEntryId = entry.Id,
                    SurpathServiceId = serviceA.Id,
                    Amount = detailAAmount,
                    Discount = 0m,
                    DiscountAmount = 0m,
                    AmountPaid = detailAPaid
                };

                var detailB = new LedgerEntryDetail
                {
                    TenantId = AbpSession.TenantId,
                    LedgerEntryId = entry.Id,
                    SurpathServiceId = serviceB.Id,
                    Amount = detailBAmount,
                    Discount = 0m,
                    DiscountAmount = 0m,
                    AmountPaid = detailBPaid
                };

                context.LedgerEntryDetails.AddRange(detailA, detailB);
                entryId = entry.Id;
                await Task.CompletedTask;
            });

            await UsingDbContextAsync(async context =>
            {
                var entry = await context.LedgerEntries.FirstAsync(e => e.Id == entryId);
                var details = await context.LedgerEntryDetails.Where(d => d.LedgerEntryId == entryId).ToListAsync();

                details.Count.ShouldBe(2);
                details.Sum(d => d.Amount).ShouldBe(entry.TotalPrice);
                details.Sum(d => d.AmountPaid).ShouldBe(entry.PaidAmount);
                entry.AmountDue.ShouldBe(entry.TotalPrice - entry.PaidAmount);
                entry.AmountDue.ShouldBe(amountDue);
            });
        }
    }
}
