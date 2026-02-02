using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.Statics
{
    public static class DayOfWeekOrderExtensions
    {
        public static IEnumerable<DayOfWeek> GetDaysStartingWithMonday()
        {
            // Yield return ensures that the days are returned one at a time, starting with Monday
            for (int i = (int)DayOfWeek.Monday; i <= (int)DayOfWeek.Saturday; i++)
            {
                yield return (DayOfWeek)i;
            }
            yield return DayOfWeek.Sunday; // Finally, yield return Sunday
        }

        public static IEnumerable<DayOfWeek> GetDaysStartingWith(DayOfWeek startDay)
        {
            // Loop through all 7 days, starting from startDay
            for (int i = 0; i < 7; i++)
            {
                // Calculate the day to yield by adding i to startDay, and wrap around using modulo 7
                int dayValue = (((int)startDay + i) % 7);
                yield return (DayOfWeek)dayValue;
            }
        }

        public static string GetDayOfWeekAbbr(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "M";
                case DayOfWeek.Tuesday:
                    return "T";
                case DayOfWeek.Wednesday:
                    return "W";
                case DayOfWeek.Thursday:
                    return "R";
                case DayOfWeek.Friday:
                    return "F";
                case DayOfWeek.Saturday:
                    return "S";
                case DayOfWeek.Sunday:
                    return "S";
                default:
                    throw new ArgumentOutOfRangeException(nameof(day), day, null);
            }
        }
    }
}
