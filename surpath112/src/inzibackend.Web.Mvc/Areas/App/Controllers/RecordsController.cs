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

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Records)]
    public class RecordsController : inzibackendControllerBase
    {
        private readonly IRecordsAppService _recordsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ITenantDocumentsAppService _tenantDocumentsAppService;
        private readonly ITenantDocumentCategoriesAppService _tenantDocumentCategoriesAppService;


        //private const long MaxfiledataLength = 5242880; //5MB
        //private const string MaxfiledataLengthUserFriendlyValue = "5MB"; //5MB
        private readonly string[] filedataAllowedFileTypes = SurpathSettings.AllowedFileExtensionsArray; // { "jpeg", "jpg", "png", "pdf", "txt", "hl7" };

        public RecordsController(IRecordsAppService recordsAppService, ITempFileCacheManager tempFileCacheManager, ITenantDocumentsAppService tenantDocumentsAppService, ITenantDocumentCategoriesAppService tenantDocumentCategoriesAppService)
        {
            _recordsAppService = recordsAppService;
            _tempFileCacheManager = tempFileCacheManager;
            _tenantDocumentsAppService = tenantDocumentsAppService;
            _tenantDocumentCategoriesAppService = tenantDocumentCategoriesAppService;
        }

        public ActionResult Index()
        {
            var model = new RecordsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

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
        //        getRecordForEditOutput.Record.EffectiveDate = DateTime.Now;
        //        getRecordForEditOutput.Record.ExpirationDate = DateTime.Now;
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

        [AbpMvcAuthorize(AppPermissions.Pages_Records_Create, AppPermissions.Pages_Records_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(Guid? id, Guid? catid)
        {
            GetRecordForEditOutput getRecordForEditOutput;

            if (catid == null) catid = Guid.Empty;
            if (id == null) id = Guid.Empty;

            if (id.HasValue && id != Guid.Empty)
            {
                getRecordForEditOutput = await _recordsAppService.GetRecordForEdit(new EntityDto<Guid> { Id = (Guid)id });
            }
            else
            {
                getRecordForEditOutput = new GetRecordForEditOutput
                {
                    Record = new CreateOrEditRecordDto()
                };
                getRecordForEditOutput.Record.DateUploaded = DateTime.UtcNow;
                getRecordForEditOutput.Record.DateLastUpdated = DateTime.UtcNow;
            }
            var _tenantDocCat = new GetTenantDocumentCategoryForViewDto();
            Guid? _tenantDocCatId;
            if (catid != Guid.Empty)
            {
                _tenantDocCat = await _tenantDocumentCategoriesAppService.GetTenantDocumentCategoryForView((Guid)catid);
            }

            var viewModel = new CreateOrEditRecordModalViewModel()
            {
                Record = getRecordForEditOutput.Record,
                TenantDocumentCategoryName = _tenantDocCat.TenantDocumentCategory.Name,
                filedataFileName = getRecordForEditOutput.filedataFileName,
                TenantDocumentCategoryId = (catid == Guid.Empty) ? null : _tenantDocCat.TenantDocumentCategory.Id,
            };

            foreach (var filedataAllowedFileType in filedataAllowedFileTypes)
            {
                viewModel.filedataFileAcceptedTypes += "." + filedataAllowedFileType + ",";
            }

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewRecordModal(Guid id)
        {
            var getRecordForViewDto = await _recordsAppService.GetRecordForView(id);

            var model = new RecordViewModel()
            {
                Record = getRecordForViewDto.Record
                ,
                TenantDocumentCategoryName = getRecordForViewDto.TenantDocumentCategoryName

            };

            return PartialView("_ViewRecordModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Records_Create, AppPermissions.Pages_Records_Edit)]
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

        [AbpMvcAuthorize(AppPermissions.Pages_RecordStates)]
        public FileUploadCacheOutput UploadfiledataFile()
        {
            try
            {
                //Check input
                if (Request.Form.Files.Count == 0)
                {
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > SurpathSettings.MaxfiledataLength)
                {
                    //throw new UserFriendlyException(L("Warn_File_SizeLimit", SurpathSettings.MaxfiledataLengthUserFriendlyValue));
                    //throw new UserFriendlyException(L("Warn_File_SizeLimit", $"The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}"));
                    throw new UserFriendlyException(L("Warn_File_SizeLimit") + $" The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}");
                    //throw new UserFriendlyException(L("Warn_File_SizeLimit"), $"The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}");
                }

                var fileType = Path.GetExtension(file.FileName).Substring(1);
                if (filedataAllowedFileTypes != null && filedataAllowedFileTypes.Length > 0 && !filedataAllowedFileTypes.Contains(fileType))
                {
                    //throw new UserFriendlyException(L("FileNotInAllowedFileTypes", filedataAllowedFileTypes));
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes") + $" Allowed file types are {SurpathSettings.AllowedFileExtensions}");
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var fileToken = Guid.NewGuid().ToString("N");
                _tempFileCacheManager.SetFile(fileToken, new TempFileInfo(file.FileName, fileType, fileBytes));

                return new FileUploadCacheOutput(fileToken);
            }
            catch (UserFriendlyException ex)
            {
                return new FileUploadCacheOutput(new ErrorInfo(ex.Message));
            }
        }

    }
}