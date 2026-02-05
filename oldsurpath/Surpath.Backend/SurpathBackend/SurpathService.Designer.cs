using Microsoft.Extensions.Configuration;
using Serilog;
using System.Configuration;

namespace SurpathBackend
{
    partial class SurpathService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //// Load the json settings file
            //ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddJsonFile("appsettings.json", false, true);
            //ConfigurationRoot = configurationBuilder.Build();

            //// Construct a logging mechanism
            //_logger = new LoggerFactory().AddFile(ConfigurationRoot.GetSection("Logging")).CreateLogger<HL7BabbleFishService>();

            //// set our service name
            //ServiceSettings serviceSettings = new ServiceSettings();
            //IConfigurationSection serviceSettingsData = ConfigurationRoot.GetSection("ServiceSettings");
            //serviceSettingsData.Bind(serviceSettings);
            //this.ServiceName = serviceSettings.ServiceName;

            //UpdateRuntimeSettings();

            components = new System.ComponentModel.Container();
            //this.ServiceName = "SurpathBackend";
        }

        #endregion
    }
}
