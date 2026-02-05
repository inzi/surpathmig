namespace SurPath.Enum
{
    public enum NotificationExceptions
    {
        None = 0,
        InvalidEmail = 1,
        SMSBlocked = 2,
        NoClinics = 3,
        InvalidPhone = 4,
    }

    public enum NotificationClinicExceptions
    {
        None = 0,
        NotInRange = 1,
        FormFoxClinicNotFound = 52,
        FormFoxOrderRejected = 53,
        FormFoxInvalidAuthForm = 54,
        FormFoxMarketplaceRejected = 56,
        FormFoxClientContactMissing = 57,
        FormFoxUnknownIssue2 = 58,
        FormFoxOrderGeneralFailure = 59
    }

}