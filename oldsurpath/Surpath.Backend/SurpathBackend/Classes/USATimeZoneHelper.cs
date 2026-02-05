using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpathBackend.Classes
{
    public static class USATimeZoneHelper
    {
        public static List<TimeZoneInfo> USATZList()
        {
            var zones = new List<TimeZoneInfo> {
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Aleutian Standard Time"),
            };

            return zones;
        }
        public static List<string> USATZIdList()
        {
            var zones = new List<string> {
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").Id,
                TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time").Id,
                TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time").Id,
                TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time").Id,
                TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time").Id,
                TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time").Id,
                TimeZoneInfo.FindSystemTimeZoneById("Aleutian Standard Time").Id,
            };

            return zones;
        }

    }
}
