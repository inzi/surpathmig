using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.LedgerEntryDetails;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntryDetails)]
    public class MasterDetailChild_LedgerEntry_LedgerEntryDetailsController : inzibackendControllerBase
    {
        private readonly ILedgerEntryDetailsAppService _ledgerEntryDetailsAppService;

        public MasterDetailChild_LedgerEntry_LedgerEntryDetailsController(ILedgerEntryDetailsAppService ledgerEntryDetailsAppService)
        {
            _ledgerEntryDetailsAppService = ledgerEntryDetailsAppService;
        }

        public ActionResult Index(Guid ledgerEntryId)
        {
            var model = new MasterDetailChild_LedgerEntry_LedgerEntryDetailsViewModel
            {
                FilterText = "",
                LedgerEntryId = ledgerEntryId
            };

            return View(model);
        }
        public ActionResult UserLedgerDetails(Guid ledgerEntryId)
        {
            var model = new MasterDetailChild_LedgerEntry_LedgerEntryDetailsViewModel
            {
                FilterText = "",
                LedgerEntryId = ledgerEntryId
            };

            return View(model);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntryDetails_Create, AppPermissions.Pages_LedgerEntryDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetLedgerEntryDetailForEditOutput getLedgerEntryDetailForEditOutput;

            if (id.HasValue)
            {
                getLedgerEntryDetailForEditOutput = await _ledgerEntryDetailsAppService.GetLedgerEntryDetailForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getLedgerEntryDetailForEditOutput = new GetLedgerEntryDetailForEditOutput
                {
                    LedgerEntryDetail = new CreateOrEditLedgerEntryDetailDto()
                };
                getLedgerEntryDetailForEditOutput.LedgerEntryDetail.DatePaidOn = DateTime.Now;
            }

            var viewModel = new MasterDetailChild_LedgerEntry_CreateOrEditLedgerEntryDetailModalViewModel()
            {
                LedgerEntryDetail = getLedgerEntryDetailForEditOutput.LedgerEntryDetail,
                SurpathServiceName = getLedgerEntryDetailForEditOutput.SurpathServiceName,
                TenantSurpathServiceName = getLedgerEntryDetailForEditOutput.TenantSurpathServiceName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewLedgerEntryDetailModal(Guid id)
        {
            var getLedgerEntryDetailForViewDto = await _ledgerEntryDetailsAppService.GetLedgerEntryDetailForView(id);

            var model = new MasterDetailChild_LedgerEntry_LedgerEntryDetailViewModel()
            {
                LedgerEntryDetail = getLedgerEntryDetailForViewDto.LedgerEntryDetail
                ,
                SurpathServiceName = getLedgerEntryDetailForViewDto.SurpathServiceName

                ,
                TenantSurpathServiceName = getLedgerEntryDetailForViewDto.TenantSurpathServiceName

            };

            return PartialView("_ViewLedgerEntryDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntryDetails_Create, AppPermissions.Pages_LedgerEntryDetails_Edit)]
        public PartialViewResult SurpathServiceLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new MasterDetailChild_LedgerEntry_LedgerEntryDetailSurpathServiceLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LedgerEntryDetailSurpathServiceLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntryDetails_Create, AppPermissions.Pages_LedgerEntryDetails_Edit)]
        public PartialViewResult TenantSurpathServiceLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new MasterDetailChild_LedgerEntry_LedgerEntryDetailTenantSurpathServiceLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LedgerEntryDetailTenantSurpathServiceLookupTableModal", viewModel);
        }

    }
}