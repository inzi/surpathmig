using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using inzibackend.Authorization.Roles;
using inzibackend.Configuration;
using inzibackend.Debugging;
using inzibackend.MultiTenancy;
using inzibackend.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using inzibackend.Authorization.Roles;
using inzibackend.Configuration;
using inzibackend.Security;
using inzibackend.Authorization.Users;
using inzibackend.Authorization;
using Abp.Application.Features;
using inzibackend.Features;
using Abp.Collections.Extensions;

namespace inzibackend.Surpath.Compliance
{
    public class SurpathManager : inzibackendDomainServiceBase
    {
        private readonly IFeatureChecker _featureChecker;

        private readonly ILogger<SurpathManager> _logger;
        private readonly IRepository<UserPid, Guid> _UserPidRepository;
        private readonly IRepository<TenantDepartment, Guid> _TenantDepartmentRepository;
        private readonly IRepository<TenantDepartmentUser, Guid> _TenantDepartmentUserRepository;
        private readonly IRepository<Cohort, Guid> _CohortRepository;
        private readonly IRepository<CohortUser, Guid> _CohortUserRepository;
        private readonly IRepository<RecordRequirement, Guid> _RecordRequirementRepository;
        private readonly IRepository<RecordCategory, Guid> _RecordCategoryRepository;
        private readonly IRepository<RecordCategoryRule, Guid> _RecordCategoryRuleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<RecordState, Guid> _recordStateRepository;
        private readonly IRepository<Record, Guid> _recordRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IPermissionChecker _permissionChecker;


        //private UserManager UserManager { get; set; }
        private readonly UserManager _userManager;

        public SurpathManager(
            ILogger<SurpathManager> logger,
            IRepository<UserPid, Guid> UserPidRepository,
            IRepository<TenantDepartment, Guid> TenantDepartmentRepository,
            IRepository<TenantDepartmentUser, Guid> TenantDepartmentUserRepository,
            IRepository<Cohort, Guid> CohortRepository,
            IRepository<CohortUser, Guid> CohortUserRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Tenant> tenantRepository,
            IRepository<RecordRequirement, Guid> RecordRequirementRepository,
            IRepository<RecordCategory, Guid> RecordCategoryRepository,
            IRepository<RecordCategoryRule, Guid> RecordCategoryRuleRepository,
            IRepository<RecordState, Guid> recordStateRepository,
            IRepository<Record, Guid> recordRepository,
            IFeatureChecker featureChecker,
            IPermissionChecker permissionChecker,
            UserManager userManager
            )
        {
            _logger = logger;
            _UserPidRepository = UserPidRepository;
            _TenantDepartmentRepository = TenantDepartmentRepository;
            _TenantDepartmentUserRepository = TenantDepartmentUserRepository;
            _CohortRepository = CohortRepository;
            _CohortUserRepository = CohortUserRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantRepository = tenantRepository;
            _RecordRequirementRepository = RecordRequirementRepository;
            _RecordCategoryRepository = RecordCategoryRepository;
            _RecordCategoryRuleRepository = RecordCategoryRuleRepository;
            _featureChecker = featureChecker;
            _userManager = userManager;
            _recordStateRepository = recordStateRepository;
            _recordRepository = recordRepository;
            _permissionChecker = permissionChecker;
        }

        public async Task<string> GetUserNameById(long userid)
        {
            var retval = string.Empty;
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                    var user = await _userManager.GetUserByIdAsync(userid);
                    if (user != null)
                    {
                        retval = user.FullName;
                    }
                    //CurrentUnitOfWork.DisableFilter("IsDeleted");
                    //if (_tenantRepository.FirstOrDefault(o=>o.Id == tenantId))
                    //var _tenant = _tenantRepository.FirstOrDefault(tenantId);
                    //var _user = await UserManager.fin.FirstOrDefaultAsync(o => o.Id == tenantId);
                    //if (_tenant != null)
                    //{
                    //    retval = _tenant.Name;
                    //}
                }
                uow.Complete();
            }
            return retval;
        }


        public async Task AssignUserToTenantDepartment(long? userid, Guid TenantDepartmentId, int TenantId)
        {
            //using (var uow = UnitOfWorkManager.Begin())
            //{
            //CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _t = _TenantDepartmentRepository.GetAll().AsNoTracking().Where(t => t.Id == TenantDepartmentId).FirstOrDefault();
            if (!(_t == null))
            {
                var _tu = _TenantDepartmentUserRepository.GetAll().AsNoTracking().Where(tu => tu.UserId == userid && tu.TenantDepartmentId == TenantDepartmentId).FirstOrDefault();
                if (_tu == null)
                {
                    _TenantDepartmentUserRepository.Insert(new TenantDepartmentUser()
                    {
                        TenantDepartmentId = TenantDepartmentId,
                        UserId = (long)userid,
                        TenantId = TenantId
                    });
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
            //    uow.Complete();
            //}
        }

        public async Task AssignUserToCohort(long? userid, Guid CohortId, int TenantId)
        {
            //using (var uow = UnitOfWorkManager.Begin())
            //{
            //CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _c = _CohortRepository.GetAll().AsNoTracking().Where(c => c.Id == CohortId).FirstOrDefault();
            if (_c == null)
            {
                _c = _CohortRepository.GetAll().AsNoTracking().Where(c => c.DefaultCohort == true).FirstOrDefault();
            }

            if (!(_c == null))
            {
                var _cu = _CohortUserRepository.GetAll().AsNoTracking().Where(cu => cu.UserId == userid && cu.CohortId == CohortId).FirstOrDefault();
                if (_cu == null)
                {
                    _CohortUserRepository.Insert(new CohortUser()
                    {
                        UserId = (long)userid,
                        CohortId = CohortId,
                        TenantId = TenantId
                    });
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
            //    uow.Complete();
            //}
        }

        public async Task CreateOrUpdateUserPid(long? userid, UserPid userPid, int TenantId)
        {
            //using (var uow = UnitOfWorkManager.Begin())
            //{
            //    //CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var _up = _UserPidRepository.GetAll().Where(up => up.UserId == userid && up.PidTypeId == userPid.PidTypeId).FirstOrDefault();
            if (_up == null)
            {
                _UserPidRepository.Insert(new UserPid()
                {
                    Pid = userPid.Pid,
                    UserId = userid,
                    PidTypeId = userPid.PidTypeId
                });

            }
            else
            {
                _up.Pid = userPid.Pid;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            //    uow.Complete();
            //}
        }

        public async Task<Guid?> AddUserToDefaultCohort(long userId, int tenantId)
        {
            Guid? retval = null;

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                    var defCohort = await GetDefaultCohortForTeanantOrCreate(tenantId);
                    if (defCohort != null)
                    {
                        if (_CohortUserRepository.GetAll().AsNoTracking().Where(cu => cu.TenantId == tenantId && cu.UserId == userId).FirstOrDefault() == null)
                        {
                            CohortUser newCohortUser = new CohortUser()
                            {
                                CohortId = defCohort.Id,
                                UserId = (long)userId,
                                TenantId = tenantId
                            };
                            _CohortUserRepository.Insert(newCohortUser);
                            await CurrentUnitOfWork.SaveChangesAsync();
                            //return newCohortUser.Id;
                            retval = newCohortUser.Id;


                        }

                        //await Create(createOrEditCohortUserDto);
                        //_id = createOrEditCohortUserDto.Id;
                        //await CurrentUnitOfWork.SaveChangesAsync();
                        //_id = await GetCohortUserIdForUser();
                    }
                    else
                    {
                        throw new AbpAuthorizationException("There is no cohort user with that ID or no default cohort has been created!");
                    }
                }
                uow.Complete();
            }

            return retval;
        }

        public async Task<Guid?> GetCohortUserIdForUser(long userId, int tenantId)
        {
            var _cohortUser = _CohortUserRepository.GetAll().AsNoTracking().Where(cu => cu.UserId == userId && cu.TenantId == tenantId).FirstOrDefault();
            if (_cohortUser != null)
            {
                return _cohortUser.Id;
            }


            return null;
        }

        public async Task<Cohort?> GetDefaultCohortForTeanantOrCreate(int tenantId)
        {
            Cohort retval = null;
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                    retval = _CohortRepository.GetAll().AsNoTracking().Where(c => c.DefaultCohort == true && c.TenantId == tenantId).FirstOrDefault();
                    if (retval == null)
                    {
                        var _cohort = new Cohort()
                        {
                            Name = "Unassigned",
                            DefaultCohort = true,
                            Description = "This is a generic cohort, users will automatically be assigned to this cohort if they're not assigned manually or during registration",
                            TenantDepartmentId = null,
                            TenantId = tenantId

                        };
                        _CohortRepository.Insert(_cohort);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        retval = _cohort;
                    }
                }
                uow.Complete();
            }
            return retval;
        }


        public async Task<string> GetTenantNameById(int tenantId)
        {
            var retval = string.Empty;
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                    //CurrentUnitOfWork.DisableFilter("IsDeleted");
                    //if (_tenantRepository.FirstOrDefault(o=>o.Id == tenantId))
                    //var _tenant = _tenantRepository.FirstOrDefault(tenantId);
                    var _tenant = await _tenantRepository.FirstOrDefaultAsync(o => o.Id == tenantId);
                    if (_tenant != null)
                    {
                        retval = _tenant.Name;
                    }
                }
                uow.Complete();
            }
            return retval;
        }

        public async Task<bool> IsTenantDonorPay(int tenantId)
        {
            var retval = false;
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
                    //CurrentUnitOfWork.DisableFilter("IsDeleted");
                    //if (_tenantRepository.FirstOrDefault(o=>o.Id == tenantId))
                    //var _tenant = _tenantRepository.FirstOrDefault(tenantId);
                    var _tenant = await _tenantRepository.FirstOrDefaultAsync(o => o.Id == tenantId);
                    if (_tenant != null)
                    {
                        retval = _tenant.IsDonorPay;
                    }
                }
                uow.Complete();
            }
            return retval;
        }

        #region CreateRequirement
        public async Task<List<RecordCategory>> GetRecordCategoriesForRuleIdAsync(Guid RuleId)
        {
            return await _RecordCategoryRepository.GetAll().Where(rc => rc.RecordCategoryRuleId == RuleId).ToListAsync();
        }

        public async Task<List<RecordCategory>> GetRecordCategoriesForRequirementAsync(Guid RequirementId)
        {
            var retval = await _RecordCategoryRepository.GetAll().Include(r => r.RecordRequirementFk).Include(r => r.RecordCategoryRuleFk).Where(rc => rc.RecordRequirementId == RequirementId).ToListAsync();
            return retval;

        }


        #endregion CreateRequirement


        #region Requirements

        public async Task<List<RecordRequirement>> GetRequirementListFor(Guid cohortId, Guid tenantDeptId)
        {
            var reqs = _RecordRequirementRepository.GetAll().Where(r=>r.IsSurpathOnly==false).Where(r => (r.TenantDepartmentId == tenantDeptId || r.TenantDepartmentId == null) && (r.CohortId == null || r.CohortId == cohortId)).ToList();

            return reqs;
        }
        #endregion Requirements

        #region accessChecks
        //public async Task<bool> UserAccessToFile(long userid, long? impersonaterUserId, Guid binaryObjId)
        public async Task<bool> UserAccessToFile(long userid, long? impersonaterUserId, Guid binaryObjId)
        {
            try
            {
                if (impersonaterUserId == null) impersonaterUserId = 0;
                // used by any recordstates
                var _rs = await _recordStateRepository.GetAll().IgnoreQueryFilters().AsNoTracking().Where(rs => rs.RecordFk.BinaryObjId == binaryObjId).Include(rs => rs.RecordCategoryFk).ThenInclude(rs => rs.RecordRequirementFk).ThenInclude(rs => rs.SurpathServiceFk).FirstOrDefaultAsync();
                if (_rs != null)
                {
                    // is this a service record?
                    // if it is - does this user have permission to it?
                    if (_rs.RecordCategoryFk.RecordRequirementFk.SurpathServiceFk != null)
                    {
                        var _service = _rs.RecordCategoryFk.RecordRequirementFk.SurpathServiceFk;
                        var _user = _userManager.GetUserById(userid);
                        // tied to a record state
                        var userIdentifier = _user.ToUserIdentifier();
                        if (_service.FeatureIdentifier == AppFeatures.SurpathFeatureDrugTest)
                        {
                            if (await _permissionChecker.IsGrantedAsync(userIdentifier, AppPermissions.Surpath_Drug_Screen_Download) == true) return true;
                        }
                        else if (_service.FeatureIdentifier == AppFeatures.SurpathFeatureBackgroundCheck)
                        {
                            if (await _permissionChecker.IsGrantedAsync(userIdentifier, AppPermissions.Surpath_Background_Check_Download) == true) return true;
                        }
                        else
                        {
                            if (await _permissionChecker.IsGrantedAsync(userIdentifier, AppPermissions.Surpath_Administration_Surpath_Compliance_Review) == true) return true;
                        }

                    }
                    else
                    {
                        if (impersonaterUserId > 0)
                        {
                            CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

                            var _imposter = _userManager.GetUserById((long)impersonaterUserId);
                            var _imposterIdentifier = _imposter.ToUserIdentifier();
                            var Pages_Administration_Host_Dashboard = await _permissionChecker.IsGrantedAsync(_imposterIdentifier, AppPermissions.Pages_Administration_Host_Dashboard); // return true;
                            var Surpath_Administration_Surpath_Compliance_Review = await _permissionChecker.IsGrantedAsync(_imposterIdentifier, AppPermissions.Surpath_Administration_Surpath_Compliance_Review); // return true;

                            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);


                            if (Pages_Administration_Host_Dashboard == true) return true;
                            if (Surpath_Administration_Surpath_Compliance_Review == true) return true;

                        }
                        var _user = _userManager.GetUserById(userid);
                        // tied to a record state
                        var userIdentifier = _user.ToUserIdentifier();
                        if (await _permissionChecker.IsGrantedAsync(userIdentifier, AppPermissions.Surpath_Administration_Surpath_Compliance_Review) == true) return true;
                        if (await _permissionChecker.IsGrantedAsync(userIdentifier, AppPermissions.Pages_CohortUsers_Edit) == true) return true;
                        if (await _permissionChecker.IsGrantedAsync(userIdentifier, AppPermissions.Pages_CohortUser) == true && userIdentifier.UserId == userid && _rs.UserId == userIdentifier.UserId) return true;

                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserAccessToFile error", ex);
                return false;
            }
        }
        #endregion accessChecks

    }
}
