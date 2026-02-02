using Abp.Application.Services;
using Abp.Application.Services.Dto;
using inzibackend.Dto;
using inzibackend.Storage;
using inzibackend.Surpath.Dtos.LegalDocuments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inzibackend.Surpath
{
    /// <summary>
    /// Application service interface for managing legal documents
    /// </summary>
    public interface ILegalDocumentsAppService : IApplicationService
    {
        /// <summary>
        /// Get all legal documents with filtering and paging
        /// </summary>
        /// <param name="input">Filter and paging parameters</param>
        /// <returns>Paged list of legal documents</returns>
        Task<PagedResultDto<GetLegalDocumentForViewDto>> GetAll(GetAllLegalDocumentsInput input);

        /// <summary>
        /// Get a legal document for viewing
        /// </summary>
        /// <param name="input">Legal document ID</param>
        /// <returns>Legal document view DTO</returns>
        Task<GetLegalDocumentForViewDto> GetLegalDocumentForView(GetLegalDocumentForViewInput input);

        /// <summary>
        /// Get a legal document for editing
        /// </summary>
        /// <param name="input">Legal document ID</param>
        /// <returns>Legal document edit DTO</returns>
        Task<GetLegalDocumentForEditDto> GetLegalDocumentForEdit(GetLegalDocumentForEditInput input);

        /// <summary>
        /// Create or update a legal document
        /// </summary>
        /// <param name="input">Legal document data and file</param>
        /// <returns>Legal document ID</returns>
        Task<Guid> CreateOrEdit(CreateOrUpdateLegalDocumentDto input);

        /// <summary>
        /// Delete a legal document
        /// </summary>
        /// <param name="id">Legal document ID</param>
        /// <returns>Task</returns>
        Task Delete(Guid id);

        /// <summary>
        /// Get the latest legal document of a specific type
        /// </summary>
        /// <param name="type">Document type</param>
        /// <returns>Legal document view DTO</returns>
        Task<GetLegalDocumentForViewDto> GetLatestDocumentByType(DocumentType type);
        
        /// <summary>
        /// Retrieves a binary object from the temporary file cache using a token
        /// </summary>
        /// <param name="fileToken">The file token</param>
        /// <returns>The binary object</returns>
        Task<BinaryObject> GetBinaryObjectFromCache(string fileToken);
        
        /// <summary>
        /// Checks if the document is the last one of its type
        /// </summary>
        /// <param name="type">The document type</param>
        /// <returns>True if it's the last document of its type, false otherwise</returns>
        Task<bool> IsLastDocumentOfType(DocumentType type);

        /// <summary>
        /// Get cached legal document URLs for display in the UI
        /// </summary>
        /// <returns>Dictionary of document types and their URLs</returns>
        Task<Dictionary<DocumentType, string>> GetLegalDocumentUrls();
    }
}
