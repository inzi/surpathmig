using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Surpath.MetaData
{
    public class MetaDataNotifications
    {
        public DateTime WarnDaysBeforeFirst { get; set; } = DateTime.MaxValue;
        public bool WarnedDaysBeforeFirst { get; set; } = false;
        public DateTime WarnDaysBeforeSecond { get; set; } = DateTime.MaxValue;
        public bool WarnedDaysBeforeSecond { get; set; } = false;
        public DateTime WarnDaysBeforeFinal { get; set; } = DateTime.MaxValue;
        public bool WarnedDaysBeforeFinal { get; set; } = false;
        public DateTime ExpiredNotification { get; set; } = DateTime.MaxValue;
        public bool ExpiredNotificationSent { get; set; } = false;
    }
}
