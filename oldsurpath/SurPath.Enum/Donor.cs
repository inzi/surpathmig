using System.ComponentModel;

namespace SurPath.Enum
{
    public enum DonorRegistrationStatus
    {
        [Description("None")]
        None = 0,

        [Description("User Registration")]
        PreRegistration = 1,

        [Description("User Activated")]
        Activated = 2,

        [Description("Pre-Registered")]
        Registered = 3,

        [Description("In-Queue")]
        InQueue = 4,

        [Description("Suspension Queue")]
        SuspensionQueue = 5,

        [Description("Processing")]
        Processing = 6,

        [Description("Completed")]
        Completed = 7,

        //[Description("Notified")]
        //Notified = 8,
    }

    public enum TestInfoReasonForTest
    {
        [Description("None")]
        None = 0,

        [Description("Pre-Employment")]
        PreEmployment = 1,

        [Description("Random")]
        Random = 2,

        [Description("Reasonable Suspicion/Cause")]
        ReasonableSuspicionCause = 3,

        [Description("Post-Accident")]
        PostAccident = 4,

        [Description("Return to Duty")]
        ReturnToDuty = 5,

        [Description("Follow-Up")]
        FollowUp = 6,

        [Description("Other")]
        Other = 7
    }

    public enum SpecimenCollectionCupType
    {
        [Description("None")]
        None = 0,

        [Description("Urine Single")]
        UrineSingle = 1,

        [Description("Urine Split")]
        UrineSplit = 2,

        [Description("Saliva")]
        Saliva = 3,

        [Description("Blood")]
        Blood = 4
    }

    public enum SpecimenFormType
    {
        None = 0,

        [Description("Federal")]
        Federal = 1,

        [Description("Non Federal")]
        NonFederal = 2
    }

    public enum ProgramType
    {
        None = 0,
        OneTimeTesting = 1,
        Random = 2
    }

    public enum DonorActivityCategories
    {
        [Description("None")]
        None = 0,

        [Description("General")]
        General = 1,

        [Description("Notification")]
        Notification = 2,

        [Description("Information")]
        Information = 3,

        [Description("DonorLink")]
        DonorLink = 4
    }

    public enum InstantTestResult
    {
        None = 0,
        Positive = 1,
        Negative = 2
    }

    public enum ReportType
    {
        [Description("None")]
        None = 0,
        [Description("LabReport")]
        LabReport = 1,
        [Description("MROReport")]
        MROReport = 2,
        [Description("QuestLabReport")]
        QuestLabReport = 3,
        [Description("ChainOfCustodyReport")]
        ChainOfCustodyReport =4
    }

    public enum OverAllTestResult
    {
        [Description("None")]
        None = 0,

        [Description("POSITIVE")]
        Positive = 1,

        [Description("NEGATIVE")]
        Negative = 2,

        [Description("DISCARD")]
        Discard = 4,

        [Description("OTHER")]
        Other = 4
    }

    public enum DonorSearchFilterList
    {
        [Description("None")]
        None,
        [Description("Testing Date")]
        Testing,
        [Description("Registration Date")]
        Registration,

    }
}