using GridMvc.DataAnnotations;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{

    public class TestResultDataModel
    {
        #region Public Properties

        public string DonorTestInfoId { get; set; }

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


        [GridColumn(Title = "DonorForm Type", SortEnabled = true, FilterEnabled = true)]
        public SpecimenFormType FormType { get; set; }

        [GridColumn(Title = "Donor Form Type", SortEnabled = true, FilterEnabled = true)]
        public string StrFormType { get; set; }

        [GridColumn(Title = "ClientDepartmentId", SortEnabled = true, FilterEnabled = true)]
        public string ClientDepartmentId { get; set; }

        [GridColumn(Title = "ClearStarCode", SortEnabled = true, FilterEnabled = true)]
        public string ClearStarCode { get; set; }

        [GridColumn(Title = "DepartmentName", SortEnabled = true, FilterEnabled = true)]
        public string DepartmentName { get; set; }

        [GridColumn(Title = "ClientId", SortEnabled = true, FilterEnabled = true)]
        public string ClientId { get; set; }

        [GridColumn(Title = "ClientName", SortEnabled = true, FilterEnabled = true)]
        public string ClientName { get; set; }

        [GridColumn(Title = "Specimen ID", SortEnabled = true, FilterEnabled = true)]
        public string SpecimenId { get; set; }

        [GridColumn(Title = "Test Reason", SortEnabled = true, FilterEnabled = true)]
        public string TestReason { get; set; }

        [GridColumn(Title = "Start date", SortEnabled = true, FilterEnabled = true)]
        public string StartDate { get; set; }

        [GridColumn(Title = "End Date", SortEnabled = true, FilterEnabled = true)]
        public string EndDate { get; set; }

        public string StrTestreason { get; set; }

        public string StrTestType { get; set; }

        public string SSNview { get; set; }

        public string StrOverallResult { get; set; }

        public bool IncludeArchived { get; set; }

        public string DonorId { get; set; }

        public string DonorClearStarProfId { get; set; }

        public string Exportformat { get; set; }

        public string DownloadLink { get; set; }

        public string MRODownloadLink { get; set; }

        public string RecordKeepingLink { get; set; }

        public string isHiddenWeb { get; set; }
        public bool show_web_notify_button { get; set; } = false;
        public string has_been_notified { get; set; } = String.Empty;
        public int backend_notifications_id { get; set; } = 0;
        public string Notified_by_email_timestamp { get; set; } = "";

        //public DonorSearchFilterList DonorSearchFilterListVal { get; set; } = DonorSearchFilterList.None;
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[DataType(DataType.Date)]
        //public DateTime beforedatetime { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[DataType(DataType.Date)]
        //public DateTime afterdatetime { get; set; }
        //public string beforedated { get; set; }
        //public string beforemonth { get; set; }
        //public string beforeyear { get; set; }
        //public string afterdated { get; set; }
        //public string aftermonth { get; set; }
        //public string afteryear { get; set; }

        #endregion
    }



}