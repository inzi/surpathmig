using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Logging;
using inzibackend.Authorization;
using inzibackend.Authorization.Users;
using inzibackend.Configuration;
using inzibackend.Storage;
using inzibackend.Surpath.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using inzibackend.Surpath.SurpathPay;
using Abp.Runtime.Session;
using Abp.Application.Features;
using Abp.Domain.Entities;
using inzibackend.Surpath.Purchase;
using AuthorizeNet.Api.Contracts.V1;
using System.Transactions;
using inzibackend.MultiTenancy;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath.ComplianceManager;
using Abp.Authorization.Users;
using inzibackend.Authorization.Roles;
using inzibackend.Surpath.Logging;

namespace inzibackend.Surpath
{
    [AbpAuthorize]
    public class PurchaseAppService : inzibackendAppServiceBase, IPurchaseAppService
    {
        private readonly IRepository<CohortUser, Guid> _cohortUserRepository;
        private readonly IRepository<User, long> _userLookUpRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _tenantDepartmentUserRepository;

        private readonly IRepository<LedgerEntry, Guid> _ledgerEntryRepository;
        private readonly IRepository<LedgerEntryDetail, Guid> _ledgerEntryDetailRepository;

        private readonly IRepository<SurpathService, Guid> _surpathServiceLookUpRepository;
        private readonly IRepository<TenantSurpathService, Guid> _tenantSurpathServiceRepository;

        private readonly SurpathPayManager _surpathPayManager;
        private readonly SurpathManager _surpathManager;

        private readonly IRepository<ProcessedTransactions> _processedTransactionsRepository;

        private readonly ISurpathComplianceEvaluator _surpathComplianceEvaluator;

        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;

        private AuthNetManager _authNetManager;
        private IAbpSession _abpSession;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly HierarchicalPricingManager _hierarchicalPricingManager;

        public PurchaseAppService(
            IRepository<User, long> lookup_userRepository,
            IRepository<CohortUser, Guid> cohortUserRepository,
            IRepository<TenantDepartmentUser, Guid> tenantDepartmentUserRepository,
            IRepository<LedgerEntry, Guid> ledgerEntryRepository,
            IRepository<LedgerEntryDetail, Guid> ledgerEntryDetailRepository,
            IRepository<TenantSurpathService, Guid> tenantSurpathServiceRepository,
            IRepository<SurpathService, Guid> lookup_surpathServiceRepository,
            SurpathPayManager surpathPayManager,
            SurpathManager surpathManager,
            AuthNetManager authNetManager,
            IAbpSession abpSession,
            IAppConfigurationAccessor configurationAccessor,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Tenant> tenantRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<ProcessedTransactions> processedTransactionsRepository,
            ISurpathComplianceEvaluator surpathComplianceEvaluator,
            HierarchicalPricingManager hierarchicalPricingManager
            )
        {
            _userLookUpRepository = lookup_userRepository;
            _cohortUserRepository = cohortUserRepository;
            _tenantDepartmentUserRepository = tenantDepartmentUserRepository;

            _ledgerEntryRepository = ledgerEntryRepository;
            _ledgerEntryDetailRepository = ledgerEntryDetailRepository;
            _surpathPayManager = surpathPayManager;
            _surpathManager = surpathManager;
            _authNetManager = authNetManager;
            _abpSession = abpSession;
            _tenantSurpathServiceRepository = tenantSurpathServiceRepository;
            _surpathServiceLookUpRepository = lookup_surpathServiceRepository;

            _appConfiguration = configurationAccessor.Configuration;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantRepository = tenantRepository;

            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;

            _processedTransactionsRepository = processedTransactionsRepository;
            _surpathComplianceEvaluator = surpathComplianceEvaluator;
            _hierarchicalPricingManager = hierarchicalPricingManager;
        }

        public async Task<Guid?> GetChortUserId(long id)
        {
            return _cohortUserRepository.GetAll().Where(ch => ch.UserId == id).Select(ch => ch.Id).FirstOrDefault();
        }

        public async Task<bool> DonorCurrent(long id)
        {
            // We have to get the user
            // get their tenant
            // get their features
            // get their departments
            // get their cohortuser

            // if no cohortuser, return true
            // if host, return true

            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host) return true;

            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            //var _iscohortuser = await IsCohorUser(id);
            // TODO - if not a cohort user AND is not "isalwaysdonor" true then return
            //if (await GetChortUserId(id) == null) return true;
            var _user = _userLookUpRepository.Get(id);
            if (_user.DonorPayPromptDelayUntil != null)
            {
                if (_user.DonorPayPromptDelayUntil.ToUniversalTime() > DateTime.UtcNow)
                {
                    return true;
                }
            }
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));
            var featureValues = await TenantManager.GetFeatureValuesAsync((int)_user.TenantId);
            var tenantFeatureNames = featureValues.Where(fv => fv.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase)).Select(fv => fv.Name).ToList();
            var thisTenant = TenantManager.Tenants.AsNoTracking().IgnoreQueryFilters().Where(t => t.Id == _user.TenantId).FirstOrDefault();
            var tenantSurpathServices = _tenantSurpathServiceRepository.GetAll().Where(t => t.TenantId == _user.TenantId).ToList();
            var tenantSurpathServicesIds = tenantSurpathServices.Select(s => s.Id).ToList();
            var surpathServiceIds = tenantSurpathServices.Select(s => (Guid)s.SurpathServiceId).ToList();
            if (tenantSurpathServices.Count() > 0)
            {
                // this needs to examine the services and make sure they have a detail entry for
                // each service that is still active

                Logger.Info("DonorPaid Check");
                //var filteredLedgerEntries = _ledgerEntryRepository.GetAll()
                //        .Where(l=>l.UserId == id);

                var _les = _ledgerEntryDetailRepository.GetAll().Include(l => l.LedgerEntryFk).Include(l => l.TenantSurpathServiceFk).Include(l => l.SurpathServiceFk).Where(l => l.LedgerEntryFk.UserId == id && l.LedgerEntryFk.ExpirationDate >= DateTime.UtcNow).ToList();

                //var _validSubs = _les.Select(l => l.TenantSurpathServiceFk.SurpathServiceId).ToList();
                // get all the services except those with active subs, if no results, user is current
                var complianceInfo = await _surpathComplianceEvaluator.GetComplianceInfo((int)AbpSession.TenantId, (long)AbpSession.UserId);
                var _tservices = (SurpathOnlyRequirements.GetAllSurpathRequirementsFromComplianceInfo(complianceInfo)).ToList();

                // get all the services except those with active subs, if no results, user is current

                //var _anyNotActive = tenantSurpathServicesIds.Except<Guid>(_les.Select(a => (Guid)a.TenantSurpathServiceId).ToList());
                var _anyNotActive = surpathServiceIds.Except<Guid>(_les.Select(a => (Guid)a.SurpathServiceId).ToList());

                if (_anyNotActive.Count() == 0) return true;

                return false;
            }
            // no surpath services enabled
            return true;
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        public async Task<bool> DoPurchaseFromHelper(AuthNetSubmit authNetSubmit)
        {
            if (!authNetSubmit.DifferentBillingAddress)
            {
                var _user = _userLookUpRepository.Get((long)AbpSession.UserId);
                authNetSubmit.BillingAddress = _user.Address;
                authNetSubmit.BillingCity = _user.City;
                authNetSubmit.BillingState = _user.State;
                //authNetSubmit.BillingZipCode = _user.Zip;
                authNetSubmit.FirstNameOnCard = _user.Name;
                authNetSubmit.LastNameOnCard = _user.Surname;
                authNetSubmit.CardNameOnCard = _user.Name + " " + _user.Surname;
            }

            var tenant = await _tenantRepository.GetAll().IgnoreQueryFilters().Where(t => t.Id == AbpSession.TenantId).FirstOrDefaultAsync();
            var itemDescription = "";
            if (tenant != null)
            {
                itemDescription = tenant.Name;
            }
            createTransactionResponse response = null;
            try
            {
                response = await _authNetManager.PreAuthCreditCardRequest(authNetSubmit, authNetSubmit.amount, itemDescription);
            }
            catch (Exception ex)
            {
                Logger.Error("Error during transaction", ex);
                return false;
            }

            if (response == null || response.messages == null)
            {
                return false;
            }
            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                // Get user's department and cohort information for hierarchical pricing
                Guid? departmentId = null;
                Guid? cohortId = null;
                
                // Get user's cohort if they have one
                var userCohort = await _cohortUserRepository.GetAll()
                    .Where(cu => cu.UserId == AbpSession.UserId.Value)
                    .Include(cu => cu.CohortFk)
                    .FirstOrDefaultAsync();
                    
                if (userCohort != null)
                {
                    cohortId = userCohort.CohortId;
                    departmentId = userCohort.CohortFk?.TenantDepartmentId;
                }
                
                // If no department from cohort, check direct department membership
                if (departmentId == null)
                {
                    var userDept = await _tenantDepartmentUserRepository.GetAll()
                        .Where(tdu => tdu.UserId == AbpSession.UserId.Value)
                        .FirstOrDefaultAsync();
                    
                    if (userDept != null)
                    {
                        departmentId = userDept.TenantDepartmentId;
                    }
                }

                // Get all services with hierarchical pricing
                var hierarchicalPricing = await _hierarchicalPricingManager.GetPricingForPurchaseV2Async(
                    (int)AbpSession.TenantId,
                    departmentId,
                    cohortId,
                    AbpSession.UserId
                );

                // Get the specific services being paid for
                var _tenantservices = await _surpathPayManager.GetSurpathServicesForTenant((int)AbpSession.TenantId);
                var paidServices = _tenantservices.Where(t => authNetSubmit.TenantSurpathServiceIds.Contains(t.Id)).ToList();
                
                // Calculate total using hierarchical pricing
                decimal _TotalPrice = 0;
                foreach (var service in paidServices)
                {
                    if (service.SurpathServiceId.HasValue)
                    {
                        var pricingDecision = await _hierarchicalPricingManager.GetServicePricingDecisionAsync(
                            service.SurpathServiceId.Value,
                            (int)AbpSession.TenantId,
                            departmentId,
                            cohortId,
                            AbpSession.UserId
                        );
                        _TotalPrice += (decimal)pricingDecision.PriceToCharge;
                    }
                }
                
                var AmountDue = _TotalPrice - authNetSubmit.amount >= 0 ? _TotalPrice - authNetSubmit.amount : 0;

                // await _surpathManager.CreateLedgerEntry(user.Id, (int)AbpSession.TenantId);
                await _surpathPayManager.CreateLedgerEntry(response.transactionResponse, (long)AbpSession.UserId, AbpSession.TenantId, authNetSubmit.amount, authNetSubmit, _TotalPrice);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
            return response.messages.resultCode == messageTypeEnum.Ok;
        }

        public async Task<bool> SandboxPayment()
        {
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            return UseSandbox;
        }

        [AbpAllowAnonymous]
        public async Task<AuthorizeNetSettings> AuthorizeNetSettings()
        {
            var UseSandbox = _appConfiguration["Payment:AuthorizeNet:UseSandbox"].To<bool>();
            var retval = new AuthorizeNetSettings();
            retval.ApiLoginID = _appConfiguration["Payment:AuthorizeNet:ApiLoginId"];
            // retval.ApiTransactionKey = _appConfiguration["Payment:AuthorizeNet:TransactionKey"];
            retval.PublicClientKey = _appConfiguration["Payment:AuthorizeNet:PublicClientKey"];
            retval.TransactionId = Guid.NewGuid().ToString(); // Generate a unique transaction ID
            return retval;
        }

        [AbpAllowAnonymous]
        public async Task<createTransactionResponse> PreAuth(PreAuthDto preAuth)
        {
            Logger.Debug("PurchaseAppService PreAuth");
            if (await IsTransactionProcessed(preAuth.TransactionId))
            {
                throw new UserFriendlyException("This transaction has already been processed.");
            }
            // var tenantIsDonorPay = await _surpathManager.IsTenantDonorPay((int)AbpSession.TenantId);
            createTransactionResponse response = null;
            if (!preAuth.AuthNetSubmit.DifferentBillingAddress)
            {
                Logger.Debug($"PurchaseAppService PreAuth different billing address");
                preAuth.AuthNetSubmit.BillingAddress = preAuth.Address;
                preAuth.AuthNetSubmit.BillingCity = preAuth.City;
                preAuth.AuthNetSubmit.BillingState = preAuth.State;
                //preAuth.AuthNetSubmit.BillingZipCode = preAuth.Zip;
                preAuth.AuthNetSubmit.FirstNameOnCard = preAuth.Name;
                preAuth.AuthNetSubmit.LastNameOnCard = preAuth.Surname;
                preAuth.AuthNetSubmit.CardNameOnCard = preAuth.Name + " " + preAuth.Surname;
            }
            var tenantName = await _surpathManager.GetTenantNameById((int)AbpSession.TenantId);
            var preAuthSummary = PaymentLogHelper.SummarizePreAuth(tenantName, preAuth);
            Logger.Debug($"PurchaseAppService PreAuth about to call | {preAuthSummary}");
            response = await _authNetManager.PreAuthCreditCardRequest(preAuth.AuthNetSubmit, preAuth.AuthNetSubmit.amount, tenantName);
            Logger.Debug($"PurchaseAppService PreAuth Response: {response.messages.resultCode}");
            Logger.Debug($"PurchaseAppService PreAuth Response Details: {PaymentLogHelper.SummarizeResponse(response)}");

            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                // Mark this TransactionId as processed
                await MarkTransactionAsProcessed(preAuth.TransactionId);
            }

            return response;
        }

        private async Task<bool> IsTransactionProcessed(string transactionId)
        {
            // Implement logic to check if this transactionId exists in your database
            // Return true if it exists, false otherwise

            return _processedTransactionsRepository.GetAll().Where(p => p.TransactionId == transactionId).Any();
        }

        private async Task MarkTransactionAsProcessed(string transactionId)
        {
            // Implement logic to store this transactionId in your database
            await _processedTransactionsRepository.InsertAsync(new ProcessedTransactions { TransactionId = transactionId });
        }

        // Surpath:
        [AbpAllowAnonymous]
        public async Task<List<string>> GetUserRolesAsStringList(long userId)
        {
            using (var uow = UnitOfWorkManager.Begin())
            {
                try
                {
                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                    {
                        var q = from userRole in _userRoleRepository.GetAll().AsNoTracking()
                                join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                                where userRole.UserId == userId
                                select role.Name;

                        return q.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
                finally
                {
                    await uow.CompleteAsync();
                }
            }
        }
    }
}
