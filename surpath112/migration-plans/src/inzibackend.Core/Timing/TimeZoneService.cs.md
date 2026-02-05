# Modified
## Filename
TimeZoneService.cs
## Relative Path
inzibackend.Core\Timing\TimeZoneService.cs
## Language
C#
## Summary
The modified TimeZoneService class implements methods for handling time zones. It includes a constructor with two parameters, GetDefaultTimezoneAsync which retrieves the default timezone based on scope and tenant ID, FindTimeZoneById which converts a timezone ID to info, and GetWindowsTimezones which lists known Windows time zones.
## Changes
Added public TZConvert.GetTimeZoneInfo(TZ tz) method.
## Purpose
Provides timezone services including retrieval of default timezones by different scopes, conversion from ID to info, and listing of known Windows timezones.
