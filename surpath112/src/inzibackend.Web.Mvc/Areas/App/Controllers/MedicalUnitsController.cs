using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.MedicalUnits;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MedicalUnits)]
    public class MedicalUnitsController : inzibackendControllerBase
    {
        private readonly IMedicalUnitsAppService _medicalUnitsAppService;

        public MedicalUnitsController(IMedicalUnitsAppService medicalUnitsAppService)
        {
            _medicalUnitsAppService = medicalUnitsAppService;

        }

        public ActionResult Index()
        {
            var model = new MedicalUnitsViewModel
            {
                FilterText = ""
            };

            return View("spIndex", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MedicalUnits_Create, AppPermissions.Pages_MedicalUnits_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetMedicalUnitForEditOutput getMedicalUnitForEditOutput;

            if (id.HasValue)
            {
                getMedicalUnitForEditOutput = await _medicalUnitsAppService.GetMedicalUnitForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getMedicalUnitForEditOutput = new GetMedicalUnitForEditOutput
                {
                    MedicalUnit = new CreateOrEditMedicalUnitDto()
                };
            }

            var viewModel = new CreateOrEditMedicalUnitModalViewModel()
            {
                MedicalUnit = getMedicalUnitForEditOutput.MedicalUnit,
                HospitalName = getMedicalUnitForEditOutput.HospitalName,
                MedicalUnitHospitalList = await _medicalUnitsAppService.GetAllHospitalForTableDropdown(),

            };

            return PartialView("_spCreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewMedicalUnitModal(int id)
        {
            var getMedicalUnitForViewDto = await _medicalUnitsAppService.GetMedicalUnitForView(id);

            var model = new MedicalUnitViewModel()
            {
                MedicalUnit = getMedicalUnitForViewDto.MedicalUnit
                ,
                HospitalName = getMedicalUnitForViewDto.HospitalName

            };

            return PartialView("_ViewMedicalUnitModal", model);
        }

    }
}