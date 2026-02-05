using Mvc.Mailer;

namespace SurPathWeb.Mailers
{
    public interface IHL7Mailer
    {
        MvcMailMessage SendMisMatchedRecords();
	}
}