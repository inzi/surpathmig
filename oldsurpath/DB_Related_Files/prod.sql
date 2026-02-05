-- MySQL dump 10.13  Distrib 5.6.44, for Win64 (x86_64)
--
-- Host: localhost    Database: surpathliveprod
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
  KEY `fk_clientid` (`client_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=915 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=485 DEFAULT CHARSET=utf8;
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
  PRIMARY KEY (`client_department_id`),
  KEY `SearchPage` (`client_department_id`,`department_name`),
  KEY `SearchTab2` (`client_id`,`is_archived`,`is_department_active`,`department_name`) USING HASH
) ENGINE=InnoDB AUTO_INCREMENT=375 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=77 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=429 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=486 DEFAULT CHARSET=utf8;
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
  KEY `fk_donorid` (`donor_id`) USING BTREE,
  KEY `DonorID` (`donor_id`),
  KEY `IsApproved` (`is_Approved`),
  KEY `IsRejected` (`is_Rejected`)
) ENGINE=InnoDB AUTO_INCREMENT=8057 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=125306 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=100841 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=106368 DEFAULT CHARSET=utf8;
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
  KEY `donorregistrationstatus` (`donor_registration_status`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=82376 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=1278093 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=3406146 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=18549477 DEFAULT CHARSET=utf8;
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
  KEY `F_Name` (`donor_first_name`)
) ENGINE=InnoDB AUTO_INCREMENT=1949263 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=692 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=4827 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=28181 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=25535 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=1141 DEFAULT CHARSET=utf8;
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
-- Dumping events for database 'surpathliveprod'
--

--
-- Dumping routines for database 'surpathliveprod'
--
/*!50003 DROP PROCEDURE IF EXISTS `PurgeUser` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `PurgeUser`(IN email varchar(1024))
BEGIN
	DECLARE cursorfinished INTEGER default 0;
	declare thisTestID integer;   
  -- Remove User From System
  set @username = email;

  set @donorid = (select donor_id from users
  where user_name = @username limit 1);

  select @donorid;

  -- get rid of test info categories
  
  
  -- declare curTests
  --   cursor for 
  --     select donor_test_info_id from donor_test_info
  --     
  -- declare continue handler
  --   for not found set cursorfinished =1;
  -- 
  -- 
  -- OPEN curTests;
  -- 
  -- purgetestcats: LOOP
  --   FETCH donor_test_info_id into thisTestID
  --   if cursorfinished = 1 then
  --     leave purgetestcats;
  --   end if;
  --   delete from donor_test_info_categories where donor_test_info_id = thisTestID;
  -- END LOOP purgetestcats;
  -- close curTests;
  -- 
  -- -- get rid of test info rows
  -- 
  -- delete from backend_sms where donor_id = @donorid;
  -- delete from donor_documents where donor_id = @donorid;
  -- delete from donor_test_info where donor_id = @donorid;
  -- delete from donor_third_parties where donor_id = @donorid;
  -- delete from report_info where donor_id = @donorid;
  -- delete from users where donor_id = @donorid;
  -- delete from donors where donor_id = @donorid;
  -- 
  -- 
  -- -- Remove User
  -- 
  -- set @userid = (select user_id from users
  -- where user_name = @username limit 1);
  -- 
  -- select @userid;
  -- 
  -- delete from user_auth_rules where user_id = @userid;
  -- delete from user_departments where user_id = @userid;
  -- delete from users where user_id = @userid;

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

-- Dump completed on 2019-12-31 16:46:08
