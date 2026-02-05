using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Hospitals;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Hospitals)]
    public class HospitalsController : inzibackendControllerBase
    {
        private readonly IHospitalsAppService _hospitalsAppService;

        public HospitalsController(IHospitalsAppService hospitalsAppService)
        {
            _hospitalsAppService = hospitalsAppService;

        }

        public ActionResult Index()
        {
            var model = new HospitalsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Hospitals_Create, AppPermissions.Pages_Administration_Hospitals_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetHospitalForEditOutput getHospitalForEditOutput;

            if (id.HasValue)
            {
                getHospitalForEditOutput = await _hospitalsAppService.GetHospitalForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getHospitalForEditOutput = new GetHospitalForEditOutput
                {
                    Hospital = new CreateOrEditHospitalDto()
                };
            }

            var viewModel = new CreateOrEditHospitalViewModel()
            {
                Hospital = getHospitalForEditOutput.Hospital,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewHospital(int id)
        {
            var getHospitalForViewDto = await _hospitalsAppService.GetHospitalForView(id);

            var model = new HospitalViewModel()
            {
                Hospital = getHospitalForViewDto.Hospital
            };

            return View(model);
        }

    }
}