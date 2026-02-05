using inzibackend.Surpath;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using inzibackend.Surpath.Exporting;
using inzibackend.Surpath.Dtos;
using inzibackend.Dto;
using Abp.Application.Services.Dto;
using inzibackend.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using inzibackend.Storage;
using Abp.Domain.Uow;
using System.IO;
using inzibackend.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


namespace inzibackend.Surpath
{
    [AbpAuthorize(AppPermissions.Pages_Records)]
    public class RecordsAppService : inzibackendAppServiceBase, IRecordsAppService
    {
        private readonly IRepository<Record, Guid> _recordRepository;
        private readonly IRecordsExcelExporter _recordsExcelExporter;
        private readonly IRepository<TenantDocumentCategory, Guid> _tenantDocumentCategoryLookUpRepository;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        private readonly IRepository<TenantDocument, Guid> _tenantDocumentRepository;
        //public string SurpathRecordsRootFolder { get; private set; }
        //private readonly ISurpathComplianceAppService _surpathComplianceAppService;

        public RecordsAppService(IRepository<Record, Guid> recordRepository, IRecordsExcelExporter recordsExcelExporter, IRepository<TenantDocumentCategory, Guid> lookup_tenantDocumentCategoryRepository, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager,
            IRepository<TenantDocument, Guid> tenantDocumentRepository
            //,ISurpathComplianceAppService surpathComplianceAppService
            )
        {
            _recordRepository = recordRepository;
            _recordsExcelExporter = recordsExcelExporter;
            _tenantDocumentCategoryLookUpRepository = lookup_tenantDocumentCategoryRepository;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

            _tenantDocumentRepository = tenantDocumentRepository;
            //string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory(), _environment, true);

            //// var _dir = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
            //SurpathRecordsRootFolder = Configuration.GetValue<string>("Surpath:SurpathRecordsRootFolder");
            ////_surpathComplianceAppService = surpathComplianceAppService;


        }

        public async Task<PagedResultDto<GetRecordForViewDto>> GetAll(GetAllRecordsInput input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var filteredRecords = _recordRepository.GetAll()
                        .Include(e => e.TenantDocumentCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.filename.Contains(input.Filter) || e.physicalfilepath.Contains(input.Filter) || e.metadata.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.filenameFilter), e => e.filename.Contains(input.filenameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.physicalfilepathFilter), e => e.physicalfilepath.Contains(input.physicalfilepathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.metadataFilter), e => e.metadata.Contains(input.metadataFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjIdFilter.ToString()), e => e.BinaryObjId.ToString() == input.BinaryObjIdFilter.ToString())
                        .WhereIf(input.MinDateUploadedFilter != null, e => e.DateUploaded >= input.MinDateUploadedFilter)
                        .WhereIf(input.MaxDateUploadedFilter != null, e => e.DateUploaded <= input.MaxDateUploadedFilter)
                        .WhereIf(input.MinDateLastUpdatedFilter != null, e => e.DateLastUpdated >= input.MinDateLastUpdatedFilter)
                        .WhereIf(input.MaxDateLastUpdatedFilter != null, e => e.DateLastUpdated <= input.MaxDateLastUpdatedFilter)
                        .WhereIf(input.InstructionsConfirmedFilter.HasValue && input.InstructionsConfirmedFilter > -1, e => (input.InstructionsConfirmedFilter == 1 && e.InstructionsConfirmed) || (input.InstructionsConfirmedFilter == 0 && !e.InstructionsConfirmed))
                        .WhereIf(input.MinEffectiveDateFilter != null, e => e.EffectiveDate >= input.MinEffectiveDateFilter)
                        .WhereIf(input.MaxEffectiveDateFilter != null, e => e.EffectiveDate <= input.MaxEffectiveDateFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentCategoryNameFilter), e => e.TenantDocumentCategoryFk != null && e.TenantDocumentCategoryFk.Name == input.TenantDocumentCategoryNameFilter);

            var pagedAndFilteredRecords = filteredRecords
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var records = from o in pagedAndFilteredRecords
                          join o1 in _tenantDocumentCategoryLookUpRepository.GetAll() on o.TenantDocumentCategoryId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new
                          {

                              o.filedata,
                              o.filename,
                              o.physicalfilepath,
                              o.metadata,
                              o.BinaryObjId,
                              o.DateUploaded,
                              o.DateLastUpdated,
                              o.InstructionsConfirmed,
                              o.EffectiveDate,
                              o.ExpirationDate,
                              Id = o.Id,
                              TenantDocumentCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                          };

            var totalCount = await filteredRecords.CountAsync();

            var dbList = await records.ToListAsync();
            var results = new List<GetRecordForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRecordForViewDto()
                {
                    Record = new RecordDto
                    {

                        filedata = o.filedata,
                        filename = o.filename,
                        physicalfilepath = o.physicalfilepath,
                        metadata = o.metadata,
                        BinaryObjId = o.BinaryObjId,
                        DateUploaded = o.DateUploaded,
                        DateLastUpdated = o.DateLastUpdated,
                        InstructionsConfirmed = o.InstructionsConfirmed,
                        EffectiveDate = o.EffectiveDate,
                        ExpirationDate = o.ExpirationDate,
                        Id = o.Id,
                    },
                    TenantDocumentCategoryName = o.TenantDocumentCategoryName
                };
                res.Record.filedataFileName = await GetBinaryFileName(o.filedata);

                results.Add(res);
            }

            return new PagedResultDto<GetRecordForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRecordForViewDto> GetRecordForView(Guid id)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var record = await _recordRepository.GetAsync(id);

            var output = new GetRecordForViewDto { Record = ObjectMapper.Map<RecordDto>(record) };

            if (output.Record.TenantDocumentCategoryId != null)
            {
                var _lookupTenantDocumentCategory = await _tenantDocumentCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.Record.TenantDocumentCategoryId);
                output.TenantDocumentCategoryName = _lookupTenantDocumentCategory?.Name?.ToString();
            }

            output.Record.filedataFileName = await GetBinaryFileName(record.filedata);

            return output;

        }

        [AbpAuthorize(AppPermissions.Pages_Records_Edit)]
        public async Task<GetRecordForEditOutput> GetRecordForEdit(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var record = await _recordRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRecordForEditOutput { Record = ObjectMapper.Map<CreateOrEditRecordDto>(record) };

            if (output.Record.TenantDocumentCategoryId != null)
            {
                var _lookupTenantDocumentCategory = await _tenantDocumentCategoryLookUpRepository.FirstOrDefaultAsync((Guid)output.Record.TenantDocumentCategoryId);
                output.TenantDocumentCategoryName = _lookupTenantDocumentCategory?.Name?.ToString();
            }

            output.filedataFileName = await GetBinaryFileName(record.filedata);

            return output;

        }

        public async Task CreateOrEdit(CreateOrEditRecordDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            if (input.Id == null)
            {
                input.DateUploaded = DateTime.UtcNow;
                await Create(input);
            }
            else
            {
                input.DateLastUpdated = DateTime.UtcNow;
                await Update(input);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Records_Create)]
        protected virtual async Task Create(CreateOrEditRecordDto input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var record = ObjectMapper.Map<Record>(input);

            if (AbpSession.TenantId != null)
            {
                record.TenantId = (int?)AbpSession.TenantId;
            }

            await _recordRepository.InsertAsync(record);
            //record.filedata = await GetBinaryObjectFromCache(input.filedataToken);
            var storedFile = await GetBinaryObjectFromCache(input.filedataToken);
            record.filedata = storedFile.Id;
            record.filename = input.filename;
            record.metadata = storedFile.Metadata;
            record.physicalfilepath = storedFile.FileName;
            record.BinaryObjId = storedFile.Id;
            RecordCreateUpdatePostProcess(record);

        }

        [AbpAuthorize(AppPermissions.Pages_Records_Edit)]
        protected virtual async Task Update(CreateOrEditRecordDto input)
        {

            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var record = await _recordRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, record);
            //record.filedata = await GetBinaryObjectFromCache(input.filedataToken);
            var storedFile = await GetBinaryObjectFromCache(input.filedataToken);
            record.filedata = storedFile.Id;
            record.filename = input.filename;
            record.metadata = storedFile.Metadata;
            record.physicalfilepath = storedFile.FileName;
            record.BinaryObjId = storedFile.Id;

            RecordCreateUpdatePostProcess(record);

        }

        [AbpAuthorize(AppPermissions.Pages_Records_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            await _recordRepository.DeleteAsync(input.Id);

        }

        public async Task<FileDto> GetRecordsToExcel(GetAllRecordsForExcelInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            var filteredRecords = _recordRepository.GetAll()
                        .Include(e => e.TenantDocumentCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.filename.Contains(input.Filter) || e.physicalfilepath.Contains(input.Filter) || e.metadata.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.filenameFilter), e => e.filename.Contains(input.filenameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.physicalfilepathFilter), e => e.physicalfilepath.Contains(input.physicalfilepathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.metadataFilter), e => e.metadata.Contains(input.metadataFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjIdFilter.ToString()), e => e.BinaryObjId.ToString() == input.BinaryObjIdFilter.ToString())
                        .WhereIf(input.MinDateUploadedFilter != null, e => e.DateUploaded >= input.MinDateUploadedFilter)
                        .WhereIf(input.MaxDateUploadedFilter != null, e => e.DateUploaded <= input.MaxDateUploadedFilter)
                        .WhereIf(input.MinDateLastUpdatedFilter != null, e => e.DateLastUpdated >= input.MinDateLastUpdatedFilter)
                        .WhereIf(input.MaxDateLastUpdatedFilter != null, e => e.DateLastUpdated <= input.MaxDateLastUpdatedFilter)
                        .WhereIf(input.InstructionsConfirmedFilter.HasValue && input.InstructionsConfirmedFilter > -1, e => (input.InstructionsConfirmedFilter == 1 && e.InstructionsConfirmed) || (input.InstructionsConfirmedFilter == 0 && !e.InstructionsConfirmed))
                        .WhereIf(input.MinEffectiveDateFilter != null, e => e.EffectiveDate >= input.MinEffectiveDateFilter)
                        .WhereIf(input.MaxEffectiveDateFilter != null, e => e.EffectiveDate <= input.MaxEffectiveDateFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantDocumentCategoryNameFilter), e => e.TenantDocumentCategoryFk != null && e.TenantDocumentCategoryFk.Name == input.TenantDocumentCategoryNameFilter);

            var query = (from o in filteredRecords
                         join o1 in _tenantDocumentCategoryLookUpRepository.GetAll() on o.TenantDocumentCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetRecordForViewDto()
                         {
                             Record = new RecordDto
                             {
                                 filedata = o.filedata,
                                 filename = o.filename,
                                 physicalfilepath = o.physicalfilepath,
                                 metadata = o.metadata,
                                 BinaryObjId = o.BinaryObjId,
                                 DateUploaded = o.DateUploaded,
                                 DateLastUpdated = o.DateLastUpdated,
                                 InstructionsConfirmed = o.InstructionsConfirmed,
                                 EffectiveDate = o.EffectiveDate,
                                 ExpirationDate = o.ExpirationDate,
                                 Id = o.Id
                             },
                             TenantDocumentCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            // Enforce row count limit to prevent memory exhaustion and timeouts
            var count = await query.CountAsync();
            if (count > AppConsts.MaxExportRows)
            {
                throw new UserFriendlyException(
                    L("ExportLimitExceeded"),
                    L("ExportLimitExceededDetail", AppConsts.MaxExportRows, count)
                );
            }

            var recordListDtos = await query.ToListAsync();

            return _recordsExcelExporter.ExportToFile(recordListDtos);

        }

        [AbpAuthorize(AppPermissions.Pages_Records)]
        public async Task<PagedResultDto<RecordTenantDocumentCategoryLookupTableDto>> GetAllTenantDocumentCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            if (AbpSession.TenantId == null) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _tenantDocumentCategoryLookUpRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var tenantDocumentCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RecordTenantDocumentCategoryLookupTableDto>();
            foreach (var tenantDocumentCategory in tenantDocumentCategoryList)
            {
                lookupTableDtoList.Add(new RecordTenantDocumentCategoryLookupTableDto
                {
                    Id = tenantDocumentCategory.Id.ToString(),
                    DisplayName = tenantDocumentCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<RecordTenantDocumentCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        //private async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        //{
        //    if (fileToken.IsNullOrWhiteSpace())
        //    {
        //        return null;
        //    }

        //    var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

        //    if (fileCache == null)
        //    {
        //        throw new UserFriendlyException("There is no such file with the token: " + fileToken);
        //    }

        //    var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
        //    await _binaryObjectManager.SaveAsync(storedFile);

        //    return storedFile.Id;
        //}

        private async Task<BinaryObject> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }
            var _folder = GetDestFolder(AbpSession.TenantId, AbpSession.UserId);
            //var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, $"{fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, _folder);
            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, $"{fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, _folder, fileCache.FileName, "");
            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile;
        }

        private async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            //var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value, null, false);

            return file?.Description;
        }

        [AbpAuthorize(AppPermissions.Pages_Records_Edit)]
        public async Task RemovefiledataFile(EntityDto<Guid> input)
        {
            if (AbpSession.TenantId == null)
            {
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);
            }

            var record = await _recordRepository.FirstOrDefaultAsync(input.Id);
            if (record == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!record.filedata.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(record.filedata.Value);
            record.filedata = null;

        }

        private string GetDestFolder(int? TenantId, long? UserId)
        {
            // var destfolder = $"{appFolders.SurpathRootFolder}";
            var _tenantid = TenantId == null ? "surscan" : TenantId.Value.ToString();
            var destFolder = Path.Combine(SurpathSettings.SurpathRecordsRootFolder, _tenantid, UserId.ToString());
            destFolder = destFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            return destFolder;
        }

        private void RecordCreateUpdatePostProcess(Record record)
        {
            if (record.TenantDocumentCategoryId != null && record.TenantDocumentCategoryId != Guid.Empty)
            {
                // Create the tenantDocument record
                var _td = _tenantDocumentRepository.GetAll().Where(td => td.RecordId == record.Id).FirstOrDefault();
                if (_td == null)
                {
                    _td = new TenantDocument()
                    {
                        TenantDocumentCategoryId = (Guid)record.TenantDocumentCategoryId,
                        TenantId = record.TenantId,
                        AuthorizedOnly = true,
                        Name = record.filename,
                        RecordId = record.Id,
                    };
                    _tenantDocumentRepository.Insert(_td);
                }
                else
                {
                    _td.RecordId = record.Id;

                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Administration)]
        public async Task<Guid> ManualDocumentUpload(IFormFile file, string documentType)
        {
            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException(L("FileEmpty"));
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var binaryObject = new BinaryObject(AbpSession.TenantId, fileBytes, $"{documentType}_{file.FileName}");
                await _binaryObjectManager.SaveAsync(binaryObject);

                var record = new CreateOrEditRecordDto
                {
                    filename = file.FileName,
                    BinaryObjId = binaryObject.Id,
                    DateUploaded = DateTime.UtcNow,
                    DateLastUpdated = DateTime.UtcNow,
                    TenantDocumentCategoryId = null, // You might want to set this based on the documentType
                    metadata = $"{{\"DocumentType\": \"{documentType}\"}}"
                };

                await CreateOrEdit(record);

                return record.Id.Value;
            }
        }
    }
}