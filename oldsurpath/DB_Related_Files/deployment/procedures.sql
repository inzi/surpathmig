DROP PROCEDURE IF EXISTS surpathlive.backend_get_all_notification_window_data;
CREATE PROCEDURE surpathlive.`backend_get_all_notification_window_data`()
BEGIN
--   select a.* 
--   ,backend_in_notification_window(a.client_id, a.client_department_id,0 ) as in_window
--   ,c.client_name
--   ,cd.department_name
--   from backend_notification_window_data a
--   left outer join clients c on c.client_id = a.client_id
--   left outer join client_departments cd on cd.client_id = c.client_id;
  
  select

  a.*
  ,cd.department_name
  ,backend_in_notification_window(a.client_id, a.client_department_id,0 ) as in_window
   ,a.* 
  ,c.client_name
  ,cd.department_name
  from backend_notification_window_data a
  inner join clients c on c.client_id = a.client_id
  inner join client_departments cd on cd.client_id = c.client_id and cd.client_department_id = a.client_department_id
  where cd.is_department_active>0 and c.is_client_active > 0
  order by c.client_id, cd.client_department_id
  ;
  
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_backend_sms_client_data;
CREATE PROCEDURE surpathlive.`backend_get_backend_sms_client_data`(
  in client_id int,
  in client_department_id int
)
BEGIN

  select * from backend_sms_client_data where backend_sms_client_data.client_id = client_id and backend_sms_client_data.client_department_id = client_department_id;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_Get_Clinics_In_Range_Of_Zip;
CREATE PROCEDURE surpathlive.`backend_Get_Clinics_In_Range_Of_Zip`(IN _zip varchar(20), IN _dist int)
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

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_clinic_exception_count;
CREATE PROCEDURE surpathlive.`backend_get_clinic_exception_count`()
BEGIN

select count(*) as clinic_exception_count from
backend_notifications
where
clinic_exception>0;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_deadline_donors_dataview;
CREATE PROCEDURE surpathlive.`backend_get_deadline_donors_dataview`(
)
BEGIN

  select 
  d.donor_id AS DonorId,
  donorClearStarProfId AS DonorClearStarProfId,
  d.donor_first_name AS DonorFirstName,
  d.donor_last_name AS DonorLastName,
  d.donor_ssn AS DonorSSN,
  d.donor_date_of_birth AS DonorDateOfBirth,
  d.donor_initial_client_id AS DonorInitialClientId,
  d.donor_registration_status AS DonorRegistrationStatus,
  d.donor_city as DonorCity,
  d.donor_email as DonorEmail,
  d.donor_zip as DonorZipCode,
  dti.test_requested_date as DonorTestRegisteredDate,
  d.is_archived AS IsDonorArchived,
  dti.donor_test_info_id AS DonorTestInfoId,
  dti.client_id AS TestInfoClientId,
  dti.client_department_id AS TestInfoDepartmentId,
  dti.mro_type_id AS MROTypeId,
  dti.payment_type_id AS PaymentTypeId,
  dti.test_requested_date AS TestRequestedDate,
  dti.reason_for_test_id AS ReasonForTestId,
  dti.is_walkin_donor AS IsWalkinDonor,
  dti.instant_test_result AS InstantTestResult,
  dti.is_instant_test AS IsInstantTest,
  dti.total_payment_amount AS TotalPaymentAmount,
  dti.payment_method_id AS PaymentMethodId,
  dti.payment_received AS PaymentReceived,
  dti.payment_date AS PaymentDate,
  dti.test_status AS TestStatus,
  dti.test_overall_result AS TestOverallResult,
  bn.notified_by_email as Notified_by_email,
  if (bn.notified_by_email is null, 'Old Data',IF (bn.notified_by_email = 0, 'Not Notified',bn.notified_by_email_timestamp)) as Notified_by_email_timestamp,
  if (bn.backend_notifications_id is null, 0, bn.backend_notifications_id) as backend_notifications_id,
  CONCAT_WS(' ', u.user_first_name, u.user_last_name) AS CollectorName,
  (select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = dti.donor_test_info_id) AS SpecimenId,
  dti.screening_time AS SpecimenDate,
  c.client_name AS ClientName,
  cd.department_name AS ClientDepartmentName,
  cd.clearstarcode As ClearStarCode,
  ddt.DocsTotal, 
  ddt.DocsNotApproved, 
  ddt.DocsRejected,
  if (backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now) =0,'Yes','No') as in_window,
   
  if (bn.clinic_radius = 0, '', CAST(bn.clinic_radius as CHAR(50))) as clinic_radius_text,
  bn.*,
  bnwd.*,
  date(bnwd.notification_stop_date) as SendInStopDate

  from backend_notifications bn
    inner join donor_test_info dti on dti.donor_test_info_id= bn.donor_test_info_id
    inner join donors d on d.donor_id = dti.donor_id
    left outer join clients c on (dti.client_id = c.client_id)
    left outer join client_departments cd on dti.client_department_id = cd.client_department_id
    left outer join users u on dti.collection_site_user_id = u.user_id
    left outer join donordocumenttotals ddt on dti.donor_id = ddt.donor_id
    left outer join backend_notification_window_data bnwd on bnwd.client_id = dti.client_id and bnwd.client_department_id = dti.client_department_id
  where
  ( bn.notified_by_email < 1 or bn.notify_again>0)
  and Date(DATE_ADD(bn.notify_before_timestamp, interval -bnwd.deadline_alert_in_days day))<=Curdate()
  limit 100;


END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_donor_info_id_by_phone;
CREATE PROCEDURE surpathlive.`backend_get_donor_info_id_by_phone`(
  in phone_number varchar(50),
  out donor_test_info_id int
)
BEGIN

  select 
  dti.donor_test_info_id
  into donor_test_info_id
  from
  backend_notifications bn
  inner join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
  inner join donors d on d.donor_id = dti.donor_id
  where
  (backend_only_digits(d.donor_phone_1) like CONCAT('%', backend_only_digits(phone_number), '%')
  or
  backend_only_digits(d.donor_phone_2) like CONCAT('%', backend_only_digits(phone_number), '%'))
  and length(backend_only_digits(phone_number))>9 
  order by bn.backend_notifications_id desc
  limit 1;
  
  -- 
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_donor_notification;
CREATE PROCEDURE surpathlive.`backend_get_donor_notification`(
  IN donor_test_info_id int
)
BEGIN

  select c.client_id, d.client_department_id, a.*
  ,backend_in_notification_window(dti.client_id, dti.client_department_id, a.notify_now) as in_window
  from backend_notifications a
      left outer join donor_test_info dti on dti.donor_test_info_id = a.donor_test_info_id
    left outer join clients c on c.client_id = dti.client_id
    left outer join client_departments d on d.client_department_id = dti.client_department_id
  
  where a.donor_test_info_id = donor_test_info_id;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_exception_count;
CREATE PROCEDURE surpathlive.`backend_get_exception_count`()
BEGIN


declare clinic_exception_count int(11) DEFAULT 0;
declare sms_count int(11) DEFAULT 0;
declare sis_count int(11) DEFAULT 0;
declare did_count int(11) DEFAULT 0;
declare exception_total int(11) DEFAULT 0;


    select count(*)  
    into clinic_exception_count
    from
    backend_notifications
    where
    clinic_exception>0;


    select 
    count(*)
    into sms_count
    from backend_sms_activity ba
    inner join donor_test_info dti on dti.donor_test_info_id = ba.donor_test_info_id
    inner join donors d on d.donor_id = dti.donor_id
    left outer join clients c on c.client_id = dti.client_id
    left outer join client_departments cd on dti.client_department_id = cd.client_department_id

    where reply_read <1;

    SELECT
    count(*)
    into 
    sis_count
    FROM 
    backend_notification_window_data bnwd
    left outer join clients c on (bnwd.client_id = c.client_id)
    left outer join client_departments cd on bnwd.client_department_id = cd.client_department_id
    where
    bnwd.notification_stop_date <= Curdate();

 select count(*)
 into did_count
  from backend_notifications bn
    inner join donor_test_info dti on dti.donor_test_info_id= bn.donor_test_info_id
    inner join donors d on d.donor_id = dti.donor_id
    left outer join clients c on (dti.client_id = c.client_id)
    left outer join client_departments cd on dti.client_department_id = cd.client_department_id
    left outer join users u on dti.collection_site_user_id = u.user_id
    left outer join donordocumenttotals ddt on dti.donor_id = ddt.donor_id
    left outer join backend_notification_window_data bnwd on bnwd.client_id = dti.client_id and bnwd.client_department_id = dti.client_department_id
  where
  (bn.notified_by_email < 1 or bn.notify_again >0)
  and Date(DATE_ADD(bn.notify_before_timestamp, interval -bnwd.deadline_alert_in_days day))<=Curdate()
  limit 100;


  set exception_total = clinic_exception_count + sms_count + sis_count + did_count;
  select exception_total as ExceptionCount,
  clinic_exception_count,
  sms_count,
  sis_count,
  did_count;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_data_list;
CREATE PROCEDURE surpathlive.`backend_get_notification_data_list`()
BEGIN

 select distinct
  -- *,
  cd.client_department_id,
  dti.client_id,
  d.donor_id,
  cd.department_name,
  c.client_name,
  cd.lab_code,
  tp.test_panel_id,
  tp.test_panel_name,
  d.donor_phone_1,
  d.donor_phone_2,
  d.donor_zip,
  d.donor_email,
  d.donor_first_name,
  d.donor_last_name,
  concat(d.donor_first_name, ' ', d.donor_last_name) as donor_full_name,
  tc.test_category_id,
  tc.test_category_name,
  dti.reason_for_test_id,
  dti.donor_test_info_id,
  dti.other_reason,
  dti.test_requested_date,
  bnwd.send_asap,
  bnwd.delay_in_hours,
  bnwd.deadline_alert_in_days,
  if (bnwd.force_manual is null, 1, bnwd.force_manual) as force_manual,
  backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now) as in_window,
  if (bn.backend_notifications_id is null, 0, bn.backend_notifications_id) as backend_notifications_id,
  if (bnwd.backend_notification_window_data_id is null, 0, bnwd.backend_notification_window_data_id) as backend_notification_window_data_id
  from donor_test_info dti
    inner join donor_test_info_test_categories dtitc on dti.donor_test_info_id = dtitc.donor_test_info_id
    inner join client_departments cd on cd.client_department_id = dti.client_department_id
    inner join clients c on c.client_id = dti.client_id
    inner join client_dept_test_categories cdtc on cd.client_department_id = cdtc.client_department_id
    left outer join test_categories tc on cdtc.test_category_id = tc.test_category_id
    inner join client_dept_test_panels cdtp on cdtp.client_dept_test_category_id = cdtc.client_dept_test_category_id
    inner join test_panels tp on tp.test_panel_id = cdtp.test_panel_id
    left outer join backend_notifications bn on bn.donor_test_info_id = dti.donor_test_info_id
    left outer join donors d on d.donor_id = dti.donor_id
    left outer join backend_notification_window_data bnwd on bnwd.client_id = dti.client_id and bnwd.client_department_id = dti.client_department_id
--   where 
--   -- test_status = 4 and
--   dti.donor_test_info_id = donor_test_info_id
--   order by dti.created_on desc
--   limit 1;
  where 
  -- test_status = 4 and
  bn.notified_by_email =0 
  and bn.notification_email_exception =0 and bn.clinic_exception = 0
  order by dti.created_on desc
  limit 5;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_exceptions;
CREATE PROCEDURE surpathlive.`backend_get_notification_exceptions`(
)
BEGIN

  select 
  dti.client_id,
  dti.client_department_id,
  bn.*
  ,backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now) as in_window
  from backend_notifications bn
    left outer join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
  where bn.clinic_exception > 0 or bn.notification_email_exception > 0 or bn.notification_sms_exception >0 
  limit 500;
  
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_exceptions_dataview;
CREATE PROCEDURE surpathlive.`backend_get_notification_exceptions_dataview`(
)
BEGIN

  select 
  d.donor_id AS DonorId,
  donorClearStarProfId AS DonorClearStarProfId,
  d.donor_first_name AS DonorFirstName,
  d.donor_last_name AS DonorLastName,
  d.donor_ssn AS DonorSSN,
  d.donor_date_of_birth AS DonorDateOfBirth,
  d.donor_initial_client_id AS DonorInitialClientId,
  d.donor_registration_status AS DonorRegistrationStatus,
  d.donor_city as DonorCity,
  d.donor_email as DonorEmail,
  d.donor_zip as DonorZipCode,
  dti.test_requested_date as DonorTestRegisteredDate,
  d.is_archived AS IsDonorArchived,
  dti.donor_test_info_id AS DonorTestInfoId,
  dti.client_id AS TestInfoClientId,
  dti.client_department_id AS TestInfoDepartmentId,
  dti.mro_type_id AS MROTypeId,
  dti.payment_type_id AS PaymentTypeId,
  dti.test_requested_date AS TestRequestedDate,
  dti.reason_for_test_id AS ReasonForTestId,
  dti.is_walkin_donor AS IsWalkinDonor,
  dti.instant_test_result AS InstantTestResult,
  dti.is_instant_test AS IsInstantTest,
  dti.total_payment_amount AS TotalPaymentAmount,
  dti.payment_method_id AS PaymentMethodId,
  dti.payment_received AS PaymentReceived,
  dti.payment_date AS PaymentDate,
  dti.test_status AS TestStatus,
  dti.test_overall_result AS TestOverallResult,
  bn.notified_by_email as Notified_by_email,
  if (bn.notified_by_email is null, 'Old Data',IF (bn.notified_by_email = 0, 'Not Notified',bn.notified_by_email_timestamp)) as Notified_by_email_timestamp,
  if (bn.backend_notifications_id is null, 0, bn.backend_notifications_id) as backend_notifications_id,
  CONCAT_WS(' ', u.user_first_name, u.user_last_name) AS CollectorName,
  (select GROUP_CONCAT(donor_test_info_test_categories.specimen_id SEPARATOR ', ') from donor_test_info_test_categories where donor_test_info_test_categories.donor_test_info_id = dti.donor_test_info_id) AS SpecimenId,
  dti.screening_time AS SpecimenDate,
  c.client_name AS ClientName,
  cd.department_name AS ClientDepartmentName,
  cd.clearstarcode As ClearStarCode,
  ddt.DocsTotal, 
  ddt.DocsNotApproved, 
  ddt.DocsRejected,
  if (backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now) =0,'Yes','No') as in_window,
   
  if (bn.clinic_radius = 0, '', CAST(bn.clinic_radius as CHAR(50))) as clinic_radius_text,
  bn.*

  from backend_notifications bn
    inner join donor_test_info dti on dti.donor_test_info_id= bn.donor_test_info_id
    inner join donors d on d.donor_id = dti.donor_id
    left outer join clients c on (dti.client_id = c.client_id)
    left outer join client_departments cd on dti.client_department_id = cd.client_department_id
    left outer join users u on dti.collection_site_user_id = u.user_id
    left outer join donordocumenttotals ddt on dti.donor_id = ddt.donor_id
  where bn.clinic_exception > 0 or bn.notification_email_exception > 0 or bn.notification_sms_exception >0 limit 100;


END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_information_for_donor_info_id;
CREATE PROCEDURE surpathlive.`backend_get_notification_information_for_donor_info_id`(
  in donor_test_info_id int
)
BEGIN

  select distinct
  -- *,
  cd.client_department_id,
  dti.client_id,
  d.donor_id,
  cd.department_name,
  c.client_name,
  cd.lab_code,
  tp.test_panel_id,
  tp.test_panel_name,
  d.donor_phone_1,
  d.donor_phone_2,
  d.donor_zip,
  d.donor_email,
  d.donor_first_name,
  d.donor_last_name,
  concat(d.donor_first_name, ' ', d.donor_last_name) as donor_full_name,
  tc.test_category_id,
  tc.test_category_name,
  dti.reason_for_test_id,
  dti.donor_test_info_id,
  dti.other_reason,
  dti.test_requested_date,
  if (bnwd.send_asap is null, 1,bnwd.send_asap) as send_asap,
  if (bnwd.delay_in_hours is null,0, bnwd.delay_in_hours) as delay_in_hours,
  if (bnwd.deadline_alert_in_days is null,1,bnwd.deadline_alert_in_days) as deadline_alert_in_days,
  if (bnwd.force_manual is null, 1, bnwd.force_manual) as force_manual,
  backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now) as in_window,
  if (bn.backend_notifications_id is null, 0, bn.backend_notifications_id) as backend_notifications_id,
  if (bnwd.backend_notification_window_data_id is null, 0, bnwd.backend_notification_window_data_id) as backend_notification_window_data_id
  from donor_test_info dti
    inner join donor_test_info_test_categories dtitc on dti.donor_test_info_id = dtitc.donor_test_info_id
    inner join client_departments cd on cd.client_department_id = dti.client_department_id
    inner join clients c on c.client_id = dti.client_id
    inner join client_dept_test_categories cdtc on cd.client_department_id = cdtc.client_department_id
    left outer join test_categories tc on cdtc.test_category_id = tc.test_category_id
    inner join client_dept_test_panels cdtp on cdtp.client_dept_test_category_id = cdtc.client_dept_test_category_id
    inner join test_panels tp on tp.test_panel_id = cdtp.test_panel_id
    left outer join backend_notifications bn on bn.donor_test_info_id = dti.donor_test_info_id
    left outer join donors d on d.donor_id = dti.donor_id
    left outer join backend_notification_window_data bnwd on bnwd.client_id = dti.client_id and bnwd.client_department_id = dti.client_department_id
  where 
  -- test_status = 4 and
  dti.donor_test_info_id = donor_test_info_id
  order by dti.created_on desc
  limit 1;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_window_data;
CREATE PROCEDURE surpathlive.`backend_get_notification_window_data`(
  IN client_id int(11),
  IN client_department_id int(11))
BEGIN
  select * 
  ,backend_in_notification_window(a.client_id, a.client_department_id,0 ) as in_window
  ,c.client_name
  ,cd.department_name
  from backend_notification_window_data a 
  left outer join clients c on c.client_id = a.client_id
  left outer join client_departments cd on cd.client_id = c.client_id
  where (a.client_id = client_id and a.client_department_id= client_department_id);
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_window_data_by_donor_phone;
CREATE PROCEDURE surpathlive.`backend_get_notification_window_data_by_donor_phone`(
  in phone_number varchar(50)
)
BEGIN

  declare client_id int(11) DEFAULT 0;
  declare client_department_id int(11) DEFAULT 0;
  
  select 
  dti.client_id,
  dti.client_department_id
  into
    client_id, client_department_id
  from
  donors d
  inner join donor_test_info dti on dti.donor_id = d.donor_id
  where
  (backend_only_digits(d.donor_phone_1) like CONCAT('%', backend_only_digits(phone_number), '%')
  or
  backend_only_digits(d.donor_phone_2) like CONCAT('%', backend_only_digits(phone_number), '%'))
  and length(backend_only_digits(phone_number))>9 
  order by dti.donor_test_info_id desc
  limit 1;
  
  if client_id>0 and client_department_id>0 then
    call backend_get_notification_window_data(client_id, client_department_id);
  end if;
  
  -- backend_get_response_to_sms_message('2148019441');
  
        --   DROP PROCEDURE IF EXISTS surpathlive.backend_get_response_to_sms_message;
        -- CREATE PROCEDURE surpathlive.`backend_get_response_to_sms_message`(
        --   in phone_number varchar(50)
        -- )
        -- BEGIN
        -- 
        --   select 
        -- --   *,
        --   dti.donor_test_info_id,
        --   dti.donor_id,
        --   dti.client_id,
        --   dti.client_department_id,
        --   scd.client_sms_apikey,
        --   scd.client_sms_token,
        --   scd.client_sms_from_id,
        --   bsa.reply
        --   from
        --   backend_notifications bn
        --   inner join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
        --   inner join donors d on d.donor_id = dti.donor_id
        --   left outer join backend_sms_autoresponses bsa on bsa.client_id = dti.client_id and bsa.client_department_id = dti.client_department_id
        --   left outer join backend_sms_client_data scd on scd.client_id = dti.client_id and scd.client_department_id = dti.client_department_id
        --   where
        --   bn.notified_by_sms = 1
        --   and 
        --   (backend_only_digits(d.donor_phone_1) like CONCAT('%', phone_number, '%')
        --   or
        --   backend_only_digits(d.donor_phone_2) like CONCAT('%', phone_number, '%'))
        --   and length(phone_number)>9  order by bn.notified_by_sms_timestamp desc
        --   limit 1;
        --   
        -- END;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_notification_window_data_by_id;
CREATE PROCEDURE surpathlive.`backend_get_notification_window_data_by_id`(
  IN backend_notification_window_data_id int(11)
  )
BEGIN
  select * 
  ,backend_in_notification_window(a.client_id, a.client_department_id,0 ) as in_window
  ,c.client_name
  ,cd.department_name
  from backend_notification_window_data a 
  left outer join clients c on c.client_id = a.client_id
  left outer join client_departments cd on cd.client_id = c.client_id
  where (a.backend_notification_window_data_id = backend_notification_window_data_id);
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_ready_donor_notifications;
CREATE PROCEDURE surpathlive.`backend_get_ready_donor_notifications`(

)
BEGIN

  select c.client_id, d.client_department_id, bn.*
  ,backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now ) as in_window
  from backend_notifications bn
    left outer join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
    left outer join clients c on c.client_id = dti.client_id
    left outer join client_departments d on d.client_department_id = dti.client_department_id
    
  where 
    ( bn.notified_by_email < 1 or bn.notify_again > 0)
    and
    c.client_id is not null
    and d.client_department_id is not null
    and bn.donor_test_info_id is not null
  ;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_Send_in_Schedule_Exceptions_dataview;
CREATE PROCEDURE surpathlive.`backend_get_Send_in_Schedule_Exceptions_dataview`(
)
BEGIN

  SELECT
  date(notification_sweep_date) as notification_sweep_date_date,
  date(notification_start_date) as notification_start_date_date,
  date(notification_stop_date) as notification_stop_date_date,
  
  bnwd.*,
  c.client_name AS ClientName,
  cd.department_name AS ClientDepartmentName
  FROM 
  backend_notification_window_data bnwd
      left outer join clients c on (bnwd.client_id = c.client_id)
      left outer join client_departments cd on bnwd.client_department_id = cd.client_department_id
  where
    bnwd.notification_stop_date <= Curdate();
  
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_sent_sms;
CREATE PROCEDURE surpathlive.`backend_get_sent_sms`()
BEGIN

  
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;
  
  select * from backend_sms_queue where sent = 1;
  COMMIT;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_sms_activity;
CREATE PROCEDURE surpathlive.`backend_get_sms_activity`(
  `donor_test_info_id` int(11)
)
BEGIN

  select * from backend_sms_activity bsa
  where bsa.donor_test_info_id =donor_test_info_id
  order by bsa.backend_sms_activity_id desc;

  -- update backend_sms_activity set reply_read =0;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_sms_activity_unread;
CREATE PROCEDURE surpathlive.`backend_get_sms_activity_unread`(
)
BEGIN

  select * 
  from backend_sms_activity
  where reply_read < 0
  order by donor_test_info_id, backend_sms_activity_id desc;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_sms_activity_unread_dataview;
CREATE PROCEDURE surpathlive.`backend_get_sms_activity_unread_dataview`(
)
BEGIN
  select 
    d.donor_id AS DonorId,
    d.donor_first_name AS DonorFirstName,
    d.donor_last_name AS DonorLastName,
    d.donor_ssn AS DonorSSN,
    d.donor_date_of_birth AS DonorDateOfBirth,
    d.donor_initial_client_id AS DonorInitialClientId,
    d.donor_registration_status AS DonorRegistrationStatus,
    d.donor_city as DonorCity,
    d.donor_email as DonorEmail,
    d.donor_zip as DonorZipCode,
    dti.test_requested_date as DonorTestRegisteredDate,
    d.is_archived AS IsDonorArchived,
    dti.donor_test_info_id AS DonorTestInfoId,
    dti.client_id AS TestInfoClientId,
    dti.client_department_id AS TestInfoDepartmentId,
    dti.mro_type_id AS MROTypeId,
    dti.payment_type_id AS PaymentTypeId,
    dti.test_requested_date AS TestRequestedDate,
    dti.total_payment_amount AS TotalPaymentAmount,
    dti.payment_method_id AS PaymentMethodId,
    dti.payment_received AS PaymentReceived,
    c.client_id,
    cd.client_department_id,
    c.client_name as ClientName,
    cd.department_name as ClientDepartmentName,
    if (bn.notified_by_email>0,bn.notified_by_email_timestamp,"No Notified") as DateDonorNotified,
    ba.*
    ,bn.*
  from backend_sms_activity ba
    inner join donor_test_info dti on dti.donor_test_info_id = ba.donor_test_info_id
    inner join donors d on d.donor_id = dti.donor_id
    left outer join clients c on c.client_id = dti.client_id
    left outer join client_departments cd on dti.client_department_id = cd.client_department_id
    left outer join backend_notifications bn on ba.donor_test_info_id = bn.donor_test_info_id
  where reply_read <1
  order by ba.donor_test_info_id, ba.backend_sms_activity_id desc;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_sms_autoresponse;
CREATE PROCEDURE surpathlive.`backend_get_sms_autoresponse`(
  in client_id int,
  IN client_department_id int(11)
)
BEGIN

  select * from backend_sms_autoresponses where client_id = client_id and client_department_id = client_department_id;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_sms_replies;
CREATE PROCEDURE surpathlive.`backend_get_sms_replies`(
  in donor_test_info_id int
)
BEGIN

  select * from backend_sms_replies where donor_test_info_id = donor_test_info_id;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_unread_sms_replies;
CREATE PROCEDURE surpathlive.`backend_get_unread_sms_replies`(
  in donor_test_info_id int
)
BEGIN

  select * from backend_sms_replies 
  where is_read < 1;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_unsent_sms;
CREATE PROCEDURE surpathlive.`backend_get_unsent_sms`()
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
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_log_sms_reply;
CREATE PROCEDURE surpathlive.`backend_log_sms_reply`(
  in donor_test_info_id int,
  in reply varchar(150),
  in created_by varchar(128),
  in last_modified_by varchar(128),  
  out backend_sms_replies_id int 
)
BEGIN
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

  INSERT INTO surpathlive.backend_sms_replies 
  (donor_test_info_id, reply, created_by, last_modified_by, dt_reply_received, created_on, last_modified_on ) 
  VALUES (donor_test_info_id, reply, created_by, last_modified_by, CURRENT_TIMESTAMP(),CURRENT_TIMESTAMP(),CURRENT_TIMESTAMP() );

  set backend_sms_replies_id := last_insert_id();
  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_queue_donor_notification;
CREATE PROCEDURE surpathlive.`backend_queue_donor_notification`(
  in donor_test_info_id int,
  out backend_notifications_id int,
  in created_by varchar(128),
  in last_modified_by varchar(128)
)
BEGIN

 declare test_id int(11) DEFAULT 0;
    declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

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
  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_queue_sms;
CREATE PROCEDURE surpathlive.`backend_queue_sms`(
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
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

  INSERT INTO surpathlive.backend_sms_queue
  (`to`, `text`, donor_id, client_id, client_department_id, created_by, last_modified_by, dt_entered) 
  VALUES (sendTo, textToSend, donor_id, client_id, client_department_id, created_by, last_modified_by, CURRENT_TIMESTAMP  );


  set backend_sms_queue_id := last_insert_id();
  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_remove_notification_window_data;
CREATE PROCEDURE surpathlive.`backend_remove_notification_window_data`(
  IN client_id int,
  IN client_department_id int)
BEGIN
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

  delete from backend_notification_window_data where client_id = client_id and client_department_id = client_department_id;

  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_remove_sms_autoresponse;
CREATE PROCEDURE surpathlive.`backend_remove_sms_autoresponse`(
  in client_id int,
  IN client_department_id int
)
BEGIN
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

  delete from backend_sms_autoresponses where backend_sms_autoresponses.client_id = client_id and backend_sms_autoresponses.client_department_id = client_department_id;
  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_remove_sms_client_data;
CREATE PROCEDURE surpathlive.`backend_remove_sms_client_data`(
  in client_id int,
  IN client_department_id int
)
BEGIN
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

  delete from backend_sms_client_data where backend_sms_client_data.client_id = client_id and backend_sms_client_data.client_department_id = client_department_id; 

  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_sent_sms;
CREATE PROCEDURE surpathlive.`backend_sent_sms`(
  in param_backend_sms_queue_id int
)
BEGIN
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;


  UPDATE backend_sms 
  SET sent = 1, dt_sent = CURRENT_TIMESTAMP 
  WHERE backend_sms_queue_id = param_backend_sms_queue_id;

  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_donor_notification;
CREATE PROCEDURE surpathlive.`backend_set_donor_notification`(
  in donor_test_info_id int,
  in created_by varchar(128),
  in last_modified_by varchar(128),
  in notified_by_email int,
  in notified_by_sms int,
  in notification_email_exception int,
  in clinic_exception int,
  in clinic_radius int,
  in notify_now int,
  in notify_next_window int,
  in is_archived int,
  in notification_sms_exception int,
  in notification_sent_to_email varchar(255),
  in notification_sent_to_phone varchar(50),
  in notify_after_timestamp datetime,
  in notify_before_timestamp datetime,
  in notify_reset_sendin int,
  in notify_again int
--   in max_sendins int,
  
  -- ,out backend_notifications_id int
)
BEGIN

 declare test_id int(11) DEFAULT 0;
  
--   declare exit handler for SQLEXCEPTION, SQLWARNING
--   BEGIN
--   ROLLBACK;
--   END;
-- 
--   start transaction;


  select b.backend_notifications_id into test_id from backend_notifications as b 
  where b.donor_test_info_id =donor_test_info_id 
  order by b.backend_notifications_id desc 
  limit 1;
--   set backend_notifications_id := test_id;
 
    select 
    wd.notification_sweep_date,
    wd.notification_start_date,
    wd.notification_start_date,
    wd.notification_stop_date,
    GetSendInDate(wd.client_id,wd.client_department_id, wd.max_sendins, UNIX_TIMESTAMP(wd.notification_start_date),UNIX_TIMESTAMP(wd.notification_stop_date),
    wd.sunday,
    wd.monday,
    wd.tuesday,
    wd.wednesday,
    wd.thursday,
    wd.friday,
    wd.saturday) 
    as SendInDate
    into
    @sweep, @start, @datestart, @stop, @SendInDate
    from donor_test_info dti
    left outer join backend_notification_window_data wd on wd.client_id = dti.client_id and wd.client_department_id = dti.client_department_id
    where dti.donor_test_info_id = donor_test_info_id;

  
    
  IF (test_id =0)
  THEN
    -- Assign a date for this donor
    -- notify before = end of window
    -- notify after will be the date selected to notify, the send in date
    
    -- for the donor test info id, get client and dept, then get window for that dept
    -- then generate a random date that is between the start and end
    -- where the day of the week is enabled
    
    -- get seconds start and seconds end
    -- generate a random number between the two
    -- convert to a date, get the day of the week
    -- if that day of week isn't enabled, try again
    if (notify_after_timestamp is null) then
      set notify_after_timestamp = FROM_UNIXTIME(@SendInDate);
    end if;
    if (notify_before_timestamp is null) then
      set notify_before_timestamp = @stop;
    end if;

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
    clinic_radius,
    clinic_exception_timestamp,
    created_on, 
    created_by, 
    last_modified_on, 
    last_modified_by,
    is_archived,
    notify_now,
    notify_again,
    notify_next_window,
    notification_sms_exception,
    notification_sent_to_email,
    notification_sent_to_phone,
    notify_sms_exception_timestamp,
    notify_after_timestamp,
    notify_before_timestamp
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
    clinic_radius,
    if (clinic_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
    CURRENT_TIMESTAMP, 
    created_by, 
    CURRENT_TIMESTAMP, 
    last_modified_by,
    if (is_archived>0,1,0),
    if (notify_now>0,1,0),
    if (notify_again>0,1,0),
    if (notify_next_window>0,1,0),
    notification_sms_exception,
    notification_sent_to_email,
    notification_sent_to_phone,
    if (notification_sms_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
    notify_after_timestamp,
    notify_before_timestamp
    );

    set test_id := last_insert_id();
    

    

  ELSE
  
    if notify_reset_sendin>0 then
      set notify_after_timestamp = FROM_UNIXTIME(@SendInDate);
      set notify_before_timestamp = @stop;
    end if;
  
    update backend_notifications bn
    set 
      bn.notified_by_email = notified_by_email, 
      bn.notified_by_email_timestamp = if (notified_by_email>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'), 
      bn.notified_by_sms = notified_by_sms, 
      bn.notified_by_sms_timestamp = if (notified_by_sms>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      bn.notification_email_exception = notification_email_exception, 
      bn.notify_email_exception_timestamp = if (notification_email_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      bn.clinic_exception = clinic_exception,
      bn.clinic_radius = clinic_radius,
      bn.clinic_exception_timestamp = if (clinic_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      bn.last_modified_by = last_modified_by, 
      bn.last_modified_on = CURRENT_TIMESTAMP,
      bn.is_archived = if (is_archived>0,1,0),
      bn.notify_now= if (notify_now>0,1,0),
      bn.notify_again = if (notify_again>0,1,0),
      bn.notify_next_window= if (notify_next_window>0,1,0),
      bn.notification_sms_exception = notification_sms_exception,
      bn.notify_sms_exception_timestamp =if (notification_sms_exception>0,CURRENT_TIMESTAMP, '0000-00-00 00:00:00'),
      bn.notification_sent_to_email = notification_sent_to_email,
      bn.notification_sent_to_phone = notification_sent_to_phone,
      bn.notify_after_timestamp = if (notify_after_timestamp is not null, notify_after_timestamp,null),
      bn.notify_before_timestamp = if (notify_before_timestamp is not null, notify_before_timestamp,null)
    where backend_notifications_id = test_id;
  
  END IF;

  select  c.client_id, d.client_department_id, bn.*
   ,backend_in_notification_window(dti.client_id, dti.client_department_id, bn.notify_now) as in_window
  from backend_notifications bn
      left outer join donor_test_info dti on dti.donor_test_info_id = bn.donor_test_info_id
    left outer join clients c on c.client_id = dti.client_id
    left outer join client_departments d on d.client_department_id = dti.client_department_id
  
--   from backend_notifications as b 
  where bn.donor_test_info_id =donor_test_info_id 
  and bn.backend_notifications_id = test_id;

--   commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_donor_test_activity;
CREATE PROCEDURE surpathlive.`backend_set_donor_test_activity`(
in donor_test_info_id int,
in activity_user_id int, 
in activity_category_id int, 
in is_activity_visible int, 
in activity_note varchar(6000), 
in is_synchronized int,
out donor_test_activity_id int
)
begin
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;


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
  commit;
end;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_notification_window_data;
CREATE PROCEDURE surpathlive.`backend_set_notification_window_data`(
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
  IN deadline_alert_in_days int(11),
  IN override_day_schedule tinyint,
  in max_sendins int(11),
  in created_by varchar(128),
  in last_modified_by varchar(128),
  in notification_start_date timestamp,
  in notification_stop_date timestamp,
  in notification_sweep_date timestamp,
  
  in client_sms_token varchar(255),
  in client_sms_apikey varchar(255),
  in client_autoresponse varchar(150),
  in client_sms_from_number varchar(150),
  
  in show_web_notify_button tinyint,
  out backend_notification_window_data_id int(11)
)
BEGIN

  -- if there's a client record, find it
  declare test_id int(11) DEFAULT 0;
--   set @notification_start_date = ifnull(notification_start_date, date('1970-01-01'));
--   set @notification_stop_date = ifnull(notification_stop_date, date('1970-01-01'));
--   set @notification_sweep_date = ifnull(notification_sweep_date, date('1970-01-01'));
--   declare exit handler for SQLEXCEPTION, SQLWARNING
--   BEGIN
--     ROLLBACK;
--   END;
  
--   start transaction;

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
     deadline_alert_in_days,
     override_day_schedule,
     last_modified_by,
     notification_start_date,
     notification_stop_date,
     notification_sweep_date,
     client_sms_token,
     client_sms_apikey,
     client_autoresponse,
     client_sms_from_number,
     show_web_notify_button,
     max_sendins
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
      deadline_alert_in_days,
      override_day_schedule,
      last_modified_by,
      notification_start_date,
      notification_stop_date,
      notification_sweep_date,
      client_sms_token,
      client_sms_apikey,
      client_autoresponse,
      client_sms_from_number,
      show_web_notify_button,
      max_sendins
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
     a.deadline_alert_in_days = deadline_alert_in_days,
     a.override_day_schedule = override_day_schedule,
     a.last_modified_by = last_modified_by,
     a.pdf_template_filename = pdf_template_filename,
     a.pdf_render_settings_filename = pdf_render_settings_filename,
     a.enable_sms = enable_sms,
     a.force_manual = force_manual,
     a.onsite_testing = onsite_testing,
     a.notification_start_date = if (notification_start_date is null,a.notification_start_date, notification_start_date),
     a.notification_stop_date = if(notification_stop_date is null ,a.notification_stop_date, notification_stop_date),
     a.notification_sweep_date = if (notification_sweep_date is null ,a.notification_sweep_date, notification_sweep_date),
      a.client_sms_token = client_sms_token,
      a.client_sms_apikey = client_sms_apikey,
      a.client_autoresponse = client_autoresponse,
      a.client_sms_from_number= client_sms_from_number,
      a.show_web_notify_button = show_web_notify_button,
      a.max_sendins = max_sendins
    WHERE a.client_id = client_id and a.client_department_id = client_department_id;

    
    set backend_notification_window_data_id =  test_id;

  END IF;


--   commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_sms;
CREATE PROCEDURE surpathlive.`backend_set_sms`(
  `backend_sms_activity_id` int(11),
  `donor_test_info_id` int(11),
  `sent_text` varchar(150),
  `dt_sms_sent` DATETIME,
  `reply_text` varchar(150),
  `dt_reply_received` DATETIME,
  `created_by` varchar(200),
  `last_modified_by` varchar(200),
  `reply_read` int(11),
  `user_id` int(11),
  `reply_read_timestamp` datetime
)
BEGIN

  declare test_id int(11) DEFAULT 0;

  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

  select b.backend_sms_activity_id 
  into test_id
  from backend_sms_activity as b 
  where b.backend_sms_activity_id =backend_sms_activity_id;
  
  if (test_id =0)
  then
      -- insert
      INSERT INTO surpathlive.backend_sms_activity
      (
        donor_test_info_id, 
        sent_text, 
        dt_sms_sent, 
        reply_text, 
        dt_reply_received, 
        created_on, 
        created_by, 
        last_modified_on, 
        last_modified_by, 
        reply_read, 
        user_id, 
        reply_read_timestamp
      ) 
      VALUES 
      (
        donor_test_info_id, 
        sent_text, 
        dt_sms_sent, 
        reply_text, 
        dt_reply_received, 
        CURRENT_TIMESTAMP, 
        created_by, 
        last_modified_on, 
        last_modified_by, 
        reply_read, 
        user_id, 
        reply_read_timestamp
      );


    set test_id := last_insert_id();
  else
  -- update
  
      UPDATE surpathlive.backend_sms_activity 
        SET 
          donor_test_info_id = donor_test_info_id, 
          sent_text = sent_text, 
          dt_sms_sent = dt_sms_sent, 
          reply_text = reply_text, 
          dt_reply_received = dt_reply_received, 
          last_modified_on = last_modified_on, 
          last_modified_by = CURRENT_TIMESTAMP, 
          reply_read = reply_read, 
          user_id = user_id, 
          reply_read_timestamp = reply_read_timestamp
    WHERE backend_sms_activity_id = test_id;

  end if;

  select * from backend_sms_activity
  where b.backend_sms_activity_id =test_id;


  commit;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_sms_activity;
CREATE PROCEDURE surpathlive.`backend_set_sms_activity`(
  `backend_sms_activity_id` int(11),
  `donor_test_info_id` int(11),
  `sent_text` varchar(150),
  `dt_sms_sent` DATETIME,
  `auto_reply_text`  varchar(150),
  `reply_text` varchar(150),
  `dt_reply_received` DATETIME,
  `created_by` varchar(200),
  `last_modified_by` varchar(200),
  `reply_read` int(11),
  `user_id` int(11),
  `reply_read_timestamp` datetime
)
BEGIN

  declare test_id int(11) DEFAULT 0;

--   declare exit handler for SQLEXCEPTION, SQLWARNING
--   BEGIN
--     ROLLBACK;
--   END;
--   
--   start transaction;

  select b.backend_sms_activity_id 
  into test_id
  from backend_sms_activity as b 
  where b.backend_sms_activity_id =backend_sms_activity_id;
  
  if (test_id =0)
  then
      -- insert
      INSERT INTO surpathlive.backend_sms_activity
      (
        donor_test_info_id, 
        sent_text, 
        dt_sms_sent, 
        reply_text, 
        auto_reply_text,
        dt_reply_received, 
        created_on, 
        created_by, 
        last_modified_on, 
        last_modified_by, 
        reply_read, 
        user_id, 
        reply_read_timestamp
      ) 
      VALUES 
      (
        donor_test_info_id, 
        sent_text, 
        dt_sms_sent, 
        reply_text, 
        auto_reply_text,
        dt_reply_received, 
        CURRENT_TIMESTAMP, 
        created_by, 
        last_modified_on, 
        last_modified_by, 
        reply_read, 
        user_id, 
        reply_read_timestamp
      );


    set test_id := last_insert_id();
  else
  -- update
  
      UPDATE surpathlive.backend_sms_activity ba
        SET 
          ba.donor_test_info_id = donor_test_info_id, 
          ba.sent_text = sent_text, 
          ba.dt_sms_sent = dt_sms_sent, 
          ba.reply_text = reply_text, 
          ba.auto_reply_text = auto_reply_text,
          ba.dt_reply_received = dt_reply_received, 
          ba.last_modified_on = last_modified_on, 
          ba.last_modified_by = CURRENT_TIMESTAMP, 
          ba.reply_read = reply_read, 
          ba.user_id = user_id, 
          ba.reply_read_timestamp = reply_read_timestamp
    WHERE ba.backend_sms_activity_id = test_id;

  end if;

  select * from backend_sms_activity ba
  where ba.backend_sms_activity_id =test_id;


--   commit;

END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_sms_autoresponse;
CREATE PROCEDURE surpathlive.`backend_set_sms_autoresponse`(
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
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;

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
  commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_sms_client_data;
CREATE PROCEDURE surpathlive.`backend_set_sms_client_data`(
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
--   declare exit handler for SQLEXCEPTION, SQLWARNING
--   BEGIN
--     ROLLBACK;
--   END;
--   
--   start transaction;




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
--   commit;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_sms_sent;
CREATE PROCEDURE surpathlive.`backend_set_sms_sent`(
  in donor_test_info_id int
)
BEGIN

  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;
--   UPDATE backend_sms_queue 
--   SET sent = 1
--   WHERE backend_sms_queue_id = backend_sms_queue_id;

  update backend_notifications
  set notified_by_sms = 1, notified_by_sms_timestamp = CURRENT_TIMESTAMP()
  where donor_test_info_id = donor_test_info_id;

  commit;

END;

DROP PROCEDURE IF EXISTS surpathlive.CreateRegistrationRequest;
CREATE PROCEDURE surpathlive.`CreateRegistrationRequest`(OUT registration_edit_requests_id char(36))
BEGIN

  set @last_uuid = uuid();

  INSERT INTO surpathlive.registration_edit_requests
  (registration_edit_requests_id) 
  VALUES (@last_uuid);

  set registration_edit_requests_id =  @last_uuid;
END;

DROP PROCEDURE IF EXISTS surpathlive.CreateUniqueRequest;
CREATE PROCEDURE surpathlive.`CreateUniqueRequest`(OUT registration_edit_requests_id char(36))
BEGIN

  set @last_uuid = uuid();

  INSERT INTO surpathlive.registration_edit_requests
  (registration_edit_requests_id) 
  VALUES (@last_uuid);

  set registration_edit_requests_id =  @last_uuid;
END;

DROP PROCEDURE IF EXISTS surpathlive.Find_X_Clinics_For_Donor;
CREATE PROCEDURE surpathlive.`Find_X_Clinics_For_Donor`(IN donor_id int, in min_count int, in start_miles int, in max_miles int)
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
  
  select 1 into @loop;

  select
    donors.donor_zip, zip_codes.latitude, zip_codes.longitude
    into @donor_zip,@DLAT, @DLONG 
    from donors 
      left outer join zip_codes on donors.donor_zip = zip_codes.zip
    where
      donors.donor_id = donor_id;
  
  findLoop: WHILE @loop = 1 DO -- WHILE @select_count < @min_count OR @dist <= @max_miles DO
--     select "looping";
    
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
--     select @min_count, @select_count < @min_count as calc, @dist as finding_dist, @max_miles, @select_count as found_so_far;  
--     select "testing";

    if @dist > @max_miles then
--        select "out of range";
      set @loop = 0;
    elseif @select_count >= @min_count then
--       select "found enough";
      set @loop =0;
    else
--       select "increasing radius";
      select @dist + @dist_add into @dist;
    end if;
   
  
    
    
--     if @select_count <= @min_count then
--       select 'hit min';
--     end if;
  
  END WHILE findLoop;

  if @dist > @max_miles then
--     select "setting dist to -1";
    set @dist = -1;
  end if;


    select 
      vendors.vendor_name, vendor_addresses.*, dt.d2c as d2c
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
  
END;

DROP PROCEDURE IF EXISTS surpathlive.Get_Clinics_For_Donor;
CREATE PROCEDURE surpathlive.`Get_Clinics_For_Donor`(IN donor_id int, in _dist int)
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
  
  call backend_Get_Clinics_In_Range_Of_Zip(@donor_zip,@dist);
  
  
END;

DROP PROCEDURE IF EXISTS surpathlive.PurgeUser;
CREATE PROCEDURE surpathlive.`PurgeUser`(IN email varchar(1024))
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

END;

DROP PROCEDURE IF EXISTS surpathlive.scratch;
CREATE PROCEDURE surpathlive.`scratch`( client_id int, client_department_id int, max_sendins int, utstart INT, utend INT, sunday INT, monday INT, tuesday INT, wednesday INT, thursday INT, friday INT, saturday INT )
BEGIN

  -- day of week: (1 for Sunday,2 for Monday ?? 7 for Saturday )

   -- make sure we have a day to find
   set @bail = sunday + monday + tuesday + wednesday + thursday + friday + saturday;
   if utstart is null then 
    select "null start"; -- return 0;
   end if;
    if utend is null then
    select "null end"; -- return 0;
   end if;  
   if @bail < 1 then
    select "no days"; -- return utend;
   end if;
   
   -- if utstart is in the past, set utstart = today
   if UNIX_TIMESTAMP(date(CURRENT_DATE())) > utstart then
     set utstart = UNIX_TIMESTAMP(date(CURRENT_DATE()));
   end if;
   
   -- Bail conditions
   -- if none of the days between the start and end are enabled, bail
     
  set @datestart =  Date(FROM_UNIXTIME(utstart));
  set @dateend = DATE(FROM_UNIXTIME(utend));

   if datediff(@dateend, @datestart)<1 then
    select "datediff", utend; -- return utend;
   end if;
  
  set @su = 0, @mo=0, @tu=0, @we=0, @th=0, @fr=0, @sa=0;

  finddays: WHILE datediff(@dateend, @datestart)>-1 DO
      set @dow = DAYOFWEEK(@datestart);
      if @dow = 1 AND sunday > 0 then
        set @su = 1;
      end if;
      if (@dow = 2 and monday>0) then
        set @mo = 1;
      end if;
      if @dow = 3 AND tuesday > 0 then
        set @tu = 1;
      end if;
      if @dow = 4 AND wednesday > 0 then
        set @we = 1;
      end if;
      if @dow = 5 AND thursday > 0 then
        set @th = 1;
      end if;
      if @dow = 6 AND friday > 0 then
        set @fr = 1;
      end if;
      if @dow = 7 AND saturday > 0 then
        set @sa = 1;
      end if;  
      set @datestart = DATE_ADD(@datestart, INTERVAL 1 DAY);
  END WHILE finddays;
  set @bail = @su + @mo + @tu + @we + @th + @fr + @sa;
  if @bail < 1 then
    select utend; -- return utend;
  end if;
   
   set @tries = 20; -- try failsafe - try 20 times and give up
   
   set @found = 0;
   
   
  label1: LOOP  
  
   set @tries = @tries-1;
  
    set @uttest = FLOOR(RAND()*(utend-utstart+1)+utstart);
     
    SET @DTTEST = FROM_UNIXTIME(@UTTEST);
    SET @DOW = DAYOFWEEK(@DTTEST);


    
    if @dow = 1 AND sunday > 0 then
      set @found = 1;
    end if;
    if (@dow = 2 and monday>0) then
      set @found = 1;
    end if;
    if @dow = 3 AND tuesday > 0 then
      set @found = 1;
    end if;
    if @dow = 4 AND wednesday > 0 then
      set @found = 1;
    end if;
    if @dow = 5 AND thursday > 0 then
      set @found = 1;
    end if;
    if @dow = 6 AND friday > 0 then
      set @found = 1;
    end if;
    if @dow = 7 AND saturday > 0 then
      set @found = 1;
    end if;    
     IF @found =0 THEN
       if (@tries<0) then
        set @uttest = utend;
        leave label1;
       end if;
       ITERATE label1;
     END IF;
     
--      -- check count for client_id and client_department_id
     select 
    date(b.notify_after_timestamp) as a, date(FROM_UNIXTIME(@uttest)) as b, count(*)
    from
    backend_notifications b
    inner join donor_test_info d on b.donor_test_info_id = d.donor_test_info_id
    where d.client_id = 110 and d.client_department_id = 372
    and date(b.notify_after_timestamp) = date(FROM_UNIXTIME(@uttest));
    
    if (
    select 
    count(*)
    from
    backend_notifications b
    inner join donor_test_info d on b.donor_test_info_id = d.donor_test_info_id
    where d.client_id = 110 and d.client_department_id = 372
    and date(b.notify_after_timestamp) = date(FROM_UNIXTIME(@uttest))
    )>= max_sendins then
      select "hit max";
      ITERATE label1;
    end if;
     
     LEAVE label1;
   END LOOP label1;
   
   select "fin",@uttest;
   
--  scratch(110,372,1,1578018954,1578105354,0,1,1,1,1,1,0);
END;

