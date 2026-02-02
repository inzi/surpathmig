using SurPath.Enum;
using System;
using System.Collections.Generic;

namespace SurPath.Entity
{
    public class ReportInfo
    {
        public int ReportId { get; set; }
        public string SpecimenId { get; set; }
        public string LabSampleId { get; set; }
        public string SsnId { get; set; }
        public string PID { get; set; }
        public string PID2 { get; set; } = string.Empty;
        public int PIDType { get; set; }
        public int PIDType2 { get; set; } = 0;
        public string DonorLastName { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorMI { get; set; }
        public string DonorDOB { get; set; }
        public string DonorGender { get; set; }
        public string LabReport { get; set; }
        public byte[] LabReportByte { get; set; }
        public DateTime CreatedOn { get; set; }
        public OverAllTestResult ReportStatus { get; set; }
        public ReportType ReportType { get; set; }

        public int FinalReportId { get; set; }
        public string LabName { get; set; }
        public string LabCode { get; set; }
        public string LabReportDate { get; set; }
        public string PID_NODASHES_4 { get; set; } = String.Empty;
        public string PID_DASHES_19 { get; set; } = String.Empty;
        public string TestPanelCode { get; set; }
        public string TestPanelName { get; set; }
        public string TpaCode { get; set; }
        public string RegionCode { get; set; }
        public string ClientCode { get; set; }
        public bool FoundDeptByLabCode { get; set; } = false;
        public string LabAccountNumber { get; set; } = string.Empty;
        public string AccountInformation { get; set; } = string.Empty;
        public string AccountInformationAcct { get; set; } = string.Empty;
        public string lab_code { get; set; } = string.Empty;

        public string DepartmentCode { get; set; }
        public int DonorId { get; set; }
        public int ClientId { get; set; }
        public int ClientDepartmentId { get; set; }
        public string SpecimenCollectionDate { get; set; }
        public string CrlClientCode { get; set; }
        public string QuestCode { get; set; }

        public string lab_report_source_filename { get; set; }
        public string data_checksum { get; set; }

        public string ClientName { get; set; }
        public string ClientDepartmentName { get; set; }
        public string OBRSegment { get; set; } = string.Empty;
        public string ORCSegment { get; set; } = string.Empty;
    }

    public class OBX_Info
    {
        public int OBXInfoId { get; set; }
        public int Sequence { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public string UnitOfMeasure { get; set; }
        public string ReferenceRange { get; set; }
        public string OrderStatus { get; set; }

        public string ValueType { get; set; }
    }

    public class OBR_Info
    {
        public int OBRInfoId { get; set; }
        public int TransmitedOrder { get; set; }
        public string CollectionSiteInfo { get; set; }
        public string SpecimenCollectionDate { get; set; }
        public string SpecimenReceivedDate { get; set; }
        public string QuestSpeciminID { get; set; }
        public string CrlClientCode { get; set; }
        public string LabID { get; set; } = string.Empty;
        public string lab_code { get; set; } = string.Empty;
        public string SpecimenType { get; set; }
        public string SectionHeader { get; set; }
        public string CrlTransmitDate { get; set; }
        public string ServiceSectionId { get; set; }
        public string OrderStatus { get; set; }
        public string ReasonType { get; set; }

        public List<OBX_Info> observatinos = new List<OBX_Info>();

        public string CollectionSiteId { get; set; }
        public string SpecimenActionCode { get; set; }
        public string TpaCode { get; set; }
        public string RegionCode { get; set; }
        public string ClientCode { get; set; }
        public string DepartmentCode { get; set; }

        public string OBRQuestCode { get; set; }


    }

    public class ReturnValues
    {
        public bool MroAttentionFlag { get; set; }
        public bool IntegrationPartner { get; set; }
        public int DonorId { get; set; }
        public int ClientId { get; set; }
        public int ClientDepartmentId { get; set; }
        public int ClientDeptTestPanelId { get; set; }
        public int DonorTestInfoId { get; set; }
        public int ReportId { get; set; }
        public int MismatchRecordId { get; set; }
        public string LabClientCode { get; set; }
        public bool ErrorFlag { get; set; }
        public string ErrorMessage { get; set; }
        public ClientMROTypes MROType { get; set; }
        public int TestInfoRecordCount { get; set; }
        public int MismatchedCount { get; set; }
        public DonorRegistrationStatus TestStatus { get; set; } = DonorRegistrationStatus.None;
        public ReturnValues()
        {
            this.MroAttentionFlag = false;
            this.IntegrationPartner = false;
            this.DonorId = 0;
            this.ClientId = 0;
            this.ClientDepartmentId = 0;
            this.ClientDeptTestPanelId = 0;
            this.DonorTestInfoId = 0;
            this.ReportId = 0;
            this.MismatchRecordId = 0;
            this.LabClientCode = string.Empty;
            this.ErrorFlag = false;
            this.ErrorMessage = string.Empty;
            this.MROType = ClientMROTypes.None;
            this.TestInfoRecordCount = 0;
            this.MismatchedCount = 0;
        }
    }
}