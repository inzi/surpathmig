select 

bipr.*,
bip.*,
bipm.*

from backend_integration_partner_release bipr
inner join donor_test_info dti on bipr.donor_test_info_id = dti.donor_test_info_id
inner join backend_integration_partner_client_map bipm on bipm.client_id = dti.client_id AND bipm.client_department_id = dti.client_department_id
inner join backend_integration_partners bip on bipm.backend_integration_partner_client_map_id = bip.backend_integration_partner_id
where 
bip.active = 1 and
bipr.sent_on is null;