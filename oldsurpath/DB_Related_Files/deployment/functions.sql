DROP FUNCTION IF EXISTS surpathlive.backend_in_notification_window;
CREATE FUNCTION surpathlive.`backend_in_notification_window`(
        client_id int,
        client_department_id int,
        notify_now int        
     ) RETURNS int(11)
    NO SQL
    DETERMINISTIC
    COMMENT 'Returns if the current client department is within notification window'
BEGIN

set @now = now();
set @secs = time_to_sec(time(@now));
set @day = DAYOFWEEK((@now));
set @in_window = 0;


if exists(  
    select bn.client_id 
    from backend_notification_window_data bn
    where   
    (bn.client_id = client_id  and bn.client_department_id = client_department_id)
    ) then
 
    set @in_window = exists(select *
    from backend_notification_window_data
    where
      (
        backend_notification_window_data.client_id = client_id 
        and 
        backend_notification_window_data.client_department_id = client_department_id
      )
      and 
      (
        (sunday > 0 and @day = 2 and @secs between sunday_send_time_start_seconds_from_midnight and sunday_send_time_stop_seconds_from_midnight )
        or
        (monday > 0 and @day = 2 and @secs between monday_send_time_start_seconds_from_midnight and monday_send_time_stop_seconds_from_midnight )
        or
        (tuesday > 0 and @day = 3 and @secs between tuesday_send_time_start_seconds_from_midnight and tuesday_send_time_stop_seconds_from_midnight )
        or
        (wednesday > 0 and @day = 4 and @secs between wednesday_send_time_start_seconds_from_midnight and wednesday_send_time_stop_seconds_from_midnight )
        or
        (thursday > 0 and @day = 5 and @secs between thursday_send_time_start_seconds_from_midnight and thursday_send_time_stop_seconds_from_midnight )
        or
        (friday > 0 and @day = 6 and @secs between friday_send_time_start_seconds_from_midnight and friday_send_time_stop_seconds_from_midnight )
        or
        (saturday > 0 and @day = 7 and @secs between saturday_send_time_start_seconds_from_midnight and saturday_send_time_stop_seconds_from_midnight )
      )
    );
    
    IF notify_now = 1 then
      set @in_window = 1;
    end if;
     
end if;
   
return @in_window;

END;

DROP FUNCTION IF EXISTS surpathlive.backend_only_digits;
CREATE FUNCTION surpathlive.`backend_only_digits`(as_val VARCHAR(2048)) RETURNS varchar(2048) CHARSET latin1
    DETERMINISTIC
BEGIN
   DECLARE retval VARCHAR(2048);
   DECLARE i INT;
   DECLARE strlen INT;
   -- shortcut exit for special cases
   IF as_val IS NULL OR as_val = '' THEN
     RETURN as_val;
   END IF;
   -- initialize for loop
   SET retval = '';
   SET i = 1;
   SET strlen = CHAR_LENGTH(as_val);
 do_loop:
   LOOP
     IF i > strlen THEN
       LEAVE do_loop;
     END IF;
     IF SUBSTR(as_val,i,1) IN ('0','1','2','3','4','5','6','7','8','9') THEN
       SET retval = CONCAT(retval,SUBSTR(as_val,i,1));
     END IF;
     SET i = i + 1;
   END LOOP do_loop;
   RETURN retval;
 END;

DROP FUNCTION IF EXISTS surpathlive.GetDistance;
CREATE FUNCTION surpathlive.`GetDistance`(
    originLatitude decimal(11, 7), 
    originLongitude decimal(11, 7),
    relativeLatitude decimal(11, 7),
    relativeLongitude decimal(11, 7)) RETURNS int(1)
    DETERMINISTIC
BEGIN
   DECLARE x decimal(18,15);
   DECLARE EarthRadius decimal(7,3);
   DECLARE distInSelectedUnit decimal(18, 10);

   IF originLatitude = relativeLatitude AND originLongitude = relativeLongitude THEN
          RETURN 0; -- same lat/lon points, 0 distance
   END IF;

		   -- default unit of measurement will be miles
-- 		   IF measure != 'Miles' AND measure != 'Kilometers' THEN
-- 				  SET measure = 'Miles';
-- 		   END IF;

   -- lat and lon values must be within -180 and 180.
   IF originLatitude < -180 OR relativeLatitude < -180 OR originLongitude < -180 OR relativeLongitude < -180 THEN
          RETURN 0;
   END IF;

   IF originLatitude > 180 OR relativeLatitude > 180 OR originLongitude > 180 OR relativeLongitude > 180 THEN
         RETURN 0;
   END IF;

   SET x = 0.0;

   -- convert from degrees to radians
   SET originLatitude = originLatitude * PI() / 180.0,
       originLongitude = originLongitude * PI() / 180.0,
       relativeLatitude = relativeLatitude * PI() / 180.0,
       relativeLongitude = relativeLongitude * PI() / 180.0;

   -- distance formula, accurate to within 30 feet
   SET x = Sin(originLatitude) * Sin(relativeLatitude) + Cos(originLatitude) * Cos(relativeLatitude) * Cos(relativeLongitude - originLongitude);

   IF 1 = x THEN
          SET distInSelectedUnit = 0; -- same lat/long points
          -- not enough precision in MySQL to detect this earlier in the function
   END IF;

   SET EarthRadius = 3963.189;
   SET distInSelectedUnit = EarthRadius * (-1 * Atan(x / Sqrt(1 - x * x)) + PI() / 2);

	-- convert the result to kilometers if desired
-- 		   IF measure = 'Kilometers' THEN
-- 				  SET distInSelectedUnit = MilesToKilometers(distInSelectedUnit);
-- 		   END IF;

   RETURN distInSelectedUnit;
END;

DROP FUNCTION IF EXISTS surpathlive.GetSendInDate;
CREATE FUNCTION surpathlive.`GetSendInDate`( client_id int, client_department_id int, max_sendins int, utstart INT, utend INT, sunday INT, monday INT, tuesday INT, wednesday INT, thursday INT, friday INT, saturday INT ) RETURNS int(11)
    DETERMINISTIC
BEGIN
-- day of week: (1 for Sunday,2 for Monday ?? 7 for Saturday )

   set @exceptionSendIndate = UNIX_TIMESTAMP(DATE_ADD(DATE(FROM_UNIXTIME(utend)), INTERVAL 1 DAY)); -- this is 1 day after the end date, which will push to exception tool
   -- make sure we have a day to find and dates to work with
   set @bail = sunday + monday + tuesday + wednesday + thursday + friday + saturday;
   if utstart is null then 
    return 0;
   end if;
    if utend is null then
    return 0;
   end if;  
   if @bail < 1 then
    return @exceptionSendIndate;
   end if;
   
 
   
   
   -- if utstart is in the past, set utstart = today
   if UNIX_TIMESTAMP(date(CURRENT_DATE())) > utstart then
     set utstart = UNIX_TIMESTAMP(date(CURRENT_DATE()));
   end if;
   
   
   
   -- Bail conditions
   -- if none of the days between the start and end are enabled, bail
     
  set @datestart =  Date(FROM_UNIXTIME(utstart));
  set @dateend = DATE(FROM_UNIXTIME(utend));
  
    -- we're past the deadline!
   if datediff(@dateend, @datestart)<1 then
    return @exceptionSendIndate; -- we're past the date, so set the send in date after the last day
   end if;
  
  -- setup some day variables to see if we get a hit
  set @su = 0, @mo=0, @tu=0, @we=0, @th=0, @fr=0, @sa=0;
  -- begin to loop through the days in the window.
  finddays: WHILE datediff(@dateend, @datestart)>-1 DO
      set @dow = DAYOFWEEK(@datestart);
      if @dow = 1 AND sunday > 0 then
        set @su = 1;
      end if;
      if (@dow = 2 and monday>0) then
        set @mo = 1;
      end if;
      if @dow = 3 AND tuesday > 0 then
        set @tu = 1;
      end if;
      if @dow = 4 AND wednesday > 0 then
        set @we = 1;
      end if;
      if @dow = 5 AND thursday > 0 then
        set @th = 1;
      end if;
      if @dow = 6 AND friday > 0 then
        set @fr = 1;
      end if;
      if @dow = 7 AND saturday > 0 then
        set @sa = 1;
      end if;  
      set @datestart = DATE_ADD(@datestart, INTERVAL 1 DAY);
  END WHILE finddays;
  set @bail = @su + @mo + @tu + @we + @th + @fr + @sa;
  if @bail < 1 then
    return @exceptionSendIndate; -- no days available, so put it past the end to push to exception report.
  end if;
   
   set @tries = 20; -- try failsafe - try 20 times and give up
   
   set @found = 0;
   
   
  label1: LOOP  
  
   set @tries = @tries-1;
  
    set @uttest = FLOOR(RAND()*(utend-utstart+1)+utstart);
     
    SET @DTTEST = FROM_UNIXTIME(@UTTEST);
    SET @DOW = DAYOFWEEK(@DTTEST);


    
    if @dow = 1 AND sunday > 0 then
      set @found = 1;
    end if;
    if (@dow = 2 and monday>0) then
      set @found = 1;
    end if;
    if @dow = 3 AND tuesday > 0 then
      set @found = 1;
    end if;
    if @dow = 4 AND wednesday > 0 then
      set @found = 1;
    end if;
    if @dow = 5 AND thursday > 0 then
      set @found = 1;
    end if;
    if @dow = 6 AND friday > 0 then
      set @found = 1;
    end if;
    if @dow = 7 AND saturday > 0 then
      set @found = 1;
    end if;    
     IF @found =0 THEN
       if (@tries<0) then
        set @uttest = @exceptionSendIndate; -- we couldn't find a date x tries, give up
        leave label1;
       end if;
       ITERATE label1;
     END IF;
     
   -- check count for client_id and client_department_id
    if (
      select 
      count(*)
      from
      backend_notifications b
      inner join donor_test_info d on b.donor_test_info_id = d.donor_test_info_id
      where d.client_id = 110 and d.client_department_id = 372
      and date(b.notify_after_timestamp) = date(FROM_UNIXTIME(@uttest))
    )>= max_sendins then
      -- This day is full, pick another.
      ITERATE label1;
    end if;
     
     LEAVE label1;
   END LOOP label1;
      
   return @uttest;

END;

DROP FUNCTION IF EXISTS surpathlive.haversine;
CREATE FUNCTION surpathlive.`haversine`(
        lat1 FLOAT, lon1 FLOAT,
        lat2 FLOAT, lon2 FLOAT
     ) RETURNS float
    NO SQL
    DETERMINISTIC
    COMMENT 'Returns the distance in degrees on the Earth between two known points of latitude and longitude'
BEGIN

-- kilometers (111.045), statute miles (69), or nautical miles (60)


    RETURN 69 * DEGREES(ACOS(
              COS(RADIANS(lat1)) *
              COS(RADIANS(lat2)) *
              COS(RADIANS(lon2) - RADIANS(lon1)) +
              SIN(RADIANS(lat1)) * SIN(RADIANS(lat2))
            ));
END;

