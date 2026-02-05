-- PC
update backend_integration_partners
set 
partner_push_username = 'surscan_test', 
partner_push_password = '9ub2Vxvmd3FEd68s',
partner_key = '242575a2-d320-11eb-b1e0-7c4c58ced981',
partner_crypto = '9edef9be-4ec6-ad3f-75e9-45d031d51741'
where partner_name = 'Project Concert';


-- MCE

update backend_integration_partners
set 
partner_key = '44af9269-d320-11eb-b1e0-7c4c58ced981',
partner_crypto = 'fb566ea8-1740-823b-4566-d44f8f7b513e'
where partner_name = 'My Clinical Exchange';



-- set PC to invoice client for testing


update
client_departments cd 
inner join clients c on cd.client_id = c.client_id
inner join backend_integration_partner_client_map ipm on c.client_id = ipm.client_id
inner join backend_integration_partners ip on  ip.backend_integration_partner_id = ipm.backend_integration_partner_id
set cd.payment_type_id = 2
where ip.partner_key = '242575a2-d320-11eb-b1e0-7c4c58ced981';


update mysql.proc set definer = 'surpath@%'
where 
definer = '@' and 
db='surpathlive';

call RemoveReportInfoDuplicates();
