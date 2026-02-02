using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.SurpathServices;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Features;
using inzibackend.MultiTenancy;
using inzibackend.Editions;
using inzibackend.Web.Areas.App.Models.Editions;
using System.Linq;
using inzibackend.Editions.Dto;
using System.Collections.Generic;
using inzibackend.Web.Areas.App.Models.Tenants;
using inzibackend.Common;
using System.Data.Entity;
using inzibackend.MultiTenancy.Dto;
using AutoMapper;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SurpathServices)]
    public class SurpathServicesController : inzibackendControllerBase
    {
        private readonly ISurpathServicesAppService _surpathServicesAppService;
        private readonly ITenantAppService _tenantAppService;
        private readonly IEditionAppService _editionAppService;
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly TenantManager _tenantManager;
        private readonly ITenantSurpathServicesAppService _tenantSurpathServiceAppService;


        public SurpathServicesController(
          ISurpathServicesAppService surpathServicesAppService,
          ITenantAppService tenantAppService,
          IEditionAppService editionAppService,
          ITenantSurpathServicesAppService tenantSurpathServiceAppService,
          TenantManager tenantManager
          )
        {
            _surpathServicesAppService = surpathServicesAppService;
            _tenantAppService = tenantAppService;
            _editionAppService = editionAppService;
            _tenantSurpathServiceAppService = tenantSurpathServiceAppService;
            _tenantManager = tenantManager;
        }

        public ActionResult Index()
        {
            var model = new SurpathServicesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        public async Task<ActionResult> Manage(int id)
        {
            var viewmodel = new SurpathManageServicesViewModel();

            //viewmodel.Tenants = ObjectMapper.Map<List<TenantListDto>>(
            //        await _tenantManager.Tenants.Include(t => t.Edition).ToListAsync()
            //    );

            //var t = (await _tenantAppService.GetTenants(new GetTenantsInput() { MaxResultCount = -1, Sorting = "Name desc" })).Items.ToList();

            // viewmodel.Tenants = await _tenantAppService.GetTenantsList();

            //var s = await _surpathServicesAppService.GetAll(new GetAllSurpathServicesInput()
            //{
            //    MaxResultCount = 5000,
            //    Sorting = "Name desc"
            //}).Result.Items.ToList();



            return View(viewmodel);

        }

        [AbpMvcAuthorize(AppPermissions.Pages_SurpathServices_Create, AppPermissions.Pages_SurpathServices_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetSurpathServiceForEditOutput getSurpathServiceForEditOutput;

            if (id.HasValue)
            {
                getSurpathServiceForEditOutput = await _surpathServicesAppService.GetSurpathServiceForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getSurpathServiceForEditOutput = new GetSurpathServiceForEditOutput
                {
                    SurpathService = new CreateOrEditSurpathServiceDto()
                };
            }

            var featList = await GetAllSurpathFeatures();

            var viewModel = new CreateOrEditSurpathServiceModalViewModel()
            {
                SurpathService = getSurpathServiceForEditOutput.SurpathService,
                TenantDepartmentName = getSurpathServiceForEditOutput.TenantDepartmentName,
                CohortName = getSurpathServiceForEditOutput.CohortName,
                UserName = getSurpathServiceForEditOutput.UserName,
                RecordCategoryRuleName = getSurpathServiceForEditOutput.RecordCategoryRuleName,
                FeatureList = featList

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSurpathServiceModal(Guid id)
        {
            var getSurpathServiceForViewDto = await _surpathServicesAppService.GetSurpathServiceForView(id);

            var model = new SurpathServiceViewModel()
            {
                SurpathService = getSurpathServiceForViewDto.SurpathService
                ,
                TenantDepartmentName = getSurpathServiceForViewDto.TenantDepartmentName

                ,
                CohortName = getSurpathServiceForViewDto.CohortName

                ,
                UserName = getSurpathServiceForViewDto.UserName

                ,
                RecordCategoryRuleName = getSurpathServiceForViewDto.RecordCategoryRuleName

            };

            return PartialView("_ViewSurpathServiceModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SurpathServices_Create, AppPermissions.Pages_SurpathServices_Edit)]
        public PartialViewResult TenantDepartmentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new SurpathServiceTenantDepartmentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SurpathServiceTenantDepartmentLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_SurpathServices_Create, AppPermissions.Pages_SurpathServices_Edit)]
        public PartialViewResult CohortLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new SurpathServiceCohortLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SurpathServiceCohortLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_SurpathServices_Create, AppPermissions.Pages_SurpathServices_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new SurpathServiceUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SurpathServiceUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_SurpathServices_Create, AppPermissions.Pages_SurpathServices_Edit)]
        public PartialViewResult RecordCategoryRuleLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new SurpathServiceRecordCategoryRuleLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_SurpathServiceRecordCategoryRuleLookupTableModal", viewModel);
        }


        private async Task<List<ComboboxItemDto>> GetAllSurpathFeatures()
        {
            //var output = await _tenantAppService.GetTenantFeaturesForEdit(new EntityDto(id));
            // var viewModel = ObjectMapper.Map<TenantFeaturesEditViewModel>(output);
            //viewModel.Tenant = await _tenantManager.GetByIdAsync(id);

            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = null });

            //public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

            var viewModel = ObjectMapper.Map<EditEditionModalViewModel>(output);
            //viewModel.EditionItems = await _editionAppService.GetEditionComboboxItems(); ;
            //viewModel.FreeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true);
            var featureList = new List<ComboboxItemDto>(); //  output.Features;

            // var oFeatureList = ObjectMapper.Map<List<ComboboxItemDto>>(output.Features);
            foreach (var f in output.Features)
            {
                if (f.Name.Contains("surpath", StringComparison.InvariantCultureIgnoreCase))
                {
                    var o = new ComboboxItemDto
                    {
                        DisplayText = f.DisplayName,
                        Value = f.Name,
                        IsSelected = false
                    };
                    featureList.Add(o);
                }

            }


            return featureList;


            // var featureList = output.Features.Where(f => f.ParentName.Contains("Surpath")).ToList();


            //return featureList;

        }
    }
}
