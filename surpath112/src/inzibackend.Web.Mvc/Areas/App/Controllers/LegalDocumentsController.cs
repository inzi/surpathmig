using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using inzibackend.Authorization;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos.LegalDocuments;
using inzibackend.Surpath.Helpers;
using inzibackend.Web.Areas.App.Models.LegalDocuments;
using inzibackend.Web.Controllers;
using inzibackend.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace inzibackend.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class LegalDocumentsController : inzibackendControllerBase
    {
        private readonly ILegalDocumentsAppService _legalDocumentsAppService;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public LegalDocumentsController(
            ILegalDocumentsAppService legalDocumentsAppService,
            IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager)
        {
            _legalDocumentsAppService = legalDocumentsAppService;
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments)]
        public async Task<ActionResult> Index()
        {
            var model = new LegalDocumentsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public async Task<ActionResult> CreateOrEdit(Guid? id)
        {
            GetLegalDocumentForEditDto editDto;

            if (id.HasValue && id.Value != Guid.Empty)
            {
                editDto = await _legalDocumentsAppService.GetLegalDocumentForEdit(
                    new GetLegalDocumentForEditInput { Id = id.Value });
            }
            else
            {
                editDto = new GetLegalDocumentForEditDto
                {
                    LegalDocument = new LegalDocumentDto()
                };
            }

            var viewModel = new CreateOrEditLegalDocumentViewModel
            {
                LegalDocument = editDto.LegalDocument
            };

            return View(viewModel);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrEditLegalDocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var input = new CreateOrUpdateLegalDocumentDto
                {
                    LegalDocument = model.LegalDocument,
                    FileToken = model.FileToken
                };

                await _legalDocumentsAppService.CreateOrEdit(input);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_View)]
        public async Task<ActionResult> View(DocumentType type)
        {
            var document = await _legalDocumentsAppService.GetLatestDocumentByType(type);

            if (document == null)
            {
                return NotFound();
            }

            // If external URL is provided, redirect to it
            if (!string.IsNullOrEmpty(document.LegalDocument.ExternalUrl))
            {
                return Redirect(document.LegalDocument.ExternalUrl);
            }

            if (!document.LegalDocument.FileId.HasValue)
            {
                return NotFound();
            }

            // Determine content type based on file extension
            string contentType = "application/octet-stream";
            string fileName = document.LegalDocument.FileName;
            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/pdf";
            }
            else if (fileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/html";
            }
            else if (fileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/css";
            }
            else if (fileName.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/msword";
            }

            // Redirect to the FileController to handle the file viewing
            return RedirectToAction("ViewBinaryFile", "File", new
            {
                id = document.LegalDocument.FileId.Value,
                contentType,
                fileName = document.LegalDocument.FileName
            });
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _legalDocumentsAppService.Delete(id);
            return RedirectToAction("Index");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_View)]
        public async Task<ActionResult> Download(Guid id)
        {
            var document = await _legalDocumentsAppService.GetLegalDocumentForView(
                new GetLegalDocumentForViewInput { Id = id });

            if (document == null || !document.LegalDocument.FileId.HasValue)
            {
                return NotFound();
            }

            // Determine content type based on file extension
            string contentType = "application/octet-stream";
            if (document.LegalDocument.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/pdf";
            }
            else if (document.LegalDocument.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
                     document.LegalDocument.FileName.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/html";
            }
            else if (document.LegalDocument.FileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/css";
            }

            // Redirect to the FileController to handle the file download
            return RedirectToAction("DownloadBinaryFile", "File", new
            {
                id = document.LegalDocument.FileId.Value,
                contentType,
                fileName = document.LegalDocument.FileName
            });
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_View)]
        public async Task<JsonResult> IsLastDocumentOfType(DocumentType type)
        {
            var result = await _legalDocumentsAppService.IsLastDocumentOfType(type);
            return Json(result);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_View)]
        public async Task<JsonResult> PreviewHtml(string fileToken)
        {
            if (string.IsNullOrEmpty(fileToken))
            {
                return Json(new { success = false, error = L("InvalidFileToken") });
            }

            try
            {
                var fileInfo = _tempFileCacheManager.GetFileInfo(fileToken);
                if (fileInfo == null)
                {
                    return Json(new { success = false, error = L("FileNotFound") });
                }

                // Only allow HTML files to be previewed
                if (!fileInfo.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = false, error = L("OnlyHtmlFilesCanBePreviewed") });
                }

                // Get the HTML content
                string htmlContent = System.Text.Encoding.UTF8.GetString(fileInfo.File);

                // Sanitize the HTML content
                string sanitizedHtml = HtmlSanitizer.SanitizeHtml(htmlContent);

                // Check if the HTML is safe after sanitization
                if (!HtmlSanitizer.IsHtmlSafe(sanitizedHtml))
                {
                    return Json(new { success = false, error = L("UnsafeHtmlContent") });
                }

                // Create response with additional metadata
                return Json(new
                {
                    success = true,
                    html = sanitizedHtml,
                    fileName = fileInfo.FileName,
                    fileSize = fileInfo.File.Length,
                    originalSize = htmlContent.Length,
                    sanitizedSize = sanitizedHtml.Length,
                    isSanitized = htmlContent != sanitizedHtml,
                    lastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                Logger.Error("Error previewing HTML file", ex);
                return Json(new { success = false, error = L("ErrorPreviewingHtml") });
            }
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_LegalDocuments_CreateEdit)]
        public FileUploadCacheOutput UploadLegalDocumentFile()
        {
            Logger.Debug("UploadLegalDocumentFile called");
            string[] legalDocAllowedFileTypes = { "pdf", "html", "css" };
            try
            {
                // Check input
                if (Request.Form.Files.Count == 0)
                {
                    Logger.Warn("No file uploaded, throwing");
                    throw new UserFriendlyException(L("NoFileFoundError"));
                }

                var file = Request.Form.Files.First();
                if (file.Length > SurpathSettings.MaxfiledataLength)
                {
                    Logger.Warn("File size exceeded, throwing");
                    throw new UserFriendlyException(L("Warn_File_SizeLimit") + $" The maxiumum size is {SurpathSettings.MaxfiledataLengthUserFriendlyValue}");
                }

                var fileExtension = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(fileExtension) || fileExtension.Length <= 1)
                {
                    Logger.Warn("File extension is empty, throwing");
                    throw new UserFriendlyException(L("InvalidFileExtension"));
                }

                var fileType = fileExtension.Substring(1);
                if (legalDocAllowedFileTypes != null && legalDocAllowedFileTypes.Length > 0 && !legalDocAllowedFileTypes.Contains(fileType))
                {
                    Logger.Warn("File type not allowed, throwing");
                    throw new UserFriendlyException(L("FileNotInAllowedFileTypes") + $" Allowed file types are {string.Join(", ", legalDocAllowedFileTypes)}");
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

        [HttpGet]
        public PartialViewResult PreviewModal(string fileToken)
        {
            var model = new PreviewModalViewModel { FileToken = fileToken };
            return PartialView("_PreviewModal", model);
        }
    }
}