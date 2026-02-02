CREATE FUNCTION GetDistance(
    originLatitude decimal(11, 7), 
    originLongitude decimal(11, 7),
    relativeLatitude decimal(11, 7),
    relativeLongitude decimal(11, 7))
    RETURNS INT(1)
BEGIN
   DECLARE x decimal(18,15);
   DECLARE EarthRadius decimal(7,3);
   DECLARE distInSelectedUnit decimal(18, 10);

   IF originLatitude = relativeLatitude AND originLongitude = relativeLongitude THEN
          RETURN 0; -- same lat/lon points, 0 distance
   END IF;

	--	   -- default unit of measurement will be miles
	--	   IF measure != 'Miles' AND measure != 'Kilometers' THEN
	--			  SET measure = 'Miles';
	--	   END IF;

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

	--	   -- convert the result to kilometers if desired
	--	   IF measure = 'Kilometers' THEN
	--			  SET distInSelectedUnit = MilesToKilometers(distInSelectedUnit);
	--	   END IF;

   RETURN distInSelectedUnit;
END
