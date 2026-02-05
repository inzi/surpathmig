using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace inzibackend.Net.Emailing;

public class inzibackendSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
{
    public inzibackendSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
    {

    }

    public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
}

