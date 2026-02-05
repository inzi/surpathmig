-- IntegrationTest1 110, 478
-- IntegrationTest2 110, 479


DROP TABLE IF EXISTS `surpathlive`.`backend_integration_partners`;
CREATE TABLE `backend_integration_partners` (
  `backend_integration_partner_id` int(11) NOT NULL AUTO_INCREMENT,
  `partner_name` varchar(150) DEFAULT NULL,
  `partner_key` varchar(150) DEFAULT NULL,
  `partner_crypto` varchar(150) DEFAULT NULL,
  `backend_integration_partners_pidtype` int(11) NOT NULL,
  `html_instructions` longblob,
  `login_url` varchar(200) DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `active` tinyint(4) NOT NULL DEFAULT 1,
  PRIMARY KEY (`backend_integration_partner_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `surpathlive`.`backend_integration_partner_client_map`;

CREATE TABLE `backend_integration_partner_client_map` (
  `backend_integration_partner_client_map_id` int(11) NOT NULL AUTO_INCREMENT,
  `backend_integration_partner_client_map_GUID` char(36) NOT NULL DEFAULT '',
  `backend_integration_partner_id` int(11) NOT NULL,
  `client_id` int(11) NOT NULL,
  `client_department_id` int(11) NOT NULL,
  `partner_client_id` varchar(150) DEFAULT NULL,
  `partner_client_code` varchar(150) DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `require_login` tinyint(4) NOT NULL DEFAULT 0,
  `require_remote_login` tinyint(4) NOT NULL DEFAULT 0,
  `active` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`backend_integration_partner_client_map_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_client_map_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;



DROP TABLE IF EXISTS `surpathlive`.`backend_integration_partner_donor_documents`;
-- CREATE TABLE `backend_integration_partner_donor_documents` (
--   `backend_integration_partner_donor_documents_id` int(11) NOT NULL AUTO_INCREMENT,
--   `donor_document_id` int(11) NOT NULL,
--   `donor_document_released` tinyint(4) NOT NULL DEFAULT 0,
--   `donor_document_transmitted` tinyint(4) NOT NULL DEFAULT 0,
--   `donor_document_transmitted_on` tinyint(4) NOT NULL DEFAULT 0,
--   `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
--   `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
--   `last_modified_by` varchar(200) NOT NULL,
--   `released_by` varchar(200) NOT NULL,
--   PRIMARY KEY (`backend_integration_partner_donor_documents_id`),
--   UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_donor_documents_id`)
-- ) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `surpathlive`.`backend_integration_partner_release`;
CREATE TABLE `backend_integration_partner_release` (
  `backend_integration_partner_release_id` int(11) NOT NULL AUTO_INCREMENT,
  `backend_integration_partner_release_GUID` char(36) NOT NULL DEFAULT '',
  `donor_test_info_id` int(11) NOT NULL,
  `donor_test_info_id_released` tinyint(4) NOT NULL DEFAULT 0,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `released_by` varchar(200) NOT NULL,
  PRIMARY KEY (`backend_integration_partner_release_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_release_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- populate integration partners

-- PC: 242575a2-d320-11eb-b1e0-7c4c58ced981
-- crypto 9edef9be-4ec6-ad3f-75e9-45d031d51741

-- MCE: 44af9269-d320-11eb-b1e0-7c4c58ced981
-- crypto: fb566ea8-1740-823b-4566-d44f8f7b513e

TRUNCATE TABLE backend_integration_partners;

INSERT
INTO surpathlive.backend_integration_partners(
        partner_name,
        partner_key,
        partner_crypto,
        backend_integration_partners_pidtype,
        html_instructions,
        login_url,
        last_modified_by,
        active)
VALUES ('Project Concert',
        '242575a2-d320-11eb-b1e0-7c4c58ced981',
        '9edef9be-4ec6-ad3f-75e9-45d031d51741',
        10,
        'PC instructions',
        'login url',
        'SYSTEM',
        1);

INSERT
INTO surpathlive.backend_integration_partners(
        partner_name,
        partner_key,
        partner_crypto,
        backend_integration_partners_pidtype,
        html_instructions,
        login_url,
        last_modified_by,
        active)
VALUES ('My Clinical Exchange',
        '44af9269-d320-11eb-b1e0-7c4c58ced981',
        'fb566ea8-1740-823b-4566-d44f8f7b513e',
        9,
        'MCE Instructions',
        'login url',
        'SYSTEM',
        1);



--- Procedures


DROP PROCEDURE IF EXISTS surpathlive.backend_get_partner_key;

CREATE PROCEDURE surpathlive.`backend_get_partner_key`(
   IN  partner_key                       varchar(128),
   OUT backend_integration_partners_id   int)
BEGIN
   SELECT p.backend_integration_partners_id
   INTO backend_integration_partners_id
   FROM backend_integration_partners p
   WHERE p.partner_key = partner_key;
END;


DROP PROCEDURE IF EXISTS surpathlive.backend_get_integration_partner_by_key;

CREATE PROCEDURE surpathlive.`backend_get_integration_partner_by_key`(
   IN partner_key   varchar(128))
BEGIN
   SELECT p.*
   FROM backend_integration_partners p
   WHERE p.partner_key = partner_key;
END;



DROP PROCEDURE IF EXISTS surpathlive.backend_set_integration_partner_by_key;

CREATE PROCEDURE surpathlive.`backend_set_integration_partner_by_key`(
   IN  partner_name                           varchar(128),
   IN  partner_key                            varchar(128),
   IN  partner_crypto                         varchar(128),
   IN  html_instructions                       longblob,
   IN  login_url                               varchar(200),
   IN  backend_integration_partners_pidtype   int,
   IN  last_modified_by                       varchar(128),
   IN  active                                 int,
   OUT backend_integration_partner_id         int)
BEGIN
   DECLARE test_id   int(11) DEFAULT 0;

   SELECT backend_integration_partner_client_map.backend_integration_partner_client_map_id
   INTO test_id
   FROM backend_integration_partner_client_map
   WHERE     backend_integration_partner_client_map.client_id = client_id
         AND backend_integration_partner_client_map.client_department_id =
                client_department_id
   LIMIT 1;


   IF (test_id > 0)
   THEN
      UPDATE surpathlive.backend_integration_partners p
      SET p.partner_name = partner_name,
          p.partner_key = partner_key,
          p.partner_crypto = partner_crypto,
          p.backend_integration_partners_pidtype =
             backend_integration_partners_pidtype,
          p.last_modified_on = CURRENT_TIMESTAMP(),
          p.last_modified_by = last_modified_by,
          p.login_url = login_url,
          p.html_instructions = html_instructions,
          p.active = IF(active > 0, 1, 0)
      WHERE p.partner_key = partner_key;


      SET backend_integration_partner_id = test_id;
   ELSE
      INSERT
      INTO surpathlive.backend_integration_partners(
              partner_name,
              partner_key,
              partner_crypto,
              backend_integration_partners_pidtype,
              html_instructions,
              login_url,
              created_on,
              last_modified_by,
              active)
      VALUES (partner_name,
              partner_key,
              partner_crypto,
              backend_integration_partners_pidtype,
              html_instructions,
              login_url,
              CURRENT_TIMESTAMP(),
              last_modified_by,
              IF(active > 0, 1, 0));


      SET backend_integration_partner_id = LAST_INSERT_ID();
   END IF;
END;


DROP PROCEDURE IF EXISTS surpathlive.backend_get_integration_clients_by_key;

CREATE PROCEDURE surpathlive.`backend_get_integration_clients_by_key`(
   IN partner_key   varchar(128))
BEGIN
   SELECT *
   FROM backend_integration_partner_client_map    pc
        INNER JOIN backend_integration_partners p
           ON p.backend_integration_partner_id =
                 pc.backend_integration_partner_id
        INNER JOIN clients c ON c.client_id = pc.client_id
        INNER JOIN client_departments cd
           ON cd.client_department_id = pc.client_department_id
   WHERE p.partner_key = partner_key;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_integration_clients_by_partner_client_id;

CREATE PROCEDURE surpathlive.`backend_get_integration_clients_by_partner_client_id`(
   IN partner_client_id   varchar(128))
BEGIN
   SELECT *
   FROM backend_integration_partner_client_map    pc
        INNER JOIN backend_integration_partners p
           ON p.backend_integration_partner_id =
                 pc.backend_integration_partner_id
        INNER JOIN clients c ON c.client_id = pc.client_id
        INNER JOIN client_departments cd
           ON cd.client_department_id = pc.client_department_id
   WHERE pc.partner_client_id = partner_client_id;
END;



DROP PROCEDURE IF EXISTS surpathlive.backend_get_integration_clients_by_client_and_dept_id;

CREATE PROCEDURE surpathlive.`backend_get_integration_clients_by_client_and_dept_id`(
   IN client_id              int,
   IN client_department_id   int)
BEGIN
   SELECT pc.*
   FROM backend_integration_partner_client_map    pc
        INNER JOIN backend_integration_partners p
           ON p.backend_integration_partner_id =
                 pc.backend_integration_partner_id
        INNER JOIN clients c ON c.client_id = pc.client_id
        INNER JOIN client_departments cd
           ON cd.client_department_id = pc.client_department_id 
    WHERE pc.client_id = client_id AND pc.client_department_id = client_department_id;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_integration_partner_client;

CREATE PROCEDURE surpathlive.`backend_set_integration_partner_client`(
   IN  partner_key                                 varchar(128),
   IN  guid                                        char(36),
   IN  client_id                                   int,
   IN  client_department_id                        int,
   IN  partner_client_id                           varchar(150),
   IN  partner_client_code                         varchar(150),
   IN  backend_integration_partner_id              int,
   IN  last_modified_by                            varchar(128),
   IN  require_login                               int,
   IN  require_remote_login                        int,
   IN  active                                      int,
   OUT backend_integration_partner_client_map_id   int)
BEGIN
   -- DECLARE ClientID   int(11);                                   -- DEFAULT 0;
   DECLARE test_id    int(11) DEFAULT 0;
   -- SET ClientID = client_id;

   SELECT bipc.backend_integration_partner_client_map_id
   INTO test_id
   FROM backend_integration_partner_client_map    bipc
--         INNER JOIN backend_integration_partners p
--            ON p.backend_integration_partner_id =
--                  bipc.backend_integration_partner_id
   WHERE bipc.backend_integration_partner_client_map_GUID = guid
   --   bipc.client_id = ClientID and
   --   bipc.client_department_id =client_department_id and
   --   (p.partner_key = partner_key or p.backend_integration_partner_id = backend_integration_partner_id)
   LIMIT 1;


   IF (test_id > 0)
   THEN
      UPDATE surpathlive.backend_integration_partner_client_map  pcm
             INNER JOIN backend_integration_partners p
                ON p.backend_integration_partner_id =
                      pcm.backend_integration_partner_id
      SET pcm.backend_integration_partner_id =
             backend_integration_partner_id,
          pcm.partner_client_code = partner_client_code,
          pcm.active = IF(active > 0, 1, 0),
          pcm.require_login = IF(require_login>0,1,0),
          pcm.require_remote_login = IF(require_remote_login>0,1,0),
          pcm.last_modified_by = last_modified_by
      WHERE pcm.backend_integration_partner_client_map_GUID = Guid;

      --     pcm.client_id = client_id and
      --     pcm.client_department_id = client_department_id and
      --     (p.partner_key = partner_key or p.backend_integration_partner_id = backend_integration_partner_id);

      SET backend_integration_partner_client_map_id = test_id;
   ELSE
      INSERT
      INTO surpathlive.backend_integration_partner_client_map(
              backend_integration_partner_client_map_GUID,
              client_id,
              client_department_id,
              backend_integration_partner_id,
              partner_client_code,
              partner_client_id,              
              last_modified_by,
              require_login,
              require_remote_login,
              active)
      VALUES (uuid(),
              client_id,
              client_department_id,
              backend_integration_partner_id,
              partner_client_code,
              partner_client_id,              
              last_modified_by,
              IF(require_login>0,1,0),
              IF(require_remote_login>0,1,0),
              IF(active > 0, 1, 0));

      SET backend_integration_partner_client_map_id = LAST_INSERT_ID();
   END IF;
END;


DROP PROCEDURE IF EXISTS surpathlive.backend_get_integration_partners;

CREATE PROCEDURE surpathlive.`backend_get_integration_partners` ()
BEGIN
   --   select
   --   p.*
   --
   --   from backend_integration_partners p;
   SELECT backend_integration_partner_id,
          partner_name,
          partner_key,
          partner_crypto,
          backend_integration_partners_pidtype,
          html_instructions,
          login_url,
          created_on,
          last_modified_on,
          last_modified_by,
          active
   FROM surpathlive.backend_integration_partners;
END;


DROP PROCEDURE IF EXISTS surpathlive.backend_set_integration_partners;

CREATE PROCEDURE surpathlive.`backend_set_integration_partners`(
   INOUT backend_integration_partner_id       int,
   IN  partner_name                           varchar(150),
   IN  partner_key                            varchar(150),
   IN  partner_crypto                         varchar(150),
   IN  backend_integration_partners_pidtype   int(11),
   IN  html_instructions                      longblob,
   IN  login_url                              varchar(200),
   IN  last_modified_by                       varchar(200),
   IN  active                                 tinyint(4)-- out backend_integration_partner_id int(11)
   )
BEGIN
  
   Declare newPidTypeID int default 0;
   
   IF (backend_integration_partner_id > 0)
   THEN
      UPDATE backend_integration_partners bp
      SET bp.partner_name = partner_name,
          bp.partner_key = partner_key,
          bp.partner_crypto = partner_crypto,
          bp.backend_integration_partners_pidtype =
             backend_integration_partners_pidtype,
          bp.html_instructions = html_instructions,
          bp.login_url = login_url,
          bp.last_modified_by = last_modified_by,
          bp.last_modified_on = CURRENT_TIMESTAMP(),
          bp.active = If(active > 0, 1, 0)
      WHERE bp.backend_integration_partner_id =
               backend_integration_partner_id;
   ELSE
   
      INSERT
      INTO surpathlive.backend_integration_partners(
              partner_name,
              partner_key,
              partner_crypto,
              backend_integration_partners_pidtype,
              html_instructions,
              login_url,
              last_modified_by,
              active)
      VALUES (partner_name,
              partner_key,
              partner_crypto,
              backend_integration_partners_pidtype,
              html_instructions,
              login_url,
              last_modified_by,
              If(active > 0, 1, 0));

      SET backend_integration_partner_id = LAST_INSERT_ID();
   END IF;
END;



DROP PROCEDURE IF EXISTS surpathlive.backend_get_partner_donors;
CREATE PROCEDURE surpathlive.`backend_get_partner_donors`(
  in fromDateTime timestamp,
  in toDateTime timestamp,
  in partner_client_id nvarchar(255),
  in partner_client_code nvarchar(255),
  in partner_donor_id nvarchar(255),
  in maxResults int
)
BEGIN

-- set maxResults = if(maxResults< 1, 18446744073709551615);
declare thislimit int DEFAULT 0;
set thislimit = IF(maxResults>0, maxResults, 5000);


select
ip.*,
dti.test_requested_date,
d.created_on,
d.*
from
donors d
inner join individual_pids ip on ip.donor_id = d.donor_id
inner join donor_test_info dti on dti.donor_id = d.donor_id
inner join backend_integration_partner_client_map pcm on pcm.client_id = dti.client_id and pcm.client_department_id = dti.client_department_id
inner join backend_integration_partners ips on ips.backend_integration_partner_id = pcm.backend_integration_partner_id
where 
( 
ip.pid_type_id = 2
AND
(
  dti.test_requested_date > @vatsaDATETIME OR @vatsaDATETIME is null
)
AND
(
  dti.test_requested_date < @vatsaDATETIME2 OR @vatsaDATETIME2 is null
)
AND
(
  pcm.partner_client_id = partner_client_id or partner_client_id is null
)
AND
(
  pcm.partner_client_code = partner_client_code or partner_client_code is null
)
AND
(
 ip.pid = partner_donor_id or partner_donor_id is null
)
)
limit 0, thislimit; -- 132075

END;



DROP PROCEDURE IF EXISTS SURPATHLIVE.BACKEND_GET_PARTNER_DONORS_AND_DOCUMENTS;
CREATE PROCEDURE SURPATHLIVE.`BACKEND_GET_PARTNER_DONORS_AND_DOCUMENTS`(
  IN backend_integration_partner_id int,
  IN FROMDATETIME TIMESTAMP,
  IN TODATETIME TIMESTAMP,
  IN PARTNER_CLIENT_ID NVARCHAR(255),
  IN PARTNER_CLIENT_CODE NVARCHAR(255),
  IN PARTNER_DONOR_ID NVARCHAR(255),
  IN MAXRESULTS INT
)
BEGIN

-- SET @FROMDATETIME = NULL;
-- SET @TODATETIME = NULL;
-- SET @partner_donor_id = NULL;
-- SET @client_id = NULL;
-- SET @partner_client_id = '';
-- SET @partner_client_code = '';


-- SET MAXRESULTS = IF(MAXRESULTS< 1, 18446744073709551615);
DECLARE THISLIMIT INT DEFAULT 0;
SET THISLIMIT = IF(MAXRESULTS>0, MAXRESULTS, 5000);


-- SELECT ip.*,
--        dti.test_requested_date--        d.created_on,
--                               --        d.*
--                               -- dd.*,
--        ,
--        r.*,
--        bipr.*
-- FROM donors    d
--      INNER JOIN individual_pids ip ON ip.donor_id = d.donor_id
--      INNER JOIN donor_test_info dti ON dti.donor_id = d.donor_id
--      INNER JOIN backend_integration_partner_client_map pcm
--         ON     pcm.client_id = dti.client_id
--            AND pcm.client_department_id = dti.client_department_id
--      INNER JOIN backend_integration_partners ips
--         ON ips.backend_integration_partner_id =
--               pcm.backend_integration_partner_id
--      INNER JOIN report_info r
--         ON r.donor_test_info_id = dti.donor_test_info_id
--      LEFT OUTER JOIN backend_integration_partner_release bipr
--         ON bipr.donor_test_info_id = dti.donor_test_info_id
--      LEFT OUTER JOIN client_departments cd 
--         ON cd.client_department_id = dti.client_department_id
Select        
 ip.donor_id,
  ip.pid,
  d.donorClearstarProfId,
  dti.donor_test_info_id,
  cd.ClearStarCode,
  TC.test_category_id,
  dti.created_on as dti_created_on,
  dti.test_status,
  dti.test_requested_date,
  r.created_on as doc_created_on,
  r.inserted_on as doc_received_on,
  r.report_type,
  r.report_info_id,
  r.lab_report,
  IFNULL(r.lab_report_source_filename,'na') as lab_report_source_filename
FROM donors    d
     INNER JOIN individual_pids ip ON ip.donor_id = d.donor_id
     INNER JOIN donor_test_info dti ON dti.donor_id = d.donor_id
     INNER JOIN backend_integration_partner_client_map pcm
        ON     pcm.client_id = dti.client_id
           AND pcm.client_department_id = dti.client_department_id
     INNER JOIN backend_integration_partners ips
        ON ips.backend_integration_partner_id =
              pcm.backend_integration_partner_id
     INNER JOIN report_info r
        ON r.donor_test_info_id = dti.donor_test_info_id
     LEFT OUTER JOIN backend_integration_partner_release bipr
        ON bipr.donor_test_info_id = dti.donor_test_info_id
     LEFT OUTER JOIN CLIENT_DEPARTMENTS CD 
        ON CD.CLIENT_DEPARTMENT_ID = DTI.CLIENT_DEPARTMENT_ID
     LEFT OUTER JOIN donor_test_info_test_categories dtitc
        ON dtitc.donor_test_info_id = dti.donor_test_info_id
     LEFT OUTER JOIN TEST_CATEGORIES TC 
        ON TC.TEST_CATEGORY_ID = dtitc.test_category_id        
        
WHERE     
(                                                   -- Only complete
           dti.test_status = 7)
         AND                                     -- if positive we have a release
         (   (-- 2=negative
              dti.test_overall_result = 2
              and
              ( r.report_type = 1 -- lab report
              OR
              r.report_info_id is null
              )
              )
          OR (                            -- is positive and we have a release
              dti .test_overall_result = 1                     -- 1 = positive
              and ( r.report_type = 2 or r.report_info_id is null) -- MRO report
              AND bipr.backend_integration_partner_release_id IS NOT NULL
              AND bipr.donor_test_info_id_released > 0))
      AND 
      (-- report info for neg is not archive
        r.is_archived <1
      )
      AND (                                        -- This integration partner
           ips.backend_integration_partner_id =
              backend_integration_partner_id)
      AND (                            -- donor has a pid type for the partner
           ip  .pid_type_id = ips.backend_integration_partners_pidtype
           AND (                               -- and it matches (if provided)
                ip.pid = PARTNER_DONOR_ID OR PARTNER_DONOR_ID IS NULL OR PARTNER_DONOR_ID = ''))
      AND (                               -- the test date is within the range
           dti.test_requested_date > FROMDATETIME
           OR FROMDATETIME IS NULL)
      AND (                               -- the test date is within the range
           dti.test_requested_date < TODATETIME
           OR TODATETIME IS NULL)
      AND (  -- This matches their client ID if provided or all clients (null)
           pcm.partner_client_id = PARTNER_CLIENT_ID
           OR PARTNER_CLIENT_ID IS NULL
           OR PARTNER_CLIENT_ID = '')
      AND ( -- This matches their client code if provided or all client codes (null)
           pcm.partner_client_code = PARTNER_CLIENT_CODE
           OR PARTNER_CLIENT_CODE IS NULL
           OR PARTNER_CLIENT_CODE = '')
LIMIT 0, THISLIMIT; -- 132075

END;

-- IntegrationTest1 110, 478
-- IntegrationTest2 110, 479

truncate table backend_integration_partner_client_map;


INSERT INTO surpathlive.backend_integration_partner_client_map
(
backend_integration_partner_id, 
client_id, 
client_department_id, 
backend_integration_partner_client_map_GUID,
partner_client_id, 
partner_client_code, 
last_modified_by, 
require_login,
require_remote_login,
active) 
VALUES 
(1, 110, 478, UUID(), 'pcclientid1', 'pcclientcode1', 'test', 0,1,1);

INSERT INTO surpathlive.backend_integration_partner_client_map
(
backend_integration_partner_id, 
client_id, 
client_department_id, 
backend_integration_partner_client_map_GUID,
partner_client_id, 
partner_client_code, 
last_modified_by, 
require_login,
require_remote_login,
active) 
VALUES 
(2, 110, 479, UUID(), 'mceclientid2', 'mceclientcode2', 'test', 1,0,1);


-- INSERT INTO surpathlive.backend_integration_partner_client_map
-- (
-- backend_integration_partner_id, 
-- client_id, 
-- client_department_id, 
-- backend_integration_partner_client_map_GUID,
-- partner_client_id,   
-- partner_client_code, 
-- last_modified_by, 
-- require_login,
-- active) 
-- VALUES 
-- (1, 1, 1, UUID(), 'pcclientid2', 'pcclientcode2', 'pctest', 0,1);


select c.client_name, c.client_code, cd.department_name from backend_integration_partner_client_map b
inner join clients c on c.client_id = b.client_id
inner join client_departments cd on cd.client_department_id = b.client_department_id;
-- select * from individual_pids where donor_id in (86081,85718);

set @PID = 'aa84hbabfoh';
delete from individual_pids where pid = @PID;
INSERT INTO surpathlive.individual_pids
(donor_id, pid, pid_type_id, individual_pid_type_description, mask_pid) 
VALUES (86081, @PID, 9, 'MCE', 0);

set @PID = 'alfjn48shv48';
delete from individual_pids where pid = @PID;
INSERT INTO surpathlive.individual_pids
(donor_id, pid, pid_type_id, individual_pid_type_description, mask_pid) 
VALUES (85718, @PID, 9, 'MCE', 0);


-- 2863090 -- type 1 lab
-- 3683804 -- type 2 mro
-- 102972	85718	1	1
-- truncate table backend_integration_partner_release;
-- INSERT INTO surpathlive.backend_integration_partner_release
-- (
-- backend_integration_partner_release_GUID, donor_test_info_id, donor_test_info_id_released, last_modified_by, released_by
-- ) 
-- VALUES 
-- (
-- UUID(),102972, 1, 'test', 'test'
-- );

-- 
-- select * from donor_test_info 
-- where test_overall_result in (1,2)
-- order by client_id, client_department_id, donor_test_info_id desc limit 50;
-- 
-- select * from donor_test_info 
-- where test_overall_result in (1,2) and test_status =7
-- order by donor_test_info_id desc limit 150;
-- 
-- 
-- 
-- select * from backend_integration_partner_client_map;
-- 
-- 
-- select * from backend_integration_partners;
-- 
-- select * from donor_documents order by donor_document_id desc limit 50;
-- 
-- select * from backend_integration_partner_donor_documents;
-- 
-- 
-- select * from donor_test_info dti
-- inner join donor_documents dd on dti.donor_id = dd.donor_id
-- order by dti.donor_test_info_id desc limit 50;
-- 
-- -- DTI    DiD   C   CD
-- -- 113717	95187	112	384
-- 
-- 
-- -- neg
-- -- 103437	86081	1	1
-- -- pos 
-- -- 102972	85718	1	1
-- 
-- -- 1	Project Concert	pckey	pccrypto	10	4/21/2021 6:19:55 PM		SYSTEM	1
-- -- 2	My Clinical Exchange	mcekey	mcecrypto	9	4/21/2021 6:19:55 PM		SYSTEM	1
-- 
-- -- INSERT INTO surpathlive.backend_integration_partner_client_map
-- -- (backend_integration_partner_id, client_id, client_department_id, partner_client_id, partner_client_code, created_on, last_modified_on, last_modified_by, active) 
-- -- VALUES (backend_integration_partner_id, client_id, client_department_id, 'partner_client_id', 'partner_client_code', 'created_on', 'last_modified_on', 'last_modified_by', active);
-- 
-- -- select * from donor_test_info 
-- -- where donor_id  in (86081,85718);
-- select * from donor_documents where donor_document_id = 8570;
-- 
-- 
-- select * from donor_test_info 
-- 
-- limit 50;
-- 
-- 
-- select 
-- d.donor_first_name, 
-- d.donor_last_name, 
-- d.donorClearstarProfId,
-- -- d.ClearStarCode,
-- dti.client_id, 
-- dti.client_department_id, 
-- dtitc.*,
-- dti.*
-- from donor_test_info_test_categories dtitc
-- inner join donor_test_info dti on dti.donor_test_info_id = dtitc.donor_test_info_id
-- inner join donors d on d.donor_id = dti.donor_id
-- where dtitc.test_category_id=4 
-- and d.donorClearstarProfId = '2020080642927207'
-- order by dtitc.donor_test_info_id desc;