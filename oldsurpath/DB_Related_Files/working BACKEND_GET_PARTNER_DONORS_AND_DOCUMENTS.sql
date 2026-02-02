-- DTI    DiD   C   CD
-- 113717	95187	112	384

-- mysql wants: '2012-12-23 18:00:00'
-- SFUSfn298fn

SET @partner_donor_id = 'SFUSfn298fn';
-- '645-74-7490';
SET @FROMDATETIME = str_to_date('2020-08-03 13:49:23', '%Y-%m-%d %H:%I:%s');
-- '8/4/2020 1:49:23 PM'; -- '2020-08-04 13:49:23'
SET @TODATETIME = str_to_date('2020-04-05 13:50:00', '%Y-%m-%d %H:%I:%s');
-- '8/5/2020 1:49:23 PM';
-- SET @client_id = 112;
-- set @PARTNER_CLIENT_ID = 'mceclientid';
-- set @PARTNER_CLIENT_CODE = '';

SET @FROMDATETIME = NULL;
SET @TODATETIME = NULL;
SET @partner_donor_id = NULL;
-- SET @client_id = NULL;
SET @PARTNER_CLIENT_ID = '';
SET @PARTNER_CLIENT_CODE = '';


-- SET @PIDTYPE = 9;
SET @backend_integration_partner_id = 2;

SELECT                                                                -- ip.*,
      d.donor_id,
       ip.pid,
       dti.test_requested_date                         --        d.created_on,
                                                                 --        d.*
                                                                      -- dd.*,
       ,
       dti.created_on AS dti_created_on,
       dti.test_status,
       dtitc.test_category_id,
       d.donorClearstarProfId,
       cd.clearstarcode,
       r.created_on   AS doc_created_on,
       r.*,
       bipr.*
FROM donors    d
     INNER JOIN individual_pids ip ON ip.donor_id = d.donor_id
     INNER JOIN donor_test_info dti ON dti.donor_id = d.donor_id
     INNER JOIN backend_integration_partner_client_map pcm
        ON     pcm.client_id = dti.client_id
           AND pcm.client_department_id = dti.client_department_id
     INNER JOIN backend_integration_partners ips
        ON ips.backend_integration_partner_id =
              pcm.backend_integration_partner_id
     INNER JOIN donor_test_info_test_categories dtitc
        ON dtitc.donor_test_info_id = dti.donor_test_info_id
     inner join client_departments cd 
        on cd.client_department_id = dti.client_department_id and cd.client_id = dti.client_id
     LEFT OUTER JOIN report_info r
        ON r.donor_test_info_id = dti.donor_test_info_id
     LEFT OUTER JOIN backend_integration_partner_release bipr
        ON bipr.donor_test_info_id = dti.donor_test_info_id
WHERE     (                                                   -- Only complete
           dti.test_status = 7)
      AND                                     -- if positive we have a release
         (   (                                                   -- 2=negative
              dti .test_overall_result = 2
              AND (r.report_type = 1                             -- lab report
                                    OR r.report_info_id IS NULL     -- or none
                                                               ))
          OR (                            -- is positive and we have a release
              dti .test_overall_result = 1                     -- 1 = positive
              AND (r.report_type = 2                             -- MRO report
                                    OR r.report_info_id IS NULL     -- OR NONE
                                                               )
              AND bipr.backend_integration_partner_release_id IS NOT NULL
              AND bipr.donor_test_info_id_released > 0))
      AND (                              -- report info for neg is not archive
           r.is_archived < 1)
      AND (                                        -- This integration partner
           ips.backend_integration_partner_id =
              @backend_integration_partner_id)
      AND (                            -- donor has a pid type for the partner
           ip  .pid_type_id = ips.backend_integration_partners_pidtype
           AND (                               -- and it matches (if provided)
                ip.pid = @partner_donor_id OR @partner_donor_id IS NULL))
      AND (                               -- the test date is within the range
           dti.test_requested_date > @FROMDATETIME OR @FROMDATETIME IS NULL)
      AND (                               -- the test date is within the range
           dti.test_requested_date < @TODATETIME OR @TODATETIME IS NULL)
      AND (  -- This matches their client ID if provided or all clients (null)
           pcm.partner_client_id = @PARTNER_CLIENT_ID
           OR @PARTNER_CLIENT_ID IS NULL
           OR @PARTNER_CLIENT_ID = '')
      AND ( -- This matches their client code if provided or all client codes (null)
           pcm.partner_client_code = @PARTNER_CLIENT_CODE
           OR @PARTNER_CLIENT_CODE IS NULL
           OR @PARTNER_CLIENT_CODE = '')
LIMIT 0, 150;

-- 132075