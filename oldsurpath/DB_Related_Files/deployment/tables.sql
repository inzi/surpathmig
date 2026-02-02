DROP TABLE `surpathlive`.`backend_notification_window_data`;
DROP TABLE `surpathlive`.`backend_notifications`;
DROP TABLE `surpathlive`.`backend_sms_activity`;
DROP TABLE `surpathlive`.`backend_sms_autoresponses`;
DROP TABLE `surpathlive`.`backend_sms_client_data`;
DROP TABLE `surpathlive`.`backend_sms_queue`;
DROP TABLE `surpathlive`.`backend_sms_replies`;


CREATE TABLE `backend_notification_window_data` (
  `backend_notification_window_data_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_id` int(11) NOT NULL DEFAULT '0',
  `client_department_id` int(11) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `sunday` tinyint(4) NOT NULL DEFAULT '0',
  `sunday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `sunday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `monday` tinyint(4) NOT NULL DEFAULT '0',
  `monday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `monday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `tuesday` tinyint(4) NOT NULL DEFAULT '0',
  `tuesday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `tuesday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `wednesday` tinyint(4) NOT NULL DEFAULT '0',
  `wednesday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `wednesday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `thursday` tinyint(4) NOT NULL DEFAULT '0',
  `thursday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `thursday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `friday` tinyint(4) NOT NULL DEFAULT '0',
  `friday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `friday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `saturday` tinyint(4) NOT NULL DEFAULT '0',
  `saturday_send_time_start_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `saturday_send_time_stop_seconds_from_midnight` int(11) NOT NULL DEFAULT '0',
  `delay_in_hours` int(11) NOT NULL DEFAULT '0',
  `send_asap` tinyint(4) NOT NULL DEFAULT '0',
  `deadline_alert_in_days` int(11) NOT NULL DEFAULT '0',
  `override_day_schedule` tinyint(4) NOT NULL DEFAULT '0',
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `pdf_template_filename` varchar(200) DEFAULT NULL,
  `enable_sms` tinyint(4) DEFAULT '0',
  `force_manual` tinyint(4) DEFAULT '1',
  `onsite_testing` tinyint(4) DEFAULT '0',
  `pdf_render_settings_filename` varchar(200) DEFAULT NULL,
  `notification_sweep_date` datetime DEFAULT NULL,
  `notification_start_date` datetime DEFAULT NULL,
  `notification_stop_date` datetime DEFAULT NULL,
  `client_sms_from_number` varchar(150) DEFAULT NULL,
  `client_sms_apikey` varchar(255) DEFAULT NULL,
  `client_sms_token` varchar(255) DEFAULT NULL,
  `client_autoresponse` varchar(150) DEFAULT NULL,
  `show_web_notify_button` tinyint(4) NOT NULL DEFAULT '0',
  `max_sendins` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`backend_notification_window_data_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_notification_window_data_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

CREATE TABLE `backend_notifications` (
  `backend_notifications_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `notified_by_email` tinyint(4) NOT NULL DEFAULT '0',
  `notified_by_sms` tinyint(4) NOT NULL DEFAULT '0',
  `notification_email_exception` int(11) NOT NULL DEFAULT '0',
  `clinic_exception` int(11) NOT NULL DEFAULT '0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `notified_by_email_timestamp` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `notified_by_sms_timestamp` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `notify_email_exception_timestamp` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `clinic_exception_timestamp` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `is_archived` tinyint(4) DEFAULT '0',
  `notification_sent_to_email` varchar(255) DEFAULT NULL,
  `notification_sent_to_phone` varchar(50) DEFAULT NULL,
  `notification_sms_exception` int(11) NOT NULL DEFAULT '0',
  `notify_sms_exception_timestamp` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `notify_now` tinyint(4) NOT NULL DEFAULT '0',
  `notify_next_window` tinyint(4) NOT NULL DEFAULT '0',
  `clinic_radius` int(11) NOT NULL DEFAULT '0',
  `notify_after_timestamp` datetime DEFAULT NULL,
  `notify_before_timestamp` datetime DEFAULT NULL,
  `notify_again` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`backend_notifications_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

CREATE TABLE `backend_sms_activity` (
  `backend_sms_activity_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `sent_text` varchar(150) DEFAULT NULL,
  `dt_sms_sent` datetime DEFAULT NULL,
  `reply_text` varchar(150) DEFAULT NULL,
  `dt_reply_received` datetime DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `reply_read` int(11) NOT NULL DEFAULT '0',
  `user_id` int(11) NOT NULL DEFAULT '0',
  `reply_read_timestamp` datetime DEFAULT NULL,
  `auto_reply_text` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`backend_sms_activity_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_activity_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

CREATE TABLE `backend_sms_autoresponses` (
  `backend_sms_autoresponses_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_id` int(11) NOT NULL,
  `client_department_id` int(11) NOT NULL,
  `client_sms_from_id` varchar(150) DEFAULT NULL,
  `client_sms_apikey` varchar(150) DEFAULT NULL,
  `client_sms_token` varchar(150) DEFAULT NULL,
  `reply` varchar(150) DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`backend_sms_autoresponses_id`),
  UNIQUE KEY `client_id` (`client_id`),
  UNIQUE KEY `client_department_id` (`client_department_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_autoresponses_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

CREATE TABLE `backend_sms_client_data` (
  `backend_sms_client_data_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_id` int(11) NOT NULL,
  `client_department_id` int(11) NOT NULL,
  `client_sms_apikey` varchar(128) NOT NULL,
  `client_sms_from_id` varchar(45) NOT NULL,
  `client_sms_token` varchar(512) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `client_sms_from` varchar(20) NOT NULL DEFAULT '0',
  `client_sms_text` varchar(140) NOT NULL DEFAULT 'Drug Screening Notification - Please check your email for details',
  `client_sms_autoresponse_text` varchar(140) DEFAULT 'Thank you for your reply. Please see your email for more details.',
  PRIMARY KEY (`backend_sms_client_data_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_client_data_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

CREATE TABLE `backend_sms_queue` (
  `backend_sms_queue_id` int(11) NOT NULL AUTO_INCREMENT,
  `to` varchar(45) NOT NULL,
  `text` varchar(150) NOT NULL,
  `sent` tinyint(4) NOT NULL DEFAULT '0',
  `dt_entered` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `dt_sent` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `donor_id` int(11) DEFAULT NULL,
  `client_id` int(11) DEFAULT NULL,
  `client_department_id` int(11) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`backend_sms_queue_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_queue_id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1;

CREATE TABLE `backend_sms_replies` (
  `backend_sms_replies_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `reply` varchar(150) DEFAULT NULL,
  `dt_reply_received` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `is_read` int(11) NOT NULL DEFAULT '0',
  `user_id` int(11) NOT NULL DEFAULT '0',
  `is_read_timestamp` datetime DEFAULT NULL,
  PRIMARY KEY (`backend_sms_replies_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_replies_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

