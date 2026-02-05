using GridMvc.DataAnnotations;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class ExportModel
    {
        
        //[GridColumn(Title = "DonorFirstName ", SortEnabled = true, FilterEnabled = true)]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Display(Name="Last Name")]
        [GridColumn(Title = "DonorLastName", SortEnabled = true, FilterEnabled = true)]
        public string Last_Name { get; set; }

        [Display(Name = "SSN")]
        [GridColumn(Title = "DonorSSN", SortEnabled = true, FilterEnabled = true)]
        public string SSN { get; set; }

        [Display(Name = "DOB")]
        [GridColumn(Title = "DonorDOB", SortEnabled = true, FilterEnabled = true)]
        public string DOB { get; set; }

        [Display(Name = "Test Status")]
        [GridColumn(Title = "DonorTestStatus", SortEnabled = true, FilterEnabled = true)]
        public string Test_Status { get; set; }

        [Display(Name = "Test Date")]
        [GridColumn(Title = "DonorTestedDate", SortEnabled = true, FilterEnabled = true)]
        public string Tested_Date { get; set; }
                
        [Display(Name = "Client Name")]
        [GridColumn(Title = "ClientName", SortEnabled = true, FilterEnabled = true)]
        public string Client_Name { get; set; }

        [Display(Name = "Department Name")]
        [GridColumn(Title = "DepartmentName", SortEnabled = true, FilterEnabled = true)]
        public string Program { get; set; }

        [Display(Name = "Specimen ID")]
        [GridColumn(Title = "Specimen ID", SortEnabled = true, FilterEnabled = true)]
        public string Specimen_ID { get; set; }
                
        [Display(Name = "Test Reason")]
        public string Test_Reason { get; set; }

        [Display(Name = "Test Type")]
        public string Test_Type { get; set; }

        [Display(Name = "Overall Result")]
        public string Final_Result{ get; set; }

    }
}