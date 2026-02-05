using System;

namespace inzibackend.Surpath.Dtos.LegalDocuments
{
    /// <summary>
    /// Input DTO for getting a legal document for editing
    /// </summary>
    public class GetLegalDocumentForEditInput
    {
        /// <summary>
        /// ID of the legal document to edit
        /// </summary>
        public Guid Id { get; set; }
    }
}
