using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos.LegalDocuments
{
    /// <summary>
    /// Input DTO for getting all legal documents with paging and filtering
    /// </summary>
    public class GetAllLegalDocumentsInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for searching legal documents
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter by document type
        /// </summary>
        public DocumentType? TypeFilter { get; set; }
    }
}
