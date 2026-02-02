using Microsoft.Extensions.Configuration;
using Serilog;
using SurPath.Data;
using SurPath.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SurpathBackend
{
    public class RuntimeSettings
    {
        public string DefaultPickupFolder { get; set; }
        public string DefaultDropFolder { get; set; }
        public int RunIntervalInSeconds { get; set; }
    }

    internal class SurpathBackendWorkerWrapper
    {
        private BackendData backendData;
        private BackendLogic backendLogic;
        private BackendServiceWorker worker;
        private List<Notification> NoSendInFailureNotifications = new List<Notification>();

        protected readonly ILogger _logger;

        private readonly ManualResetEvent _doneEvent;
        private IConfigurationRoot ConfigurationRoot;

        public string DefaultPickupFolder = ""; // From Settings
        public string DefaultDropFolder = ""; // From Settings

        public bool Run { get; set; }
        public bool Stop { get; set; }

        public SurpathBackendWorkerWrapper(ILogger logger, IConfigurationRoot configuration)
        {
            ConfigurationRoot = configuration;
            _logger = logger;
            //backendData = new BackendData(null, null, _logger);
            backendLogic = new BackendLogic("SERVICE", _logger);
            backendData = backendLogic.backendData;
            // get our default folders
            RuntimeSettings runtimeSettings = new RuntimeSettings();
            runtimeSettings.RunIntervalInSeconds = 5;

            //IConfigurationSection sectionData = ConfigurationRoot.GetSection("RuntimeSettings");
            //sectionData.Bind(runtimeSettings);
        }

        public Task Process(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Debug("Process called...");
                bool result = Work();
                NoSendInFailureNotifications = worker.NoSendInFailureNotifications;
                _logger.Debug($"NoSendInFailureNotifications count: {NoSendInFailureNotifications.Count}");
                _logger.Debug($"Completed");

                _logger.Debug($"Firing Zip Updater");

                ZipUpdater zipUpdater = new ZipUpdater(_logger);
                zipUpdater.DoWork();
                _logger.Debug($"Zip Updater Complete");

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Debug(ex.StackTrace);
                return Task.FromResult(false);
            }
        }

        public bool Work()
        {
            worker = new BackendServiceWorker(_logger);
            worker.NoSendInFailureNotifications = NoSendInFailureNotifications;

            return worker.Work();
            //_logger.Debug("Surpath Backend Service: Starting services");
            //// get all the notification window data
            //List<ClientNotificationDataSettings> clientNotificationDataSettings = backendData.GetAllClientNotificationDataSettings();
            //if (clientNotificationDataSettings.Count < 1) return true;  //Task.FromResult(true);

            //// get ready notifications
            //List<Notification> notifications = backendData.GetReadyDonorNotifications(new ParamGetReadyDonorNotifications());
            //if (notifications.Count < 1) return true; // Task.FromResult(true);

            //_logger.Debug($"{notifications.Count} notifications loaded, processing....");
            //List<Notification> _thisDeptNotifications;
            //bool result;
            //try
            //{
            //    foreach (ClientNotificationDataSettings clientNotificationDataSetting in clientNotificationDataSettings)
            //    {
            //        _logger.Debug($"Checking client {clientNotificationDataSetting.client_name} {clientNotificationDataSetting.client_id} {clientNotificationDataSetting.client_department_id}");
            //        if (notifications.Exists(x => x.client_id == clientNotificationDataSetting.client_id && x.client_department_id == clientNotificationDataSetting.client_department_id))
            //        {
            //            _logger.Debug("Notifications found for this client");
            //            _thisDeptNotifications = notifications.Where(x => x.client_id == clientNotificationDataSetting.client_id && x.client_department_id == clientNotificationDataSetting.client_department_id).ToList();

            //        }
            //        else
            //        {
            //            _logger.Debug("No notifications for this client");
            //            continue; // No one to send in.
            //        }

            //        foreach (Notification notification in _thisDeptNotifications)
            //        {
            //            Thread.Sleep(500); // rest 1/2 a second
            //            _logger.Debug($"Processing notification for donor test info id {notification.donor_test_info_id}");
            //            try
            //            {
            //                if (notification.notify_now)
            //                {
            //                    // if now, send it
            //                    _logger.Debug($"Notifying Now!");

            //                    result = backendLogic.SendNotification(notification.donor_test_info_id).GetAwaiter().GetResult();
            //                    if (result) _logger.Debug($"Notified {notification.donor_test_info_id} - was flagged to notify now");

            //                }
            //                else
            //                {
            //                    _logger.Debug($"Is this client manual send in only? {clientNotificationDataSetting.force_manual}");
            //                    if (clientNotificationDataSetting.force_manual) continue; // This client is manual send in only.

            //                    // Verify we're inside the send in window for this client
            //                    DateTime start = clientNotificationDataSetting.notification_start_date ?? DateTime.Now.AddYears(-1); // if not set, go into the past to avoid sending
            //                    DateTime end = clientNotificationDataSetting.notification_stop_date ?? start.AddDays(1);

            //                    _logger.Debug($"start window {start.ToString()} - end window {end.ToString()}");

            //                    if ((start.Date <= DateTime.Now.Date) && (end.Date >= DateTime.Now.Date))
            //                    {
            //                        // are we in the window?
            //                        _logger.Debug($"Notification in window {notification.in_window}");
            //                        if (notification.in_window)
            //                        {
            //                            DateTime _notify_after_timestamp = notification.notify_after_timestamp ?? DateTime.Now;
            //                            _logger.Debug($"Notify after: {_notify_after_timestamp.ToString()}");
            //                            if ((_notify_after_timestamp.Date <= DateTime.Now.Date) || notification.notify_next_window)
            //                            {
            //                                _logger.Debug($"Notifying");
            //                                result = backendLogic.SendNotification(notification.donor_test_info_id).GetAwaiter().GetResult();
            //                                if (result) _logger.Debug($"Notified {notification.donor_test_info_id} - was in window");
            //                            }

            //                        }
            //                    }
            //                    else
            //                    {
            //                        _logger.Debug($"DID NOT notify {notification.donor_test_info_id} - was out of window or past automatic window.");
            //                    }

            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                _logger.Error(ex.Message);
            //                _logger.Error(ex.InnerException.ToString());
            //                _logger.Error("Throwing!");
            //                // No Clinics?
            //                if (ex.Message.IndexOf("no clinic", StringComparison.CurrentCultureIgnoreCase) >= 0)
            //                {
            //                    _logger.Error(ex.Message);
            //                }
            //                // No client data settings?
            //                else if (ex.Message.IndexOf("no client", StringComparison.CurrentCultureIgnoreCase) >= 0)
            //                {
            //                    _logger.Error(ex.Message);
            //                }
            //                // Invalid PDF
            //                else if (ex.Message.IndexOf("Invalid PDF", StringComparison.CurrentCultureIgnoreCase) >= 0)
            //                {
            //                    _logger.Error(ex.Message);
            //                }
            //                else
            //                {
            //                    throw ex;

            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.Message);
            //    _logger.Error(ex.InnerException.ToString());

            //}
            //return true; //Task.FromResult(true);
        }
    }
}