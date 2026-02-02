using System.ComponentModel;

namespace SurPath.Enum
{
    public enum AuthorizationCategories
    {
        [Description("None")]
        None,

        [Description("SS_DASHBOARD")]
        SS_DASHBOARD,

        [Description("DONOR_TAB")]
        DONOR_TAB,

        [Description("CLIENT_SETUP")]
        CLIENT_SETUP,

        [Description("VENDOR_SETUP")]
        VENDOR_SETUP,

        [Description("USER_SETUP")]
        USER_SETUP,

        [Description("GENERAL_SETUP")]
        GENERAL_SETUP,

        [Description("DRUG_NAMES_SETUP")]
        DRUG_NAMES_SETUP,

        [Description("TEST_PANEL_SETUP")]
        TEST_PANEL_SETUP,

        [Description("ATTORNEY_INFO_SETUP")]
        ATTORNEY_INFO_SETUP,

        [Description("COURT_INFO_SETUP")]
        COURT_INFO_SETUP,

        [Description("JUDGE_INFO_SETUP")]
        JUDGE_INFO_SETUP,

        [Description("DEPARTMENT_INFO_SETUP")]
        DEPARTMENT_INFO_SETUP,

        [Description("TESTING_AUTHORITY_SETUP")]
        TESTING_AUTHORITY_SETUP,

        [Description("CAN_VIEW_ONLY")]
        CAN_VIEW_ONLY,

        [Description("ACCOUNTING_TAB")]
        ACCOUNTING_TAB,

        [Description("WEB")]
        WEB
    }

    public enum AuthorizationRules
    {
        [Description("None")]
        None,

        //Setup - Drug Names
        [Description("DRUG_NAMES_ADD")]
        DRUG_NAMES_ADD,

        [Description("DRUG_NAMES_EDIT")]
        DRUG_NAMES_EDIT,

        [Description("DRUG_NAMES_ARCHIVE")]
        DRUG_NAMES_ARCHIVE,

        //Setup - Test Panel
        [Description("TEST_PANEL_ADD")]
        TEST_PANEL_ADD,

        [Description("TEST_PANEL_EDIT")]
        TEST_PANEL_EDIT,

        [Description("TEST_PANEL_ARCHIVE")]
        TEST_PANEL_ARCHIVE,

        [Description("TEST_PANEL_DRUG_NAMES_ADD")]
        TEST_PANEL_DRUG_NAMES_ADD,

        //Setup - Attorney
        [Description("ATTORNEY_ADD")]
        ATTORNEY_ADD,

        [Description("ATTORNEY_EDIT")]
        ATTORNEY_EDIT,

        [Description("ATTORNEY_ARCHIVE")]
        ATTORNEY_ARCHIVE,

        //Setup - Court
        [Description("COURT_ADD")]
        COURT_ADD,

        [Description("COURT_EDIT")]
        COURT_EDIT,

        [Description("COURT_ARCHIVE")]
        COURT_ARCHIVE,

        //Setup - Judge
        [Description("JUDGE_ADD")]
        JUDGE_ADD,

        [Description("JUDGE_EDIT")]
        JUDGE_EDIT,

        [Description("JUDGE_ARCHIVE")]
        JUDGE_ARCHIVE,

        //Setup - Department
        [Description("DEPARTMENT_ADD")]
        DEPARTMENT_ADD,

        [Description("DEPARTMENT_EDIT")]
        DEPARTMENT_EDIT,

        [Description("DEPARTMENT_ARCHIVE")]
        DEPARTMENT_ARCHIVE,

        //Testing Authority
        [Description("TESTING_AUTHORITY_ADD")]
        TESTING_AUTHORITY_ADD,

        [Description("TESTING_AUTHORITY_EDIT")]
        TESTING_AUTHORITY_EDIT,

        [Description("TESTING_AUTHORITY_ARCHIVE")]
        TESTING_AUTHORITY_ARCHIVE,

        //Client - Client Info
        [Description("CLIENT_ADD")]
        CLIENT_ADD,

        [Description("CLIENT_EDIT")]
        CLIENT_EDIT,

        [Description("CLIENT_ARCHIVE")]
        CLIENT_ARCHIVE,

        [Description("CLIENT_DEPARTMENT_ADD")]
        CLIENT_DEPARTMENT_ADD,

        [Description("CLIENT_DEPARTMENT_EDIT")]
        CLIENT_DEPARTMENT_EDIT,

        [Description("CLIENT_DEPARTMENT_ARCHIVE")]
        CLIENT_DEPARTMENT_ARCHIVE,

        [Description("CLIENT_TEST_PANEL_ADD")]
        CLIENT_TEST_PANEL_ADD,

        //Vendor - Vendor Info
        [Description("VENDOR_ADD")]
        VENDOR_ADD,

        [Description("VENDOR_EDIT")]
        VENDOR_EDIT,

        [Description("VENDOR_ARCHIVE")]
        VENDOR_ARCHIVE,

        [Description("VENDOR_VENDOR_SERVICE_ADD")]
        VENDOR_VENDOR_SERVICE_ADD,

        [Description("VENDOR_VENDOR_SERVICE_EDIT")]
        VENDOR_VENDOR_SERVICE_EDIT,

        [Description("VENDOR_VENDOR_SERVICE_ARCHIVE")]
        VENDOR_VENDOR_SERVICE_ARCHIVE,

        //User - User Info
        [Description("USER_ADD")]
        USER_ADD,

        [Description("USER_EDIT")]
        USER_EDIT,

        [Description("USER_ARCHIVE")]
        USER_ARCHIVE,

        [Description("USER_VIEW_PASSWORD")]
        USER_VIEW_PASSWORD,

        //Donor
        [Description("DONOR_ADD")]
        DONOR_ADD,

        [Description("DONOR_SEARCH_VIEW")]
        DONOR_SEARCH_VIEW,

        [Description("DONOR_VIEW_DONOR_INFO_TAB")]
        DONOR_VIEW_DONOR_INFO_TAB,

        [Description("DONOR_EDIT")]
        DONOR_EDIT,

        [Description("DONOR_SSN_UNMASK")]
        DONOR_SSN_UNMASK,

        [Description("DONOR_ACTIVATION_MAIL_RESEND")]
        DONOR_ACTIVATION_MAIL_RESEND,

        [Description("DONOR_VIEW_TEST_INFO_TAB")]
        DONOR_VIEW_TEST_INFO_TAB,

        [Description("DONOR_EDIT_TEST_INFO_TAB")]
        DONOR_EDIT_TEST_INFO_TAB,

        [Description("DONOR_EDIT_SPECIMEN_ID")]
        DONOR_EDIT_SPECIMEN_ID,

        [Description("DONOR_VIEW_RESULTS_TAB")]
        DONOR_VIEW_RESULTS_TAB,

        [Description("DONOR_VIEW_ACTIVITY_NOTES_TAB")]
        DONOR_VIEW_ACTIVITY_NOTES_TAB,

        [Description("DONOR_EDIT_ACTIVITY_NOTES_TAB")]
        DONOR_EDIT_ACTIVITY_NOTES_TAB,

        [Description("DONOR_VIEW_LEGAL_INFO_TAB")]
        DONOR_VIEW_LEGAL_INFO_TAB,

        [Description("DONOR_EDIT_LEGAL_INFO_TAB")]
        DONOR_EDIT_LEGAL_INFO_TAB,

        [Description("DONOR_VIEW_VENDOR_TAB")]
        DONOR_VIEW_VENDOR_TAB,

        [Description("DONOR_EDIT_VENDOR_TAB")]
        DONOR_EDIT_VENDOR_TAB,

        [Description("DONOR_VIEW_DOCUMENT_TAB")]
        DONOR_VIEW_DOCUMENT_TAB,

        [Description("DONOR_EDIT_DOCUMENT_TAB")]
        DONOR_EDIT_DOCUMENT_TAB,

        [Description("DONOR_VIEW_PAYMENT_TAB")]
        DONOR_VIEW_PAYMENT_TAB,

        [Description("DONOR_COLLECT_PAYMENT")]
        DONOR_COLLECT_PAYMENT,

        [Description("DONOR_VIEW_ACCOUNTING_TAB")]
        DONOR_VIEW_ACCOUNTING_TAB,

        [Description("DONOR_VIEW_TEST_HISTORY_TAB")]
        DONOR_VIEW_TEST_HISTORY_TAB,

        //Dashboard
        [Description("DASHBOARD_VIEW_TEST_PERFORMED_TAB")]
        DASHBOARD_VIEW_TEST_PERFORMED_TAB,

        [Description("DASHBOARD_VIEW_ISSUES_TAB")]
        DASHBOARD_VIEW_ISSUES_TAB,

        [Description("DASHBOARD_VIEW_ACCOUNTING_TAB")]
        DASHBOARD_VIEW_ACCOUNTING_TAB,

        [Description("DASHBOARD_VIEW_COMMISSIONS_TAB")]
        DASHBOARD_VIEW_COMMISSIONS_TAB,

        //CAN VIEW ONLY
        [Description("DRUG_NAMES_VIEW")]
        DRUG_NAMES_VIEW,

        [Description("TEST_PANEL_VIEW")]
        TEST_PANEL_VIEW,

        [Description("ATTORNEY_VIEW")]
        ATTORNEY_VIEW,

        [Description("COURT_VIEW")]
        COURT_VIEW,

        [Description("JUDGE_VIEW")]
        JUDGE_VIEW,

        [Description("DEPARTMENT_VIEW")]
        DEPARTMENT_VIEW,

        [Description("TESTING_AUTHORITY_VIEW")]
        TESTING_AUTHORITY_VIEW,

        [Description("CLIENT_VIEW")]
        CLIENT_VIEW,

        [Description("CLIENT_DEPARTMENT_VIEW")]
        CLIENT_DEPARTMENT_VIEW,

        [Description("VENDOR_VIEW")]
        VENDOR_VIEW,

        [Description("VENDOR_VENDOR_SERVICE_VIEW")]
        VENDOR_VENDOR_SERVICE_VIEW,

        [Description("USER_VIEW")]
        USER_VIEW,

        [Description("WEB_CAN_SEND_IN")]
        WEB_CAN_SEND_IN
    }

    public enum UserType
    {
        None = 0,
        TPA = 1,
        Donor = 2,
        Client = 3,
        Vendor = 4,
        Attorney = 5,
        Court = 6,
        Judge = 7
    }

    public enum UserActivityCategories
    {
        [Description("None")]
        None = 0,

        [Description("General")]
        General = 1,

        [Description("Login")]
        Login = 2,

        [Description("Security")]
        Security = 2
    }

}