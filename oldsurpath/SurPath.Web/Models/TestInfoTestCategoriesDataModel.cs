using GridMvc.DataAnnotations;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class TestInfoTestCategoriesDataModel
    {
        #region Public Properties

        public int DonorTestTestCategoryId { get; set; }

        public int DonorTestInfoId { get; set; }

        public TestCategories TestCategoryId { get; set; }

        public string TestPanelName { get; set; }      
        
        public int? TestPanelId { get; set; }

        public string SpecimenId { get; set; }

        public int? HairTestPanelDays { get; set; }

        public int TestPanelResult { get; set; }

        public DonorRegistrationStatus TestPanelStatus { get; set; }

        public double? TestPanelCost { get; set; }

        public double? TestPanelPrice { get; set; }

        public bool IsSynchronized { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public string LastModifiedBy { get; set; }

        #endregion
    }
}