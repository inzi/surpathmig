using Abp.Application.Services.Dto;
using System;

namespace inzibackend.Surpath.Dtos.LegalDocuments
{
    /// <summary>
    /// Base DTO for legal documents
    /// </summary>
    public class LegalDocumentDto : EntityDto<Guid>
    {
        /// <summary>
        /// Type of legal document (Privacy Policy or Terms of Service)
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// External URL if the document is hosted externally
        /// </summary>
        public string ExternalUrl { get; set; }

        /// <summary>
        /// Name of the uploaded file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// URL for viewing the document via the LegalDocuments controller
        /// </summary>
        public string ViewUrl { get; set; }

        /// <summary>
        /// ID of the file in binary storage
        /// </summary>
        public Guid? FileId { get; set; }
    }
}
