using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Web.Areas.App.Models.CohortUsers;
using inzibackend.Web.Controllers;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using inzibackend.Web.Areas.App.Models.RecordStates;
using inzibackend.Web.Areas.App.Models.Records;
using inzibackend.Storage;
using Abp.UI;
using System.Linq;
using System.IO;
using Abp.IO.Extensions;
using Abp.Web.Models;
using Castle.Core.Logging;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser, AppPermissions.Pages_CohortUser_Edit)]
    public class CohortUsersController : inzibackendControllerBase
    {
        private readonly ICohortUsersAppService _cohortUsersAppService;
        private readonly IRecordStatesAppService _recordStatesAppService;
        private readonly IUserTransferAppService _userTransferAppService;
        private readonly ILogger Logger;

        //private readonly ITempFileCacheManager _tempFileCacheManager;

        //private const long MaxfiledataLength = 5242880; //5MB
        //private const string MaxfiledataLengthUserFriendlyValue = "5MB"; //5MB
        //private readonly string[] filedataAllowedFileTypes = { "jpeg", "jpg", "png", "pdf", "txt", "hl7" };

        public CohortUsersController(
            ICohortUsersAppService cohortUsersAppService, 
            IRecordStatesAppService recordStatesAppService,
            IUserTransferAppService userTransferAppService
            //, ITempFileCacheManager tempFileCacheManager
            ,ILogger logger
            )
        {
            _cohortUsersAppService = cohortUsersAppService;
            _recordStatesAppService = recordStatesAppService;
            _userTransferAppService = userTransferAppService;
            //_tempFileCacheManager = tempFileCacheManager;
            Logger = logger;

        }

        public ActionResult Index(Guid? id)
        {

            var model = new CohortUsersViewModel
            {
                FilterText = "",
                CohortId = id
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Create, AppPermissions.Pages_CohortUsers_Edit, AppPermissions.Pages_CohortUser_Edit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetCohortUserForEditOutput getCohortUserForEditOutput;

            if (id.HasValue)
            {
                getCohortUserForEditOutput = await _cohortUsersAppService.GetCohortUserForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getCohortUserForEditOutput = new GetCohortUserForEditOutput
                {
                    CohortUser = new CreateOrEditCohortUserDto()
                };
            }

            var viewModel = new CreateOrEditCohortUserViewModel()
            {
                CohortUser = getCohortUserForEditOutput.CohortUser,
                CohortDescription = getCohortUserForEditOutput.CohortDescription,
                UserName = getCohortUserForEditOutput.UserName,
            };

            return View(viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]

        public async Task<ActionResult> ViewCohortUser(Guid id)
        {
            Logger.Warn($"ViewCohortUser called");
            var getCohortUserForViewDto = await _cohortUsersAppService.GetCohortUserForView(id);
            Logger.Warn($"getCohortUserForViewDto: {getCohortUserForViewDto.CohortUser!=null}, id: {id}");

            var model = new CohortUserViewModel()
            {
                CohortUser = getCohortUserForViewDto.CohortUser,
                CohortDescription = getCohortUserForViewDto.CohortDescription,
                UserName = getCohortUserForViewDto.UserName,
                TenantEditDto = getCohortUserForViewDto.TenantEditDto,
                UserEditDto = getCohortUserForViewDto.UserEditDto,
                TenantDepartmentDto = getCohortUserForViewDto.TenantDepartmentDto

            };
            if (model.TenantDepartmentDto == null) model.TenantDepartmentDto = new TenantDepartmentDto();
            if (model.TenantEditDto == null) model.TenantEditDto = new MultiTenancy.Dto.TenantEditDto();
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Create, AppPermissions.Pages_CohortUsers_Edit)]
        public PartialViewResult CohortLookupTableModal(Guid? id, string displayName, long? tenantid)
        {
            var viewModel = new CohortUserCohortLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };
            ViewBag.TenantId = tenantid;
            return PartialView("_CohortUserCohortLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Create, AppPermissions.Pages_CohortUsers_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CohortUserUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CohortUserUserLookupTableModal", viewModel);
        }


        // _UserUploadNewRecordModal
        [AbpMvcAuthorize(AppPermissions.Pages_CohortUsers_Create, AppPermissions.Pages_CohortUsers_Edit)]
        public async Task<PartialViewResult> UserUploadNewRecordModal(long? id, string displayName)
        {


            GetRecordStateForEditOutput getRecordStateForEditOutput;

            getRecordStateForEditOutput = await _recordStatesAppService.GetRecordStateForReview(new EntityDto<Guid> { Id = (Guid)Guid.Empty });

            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            var _statuses = await _recordStatesAppService.GetAllRecordStatusForTableDropdown();
            if (getRecordStateForEditOutput.IsSurpathOnly) _statuses = await _recordStatesAppService.GetAllServiceRecordStatusForTableDropdown();//  _statuses.Where(s => s.IsSurpathServiceStatus == true).ToList();
            var viewModel = new CreateOrEditRecordStateModalViewModel()
            {
                RecordState = getRecordStateForEditOutput.RecordState,
                Recordfilename = getRecordStateForEditOutput.Recordfilename,
                RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
                UserName = getRecordStateForEditOutput.UserName,
                RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
                RecordStateRecordStatusList = _statuses,


            };

            return PartialView("_UserUploadNewRecordModal", viewModel);
        }

        ////[AbpMvcAuthorize(AppPermissions.Pages_RecordStates_Create, AppPermissions.Pages_RecordStates_Edit)]
      

        //[AbpMvcAuthorize]
        //public PartialViewResult RecordCategoryLookupTableModal(Guid? id, string displayName, bool confirm)
        //{
        //    var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
        //    {
        //        Id = id.ToString(),
        //        DisplayName = displayName,
        //        FilterText = "",
        //        confirm = confirm,
        //    };

        //    return PartialView("_RecordCategoryLookupTableModal", viewModel);
        //}


        //[AbpMvcAuthorize(AppPermissions.Pages_CohortUsers, AppPermissions.Pages_CohortUser)]
        //public FileUploadCacheOutput UploadfiledataFile()
        //{
        //    try
        //    {
        //        //Check input
        //        if (Request.Form.Files.Count == 0)
        //        {
        //            throw new UserFriendlyException(L("NoFileFoundError"));
        //        }

        //        var file = Request.Form.Files.First();
        //        if (file.Length > MaxfiledataLength)
        //        {
        //            throw new UserFriendlyException(L("Warn_File_SizeLimit", MaxfiledataLengthUserFriendlyValue));
        //        }

        //        var fileType = Path.GetExtension(file.FileName).Substring(1);
        //        if (filedataAllowedFileTypes != null && filedataAllowedFileTypes.Length > 0 && !filedataAllowedFileTypes.Contains(fileType))
        //        {
        //            throw new UserFriendlyException(L("FileNotInAllowedFileTypes", filedataAllowedFileTypes));
        //        }

        //        byte[] fileBytes;
        //        using (var stream = file.OpenReadStream())
        //        {
        //            fileBytes = stream.GetAllBytes();
        //        }

        //        var fileToken = Guid.NewGuid().ToString("N");
        //        _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

        //        return new FileUploadCacheOutput(fileToken);
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
        //    }
        //}

    }
}