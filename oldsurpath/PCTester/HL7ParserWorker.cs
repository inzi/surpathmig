using Microsoft.Extensions.Configuration;
using Serilog;
using SurPath.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using SurPath.Entity;


namespace HL7ParserService
{
    internal class HL7ParserWorker
    {
        private BackendData backendData;


        // Our Service Configuration
        public IConfigurationRoot ConfigurationRoot;

        // Our Logger
        public static ILogger _logger;

        public HL7ParserWorker(ILogger __logger)
        {
            _logger = __logger;
            _logger.Debug("HL7ParserWorker loaded");
            //backendData = new BackendData(null, null, _logger);

        }

        public bool Work()
        {
            try
            {
                HL7Processor hL7Processor = new HL7Processor(_logger);
                hL7Processor.main();

                //  We need to transmit any documents that are released and whose sent_on timestamp is null 



            }
            catch (Exception ex)
            {

                throw;
            }

            return true; //Task.FromResult(true);
        }

    }
}