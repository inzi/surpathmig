using System;

namespace inzibackend.Surpath.Dtos.LegalDocuments
{
    /// <summary>
    /// Input DTO for getting a legal document for viewing
    /// </summary>
    public class GetLegalDocumentForViewInput
    {
        /// <summary>
        /// ID of the legal document to view
        /// </summary>
        public Guid Id { get; set; }
    }
}
