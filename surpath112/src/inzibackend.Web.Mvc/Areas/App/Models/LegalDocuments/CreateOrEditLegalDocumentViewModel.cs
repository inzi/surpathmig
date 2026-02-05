using inzibackend.Surpath.Dtos.LegalDocuments;
using System.ComponentModel.DataAnnotations;

namespace inzibackend.Web.Areas.App.Models.LegalDocuments
{
    /// <summary>
    /// View model for creating or editing a legal document
    /// </summary>
    public class CreateOrEditLegalDocumentViewModel
    {
        /// <summary>
        /// Legal document information
        /// </summary>
        [Required]
        public LegalDocumentDto LegalDocument { get; set; }

        /// <summary>
        /// File token from temporary cache (if any)
        /// </summary>
        public string FileToken { get; set; }
    }
}
