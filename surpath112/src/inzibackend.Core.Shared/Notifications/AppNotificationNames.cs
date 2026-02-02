namespace inzibackend.Notifications
{
    /// <summary>
    /// Constants for notification names used in this application.
    /// </summary>
    public static class AppNotificationNames
    {
        public const string SimpleMessage = "App.SimpleMessage";
        public const string WelcomeToTheApplication = "App.WelcomeToTheApplication";
        public const string NewUserRegistered = "App.NewUserRegistered";
        public const string NewTenantRegistered = "App.NewTenantRegistered";
        public const string GdprDataPrepared = "App.GdprDataPrepared";
        public const string TenantsMovedToEdition = "App.TenantsMovedToEdition"; 
        public const string DownloadInvalidImportUsers = "App.DownloadInvalidImportUsers";

        public const string RecordStateChanged = "App.RecordStateChanged";
        public const string RecordStateExpirationWarning = "App.RecordStateExpirationWarning";
        public const string RecordStateExpirationFirstWarning = "App.RecordStateExpirationFirstWarning";
        public const string RecordStateExpirationSecondWarning = "App.RecordStateExpirationSecondWarning";
        public const string RecordStateExpirationFinalWarning = "App.RecordStateExpirationFinalWarning";
        public const string RecordStateExpirationExpired = "App.RecordStateExpirationExpired";
        public const string NewRecordUploaded = "App.NewRecordUploaded";
        public const string TicketUpdated = "App.TicketUpdated";

        public const string YouHaveUnreadNotifications = "App.YouHaveUnreadNotifications";
    }
}
