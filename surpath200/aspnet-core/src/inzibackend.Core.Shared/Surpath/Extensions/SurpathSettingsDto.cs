using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath
{
    public class SurpathSettingsDto
    {
        //public static string SurpathRecordsRootFolder;
        public int MaxfiledataLength { get; set; } = 5242880;
        public string MaxfiledataLengthUserFriendlyValue { get; set; } = "5MB";
        public int MaxProfilefiledataLength { get; set; } = 5242880;
        public string MaxProfilefiledataLengthUserFriendlyValue { get; set; } = "5MB";
        public bool EnableInSessionPayment { get; set; } = false;
        public bool DummyDocuments { get; set; } = false;
        public string DummyDocumentFileName { get; set; } = "";

        public string AllowedFileExtensions { get; set; } = "jpeg, jpg, png, pdf, txt, hl7";
    }
}
