using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.TenantDocuments;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using System.Net;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TenantDocuments)]
    public class TenantDocumentsController : inzibackendControllerBase
    {
        private readonly ITenantDocumentsAppService _tenantDocumentsAppService;
        //private readonly IRecordsAppService _recordsAppService;

        //private const long MaxfiledataLength = 5242880; //5MB
        //private const string MaxfiledataLengthUserFriendlyValue = "5MB"; //5MB
        //private readonly string[] filedataAllowedFileTypes = { "jpeg", "jpg", "png", "pdf", "txt", "hl7" };

        public TenantDocumentsController(ITenantDocumentsAppService tenantDocumentsAppService) //, IRecordsAppService recordsAppService)
        {
            _tenantDocumentsAppService = tenantDocumentsAppService;
            //_recordsAppService = recordsAppService;

        }

        public ActionResult Index(Guid? catid)
        {
            if (catid == null)
            {
                catid = Guid.Empty;
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            var model = new TenantDocumentsViewModel
            {
                FilterText = "",
                TenantDocumentCategoryId = catid
            };

            return View(model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Records_Create, AppPermissions.Pages_Records_Edit)]
        //public async Task<PartialViewResult> CreateOrEditModal(Guid? id, Guid? catid)
        //{
        //    GetRecordForEditOutput getRecordForEditOutput;
        //    GetTenantDocumentForEditOutput getTenantDocumentForEditOutput;

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
        //        getRecordForEditOutput.Record.DateUploaded = DateTime.Now;
        //        getRecordForEditOutput.Record.DateLastUpdated = DateTime.Now;
        //    }

        //    var viewModel = new CreateOrEditTenantDocumentViewModel()
        //    {
        //        Record = getRecordForEditOutput.Record,
        //        TenantDocumentCategoryName = getRecordForEditOutput.TenantDocumentCategoryName,
        //        filedataFileName = getRecordForEditOutput.filedataFileName,
        //    };

        //    foreach (var filedataAllowedFileType in filedataAllowedFileTypes)
        //    {
        //        viewModel.filedataFileAcceptedTypes += "." + filedataAllowedFileType + ",";
        //    }

        //    return PartialView("_CreateOrEditModal", viewModel);
        //}

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDocuments_Create, AppPermissions.Pages_TenantDocuments_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id, Guid? catid)
        {
            GetTenantDocumentForEditOutput getTenantDocumentForEditOutput;
            if (catid==null) catid = Guid.Empty;

            if (id.HasValue)
            {
                getTenantDocumentForEditOutput = await _tenantDocumentsAppService.GetTenantDocumentForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getTenantDocumentForEditOutput = new GetTenantDocumentForEditOutput
                {
                    TenantDocument = new CreateOrEditTenantDocumentDto()
                };
                getTenantDocumentForEditOutput.TenantDocumentCategoryId = (Guid)catid;
            }

            var viewModel = new CreateOrEditTenantDocumentViewModel()
            {
                TenantDocument = getTenantDocumentForEditOutput.TenantDocument,
                TenantDocumentCategoryName = getTenantDocumentForEditOutput.TenantDocumentCategoryName,
                Recordfilename = getTenantDocumentForEditOutput.Recordfilename,
            };

            return View(viewModel);
        }

        public async Task<ActionResult> ViewTenantDocument(Guid id)
        {
            var getTenantDocumentForViewDto = await _tenantDocumentsAppService.GetTenantDocumentForView(id);

            var model = new TenantDocumentViewModel()
            {
                TenantDocument = getTenantDocumentForViewDto.TenantDocument
                ,
                TenantDocumentCategoryName = getTenantDocumentForViewDto.TenantDocumentCategoryName

                ,
                Recordfilename = getTenantDocumentForViewDto.Recordfilename

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TenantDocuments_Create, AppPermissions.Pages_TenantDocuments_Edit)]
        public PartialViewResult TenantDocumentCategoryLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new TenantDocumentTenantDocumentCategoryLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantDocumentTenantDocumentCategoryLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_TenantDocuments_Create, AppPermissions.Pages_TenantDocuments_Edit)]
        public PartialViewResult RecordLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new TenantDocumentRecordLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TenantDocumentRecordLookupTableModal", viewModel);
        }

    }
}