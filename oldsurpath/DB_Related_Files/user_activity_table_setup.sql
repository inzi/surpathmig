CREATE TABLE `user_activity` (
  `user_activity_id` int(11) NOT NULL AUTO_INCREMENT,
  `activity_datetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `activity_user_id` int(11) NOT NULL,
  `activity_user_category_id` int(11) NOT NULL,
  `is_activity_visible` bit(1) NOT NULL,
  `activity_note` varchar(6000) NOT NULL,
  `is_synchronized` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`user_activity_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

