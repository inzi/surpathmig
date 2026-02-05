using Mvc.Mailer;

namespace SurPathWeb.Mailers
{ 
    public interface IUserMailer
    {
        MvcMailMessage VerifyEmail();

        MvcMailMessage PreRegistrationMail();

        MvcMailMessage DonorProgramRegsitrationMail();

        MvcMailMessage ClientProgramRegsitrationMail();

        MvcMailMessage TPAProgramRegsitrationMail();

        MvcMailMessage ForgotPasswordMail();

        MvcMailMessage PaymentConformationMail();

        MvcMailMessage CardPaymentConformationMail();
	}
}