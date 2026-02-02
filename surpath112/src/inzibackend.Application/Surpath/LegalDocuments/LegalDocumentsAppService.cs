using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using inzibackend.Authorization;
using inzibackend.Dto;
using inzibackend.Storage;
using inzibackend.Surpath.Dtos.LegalDocuments;
using inzibackend.Surpath.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mime;
using System.Threading.Tasks;

namespace inzibackend.Surpath
{
    /// <summary>
    /// Application service for managing legal documents
    /// </summary>
    [AbpAuthorize]
    public class LegalDocumentsAppService : inzibackendAppServiceBase, ILegalDocumentsAppService
    {
        private readonly IRepository<LegalDocument, Guid> _legalDocumentRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ICacheManager _cacheManager;
        private const string LegalDocumentUrlsCacheKey = "LegalDocumentUrls";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="legalDocumentRepository">Repository for legal documents</param>
        /// <param name="binaryObjectManager">Binary object manager for file storage</param>
        public LegalDocumentsAppService(
            IRepository<LegalDocument, Guid> legalDocumentRepository,
            IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager,
            ICacheManager cacheManager)
        {
            _legalDocumentRepository = legalDocumentRepository;
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// Get all legal documents with filtering and paging
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged list of legal documents</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_View)]
        public async Task<PagedResultDto<GetLegalDocumentForViewDto>> GetAll(GetAllLegalDocumentsInput input)
        {
            var filteredLegalDocuments = _legalDocumentRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.FileName.Contains(input.Filter))
                .WhereIf(input.TypeFilter.HasValue, e => e.Type == input.TypeFilter.Value);

            var pagedAndFilteredLegalDocuments = filteredLegalDocuments
                .OrderBy(input.Sorting ?? "creationTime desc")
                .PageBy(input);

            var totalCount = await filteredLegalDocuments.CountAsync();

            var legalDocuments = await pagedAndFilteredLegalDocuments.ToListAsync();
            var legalDocumentDtos = ObjectMapper.Map<List<LegalDocumentDto>>(legalDocuments);

            return new PagedResultDto<GetLegalDocumentForViewDto>(
                totalCount,
                legalDocumentDtos.Select(o => new GetLegalDocumentForViewDto
                {
                    LegalDocument = o
                }).ToList()
            );
        }

        /// <summary>
        /// Get a legal document for viewing
        /// </summary>
        /// <param name="input">Legal document ID</param>
        /// <returns>Legal document view DTO</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_View)]
        public async Task<GetLegalDocumentForViewDto> GetLegalDocumentForView(GetLegalDocumentForViewInput input)
        {
            var legalDocument = await _legalDocumentRepository.GetAsync(input.Id);

            var output = new GetLegalDocumentForViewDto
            {
                LegalDocument = ObjectMapper.Map<LegalDocumentDto>(legalDocument)
            };

            return output;
        }

        /// <summary>
        /// Get a legal document for editing
        /// </summary>
        /// <param name="input">Legal document ID</param>
        /// <returns>Legal document edit DTO</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public async Task<GetLegalDocumentForEditDto> GetLegalDocumentForEdit(GetLegalDocumentForEditInput input)
        {
            var legalDocument = await _legalDocumentRepository.GetAsync(input.Id);

            var output = new GetLegalDocumentForEditDto
            {
                LegalDocument = ObjectMapper.Map<LegalDocumentDto>(legalDocument)
            };

            return output;
        }

        /// <summary>
        /// Create or update a legal document
        /// </summary>
        /// <param name="input">Legal document data and file</param>
        /// <returns>Legal document ID</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public async Task<Guid> CreateOrEdit(CreateOrUpdateLegalDocumentDto input)
        {
            if (input.LegalDocument.Id != Guid.Empty)
            {
                return await Update(input);
            }
            else
            {
                return await Create(input);
            }
        }

        /// <summary>
        /// Create a new legal document
        /// </summary>
        /// <param name="input">Legal document data and file</param>
        /// <returns>Legal document ID</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        protected virtual async Task<Guid> Create(CreateOrUpdateLegalDocumentDto input)
        {
            var legalDocument = ObjectMapper.Map<LegalDocument>(input.LegalDocument);

            if (AbpSession.TenantId != null)
            {
                legalDocument.TenantId = (int?)AbpSession.TenantId;
            }

            // Handle file upload if token is provided
            if (!string.IsNullOrEmpty(input.FileToken))
            {
                var fileObject = await GetBinaryObjectFromCache(input.FileToken);
                if (fileObject != null)
                {
                    legalDocument.FileId = fileObject.Id;
                    legalDocument.FileName = fileObject.Description.Split(" - ").LastOrDefault()?.Split(" uploaded by ").FirstOrDefault() ?? "document";
                }
            }

            // Set view URL with the proper format for the ViewLegalDocument action
            legalDocument.ViewUrl = $"/LegalDocuments/View/{legalDocument.Type}";

            await _legalDocumentRepository.DeleteAsync(d =>
                d.Type == input.LegalDocument.Type && d.TenantId == (int)AbpSession.TenantId
            );

            var id = await _legalDocumentRepository.InsertAndGetIdAsync(legalDocument);
            
            // Clear the URL cache when a new document is created
            ClearLegalDocumentUrlsCache();

            return id;
        }

        /// <summary>
        /// Update an existing legal document
        /// </summary>
        /// <param name="input">Legal document data and file</param>
        /// <returns>Legal document ID</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        protected virtual async Task<Guid> Update(CreateOrUpdateLegalDocumentDto input)
        {
            var legalDocument = await _legalDocumentRepository.GetAsync(input.LegalDocument.Id);

            ObjectMapper.Map(input.LegalDocument, legalDocument);

            // Handle file upload if token is provided
            if (!string.IsNullOrEmpty(input.FileToken))
            {
                // Delete old file if exists
                if (legalDocument.FileId.HasValue)
                {
                    await _binaryObjectManager.DeleteAsync(legalDocument.FileId.Value);
                }

                var fileObject = await GetBinaryObjectFromCache(input.FileToken);
                if (fileObject != null)
                {
                    legalDocument.FileId = fileObject.Id;
                    legalDocument.FileName = fileObject.Description.Split(" - ").LastOrDefault()?.Split(" uploaded by ").FirstOrDefault() ?? "document";
                }
            }

            // Set view URL with the proper format for the ViewLegalDocument action
            legalDocument.ViewUrl = $"/LegalDocuments/View/{legalDocument.Type}";

            await _legalDocumentRepository.UpdateAsync(legalDocument);

            // Clear the URL cache when a document is updated
            ClearLegalDocumentUrlsCache();

            return legalDocument.Id;
        }

        private string GetDestFolder(int? TenantId)
        {
            // var destfolder = $"{appFolders.SurpathRootFolder}";
            var _tenantid = TenantId == null ? "surscan" : TenantId.Value.ToString();
            var destFolder = Path.Combine(SurpathSettings.SurpathRecordsRootFolder, _tenantid, "Legal");
            destFolder = destFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            return destFolder;
        }

        /// <summary>
        /// Delete a legal document
        /// </summary>
        /// <param name="id">Legal document ID</param>
        /// <returns>Task</returns>
        [AbpAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public async Task Delete(Guid id)
        {
            var legalDocument = await _legalDocumentRepository.GetAsync(id);

            // Delete the file from binary storage if exists
            if (legalDocument.FileId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(legalDocument.FileId.Value);
            }

            await _legalDocumentRepository.DeleteAsync(id);

            // Clear the URL cache when a document is deleted
            ClearLegalDocumentUrlsCache();
        }

        /// <summary>
        /// Get the latest legal document of a specific type
        /// </summary>
        /// <param name="type">Document type</param>
        /// <returns>Legal document view DTO</returns>
        public async Task<GetLegalDocumentForViewDto> GetLatestDocumentByType(DocumentType type)
        {
            var legalDocument = await _legalDocumentRepository.GetAll()
                .Where(d => d.Type == type)
                .OrderByDescending(d => d.CreationTime)
                .FirstOrDefaultAsync();

            if (legalDocument == null)
            {
                return null;
            }

            return new GetLegalDocumentForViewDto
            {
                LegalDocument = ObjectMapper.Map<LegalDocumentDto>(legalDocument)
            };
        }

        /// <summary>
        /// Retrieves a binary object from the temporary file cache using a token
        /// </summary>
        /// <param name="fileToken">The file token</param>
        /// <returns>The binary object</returns>
        public async Task<BinaryObject> GetBinaryObjectFromCache(string fileToken)
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

            //var folder = $"LegalDocuments/{AbpSession.TenantId}";
            var folder = GetDestFolder(AbpSession.TenantId);
            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, $"{DateTime.Now:g} - {fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, folder, fileCache.FileName, "");

            // Validate file type
            if (!HtmlSanitizer.IsAllowedFileType(fileCache.FileName))
            {
                throw new UserFriendlyException(L("InvalidFileType"));
            }

            // If the file is an HTML file, sanitize it to prevent XSS attacks
            if (fileCache.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
                fileCache.FileName.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
            {
                string htmlContent = System.Text.Encoding.UTF8.GetString(fileCache.File);

                // Apply enhanced sanitization
                string sanitizedHtml = HtmlSanitizer.SanitizeHtml(htmlContent);
                byte[] sanitizedBytes = System.Text.Encoding.UTF8.GetBytes(sanitizedHtml);

                // Check if the HTML is safe after sanitization
                if (!HtmlSanitizer.IsHtmlSafe(sanitizedHtml))
                {
                    throw new UserFriendlyException(L("UnsafeHtmlContent"));
                }

                storedFile = new BinaryObject(AbpSession.TenantId, sanitizedBytes, $"{DateTime.Now:g} - {fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, folder, fileCache.FileName, "");
            }

            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile;
        }

        /// <summary>
        /// Checks if the document is the last one of its type
        /// </summary>
        /// <param name="type">The document type</param>
        /// <returns>True if it's the last document of its type, false otherwise</returns>
        public async Task<bool> IsLastDocumentOfType(DocumentType type)
        {
            var count = await _legalDocumentRepository.CountAsync(d => d.Type == type);
            return count <= 1;
        }

        /// <summary>
        /// Get cached legal document URLs for display in the UI
        /// </summary>
        /// <returns>Dictionary of document types and their URLs</returns>
        [AbpAuthorize]
        public async Task<Dictionary<DocumentType, string>> GetLegalDocumentUrls()
        {
            var cacheKey = GetLegalDocumentUrlsCacheKey();
            var cache = _cacheManager.GetCache(LegalDocumentUrlsCacheKey);

            // Try to get from cache first
            var cachedUrls = await cache.GetOrDefaultAsync(cacheKey);
            if (cachedUrls != null)
            {
                return (Dictionary<DocumentType, string>)cachedUrls;
            }

            // If not in cache, get from database and add to cache
            var urls = await GetLegalDocumentUrlsFromDatabase();
            await cache.SetAsync(cacheKey, urls);

            return urls;
        }

        /// <summary>
        /// Gets legal document URLs from the database
        /// </summary>
        /// <returns>Dictionary of document types and their URLs</returns>
        private async Task<Dictionary<DocumentType, string>> GetLegalDocumentUrlsFromDatabase()
        {
            var result = new Dictionary<DocumentType, string>();

            // Get all available document types
            var documentTypes = Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>();
            
            // Get the latest version of each document type
            var latestDocuments = await _legalDocumentRepository.GetAll()
                .Where(d => d.TenantId == AbpSession.TenantId)
                .ToListAsync();

            // Group documents by type and get the latest one of each type
            var latestByType = latestDocuments
                .GroupBy(d => d.Type)
                .Select(g => g.OrderByDescending(d => d.CreationTime).FirstOrDefault())
                .Where(d => d != null)
                .ToList();

            // Create URLs for each type that has a document
            foreach (var document in latestByType)
            {
                // Use the new route format that goes through ViewLegalDocument action
                result[document.Type] = $"/LegalDocuments/View/{document.Type}";
            }

            return result;
        }

        /// <summary>
        /// Gets the cache key for legal document URLs, scoped to the current tenant
        /// </summary>
        /// <returns>The cache key</returns>
        private string GetLegalDocumentUrlsCacheKey()
        {
            return $"{AbpSession.TenantId}_{LegalDocumentUrlsCacheKey}";
        }

        /// <summary>
        /// Clears the legal document URLs cache for the current tenant
        /// </summary>
        private void ClearLegalDocumentUrlsCache()
        {
            _cacheManager.GetCache(LegalDocumentUrlsCacheKey).Remove(GetLegalDocumentUrlsCacheKey());
        }
    }
}