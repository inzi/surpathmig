using inzibackend.Surpath;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Models.LegalDocuments
{
    /// <summary>
    /// View model for the legal documents index page
    /// </summary>
    public class LegalDocumentsViewModel
    {
        /// <summary>
        /// Text filter for searching legal documents
        /// </summary>
        public string FilterText { get; set; }

        /// <summary>
        /// Filter by document type
        /// </summary>
        public DocumentType? TypeFilter { get; set; }
        
        /// <summary>
        /// List of document types for dropdown
        /// </summary>
        public List<DocumentTypeViewModel> DocumentTypes { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public LegalDocumentsViewModel()
        {
            DocumentTypes = new List<DocumentTypeViewModel>
            {
                new DocumentTypeViewModel { Type = DocumentType.PrivacyPolicy, Name = "PrivacyPolicy" },
                new DocumentTypeViewModel { Type = DocumentType.TermsOfService, Name = "TermsOfService" }
            };
        }
    }

    /// <summary>
    /// View model for document type dropdown
    /// </summary>
    public class DocumentTypeViewModel
    {
        /// <summary>
        /// Document type
        /// </summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }
    }
}
