using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath
{
    public static class SurpathSettings
    {
        public static string SettingsPathRecordsRootFolder { get; } = "Surpath:SurpathRecordsRootFolder";
        public static string SettingsPathSurpathMaxfiledataLength { get; } = "Surpath:MaxfiledataLength";
        public static string SettingsPathMaxProfilefiledataLength { get; } = "Surpath:MaxProfilefiledataLength";
        public static string SettingsPathMaxfiledataLengthUserFriendlyValue { get; } = "Surpath:MaxfiledataLengthUserFriendlyValue";
        public static string SettingsEnableInSessionPayment { get; } = "Surpath:EnableInSessionPayment";
        public static string SettingsPathMaxProfilefiledataLengthUserFriendlyValueValue { get; } = "Surpath:MaxProfilefiledataLengthUserFriendlyValue";
        public static string SettingsPathAllowedFileExtensions { get; } = "Surpath:AllowedFileExtensions";
        public static string SettingsDummyDocuments { get; } = "Surpath:DummyDocuments";
        public static string SettingsDummyDocumentFileName { get; } = "Surpath:DummyDocumentFileName";


        public static string SurpathRecordsRootFolder;
        public static int MaxfiledataLength { get; set; } = 5242880;
        public static string MaxfiledataLengthUserFriendlyValue { get; set; } = "5MB";
        public static int MaxProfilefiledataLength { get; set; } = 5242880;
        public static string MaxProfilefiledataLengthUserFriendlyValue { get; set; } = "5MB";
        public static bool EnableInSessionPayment { get; set; } = false;
        public static bool DummyDocuments { get; set; } = false;
        public static string DummyDocumentFileName { get; set; } = "";

        public static string AllowedFileExtensions { get; set; } = "jpeg, jpg, png, pdf, txt, hl7";
        public static string[] AllowedFileExtensionsArray
        {
            get
            {
                var _s = SurpathSettings.AllowedFileExtensions.Split(',').Select(ft => ft.Trim()).ToArray();
                return _s;
            }
        }

        public static SurpathSettingsDto GetDto()
        {
           return new SurpathSettingsDto
           {
               MaxfiledataLength = MaxfiledataLength,
               MaxfiledataLengthUserFriendlyValue = MaxfiledataLengthUserFriendlyValue,
               MaxProfilefiledataLength = MaxProfilefiledataLength,
               MaxProfilefiledataLengthUserFriendlyValue = MaxProfilefiledataLengthUserFriendlyValue,
               EnableInSessionPayment = EnableInSessionPayment,
               DummyDocuments = DummyDocuments,
               DummyDocumentFileName = DummyDocumentFileName,
               AllowedFileExtensions = AllowedFileExtensions
           };

        }
    }
}
