DROP PROCEDURE IF EXISTS surpathlive.backend_set_user_activity;
CREATE PROCEDURE surpathlive.`backend_set_user_activity`(
in activity_user_id int, 
in activity_user_category_id int, 
in is_activity_visible int, 
in activity_note varchar(6000), 
in is_synchronized int,
out user_activity_id int
)
begin
  declare exit handler for SQLEXCEPTION, SQLWARNING
  BEGIN
    ROLLBACK;
  END;
  
  start transaction;


  INSERT INTO surpathlive.user_activity
  (
  activity_user_id, 
  activity_user_category_id, 
  is_activity_visible, 
  activity_note, 
  is_synchronized
  ) 
  VALUES (
  activity_user_id, 
  activity_user_category_id, 
  if (is_activity_visible>0,1,0), 
  activity_note, 
  if (is_synchronized>0,1,0));

  set user_activity_id := last_insert_id();
  commit;
end;

