SELECT donor_test_info.donor_test_info_id AS DonorTestInfoId,
       donor_test_info.mro_type_id        AS MROTypeId
FROM donor_test_info
     INNER JOIN donor_test_info_test_categories
        ON donor_test_info_test_categories.donor_test_info_id =
              donor_test_info.donor_test_info_id
WHERE     
-- test_status IN (3, 4)
     test_category_id IN (1, 2, 3)
      AND DATE_ADD(DATE('2021-06-29'), INTERVAL -700 DAY) <=DATE(test_requested_date)
      AND donor_test_info.donor_id = 95461
      AND donor_test_info.client_id = 110
      AND donor_test_info.client_department_id = 389;
      
      
      select * from donor_test_info where donor_test_info_id = 113977;
      
      
      select * from individual_pids where donor_id = 95461;
      select * from report_info where donor_test_info_id = 113977;
      
      
      select * from backend_integration_partner_release;
      
      select * from donor_documents where donor_id = 95461;
      
      select * from backend_integration_partners;
      select * from backend_integration_partner_client_map;
      
      
--       delete from report_info where donor_test_info_id = 113977;
--       delete from backend_integration_partner_release where donor_test_info_id = 113977;
--       delete from donor_documents where donor_id = 95461;

-- update donor_test_info set donor_test_info.client_department_id = 389 where donor_test_info_id = 113977;

update backend_integration_partner_release set donor_test_info_id_released = 1;

-- call BACKEND_GET_PARTNER_DONORS_AND_DOCUMENTS(1,'2021-06-29', '2021-06-31','','','myexid1',0);

call backend_get_integration_partner_by_partner_client_code('115');

select * from backend_integration_partner_client_map;
select * from backend_integration_partners;


call backend_get_integration_clients_by_key ( '242575a2-d320-11eb-b1e0-7c4c58ced981');