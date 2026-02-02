using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.ConfirmationValues;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ConfirmationValues)]
    public class ConfirmationValuesController : inzibackendControllerBase
    {
        private readonly IConfirmationValuesAppService _confirmationValuesAppService;

        public ConfirmationValuesController(IConfirmationValuesAppService confirmationValuesAppService)
        {
            _confirmationValuesAppService = confirmationValuesAppService;

        }

        public ActionResult Index()
        {
            var model = new ConfirmationValuesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ConfirmationValues_Create, AppPermissions.Pages_ConfirmationValues_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetConfirmationValueForEditOutput getConfirmationValueForEditOutput;

            if (id.HasValue)
            {
                getConfirmationValueForEditOutput = await _confirmationValuesAppService.GetConfirmationValueForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getConfirmationValueForEditOutput = new GetConfirmationValueForEditOutput
                {
                    ConfirmationValue = new CreateOrEditConfirmationValueDto()
                };
            }

            var viewModel = new CreateOrEditConfirmationValueViewModel()
            {
                ConfirmationValue = getConfirmationValueForEditOutput.ConfirmationValue,
                DrugName = getConfirmationValueForEditOutput.DrugName,
                TestCategoryName = getConfirmationValueForEditOutput.TestCategoryName,
                ConfirmationValueTestCategoryList = await _confirmationValuesAppService.GetAllTestCategoryForTableDropdown(),
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewConfirmationValue(Guid id)
        {
            var getConfirmationValueForViewDto = await _confirmationValuesAppService.GetConfirmationValueForView(id);

            var model = new ConfirmationValueViewModel()
            {
                ConfirmationValue = getConfirmationValueForViewDto.ConfirmationValue
                ,
                DrugName = getConfirmationValueForViewDto.DrugName

                ,
                TestCategoryName = getConfirmationValueForViewDto.TestCategoryName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ConfirmationValues_Create, AppPermissions.Pages_ConfirmationValues_Edit)]
        public PartialViewResult DrugLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new ConfirmationValueDrugLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ConfirmationValueDrugLookupTableModal", viewModel);
        }

    }
}