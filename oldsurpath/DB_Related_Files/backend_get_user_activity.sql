DROP PROCEDURE IF EXISTS surpathlive.backend_get_user_activity;
CREATE PROCEDURE surpathlive.`backend_get_user_activity`(
in activity_user_id int, 
in activity_user_category_id int, 
in is_activity_visible int
)
BEGIN


-- for conditional where clauses
select activity_user_category_id >0 into @filtercat;
select is_activity_visible > 0 into @onlyvisible;

-- select @filtercat as filtercat;
-- select @onlyvisible as onlyvisible;

select *
from
user_activity
where
(
  (@filtercat =0)
  OR
  (@filtercat > 0 AND user_activity.activity_user_category_id =activity_user_category_id)
)
and
(
  (@onlyvisible = 0 and user_activity.is_activity_visible < 0)
  OR
  (@onlyvisible > 0)
)
and
user_activity.activity_user_id = activity_user_id
order by activity_datetime desc;

END