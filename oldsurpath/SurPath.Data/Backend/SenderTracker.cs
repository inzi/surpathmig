using Serilog;
using SurPath.Data.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurPath.Data.Backend
{
    public class SenderTracker
    {
        public ILogger _logger { get; set; }
        StringBuilder Data { get; set; } = new StringBuilder();
        public List<FFMPSearchResult> FFMPSearchResults { get; set; } = new List<FFMPSearchResult>();
        public CreateOrderTest createOrderTest { get; set; } = new CreateOrderTest();
        public SenderTracker(ILogger __logger = null)
        {
            if (__logger == null)
            {
                this._logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                this._logger = __logger;
                _logger.Debug($"SenderTracker invoked");
            }

        }

        public void AddData(string _data)
        {
            Data.AppendLine(_data);

        }

        public string Report()
        {
            return Data.ToString();
        }



    }
}
