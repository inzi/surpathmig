using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace inzibackend.Surpath.ParserClasses
{
    public class CRLLabReportParser
    {
        public CRLLabReport ParseLabReport(string pdfContent)
        {
            var report = new CRLLabReport();

            // Parse lab information
            report.LabName = ExtractValue(pdfContent, @"(.*?)\nCLIA");
            report.CliaNumber = ExtractValue(pdfContent, @"CLIA #(\w+)");
            report.SamhsaNumber = ExtractValue(pdfContent, @"SAMHSA #(\w+)");
            report.CapNumber = ExtractValue(pdfContent, @"CAP #([\w-]+)");

            // Parse patient information
            report.SurScanName = ExtractValue(pdfContent, @"SUR-SCAN NAME:\s*(.+?)\s*SAMPLE ID:");
            report.SampleId = ExtractValue(pdfContent, @"SAMPLE ID:\s*(\w+)");
            report.PhysicianName = ExtractValue(pdfContent, @"(\w+\s+\w+)\s+MD");
            report.DateOfBirth = ExtractValue(pdfContent, @"DOB:\s*(.+?)\s*COLLECTED:");
            report.CollectedDate = ParseDate(ExtractValue(pdfContent, @"COLLECTED:\s*(.+?)\s*RECEIVED:"));
            report.ReceivedDate = ParseDate(ExtractValue(pdfContent, @"RECEIVED:\s*(.+?)\s*REPORTED:"));
            report.ReportedDate = ParseDate(ExtractValue(pdfContent, @"REPORTED:\s*(.+?)\s*(?:FAX:|$)"));
            report.PatientId = ExtractValue(pdfContent, @"(?<!SAMPLE\s)ID:\s*(\w+)");
            report.Gender = ExtractValue(pdfContent, @"GENDER:\s*(.+?)\s*SLIP ID:");
            report.SlipId = ExtractValue(pdfContent, @"SLIP ID:\s*(\w+)");

            // Parse physician information
            report.PhysicianAddress = ExtractValue(pdfContent, @"MD\s+(.*?)\s+(?<!SAMPLE\s)ID:");
            report.FaxNumber = ExtractValue(pdfContent, @"FAX:\s*(.+?)\s*PH:");
            report.PhoneNumber = ExtractValue(pdfContent, @"PH:\s*(.+?)\s*REF ID:");

            // Parse reference information
            report.ReferenceId = ExtractValue(pdfContent, @"REF ID:\s*(.+?)\s*PANEL ID:");
            report.PanelId = ExtractValue(pdfContent, @"PANEL ID:\s*(.+?)\s*COLL\. SITE ID:");
            report.CollectionSiteId = ExtractValue(pdfContent, @"COLL\. SITE ID:\s*(.+?)\s*REFERENCE 1:");
            report.Reference1 = ExtractValue(pdfContent, @"REFERENCE 1:\s*(.+?)\s*REFERENCE 2:");
            report.Reference2 = ExtractValue(pdfContent, @"REFERENCE 2:\s*(.+?)\s*SITE ADDR:");

            // Parse site information
            report.SiteAddress = ExtractValue(pdfContent, @"SITE ADDR:\s*(.+?)\s*SITE BRANCH:");
            report.SiteBranch = ExtractValue(pdfContent, @"SITE BRANCH:\s*(.+?)\s*SITE PHONE:");
            report.SitePhone = ExtractValue(pdfContent, @"SITE PHONE:\s*(.+?)\s*SITE FAX:");
            report.SiteFax = ExtractValue(pdfContent, @"SITE FAX:\s*(.+?)\s*REASON FOR TESTING:");

            // Parse test information
            report.ReasonForTesting = ExtractValue(pdfContent, @"REASON FOR TESTING:\s*(.+?)\s*SAMPLE TYPE:");
            report.SampleType = ExtractValue(pdfContent, @"SAMPLE TYPE:\s*(.+?)\s*URINALYSIS");

            // Parse urinalysis results
            var urinalysisMatch = Regex.Match(pdfContent, @"URINALYSIS.*?INITIAL TEST", RegexOptions.Singleline);
            if (urinalysisMatch.Success)
            {
                var urinalysisResults = ParseTestResults(urinalysisMatch.Value);
                report.UrinalysisResults.AddRange(urinalysisResults);
            }

            // Parse initial test results
            var initialTestMatch = Regex.Match(pdfContent, @"INITIAL TEST.*?LAB DIRECTOR:", RegexOptions.Singleline);
            if (initialTestMatch.Success)
            {
                var initialTestResults = ParseTestResults(initialTestMatch.Value);
                report.InitialTestResults.AddRange(initialTestResults);
            }

            // Parse lab director and certification
            report.LabDirector = ExtractValue(pdfContent, @"LAB DIRECTOR:\s*(.+?)\s*REPORT CERTIFIED BY");
            report.ReportCertifiedBy = ExtractValue(pdfContent, @"REPORT CERTIFIED BY\s*(.+?)\s*Page");

            return report;
        }

        private string ExtractValue(string content, string pattern)
        {
            var match = Regex.Match(content, pattern, RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        private DateTime ParseDate(string dateString)
        {
            return DateTime.TryParse(dateString, out var date) ? date : DateTime.MinValue;
        }

        private List<CRLTestResult> ParseTestResults(string content)
        {
            var results = new List<CRLTestResult>();
            var lines = content.Split('\n');

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"(.+?)\.+\s+(\w+)\s+(.+)");
                if (match.Success)
                {
                    results.Add(new CRLTestResult
                    {
                        TestName = match.Groups[1].Value.Trim(),
                        Result = match.Groups[2].Value.Trim(),
                        CutoffOrExpectedValues = match.Groups[3].Value.Trim()
                    });
                }
            }

            return results;
        }
    }
}
