using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.LedgerEntries;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Authorization.Users;
using inzibackend.Common;
using Abp.UI;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntries)]
    public class LedgerEntriesController : inzibackendControllerBase
    {
        private readonly ILedgerEntriesAppService _ledgerEntriesAppService;
        private readonly IUserAppService _userAppService;
        private readonly ICommonLookupAppService _commonLookupAppService;

        public LedgerEntriesController(ILedgerEntriesAppService ledgerEntriesAppService, ICommonLookupAppService commonLookupAppService)
        {
            _ledgerEntriesAppService = ledgerEntriesAppService;
            _commonLookupAppService = commonLookupAppService;
        }

        public ActionResult Index()
        {
            var model = new LedgerEntriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public async Task<ActionResult> UserLedger(long id)
        {
            var _user = await _ledgerEntriesAppService.GetUserDetails(id);

            if (_user == null)
            {
                throw new UserFriendlyException("User not found");
            }
            var model = new LedgerEntriesViewModel
            {
                FilterText = "",
                UserEditDto = _user
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntries_Create, AppPermissions.Pages_LedgerEntries_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetLedgerEntryForEditOutput getLedgerEntryForEditOutput;

            if (id.HasValue)
            {
                getLedgerEntryForEditOutput = await _ledgerEntriesAppService.GetLedgerEntryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getLedgerEntryForEditOutput = new GetLedgerEntryForEditOutput
                {
                    LedgerEntry = new CreateOrEditLedgerEntryDto()
                };
                getLedgerEntryForEditOutput.LedgerEntry.ExpirationDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditLedgerEntryModalViewModel()
            {
                LedgerEntry = getLedgerEntryForEditOutput.LedgerEntry,
                UserName = getLedgerEntryForEditOutput.UserName,
                TenantDocumentName = getLedgerEntryForEditOutput.TenantDocumentName,
                CohortName = getLedgerEntryForEditOutput.CohortName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewLedgerEntryModal(Guid id)
        {
            var getLedgerEntryForViewDto = await _ledgerEntriesAppService.GetLedgerEntryForView(id);

            var model = new LedgerEntryViewModel()
            {
                LedgerEntry = getLedgerEntryForViewDto.LedgerEntry
                ,
                UserName = getLedgerEntryForViewDto.UserName

                ,
                TenantDocumentName = getLedgerEntryForViewDto.TenantDocumentName

                ,
                CohortName = getLedgerEntryForViewDto.CohortName

            };

            return PartialView("_ViewLedgerEntryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntries_Create, AppPermissions.Pages_LedgerEntries_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new LedgerEntryUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LedgerEntryUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntries_Create, AppPermissions.Pages_LedgerEntries_Edit)]
        public PartialViewResult TenantDocumentLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new LedgerEntryTenantDocumentLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LedgerEntryTenantDocumentLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_LedgerEntries_Create, AppPermissions.Pages_LedgerEntries_Edit)]
        public PartialViewResult CohortLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new LedgerEntryCohortLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LedgerEntryCohortLookupTableModal", viewModel);
        }

    }
}