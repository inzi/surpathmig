using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Extensions;
using Abp.MimeTypes;
using Microsoft.AspNetCore.Mvc;
using inzibackend.Dto;
using inzibackend.Storage;
using Abp.Domain.Uow;
using inzibackend.Surpath.Compliance;
using inzibackend.Surpath;
using Microsoft.Extensions.Logging;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace inzibackend.Web.Controllers
{
    public class FileController : inzibackendControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IMimeTypeMap _mimeTypeMap;
        private readonly SurpathManager _surpathManager;
        private readonly IRepository<LegalDocument, Guid> _legalDocumentRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IMimeTypeMap mimeTypeMap,
            SurpathManager surpathManager,
            IRepository<LegalDocument, Guid> legalDocumentRepository,
            IWebHostEnvironment hostingEnvironment
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _mimeTypeMap = mimeTypeMap;
            _surpathManager = surpathManager;
            _legalDocumentRepository = legalDocumentRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// View a legal document by its type
        /// </summary>
        /// <param name="type">The type of legal document to view</param>
        /// <returns>The document file</returns>
        [Route("LegalDocuments/View/{type}")]
        [DisableAuditing]
        public async Task<ActionResult> ViewLegalDocument(string type)
        {
            Logger.Info($"ViewLegalDocument called with type: {type}");

            // Validate and parse the document type
            if (!Enum.TryParse<DocumentType>(type, true, out var documentType))
            {
                Logger.Warn($"Invalid document type requested: {type}");
                return NotFound("Invalid document type");
            }

            // Get the tenant ID
            int? tenantId = AbpSession.TenantId;
            if (!tenantId.HasValue)
            {
                Logger.Warn("Attempt to access legal document without valid tenant");
                return Unauthorized("You must be logged in to view this document");
            }

            try
            {
                // Get the latest document of the specified type for this tenant
                var document = await _legalDocumentRepository.FirstOrDefaultAsync(d =>
                    d.Type == documentType &&
                    d.TenantId == tenantId);

                if (document == null || !document.FileId.HasValue)
                {
                    Logger.Warn($"Legal document not found for type: {type}, tenant: {tenantId}");
                    return NotFound("Document not found");
                }

                // Check if DummyDocuments is enabled first (only allow in Development environment for security)
                if (SurpathSettings.DummyDocuments &&
                    !string.IsNullOrEmpty(SurpathSettings.DummyDocumentFileName) &&
                    _hostingEnvironment.IsDevelopment())
                {
                    if (System.IO.File.Exists(SurpathSettings.DummyDocumentFileName))
                    {
                        string dummyFileName = Path.GetFileName(SurpathSettings.DummyDocumentFileName);
                        string dummyContentType = _mimeTypeMap.GetMimeType(dummyFileName);
                        byte[] dummyBytes = await System.IO.File.ReadAllBytesAsync(SurpathSettings.DummyDocumentFileName);

                        Response.Headers.Add("Content-Disposition", "inline");
                        Logger.Info($"Serving dummy document for legal document: {document.Id}, Type: {type}, DummyFileName: {dummyFileName}");
                        return File(dummyBytes, dummyContentType);
                    }
                }

                // Get the file from binary storage
                var fileId = document.FileId.Value;
                var fileObject = await _binaryObjectManager.GetOrNullAsync(fileId, null, false);

                if (fileObject == null)
                {
                    Logger.Warn($"Binary file not found for legal document: {document.Id}, FileId: {fileId}");
                    return NotFound("Document file not found");
                }

                // Determine the file name and content type
                string fileName = document.FileName;
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = fileObject.OriginalFileName ?? $"{documentType}Document.pdf";
                }

                // Determine content type
                string contentType = _mimeTypeMap.GetMimeType(fileName);

                // Set headers for inline viewing
                Response.Headers.Add("Content-Disposition", "inline");

                Logger.Info($"Serving legal document: {document.Id}, Type: {type}, FileName: {fileName}");
                return File(fileObject.Bytes, contentType);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error viewing legal document. Type: {type}, TenantId: {tenantId}", ex);
                return StatusCode(500, "An error occurred while retrieving the document");
            }
        }

        [Authorize]
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            // Security: Verify file belongs to current user/tenant
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken, AbpSession.UserId, AbpSession.TenantId);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            // Does this user have the rights to this file?
            // If it's used in any recordstates, then the user has to have permissions to review or must be the owner

            Logger.Info("DownloadBinaryFile called with id: " + id.ToString() + " contentType: " + contentType + " fileName: " + fileName);
            Logger.Debug("Verifying access to file");
            await CheckAccessToFile(id, EnumSurpathFileAction.download);

            if (AbpSession.TenantId == 0) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // Check if DummyDocuments is enabled - override with dummy file (only allow in Development environment for security)
            if (SurpathSettings.DummyDocuments &&
                !string.IsNullOrEmpty(SurpathSettings.DummyDocumentFileName) &&
                _hostingEnvironment.IsDevelopment())
            {
                if (System.IO.File.Exists(SurpathSettings.DummyDocumentFileName))
                {
                    string dummyFileName = Path.GetFileName(SurpathSettings.DummyDocumentFileName);
                    string dummyContentType = _mimeTypeMap.GetMimeType(dummyFileName);
                    byte[] dummyBytes = await System.IO.File.ReadAllBytesAsync(SurpathSettings.DummyDocumentFileName);

                    Logger.Info($"Serving dummy document for download. Original ID: {id}, DummyFileName: {dummyFileName}");
                    return File(dummyBytes, dummyContentType, dummyFileName);
                }
            }

            var fileObject = await _binaryObjectManager.GetOrNullAsync(id, null, false);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            if (fileObject.IsFile)
            {
                fileName = fileObject.OriginalFileName;
            }
            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            return File(fileObject.Bytes, contentType, fileName);
        }

        public async Task<ActionResult> ViewBinaryFile(Guid id, string contentType, string fileName)
        {
            await CheckAccessToFile(id, EnumSurpathFileAction.view);

            if (AbpSession.TenantId == 0) CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant);

            // Check if DummyDocuments is enabled - override with dummy file (only allow in Development environment for security)
            if (SurpathSettings.DummyDocuments &&
                !string.IsNullOrEmpty(SurpathSettings.DummyDocumentFileName) &&
                _hostingEnvironment.IsDevelopment())
            {
                if (System.IO.File.Exists(SurpathSettings.DummyDocumentFileName))
                {
                    string dummyFileName = Path.GetFileName(SurpathSettings.DummyDocumentFileName);
                    string dummyContentType = _mimeTypeMap.GetMimeType(dummyFileName);
                    byte[] dummyBytes = await System.IO.File.ReadAllBytesAsync(SurpathSettings.DummyDocumentFileName);

                    Response.Headers.Add("Content-Disposition", "inline");
                    Logger.Info($"Serving dummy document for view. Original ID: {id}, DummyFileName: {dummyFileName}");
                    return File(dummyBytes, dummyContentType);
                }
            }

            var fileObject = await _binaryObjectManager.GetOrNullAsync(id, null, false);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            if (fileObject.IsFile)
            {
                fileName = fileObject.OriginalFileName;
            }
            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = fileName,
            //    Inline = true,
            //};
            Response.Headers.Add("Content-Disposition", "inline");
            //Response.Headers.Add("Content-Disposition", cd.ToString());

            var _h = Response.Headers;

            return File(fileObject.Bytes, contentType);
        }

        private async Task CheckAccessToFile(Guid id, EnumSurpathFileAction action)
        {
            var _access = await _surpathManager.UserAccessToFile((long)AbpSession.UserId, AbpSession.ImpersonatorUserId, id);
            if (!_access)
            {
                var ErrCode = $"FA:{AbpSession.UserId ?? 0}-{AbpSession.ImpersonatorUserId ?? 0}-{AbpSession.TenantId ?? 0}-{AbpSession.ImpersonatorTenantId ?? 0}-{id.ToString().ToLower()}";
                throw new UnauthorizedAccessException($"You are not authorized for this. ERR: {ErrCode}");
            }

            //var _access = false;
            //try
            //{
            //    _access = await _surpathManager.UserAccessToFile((long)AbpSession.UserId, (long)AbpSession.ImpersonatorUserId, id);
            //}
            //catch (Exception ex)
            //{
            //    var _msg = $"Error checking access to file. File ID: {id} Action: {action.ToString()} AbpSession.TenantId: {AbpSession.TenantId} AbpSession.UserId: {AbpSession.UserId} AbpSession.ImpersonatorTenantId: {AbpSession.ImpersonatorTenantId} AbpSession.ImpersonatorUserId: {AbpSession.ImpersonatorUserId}";
            //    Logger.Error(_msg);

            //    Logger.Error(ex.Message, ex);
            //}
            //if (!_access)
            //    throw new UnauthorizedAccessException("You are not authorized for this");
        }
    }
}