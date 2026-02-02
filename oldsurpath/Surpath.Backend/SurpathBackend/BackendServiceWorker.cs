using Microsoft.Extensions.Configuration;
using Serilog;
using SurPath.Data;
using SurPath.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SurpathBackend
{
    internal class BackendServiceWorker
    {
        private BackendData backendData;
        private BackendLogic backendLogic;

        public List<Notification> NoSendInFailureNotifications = new List<Notification>();

        // Our Service Configuration
        public IConfigurationRoot ConfigurationRoot;

        // Our Logger
        public static ILogger _logger;

        public BackendServiceWorker(ILogger __logger)
        {
            _logger = __logger;
            _logger.Debug("BackendServiceWorker loaded");
            //backendData = new BackendData(null, null, _logger);
            backendLogic = new BackendLogic(null, _logger);
            backendData = backendLogic.backendData;
        }

        public bool Work()
        {
            _logger.Debug("BackendServiceWorker doing work");

            _logger.Debug($"Current NoSendInFailureNotifications count: {NoSendInFailureNotifications.Count}");
            // get all the notification window data
            List<ClientNotificationDataSettings> clientNotificationDataSettings = backendData.GetAllClientNotificationDataSettings();
            if (clientNotificationDataSettings.Count < 1) return true;  //Task.FromResult(true);

            //clientNotificationDataSettings = clientNotificationDataSettings.Where(c => c.client_id == 110 && c.client_department_id==372).ToList();

            // get ready notifications
            List<Notification> notifications = backendData.GetReadyDonorNotifications(new ParamGetReadyDonorNotifications());
            if (notifications.Count < 1) return true; // Task.FromResult(true);

            _logger.Debug($"{notifications.Count} notifications loaded, processing....");
            List<Notification> _thisDeptNotifications;
            string _skipped = string.Empty;
            bool result;
            //try
            //{
            foreach (ClientNotificationDataSettings clientNotificationDataSetting in clientNotificationDataSettings)
            {
                _skipped = string.Empty;
                _logger.Debug($"Checking client {clientNotificationDataSetting.client_name} {clientNotificationDataSetting.client_id} {clientNotificationDataSetting.client_department_id}");
                _logger.Debug($"Is this client manual send in only? {clientNotificationDataSetting.force_manual}");

                
                if (notifications.Exists(x => x.client_id == clientNotificationDataSetting.client_id && x.client_department_id == clientNotificationDataSetting.client_department_id))
                {
                    _thisDeptNotifications = notifications.Where(x => x.client_id == clientNotificationDataSetting.client_id && x.client_department_id == clientNotificationDataSetting.client_department_id).ToList();
                    _logger.Debug($"Notifications found for this client - {_thisDeptNotifications.Count}");

                }
                else
                {
                    _logger.Debug("No notifications for this client");
                    continue; // No one to send in.
                }

                // speed up our list
                if (clientNotificationDataSetting.force_manual)
                {
                    _logger.Debug($"This client is force_manual, removing notifications not set to notify_now since they would be skipped");
                    _skipped = string.Join(",", (_thisDeptNotifications.Where(n => n.notify_now == false).ToList().Select(n => n.donor_test_info_id.ToString()).ToArray()));
                    _thisDeptNotifications.RemoveAll(l => l.notify_now == false);
                    _logger.Debug($"Notifications for this client now - {_thisDeptNotifications.Count}");

                }

                foreach (Notification notification in _thisDeptNotifications)
                {
                    Thread.Sleep(100); // rest 1/2 a second
                    _logger.Debug($"Processing notification. Donor test info id {notification.donor_test_info_id}, notification ID: {notification.backend_notifications_id}");
                    try
                    {
                        if (notification.notify_now)
                        {
                            // if now, send it
                            _logger.Debug($"Notifying Now! Donor test info id {notification.donor_test_info_id}, notification ID: {notification.backend_notifications_id}");

                            Notification oldnotification = new Notification();
                            if (NoSendInFailureNotifications.Exists(n => n.donor_test_info_id == notification.donor_test_info_id))
                            {
                                _logger.Debug($"donor_test_info {notification.donor_test_info_id} has been tried before");
                                oldnotification = NoSendInFailureNotifications.Where(n => n.donor_test_info_id == notification.donor_test_info_id).First();
                            }

                            result = backendLogic.SendNotification(notification.donor_test_info_id,0, oldnotification).GetAwaiter().GetResult();
                            if (result)
                            {
                                if (NoSendInFailureNotifications.Exists(n => n.donor_test_info_id == notification.donor_test_info_id))
                                {
                                    NoSendInFailureNotifications.RemoveAll(n => n.donor_test_info_id == notification.donor_test_info_id);
                                    _logger.Debug($"donor_test_info {notification.donor_test_info_id} removed from NoSendInFailureNotifications");
                                }
                                _logger.Debug($"Notified {notification.donor_test_info_id} - was flagged to notify now");
                            }else
                            {
                                if (!NoSendInFailureNotifications.Exists(n => n.donor_test_info_id == notification.donor_test_info_id))
                                {
                                    _logger.Debug($"Adding {notification.donor_test_info_id} to NoSendInFailureNotifications");
                                    NoSendInFailureNotifications.Add(notification);
                                }
                                _logger.Debug($"{notification.donor_test_info_id} was flagged to notify now, but SendNotification return false;");
                            }
                        }
                        else
                        {
                            // If force manual and automatic send in is disabled -
                            // Perhaps an option to use the windows, but require the notification be set to go in.
                            // That way, if auto send in is disabled, but they have a window set, when sending in, it can 
                            // prompt "no or on schedule"
                            // If they say schedule, donors being sent in will go on next window
                            if (clientNotificationDataSetting.force_manual)
                            {
                                _skipped = _skipped + $",{notification.donor_test_info_id}";
                               // _logger.Debug($"This client is manual send in only.");
                                continue; // This client is manual send in only.
                            }

                            // Verify we're inside the send in window for this client
                            // otification_start_date and notification_stop_date are the start and stop of the send in dates
                            // for window, we have to look at DaySettings
                            int _today = DateTime.Now.Day;
                            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
                            _logger.Debug($"Is this client day enabled? _today = {_today.ToString()}");

                            if (!clientNotificationDataSetting.DaySettings.Exists(x => x.DayOfWeek == (int)dayOfWeek && x.Enabled == true))
                            {
                                _logger.Debug($"Client window is not enabled");
                                _skipped = _skipped + $",{notification.donor_test_info_id}";

                                continue;

                            }
                            _logger.Debug($"Client has window for today");
                            ClientNotificationDataSettingsDay clientNotificationDataSettingsDay = clientNotificationDataSetting.DaySettings.Where(x => x.DayOfWeek == (int)dayOfWeek && x.Enabled == true).ToList().FirstOrDefault();
                            DateTime start = clientNotificationDataSetting.notification_start_date ?? DateTime.Now.AddYears(-1); // if not set, go into the past to avoid sending
                            DateTime end = clientNotificationDataSetting.notification_stop_date ?? start.AddDays(1);
                            // Problem - we're not adding seconds since midnight

                            DateTime DayStart = DateTime.Now.Date.AddSeconds(clientNotificationDataSettingsDay.send_time_start_seconds_from_midnight);
                            DateTime DayEnd = DateTime.Now.Date.AddSeconds(clientNotificationDataSettingsDay.send_time_stop_seconds_from_midnight);

                            if (clientNotificationDataSettingsDay.send_time_stop_seconds_from_midnight == 0)
                            {
                                DayEnd = DateTime.Now.Date.AddDays(1);
                            }


                            DateTime sweep = clientNotificationDataSetting.notification_sweep_date ?? DateTime.Now.AddYears(1);

                            _logger.Debug($"start send in window {start.ToString()} - end send in window {end.ToString()}");
                            _logger.Debug($"start send in day window {DayStart.ToString()} - end send in day window {DayEnd.ToString()}");

                            // are we in the send in window?
                            if ((start.Date <= DateTime.Now.Date) && (end.Date >= DateTime.Now.Date))
                            {
                                // yes
                                _logger.Debug($"Client's notification settings are in window. Is the notification itself?");
                                _logger.Debug($"Notification {notification.backend_notifications_id} within window property: {notification.in_window}");
                                if (notification.in_window)
                                {

                                    DateTime _notify_after_timestamp = notification.notify_after_timestamp ?? DateTime.Now;
                                    DateTime _notify_created_on = notification.created_on;
                                    _logger.Debug($"Notify after: {_notify_after_timestamp.ToString()}");

                                    if ((_notify_after_timestamp.Date <= DateTime.Now.Date && _notify_created_on.Date >= sweep.Date) || notification.notify_next_window)
                                    {
                                        _logger.Debug($"Notifying");
                                        Notification oldnotification = new Notification();
                                        if (NoSendInFailureNotifications.Exists(n => n.donor_test_info_id == notification.donor_test_info_id))
                                        {
                                            _logger.Debug($"donor_test_info {notification.donor_test_info_id} has been tried before");
                                            oldnotification = NoSendInFailureNotifications.Where(n => n.donor_test_info_id == notification.donor_test_info_id).First();
                                        }
                                        result = backendLogic.SendNotification(notification.donor_test_info_id,0, oldnotification).GetAwaiter().GetResult();
                                        if (result)
                                        {
                                            if (NoSendInFailureNotifications.Exists(n => n.donor_test_info_id == notification.donor_test_info_id))
                                            {
                                                NoSendInFailureNotifications.RemoveAll(n => n.donor_test_info_id == notification.donor_test_info_id);
                                                _logger.Debug($"donor_test_info {notification.donor_test_info_id} removed from NoSendInFailureNotifications");
                                            }
                                            _logger.Debug($"Notified {notification.donor_test_info_id} - was in window");
                                        }
                                        else
                                        {
                                            if (!NoSendInFailureNotifications.Exists(n=>n.donor_test_info_id== notification.donor_test_info_id))
                                            {
                                                _logger.Debug($"Adding {notification.donor_test_info_id} to NoSendInFailureNotifications");
                                                NoSendInFailureNotifications.Add(notification);
                                            }
                                            _logger.Debug($"{notification.donor_test_info_id} - was in window, but SendNotification returned false");
                                        }

                                    }
                                }
                                else
                                {
                                    _skipped = _skipped + $",{notification.donor_test_info_id}";

                                    _logger.Debug($"{notification.backend_notifications_id} not in window. Not notifying.");
                                    if (notification.notify_after_timestamp!=null) _logger.Debug($"Notify After date: {notification.notify_after_timestamp.ToString()}.");

                                }
                            }
                            else
                            {
                                _skipped = _skipped + $",{notification.donor_test_info_id}";

                                _logger.Debug($"DID NOT notify {notification.donor_test_info_id} - was out of window or past automatic window.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StringBuilder stringBuilder = new StringBuilder();

                        _logger.Error(ex.Message);
                        stringBuilder.AppendLine(ex.Message);
                        stringBuilder.AppendLine(String.Empty);
                        if (ex.InnerException != null)
                        {
                            _logger.Error(ex.InnerException.ToString());
                            stringBuilder.AppendLine(ex.InnerException.ToString());

                        }
                        if (ex.StackTrace != null)
                        {
                            _logger.Error(ex.StackTrace.ToString());
                            stringBuilder.AppendFormat(ex.StackTrace.ToString());

                        }

                        backendLogic.SendError(stringBuilder.ToString());

                        _logger.Error("Throwing!");
                        // No Clinics?
                        if (ex.Message.IndexOf("no clinic", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            _logger.Error(ex.Message);
                        }
                        // No client data settings?
                        else if (ex.Message.IndexOf("no client", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            _logger.Error(ex.Message);
                        }
                        // Invalid PDF
                        else if (ex.Message.IndexOf("Invalid PDF", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            _logger.Error(ex.Message);
                        }
                        else
                        {
                            _logger.Error(ex.ToString());
                            if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                            if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());
                            throw;
                        }
                    }
                }
                _logger.Debug($"Donor Test Infos skipped this pass:");
                if (_skipped.Length > 0) _skipped = _skipped.Trim().TrimStart(',');
                _logger.Debug($"{_skipped}");
            }


            // Commented this out - not sure why I was doing this

            //// update the local config file to reflect globals after each job.
            //if (!backendLogic.SavePDFGlobalsToConfig())
            //{
            //    _logger.Error("Unable to update local application config file. If defaults are loaded from config, this could overwrite changes users have made");
            //}



            return true; //Task.FromResult(true);
        }
    }
}