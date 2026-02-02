using inzibackend.Surpath;
using System.Collections.Generic;

namespace inzibackend.Web.Areas.App.Models.Layout
{
    public class LegalDocumentFooterViewModel
    {
        public Dictionary<DocumentType, string> LegalDocumentUrls { get; set; }
        public bool HasLegalDocuments { get; set; }
    }
} 