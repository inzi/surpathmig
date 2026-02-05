using Mvc.Mailer;
using SurPathWeb.Models;
using System.Configuration;
using System.Web.Hosting;

namespace SurPathWeb.Mailers
{
    public class RegistrationMailer : MailerBase, IUserMailer
    {
        #region Public Properties

        public RegistrationDataModel _clientData { get; set; }

        #endregion

        #region Constructors
        bool Production = false;
        
        public RegistrationMailer(RegistrationDataModel clientData)
        {
            this._clientData = clientData;
            MasterName = "_Layout";
            var _production = ConfigurationManager.AppSettings["Production"].ToString().Trim();
            bool.TryParse(_production, out bool isProduction);
            this.Production = isProduction;
            if (!this.Production)
            {
                var toEmail = ConfigurationManager.AppSettings["TestEmailAddress"].ToString().Trim();
                _clientData.EmailID = toEmail;
                _clientData.DonorEmail = toEmail;
                _clientData.ClientEmail = toEmail;
                _clientData.TPAEmail = toEmail;
            }

        }

        #endregion

        #region Public Methods


        public virtual MvcMailMessage VerifyEmail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["DonorVerifyMailSubject"].ToString().Trim();
                x.ViewName = "VerifyMail";

                x.To.Add(_clientData.EmailID);
            });
        }

        public virtual MvcMailMessage PreRegistrationMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["DonorRegistrationMailSubject"].ToString().Trim();
                x.ViewName = "PreRegistrationMail";

                x.To.Add(_clientData.EmailID);
            });
        }

        public virtual MvcMailMessage DonorProgramRegsitrationMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim();
                x.ViewName = "DonorProgramRegsitrationMail";

                x.To.Add(_clientData.DonorEmail);
            });
        }

        public virtual MvcMailMessage ClientProgramRegsitrationMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim();
                x.ViewName = "ClientProgramRegsitrationMail";

                x.To.Add(_clientData.ClientEmail);
            });
        }

        public virtual MvcMailMessage TPAProgramRegsitrationMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["ProgramRegistraionMailSubject"].ToString().Trim();
                x.ViewName = "TPAProgramRegsitrationMail";

                x.To.Add(_clientData.TPAEmail);
            });
        }

        public virtual MvcMailMessage ForgotPasswordMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["ForgotPasswordMailSubject"].ToString().Trim();
                x.ViewName = "ForgotPasswordMail";

                x.To.Add(_clientData.EmailID);
            });
        }

        public virtual MvcMailMessage PaymentConformationMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["PaymentConformationMailSubject"].ToString().Trim();
                x.ViewName = "PaymentConformationMail";

                x.To.Add(_clientData.DonorEmail);
            });
        }

        public virtual MvcMailMessage CardPaymentConformationMail()
        {
            ViewData.Model = this._clientData;

            return Populate(x =>
            {
                x.Subject = ConfigurationManager.AppSettings["PaymentConformationMailSubject"].ToString().Trim();
                x.ViewName = "CardPaymentConformationMail";

                x.To.Add(_clientData.DonorEmail);
            });
        }

        #endregion

      
    }
}