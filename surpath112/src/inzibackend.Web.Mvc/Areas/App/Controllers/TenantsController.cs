using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Authorization;
using inzibackend.Common;
using inzibackend.Editions;
using inzibackend.MultiTenancy;
using inzibackend.MultiTenancy.Dto;
using inzibackend.MultiTenancy.Importing;
using inzibackend.Security;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Web.Areas.App.Models.Tenants;
using inzibackend.Web.Controllers;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantsController : inzibackendControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly TenantManager _tenantManager;
        private readonly IEditionAppService _editionAppService;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;
        private readonly ITenantSurpathServicesAppService _tenantSurpathServiceAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public TenantsController(
            ITenantAppService tenantAppService,
            TenantManager tenantManager,
            IEditionAppService editionAppService,
            ICommonLookupAppService commonLookupAppService,
            IPasswordComplexitySettingStore passwordComplexitySettingStore,
            ITenantSurpathServicesAppService tenantSurpathServiceAppService,
            IBackgroundJobManager backgroundJobManager,
            IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager)
        {
            _tenantAppService = tenantAppService;
            _tenantManager = tenantManager;
            _editionAppService = editionAppService;
            _commonLookupAppService = commonLookupAppService;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
            _tenantSurpathServiceAppService = tenantSurpathServiceAppService;
            _backgroundJobManager = backgroundJobManager;
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.FilterText = Request.Query["filterText"];
            ViewBag.Sorting = Request.Query["sorting"];
            ViewBag.SubscriptionEndDateStart = Request.Query["subscriptionEndDateStart"];
            ViewBag.SubscriptionEndDateEnd = Request.Query["subscriptionEndDateEnd"];
            ViewBag.CreationDateStart = Request.Query["creationDateStart"];
            ViewBag.CreationDateEnd = Request.Query["creationDateEnd"];
            ViewBag.EditionId = Request.Query.ContainsKey("editionId") ? Convert.ToInt32(Request.Query["editionId"]) : (int?)null;

            return View(new TenantIndexViewModel
            {
                EditionItems = await _editionAppService.GetEditionComboboxItems(selectedEditionId: ViewBag.EditionId, addAllItem: true)
            });
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_Create)]
        public async Task<PartialViewResult> CreateModal()
        {
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var defaultEditionName = _commonLookupAppService.GetDefaultEditionName().Name;
            var defaultEditionItem = editionItems.FirstOrDefault(e => e.DisplayText == defaultEditionName);
            if (defaultEditionItem != null)
            {
                defaultEditionItem.IsSelected = true;
            }

            var viewModel = new CreateTenantViewModel(editionItems)
            {
                PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };

            return PartialView("_CreateModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task<PartialViewResult> EditModal(int id)
        {
            var tenantEditDto = await _tenantAppService.GetTenantForEdit(new EntityDto(id));
            var editionItems = await _editionAppService.GetEditionComboboxItems(tenantEditDto.EditionId);
            var serviceItems = await _tenantSurpathServiceAppService.GetAllTenantSurpathServiceDtoForTenant(new EntityDto(id));
            var viewModel = new EditTenantViewModel(tenantEditDto, editionItems, serviceItems);

            return PartialView("_EditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task<PartialViewResult> FeaturesModal(int id)
        {
            var output = await _tenantAppService.GetTenantFeaturesForEdit(new EntityDto(id));
            var viewModel = ObjectMapper.Map<TenantFeaturesEditViewModel>(output);
            viewModel.Tenant = await _tenantManager.GetByIdAsync(id);

            return PartialView("_FeaturesModal", viewModel);
        }

        //NewTenantWizard
        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_Create)]
        public async Task<ActionResult> NewTenantWizard()
        {
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var defaultEditionName = _commonLookupAppService.GetDefaultEditionName().Name;
            var defaultEditionItem = editionItems.FirstOrDefault(e => e.DisplayText == defaultEditionName);
            if (defaultEditionItem != null)
            {
                defaultEditionItem.IsSelected = true;
            }

            var viewModel = new CreateTenantViewModel(editionItems)
            {
                PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };

            // return PartialView("_CreateModal", viewModel);
            return View(viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
        public PartialViewResult ImportUsersModal(int tenantId)
        {
            var viewModel = new ImportTenantUsersViewModel
            {
                TenantId = tenantId
            };

            return PartialView("_ImportUsersModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
        [HttpPost]
        public async Task<JsonResult> ImportUsersFromExcel(int tenantId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new UserFriendlyException(L("PleaseSelectAFile"));
                }

                // Validate file extension
                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                {
                    throw new UserFriendlyException(L("InvalidFileTypeExcel"));
                }

                // Save file to binary storage
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = new byte[stream.Length];
                    await stream.ReadAsync(fileBytes, 0, (int)stream.Length);
                }

                var binaryObject = new BinaryObject(tenantId, fileBytes, $"TenantUserImport_{Guid.NewGuid()}");
                await _binaryObjectManager.SaveAsync(binaryObject);

                // Queue background job
                await _backgroundJobManager.EnqueueAsync<ImportTenantUsersToExcelJob, ImportTenantUsersFromExcelJobArgs>(
                    new ImportTenantUsersFromExcelJobArgs
                    {
                        TenantId = tenantId,
                        BinaryObjectId = binaryObject.Id,
                        User = new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value)
                    }
                );

                return Json(new { success = true });
            }
            catch (UserFriendlyException ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error("Error importing tenant users", ex);
                return Json(new { success = false, error = L("ErrorImportingUsers") });
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_ImportUsers)]
        public async Task<FileResult> ExportTenantUsers(int tenantId)
        {
            var file = await _tenantAppService.GetTenantUsersToExcel(new EntityDto(tenantId));

            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, AbpSession.UserId, AbpSession.TenantId);
            if (fileBytes == null)
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}