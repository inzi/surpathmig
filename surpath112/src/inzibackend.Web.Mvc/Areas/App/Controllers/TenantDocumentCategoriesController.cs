using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.TenantDocumentCategories;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TenantDocumentCategories)]
    public class TenantDocumentCategoriesController : inzibackendControllerBase
    {
        private readonly ITenantDocumentCategoriesAppService _tenantDocumentCategoriesAppService;

        public TenantDocumentCategoriesController(ITenantDocumentCategoriesAppService tenantDocumentCategoriesAppService)
        {
            _tenantDocumentCategoriesAppService = tenantDocumentCategoriesAppService;

        }

        public ActionResult Index()
        {
            var model = new TenantDocumentCategoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDocumentCategories_Create, AppPermissions.Pages_TenantDocumentCategories_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetTenantDocumentCategoryForEditOutput getTenantDocumentCategoryForEditOutput;

            if (id.HasValue)
            {
                getTenantDocumentCategoryForEditOutput = await _tenantDocumentCategoriesAppService.GetTenantDocumentCategoryForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTenantDocumentCategoryForEditOutput = new GetTenantDocumentCategoryForEditOutput
                {
                    TenantDocumentCategory = new CreateOrEditTenantDocumentCategoryDto()
                };
            }

            var viewModel = new CreateOrEditTenantDocumentCategoryViewModel()
            {
                TenantDocumentCategory = getTenantDocumentCategoryForEditOutput.TenantDocumentCategory,
                UserName = getTenantDocumentCategoryForEditOutput.UserName,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewTenantDocumentCategory(Guid id)
        {
            var getTenantDocumentCategoryForViewDto = await _tenantDocumentCategoriesAppService.GetTenantDocumentCategoryForView(id);

            var model = new TenantDocumentCategoryViewModel()
            {
                TenantDocumentCategory = getTenantDocumentCategoryForViewDto.TenantDocumentCategory
                ,
                UserName = getTenantDocumentCategoryForViewDto.UserName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDocumentCategories_Create, AppPermissions.Pages_TenantDocumentCategories_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new TenantDocumentCategoryUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantDocumentCategoryUserLookupTableModal", viewModel);
        }

    }
}