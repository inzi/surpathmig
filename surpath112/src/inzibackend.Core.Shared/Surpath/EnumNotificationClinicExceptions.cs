using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath
{
    public enum EnumNotificationClinicExceptions
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
