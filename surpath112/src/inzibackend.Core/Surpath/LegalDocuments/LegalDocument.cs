using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace inzibackend.Surpath
{
    /// <summary>
    /// Represents a legal document such as Privacy Policy or Terms of Service
    /// </summary>
    public class LegalDocument : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// The tenant ID that owns this document
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// ID of the file in binary storage
        /// </summary>
        public Guid? FileId { get; set; }

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
        /// Constructor
        /// </summary>
        public LegalDocument()
        {
            // Generate a new GUID for the entity
            Id = Guid.NewGuid();
        }
    }
}