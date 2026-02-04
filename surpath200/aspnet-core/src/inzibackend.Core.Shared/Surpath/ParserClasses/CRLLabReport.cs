using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath.ParserClasses
{
    public class CRLLabReport
    {
        public string LabName { get; set; }
        public string CliaNumber { get; set; }
        public string SamhsaNumber { get; set; }
        public string CapNumber { get; set; }
        public string SurScanName { get; set; }
        public string SampleId { get; set; }
        public string PhysicianName { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime CollectedDate { get; set; }
        public string PhysicianAddress { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string PatientId { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Gender { get; set; }
        public string SlipId { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ReferenceId { get; set; }
        public string PanelId { get; set; }
        public string CollectionSiteId { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string SiteAddress { get; set; }
        public string SiteBranch { get; set; }
        public string SitePhone { get; set; }
        public string SiteFax { get; set; }
        public string ReasonForTesting { get; set; }
        public string SampleType { get; set; }
        public List<CRLTestResult> UrinalysisResults { get; set; }
        public List<CRLTestResult> InitialTestResults { get; set; }
        public string LabDirector { get; set; }
        public string ReportCertifiedBy { get; set; }

        public CRLLabReport()
        {
            UrinalysisResults = new List<CRLTestResult>();
            InitialTestResults = new List<CRLTestResult>();
        }
    }
}
