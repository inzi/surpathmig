using Mvc.Mailer;
using System.Configuration;

namespace SurPathWeb.Mailers
{
    public class HL7Mailer : MailerBase, IHL7Mailer
    {
      

        #region Public Methods

        public virtual MvcMailMessage SendMisMatchedRecords()
        {
            string mmrSendingMailId = ConfigurationManager.AppSettings["MissMatchedReportsMail"].ToString().Trim();

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["MissMatchedReportSubject"].ToString().Trim();
                //x.ViewName = "PreRegistrationMail";

                x.To.Add(mmrSendingMailId);
            });
        }

      
        #endregion

      
    }
}