namespace inzibackend.Surpath.Dtos.LegalDocuments
{
    /// <summary>
    /// DTO for creating or updating a legal document
    /// </summary>
    public class CreateOrUpdateLegalDocumentDto
    {
        /// <summary>
        /// Legal document information
        /// </summary>
        public LegalDocumentDto LegalDocument { get; set; }
        
        /// <summary>
        /// File token from temporary cache (if any)
        /// </summary>
        public string FileToken { get; set; }
    }
}
