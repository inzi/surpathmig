using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class RegistrationDataModel
    {
        public string ClientCode { get; set; }

        public string EmailID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string TemporaryPassword { get; set; }

        public string ActivationLink { get; set; }

        public string ClientName { get; set; }

        public string DepartmentName { get; set; }

        public string Amount { get; set; }

        public PaymentMethod PaymentType { get; set; }

        public string TPAEmail { get; set; }

        public string ClientEmail { get; set; }

        public string DonorEmail { get; set; }

        public string DonorSSN { get; set; }

        public string DonorDOB { get; set; }

        public string DonorDOBDate { get; set; }

        public string DonorDOBMonth { get; set; }

        public string DonorDOBYear { get; set; }

        public string DonorCity { get; set; }

        public string DonorState { get; set; }

        public string DonorZipCode { get; set; }

        public string DonorClearStarProfileId { get; set; }

        public PaymentDataModel PaymentData { get; set; }

        public List<PIDTypeValue> PIDTypeValues { get; set; }

        public bool BackgroundCheckEnabled { get; set; } = false;
    }

}