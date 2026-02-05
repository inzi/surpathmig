using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.Records;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Records)]
    public class MasterDetailChild_TenantDocumentCategory_RecordsController : inzibackendControllerBase
    {
        private readonly IRecordsAppService _recordsAppService;

        public MasterDetailChild_TenantDocumentCategory_RecordsController(IRecordsAppService recordsAppService)
        {
            _recordsAppService = recordsAppService;
        }

        public ActionResult Index(Guid tenantDocumentCategoryId)
        {
            var model = new MasterDetailChild_TenantDocumentCategory_RecordsViewModel
            {
                FilterText = "",
                TenantDocumentCategoryId = tenantDocumentCategoryId
            };

            return View(model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Records_Create, AppPermissions.Pages_Records_Edit)]
        //public async Task<ActionResult> CreateOrEdit(Guid? id)
        //{
        //    GetRecordForEditOutput getRecordForEditOutput;

        //    if (id.HasValue)
        //    {
        //        getRecordForEditOutput = await _recordsAppService.GetRecordForEdit(new EntityDto<Guid> { Id = (Guid)id });
        //    }
        //    else
        //    {
        //        getRecordForEditOutput = new GetRecordForEditOutput
        //        {
        //            Record = new CreateOrEditRecordDto()
        //        };
        //    }

        //    var viewModel = new CreateOrEditRecordViewModel()
        //    {
        //        Record = getRecordForEditOutput.Record,
        //    };

        //    return View(viewModel);
        //}

        public async Task<ActionResult> ViewRecord(Guid id)
        {
            var getRecordForViewDto = await _recordsAppService.GetRecordForView(id);

            var model = new MasterDetailChild_TenantDocumentCategory_RecordViewModel()
            {
                Record = getRecordForViewDto.Record
            };

            return View(model);
        }

    }
}