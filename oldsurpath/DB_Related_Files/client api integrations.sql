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
  `partner_push` tinyint(4) NOT NULL DEFAULT 0,
  `partner_push_host` varchar(150) DEFAULT NULL,
  `partner_push_username` varchar(150) DEFAULT NULL,
  `partner_push_password` varchar(150) DEFAULT NULL,
  `partner_push_port` varchar(150) DEFAULT NULL,
  `partner_push_path` varchar(150) DEFAULT NULL,

  PRIMARY KEY (`backend_integration_partner_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;


-- populate integration partners
TRUNCATE TABLE backend_integration_partners;

-- PC: 242575a2-d320-11eb-b1e0-7c4c58ced981
-- crypto 9edef9be-4ec6-ad3f-75e9-45d031d51741

-- MCE: 44af9269-d320-11eb-b1e0-7c4c58ced981
-- crypto: fb566ea8-1740-823b-4566-d44f8f7b513e

INSERT
INTO surpathlive.backend_integration_partners(
        partner_name,
        partner_key,
        partner_crypto,
        backend_integration_partners_pidtype,
        html_instructions,
        login_url,
        last_modified_by,
        active,
        partner_push,
        partner_push_host    ,
        partner_push_username,
        partner_push_password,
        partner_push_port    ,
        partner_push_path           
        )
VALUES ('Project Concert',
        '242575a2-d320-11eb-b1e0-7c4c58ced981',
        '9edef9be-4ec6-ad3f-75e9-45d031d51741',
        10,
        'PC instructions',
        'https://projectconcert.com/',
        'SYSTEM',
        1,
        1,
        'ext.projectconcert.com',
        'surscan_test',
        '9ub2Vxvmd3FEd68s',
        '22',
        'inbound');

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
        'https://myclinicalexchange.com/StudentLogin.aspx',
        'SYSTEM',
        1);




DROP TABLE IF EXISTS `surpathlive`.`backend_integration_partner_client_map`;
CREATE TABLE `backend_integration_partner_client_map` (
  `backend_integration_partner_client_map_id` int(11) NOT NULL AUTO_INCREMENT,
  `backend_integration_partner_client_map_GUID` char(36) NOT NULL DEFAULT '',
  `backend_integration_partner_id` int(11) NOT NULL,
  `client_id` int(11) NOT NULL,
  `client_department_id` int(11) NOT NULL,
  `partner_client_id` varchar(150) DEFAULT NULL,
  `partner_client_code` varchar(150) DEFAULT NULL,
  `partner_push_folder` varchar(150) DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_modified_on` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `require_login` tinyint(4) NOT NULL DEFAULT 0,
  `require_remote_login` tinyint(4) NOT NULL DEFAULT 0,
  `active` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`backend_integration_partner_client_map_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_client_map_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;



-- DROP TABLE IF EXISTS `surpathlive`.`backend_integration_partner_donor_documents`;
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
  `backend_integration_partner_release_GUID` varchar(150) NOT NULL DEFAULT '',
  `donor_test_info_id` int(11) NOT NULL,
  `report_info_id` int(11) NOT NULL DEFAULT '0',
  `donor_test_test_category_id` int(11) NOT NULL DEFAULT '0',
  `released` tinyint(4) NOT NULL DEFAULT '0',
  `sent` tinyint(4) NOT NULL DEFAULT '0',
  `donor_document_id` int(11) NOT NULL DEFAULT '0',
  `background_check` int(11) NOT NULL DEFAULT '0',
  -- `donor_test_info_id_released` tinyint(4) NOT NULL DEFAULT '0',
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_modified_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `last_modified_by` varchar(200) NOT NULL,
  `released_by` varchar(200) NOT NULL,
  `sent_on` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`backend_integration_partner_release_id`),
  UNIQUE KEY `id_UNIQUE` (`backend_integration_partner_release_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


truncate table backend_integration_partner_client_map;

INSERT INTO surpathlive.backend_integration_partner_client_map
(
backend_integration_partner_id, 
client_id, 
client_department_id, 
backend_integration_partner_client_map_GUID,
partner_client_id, 
partner_client_code, 
partner_push_folder,
last_modified_by, 
require_login,
require_remote_login,
active) 
VALUES 
(1, 119, 0, UUID(), 'pcclientid1', '115', 'inbound', 'test', 0,1,1);


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
(2, 110, 478, UUID(), 'mceclientid2', 'mceclientcode2',  'test', 1,0,1);


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

DROP PROCEDURE IF EXISTS surpathlive.backend_get_integration_partner_by_partner_client_code;

CREATE PROCEDURE surpathlive.`backend_get_integration_partner_by_partner_client_code`(
   IN partner_client_code   varchar(128))
BEGIN
   SELECT p.*
   FROM backend_integration_partners p
   inner join backend_integration_partner_client_map bipm on p.backend_integration_partner_id = bipm.backend_integration_partner_id
   WHERE bipm.partner_client_code = partner_client_code;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_set_integration_partner_by_key;

CREATE PROCEDURE surpathlive.`backend_set_integration_partner_by_key`(
   IN  partner_name                           varchar(128),
   IN  partner_key                            varchar(128),
   IN  partner_crypto                         varchar(128),
   IN  html_instructions                       longblob,
   IN  login_url                               varchar(200),
   IN partner_push                            int,
   IN partner_push_host                       varchar(150),
   IN partner_push_username                    varchar(150),
   IN partner_push_password                     varchar(150),
   IN partner_push_port                            varchar(150),
   IN partner_push_path                         varchar(150),
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
          p.active = IF(active > 0, 1, 0),
          p.partner_push = IF(partner_push>0,1,0),
          p.partner_push_host    = partner_push_host    ,
          p.partner_push_username = partner_push_username,
          p.partner_push_password = partner_push_password,
          p.partner_push_port =partner_push_port    ,
          p.partner_push_path = partner_push_path
          
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
              partner_push,
              partner_push_host    ,
              partner_push_username,
              partner_push_password,
              partner_push_port    ,
              partner_push_path    ,
              login_url,
              created_on,
              last_modified_by,
              active)
      VALUES (partner_name,
              partner_key,
              partner_crypto,
              backend_integration_partners_pidtype,
              html_instructions,
              IF(partner_push>0,1,0),
              partner_push_host    ,
              partner_push_username,
              partner_push_password,
              partner_push_port    ,
              partner_push_path    ,              
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
   SELECT pc.*
   FROM backend_integration_partner_client_map    pc
        INNER JOIN backend_integration_partners p
           ON p.backend_integration_partner_id =
                 pc.backend_integration_partner_id
        INNER JOIN clients c ON c.client_id = pc.client_id
--         INNER JOIN client_departments cd
--            ON cd.client_department_id = pc.client_department_id
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
        left outer JOIN client_departments cd
           ON cd.client_department_id = pc.client_department_id
   WHERE     pc.client_id = client_id
         AND (pc.client_department_id = client_department_id OR client_department_id=0);
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
   IN   partner_push_folder                        varchar(200),
   IN  require_login                               int,
   IN  require_remote_login                        int,
   IN  active                                      int,
   OUT backend_integration_partner_client_map_id   int)
BEGIN
   -- DECLARE ClientID   int(11);                                   -- DEFAULT 0;
   DECLARE test_id   int(11) DEFAULT 0;

   -- SET ClientID = client_id;

   SELECT bipc.backend_integration_partner_client_map_id
   INTO test_id
   FROM backend_integration_partner_client_map bipc
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
          pcm.partner_push_folder = partner_push_folder,
          pcm.require_login = IF(require_login > 0, 1, 0),
          pcm.require_remote_login = IF(require_remote_login > 0, 1, 0),
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
              partner_push_folder,
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
              partner_push_folder,
              last_modified_by,
              IF(require_login > 0, 1, 0),
              IF(require_remote_login > 0, 1, 0),
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
   SELECT *
--    backend_integration_partner_id,
--           partner_name,
--           partner_key,
--           partner_crypto,
--           backend_integration_partners_pidtype,
--           html_instructions,
--           login_url,
--           created_on,
--           last_modified_on,
--           last_modified_by,
--           active
   FROM surpathlive.backend_integration_partners;
END;

DROP PROCEDURE IF EXISTS surpathlive.backend_get_partner_release;

CREATE PROCEDURE surpathlive.`backend_get_partner_release` (
  IN partner_key                                    varchar(200),
  IN  partner_client_code                           varchar(200),
  IN  released_only                                      int,
  IN  sent                                           int
)
BEGIN

    -- SELECT * FROM table WHERE TheNameOfTimestampColumn > '2009-01-28 21:00:00'
    -- SELECT * FROM table WHERE TheNameOfTimestampColumn > DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 1 DAY)

   SELECT bipr.*, dti.donor_id, dti.client_id, dti.client_department_id
   FROM surpathlive.backend_integration_partner_release bipr
    inner join donor_test_info dti on bipr.donor_test_info_id = dti.donor_test_info_id
    inner join backend_integration_partner_client_map bipm on dti.client_id = bipm.client_id AND dti.client_department_id = bipm.client_department_id
    inner join backend_integration_partners bip on bip.backend_integration_partner_id = bipm.backend_integration_partner_id
    WHERE
      bip.partner_key = partner_key
      
      AND bipm.partner_client_code = partner_client_code
      
      AND
      ( 
        (
          released_only > 0 
          AND 
          bipr.released >0
        )
        OR
        (
          released_only < 1
        )
      )
            AND
      ( 
        (
          sent < 1 
          AND 
          bipr.sent < 1
        )
        OR
        (
          sent > 0
        )
      )
      ;
END;


DROP PROCEDURE IF EXISTS surpathlive.backend_set_partner_release;

CREATE PROCEDURE surpathlive.`backend_set_partner_release` (
  IN backend_integration_partner_release_id         int,
  IN released              tinyint(4),
  IN sent                   tinyint(4),
  IN last_modified_by       varchar(200),
  IN released_by                                varchar(200)
)
BEGIN
  UPDATE surpathlive.backend_integration_partner_release
SET
  released = IF(released>0,1,0) -- tinyint(4)
  ,sent = IF(sent>0,1,0) -- tinyint(4)
  ,last_modified_by = last_modified_by -- varchar(200)
  ,released_by = released_by -- varchar(200)
  ,sent_on = CURRENT_TIMESTAMP -- timestamp
WHERE backend_integration_partner_release_id = backend_integration_partner_release_id; -- int(11);

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
   IN  partner_push                           tinyint(4),
 IN partner_push_host                       varchar(150),
   IN partner_push_username                    varchar(150),
   IN partner_push_password                     varchar(150),
   IN partner_push_port                            varchar(150),
   IN partner_push_path                         varchar(150),   
   IN  last_modified_by                       varchar(200),
   IN  active                                 tinyint(4) -- out backend_integration_partner_id int(11)
                                                        )
BEGIN
   DECLARE newPidTypeID   int DEFAULT 0;

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
          bp.partner_push = IF(partner_push>0,1,0),
          bp.partner_push_host = partner_push_host,
          bp.partner_push_username = partner_push_username,
          bp.partner_push_password = partner_push_password,
          bp.partner_push_port =partner_push_port    ,
          bp.partner_push_path = partner_push_path,          
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
              partner_push,
              partner_push_host    ,
              partner_push_username,
              partner_push_password,
              partner_push_port    ,
              partner_push_path    ,              
              last_modified_by,
              active)
      VALUES (partner_name,
              partner_key,
              partner_crypto,
              backend_integration_partners_pidtype,
              html_instructions,
              login_url,
              IF(partner_push>0,1,0),
              partner_push_host    ,
              partner_push_username,
              partner_push_password,
              partner_push_port    ,
              partner_push_path    ,                    
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
  IFNULL(r.lab_report_source_filename,'na') as lab_report_source_filename,
  IFNULL(r.final_report_id,0) as final_report_id,
  dd.donor_document_id,
  dd.document_upload_time,
  dd.document_content,
  dd.document_title,
  dd.donor_document_typeId
FROM donors d
     INNER JOIN individual_pids ip ON ip.donor_id = d.donor_id
     INNER JOIN donor_test_info dti ON dti.donor_id = d.donor_id
     INNER JOIN backend_integration_partner_client_map pcm
        ON     pcm.client_id = dti.client_id
           AND pcm.client_department_id = dti.client_department_id
     INNER JOIN backend_integration_partners ips
        ON ips.backend_integration_partner_id =
              pcm.backend_integration_partner_id
     LEFT OUTER JOIN backend_integration_partner_release bipr
        ON bipr.donor_test_info_id = dti.donor_test_info_id
     LEFT OUTER JOIN CLIENT_DEPARTMENTS CD 
        ON CD.CLIENT_DEPARTMENT_ID = DTI.CLIENT_DEPARTMENT_ID
     LEFT OUTER JOIN donor_test_info_test_categories dtitc
        ON dtitc.donor_test_info_id = dti.donor_test_info_id
     LEFT OUTER JOIN TEST_CATEGORIES TC 
        ON TC.TEST_CATEGORY_ID = dtitc.test_category_id        
     INNER JOIN report_info r
        ON r.donor_test_info_id = dti.donor_test_info_id AND r.report_info_id = bipr.report_info_id   
     INNER JOIN parser_file_activity pfa
        ON pfa.report_info_id = r.report_info_id
     LEFT OUTER JOIN donor_documents dd
        ON dd.donor_document_id = r.final_report_id        
WHERE     
(                                                   -- Only complete
           dti.test_status = 7)
           AND
           (
            pfa.is_data_of_record = 1   -- is the current document of record for this test
           )
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
              dti.test_overall_result = 1                     -- 1 = positive
              and ( r.report_type = 2 or r.report_info_id is null) -- MRO report
              AND bipr.backend_integration_partner_release_id IS NOT NULL
              AND bipr.released > 0)
          -- Background checks are elsewhere - need to add to api for retreival
          )
      AND 
      (-- report info for neg is not archive
        r.is_archived <1
      )
      AND (                                        -- This integration partner
           ips.backend_integration_partner_id =
              backend_integration_partner_id)
      AND (                            -- donor has a pid type for the partner
           ip.pid_type_id = ips.backend_integration_partners_pidtype
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