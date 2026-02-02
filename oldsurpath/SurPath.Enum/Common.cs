using System.ComponentModel;

namespace SurPath.Enum
{
    public enum OperationMode
    {
        None = 0,
        New = 1,
        Edit = 2,
        View = 3
    }

    public enum TestCategories
    {
        [Description("None")]
        None = 0,

        [Description("UA")]
        UA = 1,

        [Description("Hair")]
        Hair = 2,

        [Description("DNA")]
        DNA = 3,

        [Description("BC")]
        BC = 4,

        [Description("RecordKeeping")]
        RC = 5,

        [Description("FingerPrint")]
        FP = 6,


    }

    public enum AddressTypes
    {
        None = 0,
        PhysicalAddress1 = 1,
        PhysicalAddress2 = 2,
        PhysicalAddress3 = 3,
        MailingAddress = 4
    }

    public enum Gender
    {
        None = 0,
        Male = 1,
        Female = 2
    }

    public enum PaymentMethod
    {
        None = 0,
        Cash = 1,
        Card = 2
    }

    public enum PaymentStatus
    {
        None = 0,
        Paid = 1,
        PartiallyPaid = 2,
        Canceled = 3,
        Pending = 4
    }

    public enum YesNo
    {
        None = 0,
        Yes = 1,
        No = 2
    }

    public enum DayOfWeekEnum
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public enum PidTypes
    {
        [Description("Invalid")]
        Invalid = 0,

        [Description("SSN")]
        SSN = 1,

        [Description("DL")]
        DL = 2,

        [Description("Passport")]
        Passport = 3,

        [Description("Other")]
        Other = 4,

        [Description("SampleID")]
        SampleID = 5,

        [Description("TaxPayer ID")]
        TaxPayerID = 6,

        [Description("Quest Donor ID")]
        QuestDonorId = 7,

        [Description("Donor ID")]
        DonorID = 7,
        [Description("Student ID")]
        StudentID = 8,
        [Description("MCE")]
        MCEID = 9,
        [Description("PC")]
        PCID = 10,

        //[Description("No Allowed")]
        //NotAllowed = 999,
    }

}