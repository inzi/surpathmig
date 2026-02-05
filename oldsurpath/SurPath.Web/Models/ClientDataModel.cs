using GridMvc.DataAnnotations;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class ClientDataModel
    {
        #region Public Properties

        public string DonorId { get; set; }

        public string DonorTestInfoId { get; set; }

        [GridColumn(Title = "ClientID ", SortEnabled = true, FilterEnabled = true)]
        public string ClientID { get; set; }

        [GridColumn(Title = "ClientName ", SortEnabled = true, FilterEnabled = true)]
        public string ClientName { get; set; }

        [GridColumn(Title = "ClientDeparmentID", SortEnabled = true, FilterEnabled = true)]
        public string ClientDeparmentID { get; set; }

        [GridColumn(Title = "ClientDeparmentName", SortEnabled = true, FilterEnabled = true)]
        public string ClientDeparmentName { get; set; }

        [GridColumn(Title = "Specimen ID", SortEnabled = true, FilterEnabled = true)]
        public string SpecimenId { get; set; }

        [GridColumn(Title = "Test Reason", SortEnabled = true, FilterEnabled = true)]
        public string TestReason { get; set; }

        [GridColumn(Title = "Start date", SortEnabled = true, FilterEnabled = true)]
        public string StartDate { get; set; }

        [GridColumn(Title = "End Date", SortEnabled = true, FilterEnabled = true)]
        public string EndDate { get; set; }

        [GridColumn(Title = "DonorFirstName ", SortEnabled = true, FilterEnabled = true)]
        public string FirstName { get; set; }

        [GridColumn(Title = "DonorLastName", SortEnabled = true, FilterEnabled = true)]
        public string LastName { get; set; }

        [GridColumn(Title = "DonorSSN", SortEnabled = true, FilterEnabled = true)]
        public string SSN { get; set; }

        [GridColumn(Title = "DonorDOB", SortEnabled = true, FilterEnabled = true)]
        public string DOB { get; set; }

        [GridColumn(Title = "DonorTestStatus", SortEnabled = true, FilterEnabled = true)]
        public string TestStatus { get; set; }

        [GridColumn(Title = "DonorTestedDate", SortEnabled = true, FilterEnabled = true)]
        public string TestDate { get; set; }

        [GridColumn(Title = "DonorFinalResult", SortEnabled = true, FilterEnabled = true)]
        public string TestOverallResult { get; set; }

        [GridColumn(Title = "DonorTest Type", SortEnabled = true, FilterEnabled = true)]
        public TestCategories TestType { get; set; }

        [GridColumn(Title = "PreRegistration", SortEnabled = true, FilterEnabled = true)]
        public string PreRegistration { get; set; }

        [GridColumn(Title = "InQueue", SortEnabled = true, FilterEnabled = true)]
        public string InQueue { get; set; }

        [GridColumn(Title = "Processing", SortEnabled = true, FilterEnabled = true)]
        public string Processing { get; set; }

        [GridColumn(Title = "Complete", SortEnabled = true, FilterEnabled = true)]
        public string Complete { get; set; }

        [GridColumn(Title = "Activated", SortEnabled = true, FilterEnabled = true)]
        public string Activated { get; set; }

        [GridColumn(Title = "Registered", SortEnabled = true, FilterEnabled = true)]
        public string Registered { get; set; }

        [GridColumn(Title = "SuspensionQueue", SortEnabled = true, FilterEnabled = true)]
        public string SuspensionQueue { get; set; }
        
        #endregion
    }
}