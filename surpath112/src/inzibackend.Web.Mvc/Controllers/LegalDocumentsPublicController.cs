using Abp.Domain.Repositories;
using Abp.UI;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Surpath.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Web.Controllers
{
    public class LegalDocumentsPublicController : inzibackendControllerBase
    {
        private readonly IRepository<LegalDocument, Guid> _legalDocumentRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public LegalDocumentsPublicController(
            IRepository<LegalDocument, Guid> legalDocumentRepository,
            IBinaryObjectManager binaryObjectManager)
        {
            _legalDocumentRepository = legalDocumentRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<ActionResult> View(Guid id)
        {
            var legalDocument = await _legalDocumentRepository.GetAsync(id);

            if (legalDocument == null)
            {
                return NotFound();
            }

            // If external URL is provided, redirect to it
            if (!string.IsNullOrEmpty(legalDocument.ExternalUrl))
            {
                return Redirect(legalDocument.ExternalUrl);
            }

            // If no file is associated, return not found
            if (!legalDocument.FileId.HasValue)
            {
                return NotFound();
            }

            // Get the file content
            var fileObject = await _binaryObjectManager.GetOrNullAsync(legalDocument.FileId.Value);
            if (fileObject == null)
            {
                return NotFound();
            }
            
            // Validate file type
            if (!HtmlSanitizer.IsAllowedFileType(legalDocument.FileName))
            {
                throw new UserFriendlyException(L("InvalidFileType"));
            }

            // Set the title based on document type
            ViewBag.Title = legalDocument.Type == DocumentType.PrivacyPolicy ? L("PrivacyPolicy") : L("TermsOfService");

            // Add Content Security Policy headers
            Response.Headers.Add("Content-Security-Policy", 
                "default-src 'self'; " +
                "script-src 'none'; " +
                "style-src 'self'; " +
                "img-src 'self' https:; " +
                "font-src 'self'; " +
                "frame-src 'none'; " +
                "object-src 'none'; " +
                "connect-src 'none';");
            
            // Add X-Content-Type-Options header
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
            
            // Add X-Frame-Options header
            Response.Headers.Add("X-Frame-Options", "DENY");
            
            // Add X-XSS-Protection header
            Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            // If the file is HTML, display it in the view
            if (legalDocument.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
                legalDocument.FileName.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
            {
                string htmlContent = Encoding.UTF8.GetString(fileObject.Bytes);
                
                // Apply additional sanitization before displaying
                htmlContent = HtmlSanitizer.SanitizeHtml(htmlContent);
                
                // Verify that the HTML is safe
                if (!HtmlSanitizer.IsHtmlSafe(htmlContent))
                {
                    throw new UserFriendlyException(L("UnsafeHtmlContent"));
                }
                
                ViewBag.Content = htmlContent;
                return View();
            }

            // For other file types, redirect to the FileController
            string contentType = "application/octet-stream";
            if (legalDocument.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/pdf";
            }
            else if (legalDocument.FileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/css";
            }

            // Redirect to the FileController to handle the file viewing
            return RedirectToAction("ViewBinaryFile", "File", new
            {
                id = legalDocument.FileId.Value,
                contentType = contentType,
                fileName = legalDocument.FileName
            });
        }

        public async Task<ActionResult> Download(Guid id)
        {
            var legalDocument = await _legalDocumentRepository.GetAsync(id);

            if (legalDocument == null || !legalDocument.FileId.HasValue)
            {
                return NotFound();
            }

            var fileObject = await _binaryObjectManager.GetOrNullAsync(legalDocument.FileId.Value);
            if (fileObject == null)
            {
                return NotFound();
            }
            
            // Validate file type
            if (!HtmlSanitizer.IsAllowedFileType(legalDocument.FileName))
            {
                throw new UserFriendlyException(L("InvalidFileType"));
            }
            
            // Set appropriate content type based on file extension
            string contentType = "application/octet-stream";
            if (legalDocument.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase) || 
                legalDocument.FileName.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/html";
            }
            else if (legalDocument.FileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "text/css";
            }
            else if (legalDocument.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                contentType = "application/pdf";
            }

            // Redirect to the FileController to handle the file download
            return RedirectToAction("DownloadBinaryFile", "File", new
            {
                id = legalDocument.FileId.Value,
                contentType = contentType,
                fileName = legalDocument.FileName
            });
        }

        public async Task<ActionResult> GetByType(DocumentType type)
        {
            var legalDocument = await _legalDocumentRepository.FirstOrDefaultAsync(d =>
                d.Type == type &&
                d.TenantId == AbpSession.TenantId &&
                !d.IsDeleted);

            if (legalDocument == null)
            {
                return NotFound();
            }

            return RedirectToAction("View", new { id = legalDocument.Id });
        }
    }
}
