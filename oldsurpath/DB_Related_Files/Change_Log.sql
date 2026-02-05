alter table vendors DROP COLUMN physical_address_1_id;
alter table vendors DROP COLUMN physical_address_2_id;
alter table vendors DROP COLUMN physical_address_3_id;
alter table vendors DROP COLUMN mailing_address_id;

/**********************09-01-2015 Arivu ******************/

ALTER TABLE donor_test_info ADD COLUMN is_walkin_donor BIT(1) NULL DEFAULT b'0' AFTER last_modified_by;

ALTER TABLE donor_test_info ADD COLUMN is_instant_test BIT(1) NULL DEFAULT NULL AFTER is_walkin_donor;

ALTER TABLE donor_test_info ADD COLUMN instant_test_result INT(11) NULL DEFAULT NULL AFTER is_instant_test;


/**********************16-01-2015 Kribu******************/

ALTER TABLE `drug_names` CHANGE COLUMN `unit_of_measurement` `ua_unit_of_measurement` VARCHAR(25) NULL , 


ADD COLUMN `hair_unit_of_measurement` VARCHAR(25) NULL AFTER `ua_unit_of_measurement`;

/**************************************** 11-02-2015 Arivu ******************************************************/

ALTER TABLE report_info ADD COLUMN report_status INT(11) NULL AFTER donor_test_info_id;

/**************************************** 11-02-2015 MI******************************************************/

ALTER TABLE vendors 
CHANGE COLUMN `is_mailing_address_physical1` `is_mailing_address_physical1` INT(1) NOT NULL ;


ALTER TABLE report_info ADD COLUMN report_status INT(11) NULL AFTER donor_test_info_id;

/**************************************** 19-02-2015 Kiruba ******************************************************/

ALTER TABLE donor_test_info
ADD COLUMN payment_received BIT(1) NULL DEFAULT b'0' AFTER instant_test_result;

/********************** 13 - 03 -2015 Kiruba ******************************************************/
ALTER TABLE `donor_test_activity` CHANGE COLUMN `activity_note` `activity_note` VARCHAR(6000) NOT NULL ;

/**************************************** 16-04-2015 Arivu ******************************************************/

ALTER TABLE mismatched_reports CHANGE COLUMN date_of_test date_of_test VARCHAR(10) NULL;


/**************************************** 22-04-2015 MI******************************************************/
ALTER TABLE `client_departments` ADD COLUMN `lab_code` VARCHAR(200) NOT NULL AFTER `department_name`;

/**************************************** 17-11-2015 Arivu ******************************************************/
ALTER TABLE mismatched_reports ADD COLUMN mismatched_count INT(11) NULL AFTER file_name;

ALTER TABLE mismatched_reports ADD COLUMN is_unmatched BIT(1) NULL DEFAULT 0 AFTER mismatched_count;