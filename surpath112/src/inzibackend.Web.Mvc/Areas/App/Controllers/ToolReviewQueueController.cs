using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.RecordStates;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Web.Areas.App.Models.Records;
using inzibackend.Storage;
using Abp.MimeTypes;
using System.Net;
using Abp.Auditing;
using inzibackend.Dto;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using inzibackend.Surpath.Compliance;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Surpath_Administration_Surpath_Compliance_Review)]
    public class ToolReviewQueueController : inzibackendControllerBase
    {
        private readonly IRecordStatesAppService _recordStatesAppService;
        private readonly SurpathManager _surpathManager;
        //private readonly ITempFileCacheManager _tempFileCacheManager;
        //private readonly IBinaryObjectManager _binaryObjectManager;
        //private readonly IMimeTypeMap _mimeTypeMap;

        public ToolReviewQueueController(IRecordStatesAppService recordStatesAppService,
              ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IMimeTypeMap mimeTypeMap,
            SurpathManager surpathManager)
        {
            _recordStatesAppService = recordStatesAppService;
            _surpathManager = surpathManager;
            //_tempFileCacheManager = tempFileCacheManager;
            //_binaryObjectManager = binaryObjectManager;
            //_mimeTypeMap = mimeTypeMap;
        }

        public ActionResult Index()
        {
            var model = new RecordStatesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
        {
            GetRecordStateForEditOutput getRecordStateForEditOutput;

            if (id.HasValue)
            {
                getRecordStateForEditOutput = await _recordStatesAppService.GetRecordStateForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordStateForEditOutput = new GetRecordStateForEditOutput
                {
                    RecordState = new CreateOrEditRecordStateDto()
                };
            }
            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            var _cohortUserDto = new CohortUserDto();
            if (getRecordStateForEditOutput.TenantId.HasValue)
            {
                _cohortUserDto.Id = (Guid)await _surpathManager.GetCohortUserIdForUser((long)getRecordStateForEditOutput.RecordState.UserId, (int)getRecordStateForEditOutput.TenantId);
            }

            var viewModel = new CreateOrEditRecordStateModalViewModel()
            {
                RecordState = getRecordStateForEditOutput.RecordState,
                Recordfilename = getRecordStateForEditOutput.Recordfilename,
                RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
                UserName = getRecordStateForEditOutput.UserName,
                RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
                RecordStateRecordStatusList = await _recordStatesAppService.GetAllRecordStatusForTableDropdown(),
                CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel(),
                CohortUser = _cohortUserDto

            };
            viewModel.RecordState.RecordDto = new RecordDto();


            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRecordStateModal(Guid id)
        {
            var getRecordStateForViewDto = await _recordStatesAppService.GetRecordStateForView(id);

            var model = new RecordStateViewModel()
            {
                RecordState = getRecordStateForViewDto.RecordState
                ,
                Recordfilename = getRecordStateForViewDto.Recordfilename

                ,
                RecordCategoryName = getRecordStateForViewDto.RecordCategoryName

                ,
                UserName = getRecordStateForViewDto.UserName

                ,
                RecordStatusStatusName = getRecordStateForViewDto.RecordStatusStatusName

            };

            return PartialView("_ViewRecordStateModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public PartialViewResult RecordLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordStateRecordLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordStateRecordLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public PartialViewResult RecordCategoryLookupTableModal(Guid? id, string displayName, bool confirm)
        {
            var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = "",
                confirm = confirm,
            };

            return PartialView("_RecordStateRecordCategoryLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new RecordStateUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordStateUserLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Surpath_Administration_Surpath_Compliance_Review)]
        public async Task<ActionResult> ReviewRecordState(Guid id)
        {


            GetRecordStateForEditOutput getRecordStateForEditOutput;

            getRecordStateForEditOutput = await _recordStatesAppService.GetRecordStateForReview(new EntityDto<Guid> { Id = (Guid)id });

            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            int? _tenantId = null;
            if (AbpSession.TenantId == null)
            {
                _tenantId = await _recordStatesAppService.GetTenantIdForRecordState(new EntityDto<Guid> { Id = (Guid)id });
            }
            var _statuses = await _recordStatesAppService.GetAllRecordStatusForTableDropdown(getRecordStateForEditOutput.TenantId);
            if (getRecordStateForEditOutput.IsSurpathOnly) _statuses = await _recordStatesAppService.GetAllServiceRecordStatusForTableDropdown();//_statuses.Where(s => s.IsSurpathServiceStatus == true).ToList();

            var viewModel = new CreateOrEditRecordStateModalViewModel()
            {
                RecordState = getRecordStateForEditOutput.RecordState,
                Recordfilename = getRecordStateForEditOutput.Recordfilename,
                RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
                UserName = getRecordStateForEditOutput.UserName,
                RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
                RecordStateRecordStatusList = _statuses,
                

            };

            return View(viewModel);
        }

        //[DisableAuditing]
        //public ActionResult DownloadTempFile(FileDto file)
        //{
        //    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
        //    if (fileBytes == null)
        //    {
        //        return NotFound(L("RequestedFileDoesNotExists"));
        //    }

        //    return File(fileBytes, file.FileType, file.FileName);
        //}

        //[DisableAuditing]
        //public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        //{
      
        //    var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
        //    if (fileObject == null)
        //    {
        //        return StatusCode((int)HttpStatusCode.NotFound);
        //    }
        //    if (fileObject.IsFile)
        //    {
        //        fileName = fileObject.OriginalFileName;
        //    }
        //    if (fileName.IsNullOrEmpty())
        //    {
        //        if (!fileObject.Description.IsNullOrEmpty() &&
        //            !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
        //        {
        //            fileName = fileObject.Description;

        //        }
        //        else
        //        {
        //            return StatusCode((int)HttpStatusCode.BadRequest);
        //        }
        //    }

        //    if (contentType.IsNullOrEmpty())
        //    {
        //        if (!Path.GetExtension(fileName).IsNullOrEmpty())
        //        {
        //            contentType = _mimeTypeMap.GetMimeType(fileName);
        //        }
        //        else
        //        {
        //            return StatusCode((int)HttpStatusCode.BadRequest);
        //        }
        //    }
        //    //var cd = new System.Net.Mime.ContentDisposition
        //    //{
        //    //    FileName = fileName,
        //    //    Inline = true,
        //    //};
        //    //Response.Headers.Add("Content-Disposition", "inline");
        //    //Response.Headers.Add("Content-Disposition", cd.ToString());

        //    return File(fileObject.Bytes, contentType, fileName);
        //}


        //public async Task<ActionResult> ViewBinaryFile(Guid id, string contentType, string fileName)
        //{
        //    var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
        //    if (fileObject == null)
        //    {
        //        return StatusCode((int)HttpStatusCode.NotFound);
        //    }
        //    if (fileObject.IsFile)
        //    {
        //        fileName = fileObject.OriginalFileName;
        //    }
        //    if (fileName.IsNullOrEmpty())
        //    {
        //        if (!fileObject.Description.IsNullOrEmpty() &&
        //            !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
        //        {
        //            fileName = fileObject.Description;
        //        }
        //        else
        //        {
        //            return StatusCode((int)HttpStatusCode.BadRequest);
        //        }
        //    }

        //    if (contentType.IsNullOrEmpty())
        //    {
        //        if (!Path.GetExtension(fileName).IsNullOrEmpty())
        //        {
        //            contentType = _mimeTypeMap.GetMimeType(fileName);
        //        }
        //        else
        //        {
        //            return StatusCode((int)HttpStatusCode.BadRequest);
        //        }
        //    }
        //    //var cd = new System.Net.Mime.ContentDisposition
        //    //{
        //    //    FileName = fileName,
        //    //    Inline = true,
        //    //};
        //    Response.Headers.Add("Content-Disposition", "inline");
        //    //Response.Headers.Add("Content-Disposition", cd.ToString());

        //    var _h = Response.Headers;

        //    return File(fileObject.Bytes, contentType);
        //}



    }
}