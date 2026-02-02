using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    [Serializable]
    public class DonorNotifyJSONModel
    {
        public string donor_test_info_id { get; set; } = "";
        public bool NotifyNow { get; set; } = false;
        public bool Queued { get; set; } = false;
        public string Msg { get; set; } = "";
    }

    [Serializable]
    public class AcccountExistsJSONModel
    {
        public string userName { get; set; } = "";
        public string verificationCode { get; set; } = "";
        public string programId { get; set; } = "";
        public string department_name { get; set; } = "";
    }

    [Serializable]
    public class LoginRegistrantJSONModel
    {
        public string userName { get; set; } = "";
        public string verificationCode { get; set; } = "";
        public string programId { get; set; } = "";
        public string password { get; set; } = "";
        public string department_name { get; set; } = "";
    }

    [Serializable]
    public class DonorProfileDataModel
    {
        #region Public Properties

        public DonorProfileDataModel()
        {
            PIDTypeValues = new List<PIDTypeValue>();

            foreach (PidTypes p in Enum.GetValues(typeof(PidTypes)))
            {
                PIDTypeValues.Add(new PIDTypeValue() { PIDType = (int)p, PIDValue = string.Empty });

            }
        }


        public string DonorId
        {
            get;
            set;
        }

        public string EmailID { get; set; }

        public string SSN { get; set; }

        public string FirstName { get; set; }

        public string CourtUsername { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        public string DonorDOBDate { get; set; }

        public string DonorDOBMonth { get; set; }

        public string DonorDOBYear { get; set; }

        public Gender Gender { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Fax { get; set; }

        public string CourtName { get; set; }

        public string Prefix { get; set; }

        public string Username { get; set; }

        public string DonorTestInfoId { get; set; }

        public string DocumentID { get; set; }

        public string DocumentTitle { get; set; }

        public byte[] documentContent { get; set; }

        public string filename { get; set; }

        public string uploaddate { get; set; }

        public TimeSpan uploadtime { get; set; }

        public string DonorClearStarProfId { get; set; }

        public bool NeedsApproval { get; set; }

        public bool Rejected { get; set; }

        public bool Approved { get; set; }
        public List<PIDTypeValue> PIDTypeValues { get; set; }
        public DonorNotifyJSONModel NotifyModel {get; set;}
        public List<string> PidErrorMesages { get; set; } = new List<string>();
        #endregion
    }
}