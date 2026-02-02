using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.TenantSurpathServices;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using System.Linq;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using Abp.Domain.Repositories;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices)]
    public class TenantSurpathServicesController : inzibackendControllerBase
    {
        private readonly ITenantSurpathServicesAppService _tenantSurpathServicesAppService;
        private readonly IRepository<Tenant> _tenantRepository;

        public TenantSurpathServicesController(
            ITenantSurpathServicesAppService tenantSurpathServicesAppService,
            IRepository<Tenant> tenantRepository)
        {
            _tenantSurpathServicesAppService = tenantSurpathServicesAppService;
            _tenantRepository = tenantRepository;
        }

        public ActionResult Index()
        {
            var model = new TenantSurpathServicesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Create, AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetTenantSurpathServiceForEditOutput getTenantSurpathServiceForEditOutput;

            if (id.HasValue)
            {
                getTenantSurpathServiceForEditOutput = await _tenantSurpathServicesAppService.GetTenantSurpathServiceForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTenantSurpathServiceForEditOutput = new GetTenantSurpathServiceForEditOutput
                {
                    TenantSurpathService = new CreateOrEditTenantSurpathServiceDto()
                };
            }

            var viewModel = new CreateOrEditTenantSurpathServiceModalViewModel()
            {
                TenantSurpathService = getTenantSurpathServiceForEditOutput.TenantSurpathService,
                SurpathServiceName = getTenantSurpathServiceForEditOutput.SurpathServiceName,
                TenantDepartmentName = getTenantSurpathServiceForEditOutput.TenantDepartmentName,
                CohortName = getTenantSurpathServiceForEditOutput.CohortName,
                UserName = getTenantSurpathServiceForEditOutput.UserName,
                RecordCategoryRuleName = getTenantSurpathServiceForEditOutput.RecordCategoryRuleName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTenantSurpathServiceModal(Guid id)
        {
            var getTenantSurpathServiceForViewDto = await _tenantSurpathServicesAppService.GetTenantSurpathServiceForView(id);

            var model = new TenantSurpathServiceViewModel()
            {
                TenantSurpathService = getTenantSurpathServiceForViewDto.TenantSurpathService
                ,
                SurpathServiceName = getTenantSurpathServiceForViewDto.SurpathServiceName

                ,
                TenantDepartmentName = getTenantSurpathServiceForViewDto.TenantDepartmentName

                ,
                CohortName = getTenantSurpathServiceForViewDto.CohortName

                ,
                UserName = getTenantSurpathServiceForViewDto.UserName

                ,
                RecordCategoryRuleName = getTenantSurpathServiceForViewDto.RecordCategoryRuleName

            };

            return PartialView("_ViewTenantSurpathServiceModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Create, AppPermissions.Pages_TenantSurpathServices_Edit)]
        public PartialViewResult SurpathServiceLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new TenantSurpathServiceSurpathServiceLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantSurpathServiceSurpathServiceLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Create, AppPermissions.Pages_TenantSurpathServices_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName, int? tenantId)
        {
            var viewModel = new TenantSurpathServiceTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = "",
                TenantId = tenantId

            };

            return PartialView("_TenantSurpathServiceTenantDepartmentLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Create, AppPermissions.Pages_TenantSurpathServices_Edit)]
        public PartialViewResult CohortLookupTableModal(Guid? id, string displayName, int? tenantId)
        {
            var viewModel = new TenantSurpathServiceCohortLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = "",
                TenantId = tenantId
            };

            return PartialView("_TenantSurpathServiceCohortLookupTableModal", viewModel);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Create, AppPermissions.Pages_TenantSurpathServices_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new TenantSurpathServiceUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantSurpathServiceUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Create, AppPermissions.Pages_TenantSurpathServices_Edit)]
        public PartialViewResult RecordCategoryRuleLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new TenantSurpathServiceRecordCategoryRuleLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantSurpathServiceRecordCategoryRuleLookupTableModal", viewModel);
        }

        public async Task<PartialViewResult> AssignmentModal(Guid id)
        {
            return PartialView("_AssignmentModal", id);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices)]
        public async Task<ActionResult> PricingManagement()
        {
            var model = new PricingManagementViewModel();
            
            // If user is host, they can select from all tenants
            if (AbpSession.MultiTenancySide == Abp.MultiTenancy.MultiTenancySides.Host)
            {
                var tenants = await _tenantRepository.GetAllListAsync();
                model.Tenants = tenants.Select(t => new TenantListDto
                {
                    Id = t.Id,
                    TenancyName = t.TenancyName,
                    Name = t.Name,
                    IsActive = t.IsActive,
                    EditionId = t.EditionId,
                    SubscriptionEndDateUtc = t.SubscriptionEndDateUtc,
                    IsInTrialPeriod = t.IsInTrialPeriod,
                    CreationTime = t.CreationTime
                })
                .OrderBy(t => t.Name)
                .ToList();
            }
            
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task<PartialViewResult> PriceEditModal(string level, string targetId, string targetName, double? currentPrice, bool currentIsInvoiced, Guid serviceId, int tenantId)
        {
            var viewModel = new PriceEditModalViewModel
            {
                Level = level,
                TargetId = targetId,
                TargetName = targetName,
                CurrentPrice = currentPrice,
                CurrentIsInvoiced = currentIsInvoiced,
                ServiceId = serviceId,
                TenantId = tenantId
            };

            return PartialView("_PriceEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantSurpathServices_Edit)]
        public async Task<PartialViewResult> SetAllServicesPriceModal(string targetType, string targetId, string targetName, int tenantId)
        {
            var services = await _tenantSurpathServicesAppService.GetAvailableSurpathServices();
            
            var viewModel = new SetAllServicesPriceModalViewModel
            {
                TargetType = targetType,
                TargetId = targetId,
                TargetName = targetName,
                TenantId = tenantId,
                Services = services
            };

            return PartialView("_SetAllServicesPriceModal", viewModel);
        }
    }
}