-- MySQL dump 10.13  Distrib 5.6.44, for Win64 (x86_64)
--
-- Host: localhost    Database: surpathlive
-- ------------------------------------------------------
-- Server version	5.6.44-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `attorneys`
--

DROP TABLE IF EXISTS `attorneys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `attorneys` (
  `attorney_id` int(11) NOT NULL AUTO_INCREMENT,
  `attorney_first_name` varchar(200) NOT NULL,
  `attorney_last_name` varchar(200) NOT NULL,
  `attorney_address_1` varchar(200) NOT NULL,
  `attorney_address_2` varchar(200) DEFAULT NULL,
  `attorney_city` varchar(200) NOT NULL,
  `attorney_state` varchar(100) NOT NULL,
  `attorney_zip` varchar(10) NOT NULL,
  `attorney_phone` varchar(15) DEFAULT NULL,
  `attorney_fax` varchar(15) DEFAULT NULL,
  `attorney_email` varchar(320) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`attorney_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `auth_rule_categories`
--

DROP TABLE IF EXISTS `auth_rule_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `auth_rule_categories` (
  `auth_rule_category_id` int(11) NOT NULL,
  `auth_rule_category_name` varchar(200) NOT NULL,
  `internal_name` varchar(200) NOT NULL,
  `parent_auth_rule_category_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `user_type` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`auth_rule_category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `auth_rules`
--

DROP TABLE IF EXISTS `auth_rules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `auth_rules` (
  `auth_rule_id` int(11) NOT NULL,
  `auth_rule_name` varchar(200) NOT NULL,
  `internal_name` varchar(200) NOT NULL,
  `parent_auth_rule_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `auth_rule_category_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`auth_rule_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backend_notification_window_data`
--

DROP TABLE IF EXISTS `backend_notification_window_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
  `deadline_to_test_by_in_days` int(11) NOT NULL DEFAULT '0',
  `override_day_schedule` tinyint(4) NOT NULL DEFAULT '0',
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `pdf_template_filename` varchar(200) DEFAULT NULL,
  `enable_sms` tinyint(4) DEFAULT '0',
  `force_manual` tinyint(4) DEFAULT '1',
  `onsite_testing` tinyint(4) DEFAULT '0',
  `pdf_render_settings_filename` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`backend_notification_window_data_id`),
  UNIQUE KEY `client_id` (`client_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_notification_window_data_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backend_notifications`
--

DROP TABLE IF EXISTS `backend_notifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `backend_notifications` (
  `backend_notifications_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `notified_by_email` tinyint(4) NOT NULL DEFAULT '0',
  `notified_by_sms` tinyint(4) NOT NULL DEFAULT '0',
  `notification_email_exception` int(11) NOT NULL DEFAULT '0',
  `clinic_exception` int(11) NOT NULL DEFAULT '0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
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
  PRIMARY KEY (`backend_notifications_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backend_sms_autoresponses`
--

DROP TABLE IF EXISTS `backend_sms_autoresponses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backend_sms_client_data`
--

DROP TABLE IF EXISTS `backend_sms_client_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
  UNIQUE KEY `client_id` (`client_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_client_data_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backend_sms_queue`
--

DROP TABLE IF EXISTS `backend_sms_queue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backend_sms_replies`
--

DROP TABLE IF EXISTS `backend_sms_replies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `backend_sms_replies` (
  `backend_sms_replies_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `reply` varchar(150) DEFAULT NULL,
  `dt_reply_received` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`backend_sms_replies_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_sms_replies_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_addresses`
--

DROP TABLE IF EXISTS `client_addresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_addresses` (
  `client_address_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_department_id` int(11) DEFAULT NULL,
  `address_type_id` int(11) NOT NULL,
  `client_address_1` varchar(255) NOT NULL,
  `client_address_2` varchar(255) DEFAULT NULL,
  `client_city` varchar(255) NOT NULL,
  `client_state` varchar(100) NOT NULL,
  `client_zip` varchar(10) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `client_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`client_address_id`),
  KEY `ca_departmentid` (`client_department_id`),
  KEY `fk_addresstype` (`address_type_id`) USING BTREE,
  KEY `fk_clientid` (`client_id`) USING BTREE,
  KEY `idx_client_addresses_client_zip` (`client_zip`)
) ENGINE=InnoDB AUTO_INCREMENT=910 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_contacts`
--

DROP TABLE IF EXISTS `client_contacts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_contacts` (
  `client_contact_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_department_id` int(11) DEFAULT NULL,
  `client_contact_first_name` varchar(200) NOT NULL,
  `client_contact_last_name` varchar(200) NOT NULL,
  `client_contact_phone` varchar(15) DEFAULT '0',
  `client_contact_fax` varchar(15) DEFAULT '1',
  `client_contact_email` varchar(320) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `client_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`client_contact_id`),
  KEY `fk_clientcontactId` (`client_department_id`) USING BTREE,
  KEY `fk_clientId` (`client_id`) USING BTREE,
  KEY `client_contacts_idx_id_name_name` (`client_id`,`client_contact_first_name`,`client_contact_last_name`),
  KEY `client_contacts_idx_id_name_namedept` (`client_department_id`,`client_contact_first_name`,`client_contact_last_name`)
) ENGINE=InnoDB AUTO_INCREMENT=483 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_departments`
--

DROP TABLE IF EXISTS `client_departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_departments` (
  `client_department_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_id` int(11) NOT NULL,
  `department_name` varchar(200) NOT NULL,
  `lab_code` varchar(200) NOT NULL,
  `mro_type_id` int(11) NOT NULL,
  `payment_type_id` int(11) NOT NULL,
  `is_department_active` bit(1) NOT NULL DEFAULT b'1',
  `is_mailing_address_physical` bit(1) NOT NULL DEFAULT b'0',
  `sales_representative_id` int(11) DEFAULT NULL,
  `sales_comissions` double DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `is_contact_info_as_client` bit(1) NOT NULL DEFAULT b'0',
  `QuestCode` varchar(45) DEFAULT NULL,
  `ClearStarCode` varchar(45) DEFAULT NULL,
  `reason_for_test_default` int(11) DEFAULT '2',
  PRIMARY KEY (`client_department_id`),
  KEY `SearchPage` (`client_department_id`,`department_name`),
  KEY `SearchTab2` (`client_id`,`is_archived`,`is_department_active`,`department_name`) USING HASH
) ENGINE=InnoDB AUTO_INCREMENT=373 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_dept_doctypes`
--

DROP TABLE IF EXISTS `client_dept_doctypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_dept_doctypes` (
  `client_departmentdoctypeid` int(11) NOT NULL AUTO_INCREMENT,
  `client_department_id` int(11) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `description` tinytext CHARACTER SET latin1 NOT NULL,
  `instructions` mediumtext CHARACTER SET latin1 NOT NULL,
  `duedate` date NOT NULL,
  `semester` varchar(50) NOT NULL,
  `is_notifystudent` bit(1) NOT NULL DEFAULT b'0',
  `notifydays1` int(11) NOT NULL DEFAULT '0',
  `notifydays2` int(11) NOT NULL DEFAULT '0',
  `notifydays3` int(11) NOT NULL DEFAULT '0',
  `is_doesexpire` bit(1) NOT NULL DEFAULT b'0',
  `is_required` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `is_approved` bit(1) DEFAULT NULL,
  PRIMARY KEY (`client_departmentdoctypeid`)
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_dept_test_categories`
--

DROP TABLE IF EXISTS `client_dept_test_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_dept_test_categories` (
  `client_dept_test_category_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_department_id` int(11) NOT NULL,
  `test_category_id` int(11) NOT NULL,
  `display_order` int(11) NOT NULL,
  `test_panel_price` double DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`client_dept_test_category_id`),
  KEY `client_dept_test_cat_idx_id_order` (`client_department_id`,`display_order`)
) ENGINE=InnoDB AUTO_INCREMENT=426 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `client_dept_test_panels`
--

DROP TABLE IF EXISTS `client_dept_test_panels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_dept_test_panels` (
  `client_dept_test_panel_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_dept_test_category_id` int(11) NOT NULL,
  `test_panel_id` int(11) NOT NULL,
  `test_panel_price` double NOT NULL,
  `display_order` int(11) NOT NULL,
  `is_main_test_panel` bit(1) NOT NULL DEFAULT b'0',
  `is_1_test_panel` bit(1) NOT NULL DEFAULT b'0',
  `is_2_test_panel` bit(1) NOT NULL DEFAULT b'0',
  `is_3_test_panel` bit(1) NOT NULL DEFAULT b'0',
  `is_4_test_panel` bit(1) NOT NULL DEFAULT b'0',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`client_dept_test_panel_id`),
  KEY `test_panel_id` (`test_panel_id`),
  KEY `TestCategoriesFK` (`test_panel_id`,`client_dept_test_category_id`,`display_order`)
) ENGINE=InnoDB AUTO_INCREMENT=479 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clients` (
  `client_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_name` varchar(255) NOT NULL,
  `client_division` varchar(200) DEFAULT NULL,
  `client_type_id` int(11) NOT NULL,
  `laboratory_vendor_id` int(11) NOT NULL,
  `mro_vendor_id` int(11) NOT NULL,
  `mro_type_id` int(11) NOT NULL,
  `is_mailing_address_physical` bit(1) NOT NULL DEFAULT b'0',
  `is_client_active` bit(1) NOT NULL DEFAULT b'1',
  `sales_representative_id` int(11) DEFAULT NULL,
  `sales_comissions` double DEFAULT NULL,
  `client_code` varchar(30) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `can_edit_test_category` bit(1) DEFAULT b'0',
  PRIMARY KEY (`client_id`),
  KEY `ActiveClient` (`client_id`,`is_client_active`),
  KEY `ClientID` (`client_type_id`),
  KEY `fk_labvendorid` (`laboratory_vendor_id`) USING BTREE,
  KEY `fk_movendorid` (`mro_vendor_id`) USING BTREE,
  KEY `fk_mrotypeid` (`mro_type_id`) USING BTREE,
  KEY `fk_clientcode` (`client_code`) USING BTREE,
  KEY `fk_salesrepid` (`sales_representative_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=111 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cost_master`
--

DROP TABLE IF EXISTS `cost_master`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cost_master` (
  `dna_test_panel_cost` double DEFAULT NULL,
  `cup_cost` double DEFAULT NULL,
  `shipping_cost` double DEFAULT NULL,
  `bc_test_panel_cost` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `courts`
--

DROP TABLE IF EXISTS `courts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `courts` (
  `court_id` int(11) NOT NULL AUTO_INCREMENT,
  `court_name` varchar(200) NOT NULL,
  `court_address_1` varchar(200) NOT NULL,
  `court_address_2` varchar(200) DEFAULT NULL,
  `court_city` varchar(200) NOT NULL,
  `court_state` varchar(100) NOT NULL,
  `court_zip` varchar(10) NOT NULL,
  `court_user_name` varchar(320) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`court_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `departments`
--

DROP TABLE IF EXISTS `departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `departments` (
  `department_id` int(11) NOT NULL AUTO_INCREMENT,
  `department_name` varchar(200) NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`department_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_documents`
--

DROP TABLE IF EXISTS `donor_documents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_documents` (
  `donor_document_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_id` int(11) NOT NULL,
  `client_id` int(11) NOT NULL,
  `document_upload_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `document_title` varchar(200) NOT NULL,
  `document_content` longblob NOT NULL,
  `source` varchar(200) DEFAULT NULL,
  `uploaded_by` varchar(200) NOT NULL,
  `file_name` varchar(200) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `donor_document_typeId` int(11) DEFAULT NULL,
  `dateRequired` datetime DEFAULT NULL,
  `is_Notify` bit(1) DEFAULT NULL,
  `lastNotified` datetime DEFAULT NULL,
  `is_NeedsApproval` bit(1) DEFAULT NULL,
  `is_Updateable` bit(1) DEFAULT NULL,
  `is_Archived` bit(1) DEFAULT NULL,
  `is_Rejected` bit(1) DEFAULT NULL,
  `is_Approved` bit(1) DEFAULT NULL,
  PRIMARY KEY (`donor_document_id`),
  KEY `fk_donorid` (`donor_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5792 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_grid_columns_header`
--

DROP TABLE IF EXISTS `donor_grid_columns_header`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_grid_columns_header` (
  `donor_grid_columns_header_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_grid_columns_header_name` varchar(200) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`donor_grid_columns_header_id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_test_activity`
--

DROP TABLE IF EXISTS `donor_test_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_test_activity` (
  `donor_test_activity_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `activity_datetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `activity_user_id` int(11) NOT NULL,
  `activity_category_id` int(11) NOT NULL,
  `is_activity_visible` bit(1) NOT NULL,
  `activity_note` varchar(6000) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`donor_test_activity_id`)
) ENGINE=InnoDB AUTO_INCREMENT=116335 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_test_info`
--

DROP TABLE IF EXISTS `donor_test_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_test_info` (
  `donor_test_info_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_id` int(11) NOT NULL,
  `client_id` int(11) NOT NULL,
  `client_department_id` int(11) NOT NULL,
  `mro_type_id` int(11) NOT NULL,
  `payment_type_id` int(11) NOT NULL,
  `test_requested_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `test_requested_by` int(11) NOT NULL,
  `is_ua` bit(1) NOT NULL DEFAULT b'0',
  `is_hair` bit(1) NOT NULL DEFAULT b'0',
  `is_dna` bit(1) NOT NULL DEFAULT b'0',
  `reason_for_test_id` int(11) DEFAULT NULL,
  `other_reason` varchar(1000) DEFAULT NULL,
  `is_temperature_in_range` int(11) DEFAULT NULL,
  `temperature_of_specimen` double DEFAULT NULL,
  `testing_authority_id` int(11) DEFAULT NULL,
  `specimen_collection_cup_id` int(11) DEFAULT NULL,
  `is_observed` int(11) DEFAULT NULL,
  `form_type_id` int(11) DEFAULT NULL,
  `is_adulteration_sign` int(11) DEFAULT NULL,
  `is_quantity_sufficient` int(11) DEFAULT NULL,
  `collection_site_vendor_id` int(11) DEFAULT NULL,
  `collection_site_location_id` int(11) DEFAULT NULL,
  `collection_site_user_id` int(11) DEFAULT NULL,
  `screening_time` datetime DEFAULT NULL,
  `is_donor_refused` bit(1) DEFAULT NULL,
  `collection_site_remarks` varchar(5000) DEFAULT NULL,
  `laboratory_vendor_id` int(11) DEFAULT NULL,
  `mro_vendor_id` int(11) DEFAULT NULL,
  `test_overall_result` int(11) DEFAULT NULL,
  `test_status` int(11) NOT NULL,
  `program_type_id` int(11) DEFAULT NULL,
  `is_surscan_determines_dates` bit(1) DEFAULT NULL,
  `is_tp_determines_dates` bit(1) DEFAULT NULL,
  `program_start_date` date DEFAULT NULL,
  `program_end_date` date DEFAULT NULL,
  `case_number` varchar(200) DEFAULT NULL,
  `court_id` int(11) DEFAULT NULL,
  `judge_id` int(11) DEFAULT NULL,
  `special_notes` varchar(5000) DEFAULT NULL,
  `total_payment_amount` double DEFAULT NULL,
  `payment_date` datetime DEFAULT NULL,
  `payment_method_id` int(11) DEFAULT NULL,
  `payment_note` varchar(5000) DEFAULT NULL,
  `payment_status` int(11) DEFAULT NULL,
  `laboratory_cost` double DEFAULT NULL,
  `mro_cost` double DEFAULT NULL,
  `cup_cost` double DEFAULT NULL,
  `shipping_cost` double DEFAULT NULL,
  `vendor_cost` double DEFAULT NULL,
  `collection_site_1_id` int(11) DEFAULT NULL,
  `collection_site_2_id` int(11) DEFAULT NULL,
  `collection_site_3_id` int(11) DEFAULT NULL,
  `collection_site_4_id` int(11) DEFAULT NULL,
  `schedule_date` datetime DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `is_walkin_donor` bit(1) DEFAULT b'0',
  `is_instant_test` bit(1) DEFAULT NULL,
  `instant_test_result` int(11) DEFAULT NULL,
  `payment_received` bit(1) DEFAULT b'0',
  `is_reverse_entry` bit(1) DEFAULT b'0',
  `is_bc` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`donor_test_info_id`),
  KEY `DonorPager` (`client_department_id`,`client_id`),
  KEY `SearchTab` (`donor_id`,`screening_time`),
  KEY `ScreeningTime` (`screening_time`),
  KEY `fk_mrotypeid` (`mro_type_id`) USING BTREE,
  KEY `fk_Paytmenttypeid` (`payment_type_id`) USING BTREE,
  KEY `fk_testingauthorityid` (`testing_authority_id`) USING BTREE,
  KEY `screendingtime` (`screening_time`) USING BTREE,
  KEY `collectionsiteid` (`collection_site_location_id`) USING BTREE,
  KEY `collectionsitevendor` (`collection_site_vendor_id`) USING BTREE,
  KEY `labvendorid` (`laboratory_vendor_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=93992 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_test_info_attorneys`
--

DROP TABLE IF EXISTS `donor_test_info_attorneys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_test_info_attorneys` (
  `donor_test_info_attorney_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `display_order` int(11) NOT NULL,
  `attorney_id` varchar(200) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`donor_test_info_attorney_id`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_test_info_drug_list`
--

DROP TABLE IF EXISTS `donor_test_info_drug_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_test_info_drug_list` (
  `donor_test_info_drug_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_test_category_id` int(11) NOT NULL,
  `test_panel_id` int(11) NOT NULL,
  `drug_name_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`donor_test_info_drug_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_test_info_test_categories`
--

DROP TABLE IF EXISTS `donor_test_info_test_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_test_info_test_categories` (
  `donor_test_test_category_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `test_category_id` int(11) NOT NULL,
  `test_panel_id` int(11) DEFAULT NULL,
  `specimen_id` varchar(200) DEFAULT NULL,
  `hair_test_panel_days` int(11) DEFAULT NULL,
  `test_panel_result` int(11) DEFAULT NULL,
  `test_panel_status` int(11) DEFAULT NULL,
  `test_panel_cost` double DEFAULT NULL,
  `test_panel_price` double DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`donor_test_test_category_id`),
  KEY `SearchPage` (`donor_test_info_id`),
  KEY `speciminid` (`specimen_id`) USING BTREE,
  KEY `fk_testinfoid` (`test_category_id`) USING BTREE,
  KEY `fk_testpanelid` (`test_panel_id`) USING BTREE,
  KEY `result` (`test_panel_result`) USING BTREE,
  KEY `status` (`test_panel_status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=98169 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_test_info_third_parties`
--

DROP TABLE IF EXISTS `donor_test_info_third_parties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_test_info_third_parties` (
  `donor_test_info_3rd_party_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_test_info_id` int(11) NOT NULL,
  `display_order` int(11) NOT NULL,
  `third_party_id` varchar(200) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`donor_test_info_3rd_party_id`)
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `donor_third_parties`
--

DROP TABLE IF EXISTS `donor_third_parties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donor_third_parties` (
  `third_party_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_id` int(11) NOT NULL,
  `third_party_first_name` varchar(200) NOT NULL,
  `third_party_last_name` varchar(200) NOT NULL,
  `third_party_address_1` varchar(200) NOT NULL,
  `third_party_address_2` varchar(200) DEFAULT NULL,
  `third_party_city` varchar(200) NOT NULL,
  `third_party_state` varchar(100) NOT NULL,
  `third_party_zip` varchar(10) NOT NULL,
  `third_party_phone` varchar(15) DEFAULT NULL,
  `third_party_fax` varchar(15) DEFAULT NULL,
  `third_party_email` varchar(320) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`third_party_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary table structure for view `donordocumenttotals`
--

DROP TABLE IF EXISTS `donordocumenttotals`;
/*!50001 DROP VIEW IF EXISTS `donordocumenttotals`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `donordocumenttotals` AS SELECT 
 1 AS `donor_id`,
 1 AS `DocsTotal`,
 1 AS `DocsNotApproved`,
 1 AS `DocsRejected`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `donors`
--

DROP TABLE IF EXISTS `donors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `donors` (
  `donor_id` int(11) NOT NULL AUTO_INCREMENT,
  `donor_first_name` varchar(200) NOT NULL,
  `donor_mi` varchar(50) DEFAULT NULL,
  `donor_last_name` varchar(200) NOT NULL,
  `donor_suffix` varchar(10) DEFAULT NULL,
  `donor_ssn` varchar(11) DEFAULT NULL,
  `donor_date_of_birth` date DEFAULT NULL,
  `donor_phone_1` varchar(15) DEFAULT NULL,
  `donor_phone_2` varchar(15) DEFAULT NULL,
  `donor_address_1` varchar(200) DEFAULT NULL,
  `donor_address_2` varchar(200) DEFAULT NULL,
  `donor_city` varchar(200) DEFAULT NULL,
  `donor_state` varchar(100) DEFAULT NULL,
  `donor_zip` varchar(10) DEFAULT NULL,
  `donor_email` varchar(320) DEFAULT NULL,
  `donor_gender` char(1) DEFAULT NULL,
  `donor_initial_client_id` int(11) DEFAULT NULL,
  `donor_initial_department_id` int(11) DEFAULT NULL,
  `donor_registration_status` int(11) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `is_hidden_web` bit(1) DEFAULT b'0',
  `donorClearstarProfId` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`donor_id`),
  KEY `SearchPage` (`donor_id`,`is_archived`),
  KEY `firstname` (`donor_first_name`) USING BTREE,
  KEY `lastname` (`donor_last_name`) USING BTREE,
  KEY `fk_initialclientid` (`donor_initial_client_id`) USING BTREE,
  KEY `fk_initialdepartmentid` (`donor_initial_department_id`) USING BTREE,
  KEY `donorregistrationstatus` (`donor_registration_status`) USING BTREE,
  KEY `idx_donors_donor_zip` (`donor_zip`)
) ENGINE=InnoDB AUTO_INCREMENT=77958 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `drug_names`
--

DROP TABLE IF EXISTS `drug_names`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `drug_names` (
  `drug_name_id` int(11) NOT NULL AUTO_INCREMENT,
  `drug_name` varchar(200) NOT NULL,
  `drug_code` varchar(200) NOT NULL,
  `ua_screen_value` varchar(10) DEFAULT NULL,
  `ua_confirmation_value` varchar(10) DEFAULT NULL,
  `hair_screen_value` varchar(10) DEFAULT NULL,
  `hair_confirmation_value` varchar(10) DEFAULT NULL,
  `ua_unit_of_measurement` varchar(25) DEFAULT NULL,
  `hair_unit_of_measurement` varchar(25) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `isBC` bit(1) DEFAULT b'0',
  `isDNA` bit(1) DEFAULT b'0',
  PRIMARY KEY (`drug_name_id`)
) ENGINE=InnoDB AUTO_INCREMENT=117 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `drug_names_categories`
--

DROP TABLE IF EXISTS `drug_names_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `drug_names_categories` (
  `drug_name_categories_id` int(11) NOT NULL AUTO_INCREMENT,
  `drug_name_id` int(11) NOT NULL,
  `test_category_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`drug_name_categories_id`)
) ENGINE=InnoDB AUTO_INCREMENT=132 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dst_data`
--

DROP TABLE IF EXISTS `dst_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dst_data` (
  `dst_data_id` int(11) NOT NULL AUTO_INCREMENT,
  `zip` varchar(255) DEFAULT NULL,
  `state` varchar(255) DEFAULT NULL,
  `start_month` int(11) DEFAULT NULL,
  `start_day` int(11) DEFAULT NULL,
  `start_hour` int(11) DEFAULT NULL,
  `end_month` int(11) DEFAULT NULL,
  `end_day` int(11) DEFAULT NULL,
  `end_hour` int(11) DEFAULT NULL,
  PRIMARY KEY (`dst_data_id`),
  KEY `IDX_ZIP_CODES` (`zip`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judges`
--

DROP TABLE IF EXISTS `judges`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judges` (
  `judge_id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_prefix` varchar(100) DEFAULT NULL,
  `judge_first_name` varchar(200) NOT NULL,
  `judge_last_name` varchar(200) NOT NULL,
  `judge_suffix` varchar(100) DEFAULT NULL,
  `judge_address_1` varchar(200) NOT NULL,
  `judge_address_2` varchar(200) DEFAULT NULL,
  `judge_city` varchar(200) NOT NULL,
  `judge_state` varchar(100) NOT NULL,
  `judge_zip` varchar(10) NOT NULL,
  `judge_user_name` varchar(320) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`judge_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mismatched_reports`
--

DROP TABLE IF EXISTS `mismatched_reports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mismatched_reports` (
  `mismatched_report_id` int(11) NOT NULL AUTO_INCREMENT,
  `report_info_id` int(11) NOT NULL,
  `specimen_id` varchar(200) DEFAULT NULL,
  `donor_full_name` varchar(200) DEFAULT NULL,
  `client_code` varchar(200) DEFAULT NULL,
  `date_of_test` varchar(10) DEFAULT NULL,
  `ssn_id` varchar(200) DEFAULT NULL,
  `donor_dob` varchar(200) DEFAULT '0',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `crl_client_code` varchar(50) DEFAULT NULL,
  `file_name` varchar(200) NOT NULL,
  `mismatched_count` int(11) DEFAULT NULL,
  `is_unmatched` bit(1) DEFAULT b'0',
  PRIMARY KEY (`mismatched_report_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1186769 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `neareset_zip_code`
--

DROP TABLE IF EXISTS `neareset_zip_code`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `neareset_zip_code` (
  `donor_zip` varchar(10) NOT NULL,
  `nearest_vendor_zip` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `obr_info`
--

DROP TABLE IF EXISTS `obr_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `obr_info` (
  `obr_info_id` int(11) NOT NULL AUTO_INCREMENT,
  `report_info_id` int(11) NOT NULL,
  `transmited_order` int(11) NOT NULL,
  `collection_site_info` varchar(200) DEFAULT NULL,
  `specimen_collection_date` varchar(10) DEFAULT NULL,
  `specimen_received_date` varchar(10) DEFAULT NULL,
  `crl_client_code` varchar(50) DEFAULT NULL,
  `specimen_type` varchar(200) DEFAULT NULL,
  `section_header` varchar(200) DEFAULT NULL,
  `crl_transmit_date` varchar(10) DEFAULT NULL,
  `service_section_id` varchar(200) DEFAULT NULL,
  `order_status` varchar(50) DEFAULT NULL,
  `reason_type` varchar(200) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `collection_site_id` varchar(200) DEFAULT NULL,
  `specimen_action_code` varchar(50) DEFAULT NULL,
  `tpa_code` varchar(200) DEFAULT NULL,
  `region_code` varchar(200) DEFAULT NULL,
  `client_code` varchar(200) DEFAULT NULL,
  `deaprtment_code` varchar(200) DEFAULT NULL,
  `obr_note` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`obr_info_id`),
  KEY `obr_infoid` (`obr_info_id`),
  KEY `obr_reportinfoid` (`report_info_id`),
  KEY `regioncode` (`region_code`) USING BTREE,
  KEY `clientcode` (`client_code`) USING BTREE,
  KEY `deartmentcode` (`client_code`) USING BTREE,
  KEY `tpacode` (`tpa_code`) USING BTREE,
  KEY `specimenactioncode` (`specimen_action_code`) USING BTREE,
  KEY `orderstatus` (`order_status`) USING BTREE,
  KEY `crlclientcode` (`crl_client_code`) USING BTREE,
  KEY `specimintype` (`specimen_type`) USING BTREE,
  KEY `servicestationid` (`service_section_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3143794 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `obx_info`
--

DROP TABLE IF EXISTS `obx_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `obx_info` (
  `obx_info_id` int(11) NOT NULL AUTO_INCREMENT,
  `obr_info_id` int(11) NOT NULL,
  `sequence` int(11) NOT NULL,
  `test_code` varchar(50) DEFAULT NULL,
  `test_name` varchar(200) DEFAULT NULL,
  `result` varchar(50) DEFAULT NULL,
  `status` varchar(50) DEFAULT NULL,
  `unit_of_measure` varchar(100) DEFAULT NULL,
  `reference_range` varchar(100) DEFAULT NULL,
  `order_status` varchar(100) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `value_type` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`obx_info_id`),
  KEY `obx_obrinfoid` (`obr_info_id`),
  KEY `testcode` (`test_code`) USING BTREE,
  KEY `status` (`status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=17168832 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `registration_edit_requests`
--

DROP TABLE IF EXISTS `registration_edit_requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `registration_edit_requests` (
  `registration_edit_requests_id` char(36) NOT NULL DEFAULT 'UUID()',
  `dt_request` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`registration_edit_requests_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `report_info`
--

DROP TABLE IF EXISTS `report_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `report_info` (
  `report_info_id` int(11) NOT NULL AUTO_INCREMENT,
  `report_type` int(11) NOT NULL,
  `specimen_id` varchar(50) DEFAULT NULL,
  `lab_sample_id` varchar(50) DEFAULT NULL,
  `ssn_id` varchar(50) DEFAULT NULL,
  `donor_last_name` varchar(200) DEFAULT NULL,
  `donor_first_name` varchar(200) DEFAULT NULL,
  `donor_mi` varchar(200) DEFAULT NULL,
  `donor_dob` varchar(10) DEFAULT NULL,
  `donor_gender` varchar(2) DEFAULT NULL,
  `lab_report` longblob,
  `donor_test_info_id` int(11) DEFAULT NULL,
  `report_status` int(11) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  `final_report_id` int(11) DEFAULT NULL,
  `lab_name` varchar(200) DEFAULT NULL,
  `lab_code` varchar(200) DEFAULT NULL,
  `lab_report_date` varchar(20) DEFAULT NULL,
  `donor_driving_license` varchar(30) DEFAULT NULL,
  `test_panel_code` varchar(200) DEFAULT NULL,
  `test_panel_name` varchar(200) DEFAULT NULL,
  `report_note` varchar(1000) DEFAULT NULL,
  `tpa_code` varchar(200) DEFAULT NULL,
  `region_code` varchar(200) DEFAULT NULL,
  `client_code` varchar(200) DEFAULT NULL,
  `deaprtment_code` varchar(200) DEFAULT NULL,
  `donor_id` int(11) DEFAULT NULL,
  `client_id` int(11) DEFAULT NULL,
  `client_department_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`report_info_id`),
  KEY `Speedup1` (`report_type`,`specimen_id`,`donor_test_info_id`),
  KEY `searchtab` (`is_archived`,`donor_test_info_id`,`report_type`),
  KEY `SSN_ID` (`ssn_id`),
  KEY `DONOR_DOB` (`donor_dob`),
  KEY `RI_SpecimenID` (`specimen_id`),
  KEY `ri_donortestinfoid` (`donor_test_info_id`),
  KEY `labsampleid` (`lab_sample_id`) USING BTREE,
  KEY `donor_id` (`donor_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1811751 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `test_categories`
--

DROP TABLE IF EXISTS `test_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `test_categories` (
  `test_category_id` int(11) NOT NULL AUTO_INCREMENT,
  `test_category_name` varchar(200) NOT NULL,
  `internal_name` varchar(200) NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`test_category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `test_panel_drug_names`
--

DROP TABLE IF EXISTS `test_panel_drug_names`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `test_panel_drug_names` (
  `test_panel_drug_names_id` int(11) NOT NULL AUTO_INCREMENT,
  `test_panel_id` int(11) NOT NULL,
  `drug_name_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`test_panel_drug_names_id`)
) ENGINE=InnoDB AUTO_INCREMENT=691 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `test_panels`
--

DROP TABLE IF EXISTS `test_panels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `test_panels` (
  `test_panel_id` int(11) NOT NULL AUTO_INCREMENT,
  `test_panel_name` varchar(200) NOT NULL,
  `test_panel_description` varchar(200) DEFAULT NULL,
  `test_category_id` int(11) NOT NULL,
  `cost` double NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`test_panel_id`),
  KEY `fk_testcategoryid` (`test_category_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `test_types`
--

DROP TABLE IF EXISTS `test_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `test_types` (
  `test_type_id` int(11) NOT NULL AUTO_INCREMENT,
  `test_type_name` varchar(200) NOT NULL,
  `test_category_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`test_type_id`),
  KEY `fk_testcategoryid` (`test_category_id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `testing_authority`
--

DROP TABLE IF EXISTS `testing_authority`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `testing_authority` (
  `testing_authority_id` int(11) NOT NULL AUTO_INCREMENT,
  `testing_authority_name` varchar(200) NOT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`testing_authority_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `timezonebyzipcode`
--

DROP TABLE IF EXISTS `timezonebyzipcode`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `timezonebyzipcode` (
  `idtimezonebyzipcode` int(11) NOT NULL AUTO_INCREMENT,
  `zip` varchar(5) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  `county` varchar(45) DEFAULT NULL,
  `state` varchar(2) DEFAULT NULL,
  `country` varchar(45) DEFAULT NULL,
  `timezone` varchar(45) DEFAULT NULL,
  `addressquality` int(11) DEFAULT NULL,
  `source` varchar(45) DEFAULT NULL,
  `sourcedate` date DEFAULT NULL,
  `utc` int(11) DEFAULT NULL,
  `ignore_dst` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`idtimezonebyzipcode`),
  UNIQUE KEY `zip_UNIQUE` (`zip`)
) ENGINE=InnoDB AUTO_INCREMENT=42598 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user_auth_rules`
--

DROP TABLE IF EXISTS `user_auth_rules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_auth_rules` (
  `user_auth_rule_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `auth_rule_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`user_auth_rule_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4656 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user_departments`
--

DROP TABLE IF EXISTS `user_departments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_departments` (
  `user_department_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `client_department_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`user_department_id`),
  KEY `searchPage` (`user_id`),
  KEY `client_departmentid` (`client_department_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=26721 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_name` varchar(150) DEFAULT NULL,
  `user_password` varchar(2000) NOT NULL,
  `is_user_active` bit(1) NOT NULL DEFAULT b'1',
  `user_first_name` varchar(200) NOT NULL,
  `user_last_name` varchar(200) DEFAULT NULL,
  `user_phone_number` varchar(15) DEFAULT NULL,
  `user_fax` varchar(15) DEFAULT NULL,
  `user_email` varchar(200) DEFAULT NULL,
  `change_password_required` bit(1) NOT NULL DEFAULT b'0',
  `user_type` int(11) NOT NULL,
  `department_id` int(11) DEFAULT NULL,
  `donor_id` int(11) DEFAULT NULL,
  `client_id` int(11) DEFAULT NULL,
  `vendor_id` int(11) DEFAULT NULL,
  `court_id` int(11) DEFAULT NULL,
  `attorney_id` int(11) DEFAULT NULL,
  `judge_id` int(11) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`user_id`),
  KEY `fk_users_department_id` (`department_id`),
  KEY `fk_users_donor_id` (`donor_id`),
  KEY `fk_users_client_id` (`client_id`),
  KEY `fk_users_vendor_id` (`vendor_id`),
  KEY `fk_users_court_id` (`court_id`),
  KEY `fk_users_attorney_id` (`attorney_id`),
  KEY `fk_users_judge_id` (`judge_id`),
  KEY `Login` (`user_name`,`is_user_active`),
  KEY `loginusers fx` (`is_archived`,`user_name`,`user_email`,`is_user_active`),
  CONSTRAINT `fk_users_attorney_id` FOREIGN KEY (`attorney_id`) REFERENCES `attorneys` (`attorney_id`),
  CONSTRAINT `fk_users_client_id` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`),
  CONSTRAINT `fk_users_court_id` FOREIGN KEY (`court_id`) REFERENCES `courts` (`court_id`),
  CONSTRAINT `fk_users_department_id` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`),
  CONSTRAINT `fk_users_donor_id` FOREIGN KEY (`donor_id`) REFERENCES `donors` (`donor_id`),
  CONSTRAINT `fk_users_judge_id` FOREIGN KEY (`judge_id`) REFERENCES `judges` (`judge_id`),
  CONSTRAINT `fk_users_vendor_id` FOREIGN KEY (`vendor_id`) REFERENCES `vendors` (`vendor_id`)
) ENGINE=InnoDB AUTO_INCREMENT=25350 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `vendor_addresses`
--

DROP TABLE IF EXISTS `vendor_addresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vendor_addresses` (
  `vendor_address_id` int(11) NOT NULL AUTO_INCREMENT,
  `vendor_id` int(11) NOT NULL,
  `address_type_id` int(11) NOT NULL,
  `vendor_address_1` varchar(255) NOT NULL,
  `vendor_address_2` varchar(255) DEFAULT NULL,
  `vendor_city` varchar(255) NOT NULL,
  `vendor_state` varchar(100) NOT NULL,
  `vendor_zip` varchar(10) NOT NULL,
  `vendor_phone` varchar(15) DEFAULT NULL,
  `vendor_fax` varchar(15) DEFAULT NULL,
  `vendor_email` varchar(320) DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`vendor_address_id`),
  KEY `va_vendorid` (`vendor_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1139 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `vendor_services`
--

DROP TABLE IF EXISTS `vendor_services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vendor_services` (
  `vendor_service_id` int(11) NOT NULL AUTO_INCREMENT,
  `vendor_service_name` varchar(200) NOT NULL,
  `cost` double NOT NULL,
  `test_category_id` int(11) DEFAULT NULL,
  `is_observed` int(11) DEFAULT NULL,
  `form_type_id` int(11) DEFAULT NULL,
  `is_active` bit(1) NOT NULL DEFAULT b'1',
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`vendor_service_id`)
) ENGINE=InnoDB AUTO_INCREMENT=358 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `vendor_vendor_services`
--

DROP TABLE IF EXISTS `vendor_vendor_services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vendor_vendor_services` (
  `vendor_service_mapping_id` int(11) NOT NULL AUTO_INCREMENT,
  `vendor_id` int(11) NOT NULL,
  `vendor_service_id` int(11) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`vendor_service_mapping_id`)
) ENGINE=InnoDB AUTO_INCREMENT=358 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `vendors`
--

DROP TABLE IF EXISTS `vendors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vendors` (
  `vendor_id` int(11) NOT NULL AUTO_INCREMENT,
  `vendor_type_id` int(11) NOT NULL,
  `vendor_name` varchar(255) NOT NULL,
  `main_contact` varchar(200) NOT NULL,
  `vendor_phone` varchar(15) DEFAULT NULL,
  `vendor_fax` varchar(15) DEFAULT NULL,
  `vendor_email` varchar(320) DEFAULT NULL,
  `vendor_status` int(11) NOT NULL,
  `inactive_date` datetime DEFAULT NULL,
  `inactive_reason` varchar(2000) DEFAULT NULL,
  `is_mailing_address_physical1` int(1) NOT NULL,
  `mpos_mro_cost` double DEFAULT NULL,
  `mall_mro_cost` double DEFAULT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  `is_archived` bit(1) NOT NULL DEFAULT b'0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created_by` varchar(200) NOT NULL,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `last_modified_by` varchar(200) NOT NULL,
  PRIMARY KEY (`vendor_id`)
) ENGINE=InnoDB AUTO_INCREMENT=224 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `zip_codes`
--

DROP TABLE IF EXISTS `zip_codes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `zip_codes` (
  `zip_id` int(11) NOT NULL AUTO_INCREMENT,
  `zip` varchar(255) DEFAULT NULL,
  `latitude` varchar(255) DEFAULT NULL,
  `longitude` varchar(255) DEFAULT NULL,
  `city` varchar(255) DEFAULT NULL,
  `state` varchar(255) DEFAULT NULL,
  `county` varchar(255) DEFAULT NULL,
  `type` varchar(255) DEFAULT NULL,
  `tz_std` int(11) DEFAULT '0',
  `tz_dst` int(11) DEFAULT '0',
  PRIMARY KEY (`zip_id`),
  KEY `IDX_ZIP_CODES` (`zip`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=42742 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `zipdata`
--

DROP TABLE IF EXISTS `zipdata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `zipdata` (
  `ZIP` varchar(255) DEFAULT NULL,
  `LATITUDE` varchar(255) DEFAULT NULL,
  `LONGITUDE` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'surpathlive'
--

--
-- Dumping routines for database 'surpathlive'
--
/*!50003 DROP FUNCTION IF EXISTS `backend_only_digits` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `backend_only_digits`(as_val VARCHAR(65535)) RETURNS varchar(65535) CHARSET latin1
    DETERMINISTIC
BEGIN
   DECLARE retval VARCHAR(65535);
   DECLARE i INT;
   DECLARE strlen INT;
   -- shortcut exit for special cases
   IF as_val IS NULL OR as_val = '' THEN
     RETURN as_val;
   END IF;
   -- initialize for loop
   SET retval = '';
   SET i = 1;
   SET strlen = CHAR_LENGTH(as_val);
 do_loop:
   LOOP
     IF i > strlen THEN
       LEAVE do_loop;
     END IF;
     IF SUBSTR(as_val,i,1) IN ('0','1','2','3','4','5','6','7','8','9') THEN
       SET retval = CONCAT(retval,SUBSTR(as_val,i,1));
     END IF;
     SET i = i + 1;
   END LOOP do_loop;
   RETURN retval;
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `GetDistance` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `GetDistance`(
    originLatitude decimal(11, 7), 
    originLongitude decimal(11, 7),
    relativeLatitude decimal(11, 7),
    relativeLongitude decimal(11, 7)) RETURNS int(1)
BEGIN
   DECLARE x decimal(18,15);
   DECLARE EarthRadius decimal(7,3);
   DECLARE distInSelectedUnit decimal(18, 10);

   IF originLatitude = relativeLatitude AND originLongitude = relativeLongitude THEN
          RETURN 0; -- same lat/lon points, 0 distance
   END IF;

		   -- default unit of measurement will be miles
-- 		   IF measure != 'Miles' AND measure != 'Kilometers' THEN
-- 				  SET measure = 'Miles';
-- 		   END IF;

   -- lat and lon values must be within -180 and 180.
   IF originLatitude < -180 OR relativeLatitude < -180 OR originLongitude < -180 OR relativeLongitude < -180 THEN
          RETURN 0;
   END IF;

   IF originLatitude > 180 OR relativeLatitude > 180 OR originLongitude > 180 OR relativeLongitude > 180 THEN
         RETURN 0;
   END IF;

   SET x = 0.0;

   -- convert from degrees to radians
   SET originLatitude = originLatitude * PI() / 180.0,
       originLongitude = originLongitude * PI() / 180.0,
       relativeLatitude = relativeLatitude * PI() / 180.0,
       relativeLongitude = relativeLongitude * PI() / 180.0;

   -- distance formula, accurate to within 30 feet
   SET x = Sin(originLatitude) * Sin(relativeLatitude) + Cos(originLatitude) * Cos(relativeLatitude) * Cos(relativeLongitude - originLongitude);

   IF 1 = x THEN
          SET distInSelectedUnit = 0; -- same lat/long points
          -- not enough precision in MySQL to detect this earlier in the function
   END IF;

   SET EarthRadius = 3963.189;
   SET distInSelectedUnit = EarthRadius * (-1 * Atan(x / Sqrt(1 - x * x)) + PI() / 2);

	-- convert the result to kilometers if desired
-- 		   IF measure = 'Kilometers' THEN
-- 				  SET distInSelectedUnit = MilesToKilometers(distInSelectedUnit);
-- 		   END IF;

   RETURN distInSelectedUnit;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `haversine` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `haversine`(
        lat1 FLOAT, lon1 FLOAT,
        lat2 FLOAT, lon2 FLOAT
     ) RETURNS float
    NO SQL
    DETERMINISTIC
    COMMENT 'Returns the distance in degrees on the Earth between two known points of latitude and longitude'
BEGIN

-- kilometers (111.045), statute miles (69), or nautical miles (60)


    RETURN 69 * DEGREES(ACOS(
              COS(RADIANS(lat1)) *
              COS(RADIANS(lat2)) *
              COS(RADIANS(lon2) - RADIANS(lon1)) +
              SIN(RADIANS(lat1)) * SIN(RADIANS(lat2))
            ));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_all_notification_window_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_all_notification_window_data`()
BEGIN
  select * from backend_notification_window_data;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_backend_sms_client_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_backend_sms_client_data`(
  in client_id int,
  in client_department_id int
)
BEGIN

  select * from backend_sms_client_data where backend_sms_client_data.client_id = client_id and backend_sms_client_data.client_department_id = client_department_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_donor_notification` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_donor_notification`(
  IN donor_test_info_id int
)
BEGIN

  select * from backend_notifications a
  where a.donor_test_info_id = donor_test_info_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_notification_data_for_donor_info_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_notification_data_for_donor_info_id`(
  in donor_test_info_id int
)
BEGIN

  select distinct
  -- *,
  cd.client_department_id,
  dti.client_id,
  d.donor_id,
  cd.department_name,
  cd.lab_code,
  tp.test_panel_id,
  tp.test_panel_name,
  d.donor_phone_1,
  d.donor_phone_2,
  d.donor_zip,
  d.donor_email,
  tc.test_category_id,
  tc.test_category_name,
  dti.reason_for_test_id,
  dti.other_reason,
  dti.test_requested_date,
  bnwd.send_asap,
  bnwd.delay_in_hours,
  bnwd.deadline_to_test_by_in_days
  from donor_test_info dti
    inner join donor_test_info_test_categories dtitc on dti.donor_test_info_id = dtitc.donor_test_info_id
    inner join client_departments cd on cd.client_department_id = dti.client_department_id
    inner join client_dept_test_categories cdtc on cd.client_department_id = cdtc.client_department_id
    left outer join test_categories tc on cdtc.test_category_id = tc.test_category_id
    inner join client_dept_test_panels cdtp on cdtp.client_dept_test_category_id = cdtc.client_dept_test_category_id
    inner join test_panels tp on tp.test_panel_id = cdtp.test_panel_id
    inner join backend_notifications bn on bn.donor_test_info_id = dti.donor_test_info_id
    left outer join donors d on d.donor_id = dti.donor_id
    left outer join backend_notification_window_data bnwd on bnwd.client_id = dti.client_id and bnwd.client_department_id = dti.client_department_id
  where 
  -- test_status = 4 and
  dti.donor_test_info_id = donor_test_info_id
  order by dti.created_on desc
  limit 1;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_notification_data_list` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_notification_data_list`()
BEGIN


  select distinct
  -- *,
  cd.client_department_id,
  dti.client_id,
  d.donor_id,
  cd.department_name,
  cd.lab_code,
  tp.test_panel_id,
  tp.test_panel_name,
  d.donor_phone_1,
  d.donor_phone_2,
  d.donor_zip,
  d.donor_email,
  tc.test_category_id,
  tc.test_category_name,
  dti.reason_for_test_id,
  dti.other_reason,
  dti.test_requested_date,
  bnwd.send_asap,
  bnwd.delay_in_hours,
  bnwd.deadline_to_test_by_in_days
  from donor_test_info dti
    inner join donor_test_info_test_categories dtitc on dti.donor_test_info_id = dtitc.donor_test_info_id
    inner join client_departments cd on cd.client_department_id = dti.client_department_id
    inner join client_dept_test_categories cdtc on cd.client_department_id = cdtc.client_department_id
    left outer join test_categories tc on cdtc.test_category_id = tc.test_category_id
    inner join client_dept_test_panels cdtp on cdtp.client_dept_test_category_id = cdtc.client_dept_test_category_id
    inner join test_panels tp on tp.test_panel_id = cdtp.test_panel_id
    inner join backend_notifications bn on bn.donor_test_info_id = dti.donor_test_info_id
    left outer join donors d on d.donor_id = dti.donor_id
    left outer join backend_notification_window_data bnwd on bnwd.client_id = dti.client_id and bnwd.client_department_id = dti.client_department_id
--   where 
--   -- test_status = 4 and
--   dti.donor_test_info_id = donor_test_info_id
--   order by dti.created_on desc
--   limit 1;

  where 
  -- test_status = 4 and
  (bn.notified_by_email =0 or bn.notified_by_sms = 0) 
  and bn.notification_exception =0 and bn.clinic_exception = 0
  order by dti.created_on desc
  limit 5;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_notification_exceptions` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_notification_exceptions`(
)
BEGIN

  select * from backend_notifications a
  where a.notification_exception > 0;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_notification_window_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_notification_window_data`(
  IN client_id int(11),
  IN client_department_id int(11))
BEGIN
  select * from backend_notification_window_data a where (a.client_id = client_id and a.client_department_id= client_department_id);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_response_to_sms_message` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_response_to_sms_message`(
  in phone_number varchar(50)
)
BEGIN

  select 
--   *,
  dti.donor_test_info_id,
  dti.donor_id,
  dti.client_id,
  dti.client_department_id,
  scd.client_sms_apikey,
  scd.client_sms_token,
  scd.client_sms_from_id,
  bsa.reply
  from
  backend_notifications bn
  inner join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
  inner join donors d on d.donor_id = dti.donor_id
  left outer join backend_sms_autoresponses bsa on bsa.client_id = dti.client_id and bsa.client_department_id = dti.client_department_id
  left outer join backend_sms_client_data scd on scd.client_id = dti.client_id and scd.client_department_id = dti.client_department_id
  where
  bn.notified_by_sms = 1
  and 
  (backend_only_digits(d.donor_phone_1) like CONCAT('%', phone_number, '%')
  or
  backend_only_digits(d.donor_phone_2) like CONCAT('%', phone_number, '%'))
  and length(phone_number)>9  order by bn.notified_by_sms_timestamp desc
  limit 1;
  
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_sent_sms` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_sent_sms`()
BEGIN

  select * from backend_sms_queue where sent = 1;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_sms_autoresponse` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_sms_autoresponse`(
  in client_id int,
  IN client_department_id int(11)
)
BEGIN

  select * from backend_sms_autoresponses where client_id = client_id and client_department_id = client_department_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_sms_replies` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_sms_replies`(
  in backend_sms_queue_id int
)
BEGIN

  select * from backend_sms_replies where backend_sms_queue_id = backend_sms_queue_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_get_unsent_sms` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_get_unsent_sms`()
BEGIN

--   select 
--   q.*,
--   d.client_sms_apikey,
--   d.client_sms_from_id,
--   d.client_sms_token
--   from backend_sms_queue q
--   inner join backend_sms_client_data d on q.client_id = d.client_id and q.client_department_id = d.client_department_id
--   
--   where sent = 0;
  select 
  bn.backend_notifications_id,
  bn.donor_test_info_id,
  dti.client_id,
  dti.client_department_id,
  d.donor_id,
  d.donor_phone_1,
  d.donor_phone_2,
  bscd.client_sms_from_id,
  bscd.client_sms_apikey,
  bscd.client_sms_token,
  bscd.client_sms_text,
  bn.notified_by_sms,
  bn.created_on,
  bn.notified_by_sms_timestamp
  from backend_notifications bn
  left outer join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
  left outer join backend_sms_client_data bscd on dti.client_id = bscd.client_id and dti.client_department_id = bscd.client_department_id
  left outer join donors d on d.donor_id = dti.donor_id
  
  where bn.notified_by_sms = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_log_sms_reply` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_log_sms_reply`(
  in donor_test_info_id int,
  in reply varchar(150),
  in created_by varchar(128),
  in last_modified_by varchar(128),  
  out backend_sms_replies_id int 
)
BEGIN

  INSERT INTO surpathlive.backend_sms_replies 
  (donor_test_info_id, reply, created_by, last_modified_by, dt_reply_received, created_on, last_modified_on ) 
  VALUES (donor_test_info_id, reply, created_by, last_modified_by, CURRENT_TIMESTAMP(),CURRENT_TIMESTAMP(),CURRENT_TIMESTAMP() );

  set backend_sms_replies_id := last_insert_id();

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_queue_donor_notification` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_queue_donor_notification`(
  in donor_test_info_id int,
  out backend_notifications_id int,
  in created_by varchar(128),
  in last_modified_by varchar(128)
)
BEGIN

 declare test_id int(11) DEFAULT 0;
  
  select b.backend_notifications_id into test_id from backend_notifications as b where b.donor_test_info_id =donor_test_info_id limit 1;

  IF (test_id =0)
  THEN

    INSERT INTO surpathlive.backend_notifications
    (donor_test_info_id, notified_by_email, notified_by_sms, created_on, created_by, last_modified_on, last_modified_by) 
    VALUES (donor_test_info_id, 0, 0, CURRENT_TIMESTAMP, created_by, CURRENT_TIMESTAMP, last_modified_by);

    set backend_notifications_id := last_insert_id();

  ELSE
  
    update backend_notifications
    set notified_by_email =0, notified_by_sms = 0, created_by = created_by, last_modified_on = CURRENT_TIMESTAMP, last_modified_by = last_modified_by, last_modified_on = CURRENT_TIMESTAMP
    where donor_test_info_id = donor_test_info_id;
  
  END IF;

  set backend_notifications_id := test_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_queue_sms` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_queue_sms`(
  in sendTo varchar(45),
  in textToSend varchar(150),
  in client_id int,
  IN client_department_id int(11),
  in donor_id int,
    in created_by varchar(128),
  in last_modified_by varchar(128),
  out backend_sms_queue_id int
)
BEGIN

  INSERT INTO surpathlive.backend_sms_queue
  (`to`, `text`, donor_id, client_id, client_department_id, created_by, last_modified_by, dt_entered) 
  VALUES (sendTo, textToSend, donor_id, client_id, client_department_id, created_by, last_modified_by, CURRENT_TIMESTAMP  );


  set backend_sms_queue_id := last_insert_id();

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_remove_notification_window_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_remove_notification_window_data`(
  IN client_id int,
  IN client_department_id int)
BEGIN
  delete from backend_notification_window_data where client_id = client_id and client_department_id = client_department_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_remove_sms_autoresponse` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_remove_sms_autoresponse`(
  in client_id int,
  IN client_department_id int
)
BEGIN

  delete from backend_sms_autoresponses where backend_sms_autoresponses.client_id = client_id and backend_sms_autoresponses.client_department_id = client_department_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_remove_sms_client_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_remove_sms_client_data`(
  in client_id int,
  IN client_department_id int
)
BEGIN

  delete from backend_sms_client_data where backend_sms_client_data.client_id = client_id and backend_sms_client_data.client_department_id = client_department_id; 

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_sent_sms` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_sent_sms`(
  in param_backend_sms_queue_id int
)
BEGIN


  UPDATE backend_sms 
  SET sent = 1, dt_sent = CURRENT_TIMESTAMP 
  WHERE backend_sms_queue_id = param_backend_sms_queue_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_set_donor_notification` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_set_donor_notification`(
  in donor_test_info_id int,
  in created_by varchar(128),
  in last_modified_by varchar(128),
  in notified_by_email int,
  in notified_by_sms int,
  in notification_email_exception int,
  in clinic_exception int,
  
  in is_archived int,
  in notification_sms_exception int,
  in notification_sent_to_email varchar(255),
  in notification_sent_to_phone varchar(50)
  
  -- ,out backend_notifications_id int
)
BEGIN

 declare test_id int(11) DEFAULT 0;
  
  select b.backend_notifications_id into test_id from backend_notifications as b where b.donor_test_info_id =donor_test_info_id 
  order by b.backend_notifications_id desc 
  limit 1;
--   set backend_notifications_id := test_id;
 


  IF (test_id =0)
  THEN

    INSERT INTO surpathlive.backend_notifications
    (
    donor_test_info_id, 
    notified_by_email, 
    notified_by_email_timestamp,
    notified_by_sms, 
    notified_by_sms_timestamp,
    notification_email_exception, 
    notify_email_exception_timestamp, 
    clinic_exception, 
    clinic_exception_timestamp,
    created_on, 
    created_by, 
    last_modified_on, 
    last_modified_by,
    is_archived,
    notification_sms_exception,
    notification_sent_to_email,
    notification_sent_to_phone,
    notify_sms_exception_timestamp
    ) 
    VALUES (
    donor_test_info_id, 
    notified_by_email, 
    if (notified_by_email>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'), 
    notified_by_sms, 
    if (notified_by_sms>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'), 
    notification_email_exception, 
    if (notification_email_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
    clinic_exception, 
    if (clinic_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
    CURRENT_TIMESTAMP, 
    created_by, 
    CURRENT_TIMESTAMP, 
    last_modified_by,
    if (is_archived>0,1,0),
    notification_sms_exception,
    notification_sent_to_email,
    notification_sent_to_phone,
    if (notification_sms_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00')
     
    );

    set test_id := last_insert_id();

  ELSE
  
    update backend_notifications
    set 
      notified_by_email = notified_by_email, 
      notified_by_email_timestamp = if (notified_by_email>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'), 
      notified_by_sms = notified_by_sms, 
      notified_by_sms_timestamp = if (notified_by_sms>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      notification_email_exception = notification_email_exception, 
      notify_email_exception_timestamp = if (notification_email_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      clinic_exception = clinic_exception, 
      clinic_exception_timestamp = if (clinic_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      last_modified_by = last_modified_by, 
      last_modified_on = CURRENT_TIMESTAMP,
      is_archived = if (is_archived>0,1,0),
      notification_sms_exception = notification_sms_exception,
      notify_sms_exception_timestamp =if (notification_sms_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      notification_sent_to_email = notification_sent_to_email,
      notification_sent_to_phone = notification_sent_to_phone
            
    where backend_notifications_id = test_id;
  
  END IF;

  select * from backend_notifications as b 
  where b.donor_test_info_id =donor_test_info_id 
  and b.backend_notifications_id = test_id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_set_donor_test_activity` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_set_donor_test_activity`(
in donor_test_info_id int,
in activity_user_id int, 
in activity_category_id int, 
in is_activity_visible int, 
in activity_note varchar(6000), 
in is_synchronized int,
out donor_test_activity_id int
)
begin


  INSERT INTO surpathlive.donor_test_activity
  (
  donor_test_info_id, 
  activity_user_id, 
  activity_category_id, 
  is_activity_visible, 
  activity_note, 
  is_synchronized
  ) 
  VALUES (
  donor_test_info_id, 
  activity_user_id, 
  activity_category_id, 
  if (is_activity_visible>0,1,0), 
  activity_note, 
  if (is_synchronized>0,1,0));

  set donor_test_activity_id := last_insert_id();

end ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_set_notification_window_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_set_notification_window_data`(
  IN client_id int(11),
  IN client_department_id int(11),
  in pdf_template_filename varchar(200),
  in pdf_render_settings_filename varchar(200),
  IN sunday tinyint,
    IN sunday_send_time_start_seconds_from_midnight int(11),
    IN sunday_send_time_stop_seconds_from_midnight int(11),  
  IN monday tinyint,
    IN monday_send_time_start_seconds_from_midnight int(11),
    IN monday_send_time_stop_seconds_from_midnight int(11),
  IN tuesday tinyint,
    IN tuesday_send_time_start_seconds_from_midnight int(11),
    IN tuesday_send_time_stop_seconds_from_midnight int(11),
  IN wednesday tinyint,
    IN wednesday_send_time_start_seconds_from_midnight int(11),
    IN wednesday_send_time_stop_seconds_from_midnight int(11),
  IN thursday tinyint,
    IN thursday_send_time_start_seconds_from_midnight int(11),
    IN thursday_send_time_stop_seconds_from_midnight int(11),
  IN friday tinyint,
    IN friday_send_time_start_seconds_from_midnight int(11),
    IN friday_send_time_stop_seconds_from_midnight int(11),
  IN saturday tinyint,
    IN saturday_send_time_start_seconds_from_midnight int(11),
    IN saturday_send_time_stop_seconds_from_midnight int(11),
  IN delay_in_hours int(11),
  IN send_asap tinyint,
  in enable_sms tinyint,
  in force_manual tinyint,
  in onsite_testing tinyint,
  IN deadline_to_test_by_in_days int(11),
  IN override_day_schedule tinyint,
  in created_by varchar(128),
  in last_modified_by varchar(128),
  out backend_notification_window_data_id int(11)
)
BEGIN

  -- if there's a client record, find it
  declare test_id int(11) DEFAULT 0;
  
  select b.backend_notification_window_data_id into test_id from backend_notification_window_data as b where b.client_id =client_id and b.client_department_id = client_department_id limit 1;

  IF (test_id =0)
  THEN

    INSERT INTO surpathlive.backend_notification_window_data
    (
     client_id,
     client_department_id,
     pdf_template_filename,
     pdf_render_settings_filename,
     enable_sms,
     created_by,
     sunday,
     sunday_send_time_start_seconds_from_midnight,
     sunday_send_time_stop_seconds_from_midnight,
     monday,
     monday_send_time_start_seconds_from_midnight,
     monday_send_time_stop_seconds_from_midnight,
     tuesday,
     tuesday_send_time_start_seconds_from_midnight,
     tuesday_send_time_stop_seconds_from_midnight,
     wednesday,
     wednesday_send_time_start_seconds_from_midnight,
     wednesday_send_time_stop_seconds_from_midnight,
     thursday,
     thursday_send_time_start_seconds_from_midnight,
     thursday_send_time_stop_seconds_from_midnight,
     friday,
     friday_send_time_start_seconds_from_midnight,
     friday_send_time_stop_seconds_from_midnight,
     saturday,
     saturday_send_time_start_seconds_from_midnight,
     saturday_send_time_stop_seconds_from_midnight,
     delay_in_hours,
     send_asap,
     force_manual,
     onsite_testing,
     deadline_to_test_by_in_days,
     override_day_schedule,
     last_modified_by
     ) 
    VALUES 
    (
     client_id,
     client_department_id,
     pdf_template_filename,
     pdf_render_settings_filename,
     enable_sms,
     created_by,
     sunday,
     sunday_send_time_start_seconds_from_midnight,
     sunday_send_time_stop_seconds_from_midnight,
     monday,
     monday_send_time_start_seconds_from_midnight,
     monday_send_time_stop_seconds_from_midnight,
     tuesday,
     tuesday_send_time_start_seconds_from_midnight,
     tuesday_send_time_stop_seconds_from_midnight,
     wednesday,
     wednesday_send_time_start_seconds_from_midnight,
     wednesday_send_time_stop_seconds_from_midnight,
     thursday,
     thursday_send_time_start_seconds_from_midnight,
     thursday_send_time_stop_seconds_from_midnight,
     friday,
     friday_send_time_start_seconds_from_midnight,
     friday_send_time_stop_seconds_from_midnight,
     saturday,
     saturday_send_time_start_seconds_from_midnight,
     saturday_send_time_stop_seconds_from_midnight,
     delay_in_hours,
     send_asap,
     force_manual,
     onsite_testing,
     deadline_to_test_by_in_days,
     override_day_schedule,
     last_modified_by
     );

   
    set backend_notification_window_data_id =  LAST_INSERT_ID();


  ELSE

    UPDATE surpathlive.backend_notification_window_data a
    SET 
     a.created_by = created_by,
     a.sunday = sunday,
     a.sunday_send_time_start_seconds_from_midnight = sunday_send_time_start_seconds_from_midnight,
     a.sunday_send_time_stop_seconds_from_midnight = sunday_send_time_stop_seconds_from_midnight,
     a.monday = monday,
     a.monday_send_time_start_seconds_from_midnight = monday_send_time_start_seconds_from_midnight,
     a.monday_send_time_stop_seconds_from_midnight = monday_send_time_stop_seconds_from_midnight,
     a.tuesday = tuesday,
     a.tuesday_send_time_start_seconds_from_midnight = tuesday_send_time_start_seconds_from_midnight,
     a.tuesday_send_time_stop_seconds_from_midnight = tuesday_send_time_stop_seconds_from_midnight,
     a.wednesday = wednesday,
     a.wednesday_send_time_start_seconds_from_midnight = wednesday_send_time_start_seconds_from_midnight,
     a.wednesday_send_time_stop_seconds_from_midnight = wednesday_send_time_stop_seconds_from_midnight,
     a.thursday = thursday,
     a.thursday_send_time_start_seconds_from_midnight = thursday_send_time_start_seconds_from_midnight,
     a.thursday_send_time_stop_seconds_from_midnight = thursday_send_time_stop_seconds_from_midnight,
     a.friday = friday,
     a.friday_send_time_start_seconds_from_midnight = friday_send_time_start_seconds_from_midnight,
     a.friday_send_time_stop_seconds_from_midnight = friday_send_time_stop_seconds_from_midnight,
     a.saturday = saturday,
     a.saturday_send_time_start_seconds_from_midnight = saturday_send_time_start_seconds_from_midnight,
     a.saturday_send_time_stop_seconds_from_midnight = saturday_send_time_stop_seconds_from_midnight,
     a.delay_in_hours = delay_in_hours,
     a.send_asap = send_asap,
     a.deadline_to_test_by_in_days = deadline_to_test_by_in_days,
     a.override_day_schedule = override_day_schedule,
     a.last_modified_by = last_modified_by,
     a.pdf_template_filename = pdf_template_filename,
     a.pdf_render_settings_filename = pdf_render_settings_filename,
     a.enable_sms = enable_sms,
     a.force_manual = force_manual,
     a.onsite_testing = onsite_testing
    WHERE a.client_id = client_id and a.client_department_id = client_department_id;

    
    set backend_notification_window_data_id =  test_id;

  END IF;



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_set_sms_autoresponse` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_set_sms_autoresponse`(
  in client_id int,
  in client_department_id int,
  in client_sms_from_id varchar(32),
  in client_sms_apikey varchar(128),
  in client_sms_token varchar(128),
  in reply varchar(140),
  in created_by varchar(128),
  in last_modified_by varchar(128),
  out backend_sms_autoresponses_id	int(11)
)
BEGIN
declare test_id int(11) DEFAULT 0;

select backend_sms_autoresponses.backend_sms_autoresponses_id into test_id from backend_sms_autoresponses where backend_sms_autoresponses.client_id =client_id and backend_sms_autoresponses.client_department_id =client_department_id limit 1;

  IF (test_id>0)
  THEN
    UPDATE surpathlive.backend_sms_autoresponses 
    SET 
      client_sms_from_id = client_sms_from_id, 
      client_sms_apikey = client_sms_apikey, 
      client_sms_token = client_sms_token, 
      reply = reply, 
      created_by = created_by, 
      last_modified_by = last_modified_by
    WHERE backend_sms_autoresponses.client_id = client_id and backend_sms_autoresponses.client_department_id = client_department_id;
    
    set backend_sms_autoresponses_id = test_id;
  ELSE
    INSERT INTO surpathlive.backend_sms_autoresponses
    (client_id, client_department_id, client_sms_from_id, client_sms_apikey, client_sms_token, reply, created_by, last_modified_by) 
    VALUES (client_id, client_department_id, client_sms_from_id, client_sms_apikey, client_sms_token, reply, created_by, last_modified_by);
    
    set backend_sms_autoresponses_id =  LAST_INSERT_ID();
    
  END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_set_sms_client_data` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_set_sms_client_data`(
  in client_id int,
  in client_department_id int,
  in client_sms_text varchar(250),
  in client_sms_autoresponse_text varchar(250),
  in client_sms_from_id varchar(32),
  in client_sms_apikey varchar(128),
  in client_sms_token varchar(128),
  in created_by varchar(128),
  in last_modified_by varchar(128),
  out backend_sms_client_data_id	int(11)
)
BEGIN
  declare test_id int(11) DEFAULT 0;

  select backend_sms_client_data.backend_sms_client_data_id into test_id from backend_sms_client_data where backend_sms_client_data.client_id =client_id and backend_sms_client_data.client_department_id =client_department_id limit 1;

  IF (test_id>0)
  THEN
  
    UPDATE surpathlive.backend_sms_client_data 
    SET  
    client_id = client_id, 
    client_sms_text = client_sms_text,
    client_sms_autoresponse_text = client_sms_autoresponse_text,
    client_sms_apikey = client_sms_apikey, 
    client_sms_from_id = client_sms_from_id, 
    client_sms_token = client_sms_token, 
    created_by = created_by, 
    last_modified_by = last_modified_by
    WHERE backend_sms_client_data.client_id = client_id and backend_sms_client_data.client_department_id =client_department_id;
    
    set backend_sms_client_data_id = test_id;
  ELSE
  
    INSERT INTO surpathlive.backend_sms_client_data
    (client_id,client_department_id, client_sms_text, client_sms_autoresponse_text, client_sms_apikey, client_sms_from_id, client_sms_token, created_by, last_modified_by) 
    VALUES (client_id,client_department_id, client_sms_text, client_sms_autoresponse_text, client_sms_apikey, client_sms_from_id, client_sms_token, created_by, last_modified_by);

    set backend_sms_client_data_id =  LAST_INSERT_ID();
    
  END IF;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `backend_set_sms_sent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `backend_set_sms_sent`(
  in donor_test_info_id int
)
BEGIN


--   UPDATE backend_sms_queue 
--   SET sent = 1
--   WHERE backend_sms_queue_id = backend_sms_queue_id;

  update backend_notifications
  set notified_by_sms = 1, notified_by_sms_timestamp = CURRENT_TIMESTAMP()
  where donor_test_info_id = donor_test_info_id;


END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CreateRegistrationRequest` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateRegistrationRequest`(OUT registration_edit_requests_id char(36))
BEGIN

  set @last_uuid = uuid();

  INSERT INTO surpathlive.registration_edit_requests
  (registration_edit_requests_id) 
  VALUES (@last_uuid);

  set registration_edit_requests_id =  @last_uuid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CreateUniqueRequest` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateUniqueRequest`(OUT registration_edit_requests_id char(36))
BEGIN

  set @last_uuid = uuid();

  INSERT INTO surpathlive.registration_edit_requests
  (registration_edit_requests_id) 
  VALUES (@last_uuid);

  set registration_edit_requests_id =  @last_uuid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Find_X_Clinics_For_Donor` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Find_X_Clinics_For_Donor`(IN donor_id int, in min_count int, in start_miles int, in max_miles int)
BEGIN
 
--    DECLARE exit_loop INTEGER DEFAULT 0;
  select -1 into @select_count;
  select 0 into @exit_loop;
  select donor_id into @donor_id;
  select min_count into @min_count;
  select start_miles into @dist;
  select 5 into @dist_add;
  select 1 into @locType;
  select max_miles into @max_miles;
  
  

  select
    donors.donor_zip, zip_codes.latitude, zip_codes.longitude
    into @donor_zip,@DLAT, @DLONG 
    from donors 
      left outer join zip_codes on donors.donor_zip = zip_codes.zip
    where
      donors.donor_id = 6;
  
  findLoop: WHILE @select_count < @min_count AND @dist <= @max_miles DO
    
    select 
      count(*) into @select_count
      from
      vendor_addresses
      inner join 
      (SELECT *, haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) as D2C
      FROM ZIP_CODES  
      WHERE 
      LATITUDE IS NOT NULL AND LONGITUDE IS NOT NULL AND 
      haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) < @DIST) dt on vendor_addresses.vendor_zip = dt.zip
      where vendor_addresses.address_type_id = @locType;
   
    select @min_count >= @select_count as calc, @dist as finding_dist, @select_count as found_so_far;      
  
    select @dist + @dist_add into @dist;
    
--     if @select_count <= @min_count then
--       select 'hit min';
--     end if;
  
  END WHILE findLoop;

  if @dist > @max_miles then
    set @dist = -1;
  end if;


    select 
      *
    from
  	vendor_addresses
  	inner join 
      (SELECT *, haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) as D2C
      FROM ZIP_CODES  
  	  WHERE 
      LATITUDE IS NOT NULL AND LONGITUDE IS NOT NULL AND 
  	  haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) < @DIST) dt on vendor_addresses.vendor_zip = dt.zip
  	where vendor_addresses.address_type_id = @locType;

-- --   drop table if exists results;
-- 	CREATE TEMPORARY TABLE IF NOT EXISTS results
-- 	AS
-- 	( 
--     select 
--       *
--     from
--   	vendor_addresses
--   	inner join 
--       (SELECT *, haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) as D2C
--       FROM ZIP_CODES  
--   	  WHERE 
--       LATITUDE IS NOT NULL AND LONGITUDE IS NOT NULL AND 
--   	  haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) < @DIST) dt on vendor_addresses.vendor_zip = dt.zip
--   	where vendor_addresses.address_type_id = @locType
--   );

  

--   
  
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Get_Clinics_For_Donor` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Get_Clinics_For_Donor`(IN donor_id int, in _dist int)
BEGIN
  IF _dist = 0 THEN SET _dist = 50; END IF;
-- 
--   SELECT zip_codes.latitude, zip_codes.longitude
--   INTO @DLAT, @DLONG
--   From zip_codes
--   WHERE zip_codes.zip = _zip;
--   
--   select 
--   *
--   from client_addresses
--   where GetDistance(@DLAT, @DLONG, client_addresses.latitude, client_addresses.longitude) < _dist;
--   
  select donor_id into @donor_id;
  select _dist into @dist;
  
  select donors.donor_zip into @donor_zip from donors where donors.donor_id = @donor_id;
  
  call Get_Clinics_In_Range_Of_Zip(@donor_zip,@dist);
  
  
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Get_Clinics_In_Range_Of_Zip` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Get_Clinics_In_Range_Of_Zip`(IN _zip varchar(20), IN _dist int)
BEGIN

 /*      
		[Description("None")]
		None = 0,
		[Description("Collection Center")]
		CollectionCenter = 1,
		[Description("Lab")]
		Lab = 2,
		[Description("MRO")]
		MRO = 3
*/

  select _zip into @zip;
  select _dist into @dist;
  select 1 into @locType;

	SELECT zip_codes.latitude, zip_codes.longitude
	INTO @DLAT, @DLONG
	From zip_codes
	WHERE zip_codes.zip = @zip;
  
--   select @zip, @dlat, @dlong, @dist;
  
  select vendors.vendor_name, vendor_addresses.*, dt.d2c as d2c
  from
	vendor_addresses
  left outer join vendors on vendor_addresses.vendor_id = vendors.vendor_id
	inner join 
    (SELECT *, haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) as D2C
    FROM ZIP_CODES  
	  WHERE 
    LATITUDE IS NOT NULL AND LONGITUDE IS NOT NULL AND 
	  haversine(@DLAT, @DLONG, LATITUDE, LONGITUDE) < @DIST) dt on vendor_addresses.vendor_zip = dt.zip
	where vendor_addresses.address_type_id = @locType
  order by dt.d2c;
  
  
--   select @zip, @dlat, @dlong;
  
--   drop table if exists locs;
-- 	CREATE TEMPORARY TABLE IF NOT EXISTS
-- 	LOCS
-- 	AS
-- 	( 
--   select *
--   from zip_codes  
-- 	where 
--   latitude is not null and longitude is not null and 
-- 	GetDistance(@DLAT, @DLONG, latitude, longitude) < @dist
--   );
-- 
-- 	select *, GetDistance(@DLAT, @DLONG, latitude, longitude) as dist
--   from
-- 	vendor_addresses
-- 	inner join zip_codes on vendor_addresses.vendor_zip = zip_codes.zip
-- 	where vendor_addresses.address_type_id = @locType
--   and GetDistance(@DLAT, @DLONG, latitude, longitude)  < @dist
--   order by dist;

-- 	select 1 into @locType;
--   
-- 	SELECT zip_codes.latitude, zip_codes.longitude
-- 	INTO @DLAT, @DLONG
-- 	From zip_codes
-- 	WHERE zip_codes.zip = @zip;
-- 
-- 	create temporary table if not exists
-- 	locs
-- 	as
-- 	select * from zip_codes  
-- 	where 
-- 	GetDistance(@DLAT, @DLONG, latitude, longitude) < @dist;
-- 
-- 	select * from
-- 	vendor_addresses
-- 	inner join locs on vendor_addresses.vendor_zip = locs.zip
-- 	where vendor_addresses.address_type_id = @locType;
--   
--   drop table if exists locs;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PurgeUser` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PurgeUser`(IN email varchar(1024))
BEGIN
            -- 	DECLARE done int default false;
            -- 	declare thisTestID integer;   
            --   declare curTests cursor for select donor_test_info_id from donor_test_info where donor_id = @donorid;
            --   declare continue handler for not found set done = true;

  -- Remove User From System
  set @username = email;
  
  set @userid = (select user_id from users
  where user_name = @username limit 1);
  
  set @donorid = (select donor_id from users
  where user_name = @username limit 1);

  select @donorid;

  set @donortestinfoid = (select donor_test_info_id from donor_test_info where donor_id = @donorid);

  select @donortestinfoid;
  
  -- get rid of test info categories
     
            --   OPEN curTests;
            --   
            --   read_loop: LOOP
            --     FETCH curTests into thisTestID;
            --     if done then
            --       select 'done';
            --       leave read_loop;
            --     end if;
            --     select 'deleting ' + thisTestID;
            --     delete from donor_test_info_test_categories where donor_test_info_id = thisTestID;
            --   END LOOP;
            --   select concat('purging donor id ', @donorid);

  -- get rid of test cats
   select concat( 'purging donor_test_info_test_categories ' , @donortestinfoid);
  delete from donor_test_info_test_categories where donor_test_info_id = @donortestinfoid;

  
  select concat( 'purging report_info ' , @donortestinfoid);
  delete from report_info where donor_test_info_id = @donorid;
  

  --  get rid of donor_test_activity records
 select concat( 'purging donor_test_activity ' , @donortestinfoid);
 delete from donor_test_activity where donor_test_info_id = @donortestinfoid;
  -- get rid of test info rows
  select concat( 'purging donor_test_info_test_categories ' , @donortestinfoid);
  delete from donor_test_info_test_categories where donor_test_info_id = @donortestinfoid;
  
  -- delete from backend_sms_replies where backend_sms_queue_id in (select backend_sms_queue_id from backend_sms_queue where backend_sms_queue.donor_id  = @donorid);


 
    
--   select concat( 'purging backend_sms_queue ' , @donorid);
--   delete from backend_sms_queue where donor_id = @donorid;
  select concat( 'purging donor_documents ' , @donorid);
  delete from donor_documents where donor_id = @donorid;
  
  select concat( 'purging donor_test_info ' , @donorid);
  delete from donor_test_info where donor_id = @donorid;

  select concat( 'purging donor_third_parties ' , @donorid);
  delete from donor_third_parties where donor_id = @donorid;

--   select concat( 'purging users ' , @donorid);
--   delete from users where donor_id = @donorid;

  
  
  -- Remove User
  select concat( 'purging user ' , @username);
  select concat( 'User Id ' , @userid);

  select concat( 'purging user_auth_rules ' , @userid);
  delete from user_auth_rules where user_id = @userid;
  select concat( 'purging user_departments ' , @userid);
  delete from user_departments where user_id = @userid;
  select concat( 'purging users ' , @userid);
  delete from users where user_id = @userid;
          --   close curTests;

  select concat( 'purging donors of Donor ID ' , @donorid);
  delete from donors where donor_id = @donorid;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `donordocumenttotals`
--

/*!50001 DROP VIEW IF EXISTS `donordocumenttotals`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`surpath`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `donordocumenttotals` AS select `donor_documents`.`donor_id` AS `donor_id`,count(0) AS `DocsTotal`,count(`donor_documents`.`is_NeedsApproval`) AS `DocsNotApproved`,count(`donor_documents`.`is_Rejected`) AS `DocsRejected` from `donor_documents` group by `donor_documents`.`donor_id`,`donor_documents`.`is_NeedsApproval`,`donor_documents`.`is_Rejected` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-12-05 20:37:00
