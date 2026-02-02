DROP TABLE IF EXISTS surpathlive.backend_client_edit_activity;
CREATE TABLE `backend_client_edit_activity` (
  `backend_client_edit_activity_id` int(11) NOT NULL AUTO_INCREMENT,
  `backend_notification_window_data_id` int(11) NOT NULL,
  `activity_datetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `activity_user_id` int(11) NOT NULL,
  `activity_note` varchar(6000) NOT NULL,
  PRIMARY KEY (`backend_client_edit_activity_id`)
) ENGINE=InnoDB AUTO_INCREMENT=159341 DEFAULT CHARSET=utf8;


DROP PROCEDURE IF EXISTS surpathlive.backend_set_client_edit_activity;
CREATE PROCEDURE surpathlive.`backend_set_client_edit_activity`(
in backend_notification_window_data_id int,
in activity_user_id int, 
in activity_note varchar(6000), 
in is_synchronized int,
out backend_client_edit_activity_id int
)
begin

  INSERT INTO surpathlive.backend_client_edit_activity
  (
  backend_notification_window_data_id, 
  activity_user_id, 
  activity_note
  ) 
  VALUES (
  backend_notification_window_data_id, 
  activity_user_id, 
  activity_note
  );

  set backend_client_edit_activity_id := last_insert_id();

end;

