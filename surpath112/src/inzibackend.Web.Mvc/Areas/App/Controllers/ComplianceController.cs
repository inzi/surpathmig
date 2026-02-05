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

using System.IO;
using System.Linq;
using Abp.Web.Models;
using Abp.UI;
using Abp.IO.Extensions;
using inzibackend.Storage;
using inzibackend.Web.Areas.App.Models.RecordStates;
using Abp.Authorization;
using inzibackend.Web.Areas.App.Models.TenantDepartments;
using inzibackend.Web.Areas.App.Models.Cohorts;
using inzibackend.Surpath.Compliance;
using System.Collections.Generic;
using inzibackend.Web.Areas.App.Models.Compliance;
using inzibackend.Web.Areas.App.Models.RecordRequirements;
using inzibackend.Web.Areas.App.Models.RecordCategories;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    //[AbpMvcAuthorize]
    [AbpAllowAnonymous]
    public class ComplianceController : inzibackendControllerBase
    {
        private readonly IRecordsAppService _recordsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        //private readonly IRecordStatesAppService _recordStatesAppService;
        private readonly ISurpathComplianceAppService _commplianceAppService;

        private readonly IRecordRequirementsAppService _recordRequirementsAppService;
        private readonly IRecordCategoriesAppService _recordCategoriesAppService;
        private readonly SurpathManager _surpathManager;

        //private const long MaxfiledataLength = 5242880; //5MB
        //private const string MaxfiledataLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] filedataAllowedFileTypes = SurpathSettings.AllowedFileExtensionsArray; // { "jpeg", "jpg", "png", "pdf", "txt", "hl7" };

        public ComplianceController(
            IRecordsAppService recordsAppService,
            ITempFileCacheManager tempFileCacheManager,
            ISurpathComplianceAppService commplianceAppService,
            IRecordRequirementsAppService recordRequirementsAppService,
            IRecordCategoriesAppService recordCategoriesAppService,
            SurpathManager surpathManager)
        {
            _recordsAppService = recordsAppService;
            _tempFileCacheManager = tempFileCacheManager;
            //_recordStatesAppService = recordStatesAppService;
            _commplianceAppService = commplianceAppService;
            _recordRequirementsAppService = recordRequirementsAppService;
            _recordCategoriesAppService = recordCategoriesAppService;
            _surpathManager = surpathManager;
        }

        //public ActionResult Index()
        //{
        //    var model = new RecordsViewModel
        //    {
        //        FilterText = ""
        //    };

        //    return View(model);
        //}

        //[AbpMvcAuthorize(AppPermissions.Pages_Records_Create, AppPermissions.Pages_Records_Edit)]
        //public async Task<PartialViewResult> CreateOrEditModal(Guid? id)
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
        //        getRecordForEditOutput.Record.DateUploaded = DateTime.Now;
        //        getRecordForEditOutput.Record.DateLastUpdated = DateTime.Now;
        //    }

        //    var viewModel = new CreateOrEditRecordModalViewModel()
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

        //public async Task<PartialViewResult> ViewRecordModal(Guid id)
        //{
        //    var getRecordForViewDto = await _recordsAppService.GetRecordForView(id);

        //    var model = new RecordViewModel()
        //    {
        //        Record = getRecordForViewDto.Record
        //        ,
        //        TenantDocumentCategoryName = getRecordForViewDto.TenantDocumentCategoryName

        //    };

        //    return PartialView("_ViewRecordModal", model);
        //}
        [AbpMvcAuthorize]
        public async Task<PartialViewResult> CreateNewRecordModal(long id, Guid? cohortUserId)
        {
            GetRecordStateForEditOutput getRecordStateForEditOutput;

            getRecordStateForEditOutput = new GetRecordStateForEditOutput
            {
                RecordState = new CreateOrEditRecordStateDto()
            };
            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            getRecordStateForEditOutput.RecordState.UserId = (long)id;
            var viewModel = new CreateOrEditRecordStateModalViewModel();
            viewModel.RecordState = getRecordStateForEditOutput.RecordState;
            viewModel.Recordfilename = getRecordStateForEditOutput.Recordfilename;
            viewModel.RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName;
            viewModel.UserName = getRecordStateForEditOutput.UserName;
            viewModel.RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName;
            viewModel.RecordStateRecordStatusList = await _commplianceAppService.GetAllRecordStatusForTableDropdown();
            viewModel.CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel();

            //var viewModel = new CreateOrEditRecordStateModalViewModel()
            //{
            //    RecordState = getRecordStateForEditOutput.RecordState,
            //    Recordfilename = getRecordStateForEditOutput.Recordfilename,
            //    RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
            //    UserName = getRecordStateForEditOutput.UserName,
            //    RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
            //    RecordStateRecordStatusList = await _commplianceAppService.GetAllRecordStatusForTableDropdown(),
            //    CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel()

            //};
            viewModel.RecordState.RecordDto = new RecordDto();

            return PartialView("_CreateNewRecordModal", viewModel);
        }

        [AbpMvcAuthorize]
        public async Task<PartialViewResult> CreateNewRecordForCategoryModal(long id, Guid catid, Guid? cohortUserId)
        {
            GetRecordStateForEditOutput getRecordStateForEditOutput;

            getRecordStateForEditOutput = new GetRecordStateForEditOutput
            {
                RecordState = new CreateOrEditRecordStateDto()
            };

            var cat = await _recordCategoriesAppService.GetRecordCategoryDto(catid);
            if (cat == null) throw new UserFriendlyException(L("RequirementCategoryNotFound"));
            getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification
            getRecordStateForEditOutput.RecordState.UserId = (long)id;
            var viewModel = new CreateOrEditRecordStateModalViewModel();
            viewModel.RecordState = getRecordStateForEditOutput.RecordState;
            viewModel.Recordfilename = getRecordStateForEditOutput.Recordfilename;
            viewModel.RecordCategoryName = cat.Name;
            viewModel.RecordState.RecordCategoryId = cat.Id;
            viewModel.RecordState.RecordCategoryDto = cat; //  ObjectMapper.Map<RecordCategoryDto>(cat);
            viewModel.UserName = getRecordStateForEditOutput.UserName;
            viewModel.RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName;
            viewModel.RecordStateRecordStatusList = await _commplianceAppService.GetAllRecordStatusForTableDropdown(cat.TenantId, cat.IsSurpathService);
            viewModel.CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel();

            //var viewModel = new CreateOrEditRecordStateModalViewModel()
            //{
            //    RecordState = getRecordStateForEditOutput.RecordState,
            //    Recordfilename = getRecordStateForEditOutput.Recordfilename,
            //    RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
            //    UserName = getRecordStateForEditOutput.UserName,
            //    RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
            //    RecordStateRecordStatusList = await _commplianceAppService.GetAllRecordStatusForTableDropdown(),
            //    CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel()

            //};
            viewModel.RecordState.RecordDto = new RecordDto();

            return PartialView("_CreateNewRecordForCategoryModal", viewModel);
        }

        //[AbpMvcAuthorize]
        //[HttpPost]
        //public async Task<PartialViewResult> CreateNewRecordModal(Guid? id)
        //{
        //    GetRecordStateForEditOutput getRecordStateForEditOutput;

        //    getRecordStateForEditOutput = new GetRecordStateForEditOutput
        //    {
        //        RecordState = new CreateOrEditRecordStateDto()
        //    };
        //    getRecordStateForEditOutput.RecordState.Notes = String.Empty; // so a note can be added and sent for notification

        //    var viewModel = new CreateOrEditRecordStateModalViewModel();
        //    viewModel.RecordState = getRecordStateForEditOutput.RecordState;
        //    viewModel.Recordfilename = getRecordStateForEditOutput.Recordfilename;
        //    viewModel.RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName;
        //    viewModel.UserName = getRecordStateForEditOutput.UserName;
        //    viewModel.RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName;
        //    viewModel.RecordStateRecordStatusList = await _commplianceAppService.GetAllRecordStatusForTableDropdown();
        //    viewModel.CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel();

        //    //var viewModel = new CreateOrEditRecordStateModalViewModel()
        //    //{
        //    //    RecordState = getRecordStateForEditOutput.RecordState,
        //    //    Recordfilename = getRecordStateForEditOutput.Recordfilename,
        //    //    RecordCategoryName = getRecordStateForEditOutput.RecordCategoryName,
        //    //    UserName = getRecordStateForEditOutput.UserName,
        //    //    RecordStatusStatusName = getRecordStateForEditOutput.RecordStatusStatusName,
        //    //    RecordStateRecordStatusList = await _commplianceAppService.GetAllRecordStatusForTableDropdown(),
        //    //    CreateOrEditRecordModalViewModel = new CreateOrEditRecordModalViewModel()

        //    //};
        //    viewModel.RecordState.RecordDto = new RecordDto();

        //    return PartialView("_CreateNewRecordModal", viewModel);
        //}

        public PartialViewResult RecordCategoryLookupTableModal(Guid? id, string displayName, bool confirm)
        {
            var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = "",
                confirm = confirm,
            };

            return PartialView("_RecordCategoryLookupTableModal", viewModel);
        }

        public PartialViewResult RecordCategoryConfirmLookupTableModal(Guid? id, string displayName, bool confirm)
        {
            var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = "",
                confirm = confirm,
            };

            return PartialView("_RecordCategoryConfirmLookupTableModal", viewModel);
        }

        //[AbpAllowAnonymous]

        //public PartialViewResult CohortLookupTableModal(Guid? id, string displayName, bool confirm)
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

        //[AbpAllowAnonymous]

        //public PartialViewResult CohortConfirmLookupTableModal(Guid? id, string displayName, bool confirm)
        //{
        //    var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
        //    {
        //        Id = id.ToString(),
        //        DisplayName = displayName,
        //        FilterText = "",
        //        confirm = confirm,
        //    };

        //    return PartialView("_RecordCategoryConfirmLookupTableModal", viewModel);
        //}
        [AbpAllowAnonymous]
        public PartialViewResult TenantDepartmentLookupTableRegModal(string? confirm = "false")
        {
            var _confirm = bool.Parse(confirm);
            var viewModel = new TenantDepartmentLookupTableViewModel()
            {
                FilterText = "",
                confirm = _confirm
            };

            return PartialView("_TenantDepartmentLookupTableRegModal", viewModel);
        }

        [AbpAllowAnonymous]
        public PartialViewResult CohortLookupTableRegModal(Guid? tenantDepartmentId, string? confirm = "false", long? tenantid = null, Guid? excludeCohortId = null)
        {
            var _confirm = bool.Parse(confirm);
            var viewModel = new CohortLookupTableViewModel()
            {
                FilterText = "",
                confirm = _confirm,
                TenantDepartmentId = tenantDepartmentId,
                ExcludeIdGuid = excludeCohortId
            };
            //ViewBag.TenantId = tenantid;
            if (!tenantid.HasValue)
                tenantid = AbpSession.TenantId;
            ViewBag.TenantId = tenantid.Value;

            return PartialView("_CohortLookupTableRegModal", viewModel);
        }

        //[AbpAllowAnonymous]

        //public PartialViewResult TenantDepartmentConfirmLookupTableModal(Guid? id, string displayName, bool confirm)
        //{
        //    var viewModel = new RecordStateRecordCategoryLookupTableViewModel()
        //    {
        //        Id = id.ToString(),
        //        DisplayName = displayName,
        //        FilterText = "",
        //        confirm = confirm,
        //    };

        //    return PartialView("_RecordCategoryConfirmLookupTableModal", viewModel);
        //}

        public PartialViewResult TenantDocumentCategoryLookupTableModal(Guid? id, string displayName)
        {
            var viewModel = new RecordTenantDocumentCategoryLookupTableViewModel()
            {
                Id = id.ToString(),
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_RecordTenantDocumentCategoryLookupTableModal", viewModel);
        }

        public FileUploadCacheOutput UploadfiledataFile()
        {
            Logger.Debug("UploadfiledataFile called");

            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    Logger.Warn("No file uploaded, throwing");
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > SurpathSettings.MaxfiledataLength)
                {
                    Logger.Warn("File size exceeded, throwing");
                    //throw new UserFriendlyException(L("Warn_File_SizeLimit"), $"The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}");
                    //throw new UserFriendlyException(L("Warn_File_SizeLimit", $"The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}"));
                    throw new UserFriendlyException(L("Warn_File_SizeLimit") + $" The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}");
                }
                var fileExtension = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(fileExtension) || fileExtension.Length <= 1)
                {
                    Logger.Warn("File extension is empty, throwing");
                    throw new UserFriendlyException(L("InvalidFileExtension"));
                }

                var fileType = fileExtension.Substring(1);
                if (filedataAllowedFileTypes != null && filedataAllowedFileTypes.Length > 0 && !filedataAllowedFileTypes.Contains(fileType))
                {
                    Logger.Warn("File type not allowed, throwing");
                    //throw new UserFriendlyException(L("FileNotInAllowedFileTypes", filedataAllowedFileTypes));
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes") + $" Allowed file types are {SurpathSettings.AllowedFileExtensions}");
                }

                Logger.Debug("Uploading file...");
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                Logger.Debug("File uploaded");
                var fileToken = Guid.NewGuid().ToString("N");
                Logger.Debug($"Setting file cache... {fileToken}");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));
                Logger.Debug($"File cache set... {fileToken}");
                Logger.Debug($"Returning file token... {fileToken}");
                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

        #region CreateRequirement

        public async Task<PartialViewResult> CreateEditRequirementModal(Guid? id, int step = 1)
        {
            CreateRequirementViewModel viewModel = new CreateRequirementViewModel();

            GetRecordRequirementForEditOutput getRecordRequirementForEditOutput;

            List<CreateOrEditRecordCategoryModalViewModel> CreateOrEditRecordCategoryViewModelList = new List<CreateOrEditRecordCategoryModalViewModel>();

            var _ruleList = await _recordCategoriesAppService.GetAllRecordCategoryRuleForTableDropdown();

            if (id.HasValue)
            {
                getRecordRequirementForEditOutput = await _recordRequirementsAppService.GetRecordRequirementForEdit(new EntityDto<Guid> { Id = (Guid)id });

                // GetRecordCategoriesForRequirementForEdit
                var _cats = await _commplianceAppService.GetRecordCategoriesForRequirementForEdit(new EntityDto<Guid> { Id = (Guid)id });

                foreach (var _cat in _cats)
                {
                    CreateOrEditRecordCategoryViewModelList.Add(new CreateOrEditRecordCategoryModalViewModel()
                    {
                        RecordCategory = _cat.RecordCategory,
                        RecordCategoryRecordCategoryRuleList = _ruleList,
                        RecordCategoryRuleName = _cat.RecordCategoryRuleName,
                        RecordRequirementName = _cat.RecordRequirementName,
                        RecordCategoryRecordStateCount = _cat.RecordCategoryRecordStateCount,
                    });
                }

                CreateOrEditRecordCategoryViewModelList.ForEach(rc =>
                {
                    rc.RecordCategoryRecordCategoryRuleList = _ruleList;
                });
                if (CreateOrEditRecordCategoryViewModelList.Count == 0)
                    CreateOrEditRecordCategoryViewModelList = EmptyCreateOrEditRecordCategoryViewModels(_ruleList);
            }
            else
            {
                getRecordRequirementForEditOutput = new GetRecordRequirementForEditOutput
                {
                    RecordRequirement = new CreateOrEditRecordRequirementDto()
                };

                CreateOrEditRecordCategoryViewModelList = EmptyCreateOrEditRecordCategoryViewModels(_ruleList);
            }

            viewModel.CreateOrEditRecordRequirement = new CreateOrEditRecordRequirementModalViewModel()
            {
                RecordRequirement = getRecordRequirementForEditOutput.RecordRequirement,
                TenantDepartmentName = getRecordRequirementForEditOutput.TenantDepartmentName,
                CohortName = getRecordRequirementForEditOutput.CohortName,
            };
            viewModel.CreateOrEditRecordCategories = CreateOrEditRecordCategoryViewModelList;
            viewModel.RecordCategoryRecordCategoryRuleList = _ruleList;
            viewModel.step = step;

            return PartialView("_CreateEditRequirementModal", viewModel);
        }

        private List<CreateOrEditRecordCategoryModalViewModel> EmptyCreateOrEditRecordCategoryViewModels(List<RecordCategoryRecordCategoryRuleLookupTableDto> _ruleList)
        {
            var retval = new List<CreateOrEditRecordCategoryModalViewModel>();

            GetRecordCategoryForEditOutput getRecordCategoryForEditOutput;

            getRecordCategoryForEditOutput = new GetRecordCategoryForEditOutput
            {
                RecordCategory = new CreateOrEditRecordCategoryDto()
            };
            retval.Add(new CreateOrEditRecordCategoryModalViewModel()
            {
                RecordCategory = getRecordCategoryForEditOutput.RecordCategory,
                RecordRequirementName = getRecordCategoryForEditOutput.RecordRequirementName,
                RecordCategoryRuleName = getRecordCategoryForEditOutput.RecordCategoryRuleName,
                RecordCategoryRecordCategoryRuleList = _ruleList
            });
            return retval;
        }

        #endregion CreateRequirement
    }
}