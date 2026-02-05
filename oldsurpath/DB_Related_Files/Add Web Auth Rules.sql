INSERT INTO auth_rule_categories (auth_rule_category_id, auth_rule_category_name, internal_name, parent_auth_rule_category_id, is_active, user_type, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) 
VALUES (16, 'Web', 'WEB', null, b'1', 1, b'1', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM');							

INSERT INTO auth_rules (auth_rule_id, auth_rule_name, internal_name, parent_auth_rule_id, is_active, auth_rule_category_id, is_synchronized, is_archived, created_on, created_by, last_modified_on, last_modified_by) VALUES 
(78, 'Can Send In From Web', 'WEB_CAN_SEND_IN', null, b'1', 16, b'1', b'0', NOW(), 'SYSTEM', NOW(), 'SYSTEM');										