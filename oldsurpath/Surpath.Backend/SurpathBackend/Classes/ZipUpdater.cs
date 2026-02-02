using Newtonsoft.Json;
using Serilog;
using SurPath.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace SurpathBackend
{
    public class ZipUpdater
    {
        private BackendLogic backendLogic = new BackendLogic();
        private static ILogger _logger;
        public bool IsReady { get; set; } = false;

        public WorkingFileValues Values { get; set; } = new WorkingFileValues();

        public ZipUpdater(ILogger __logger = null)
        {
            if (__logger != null)
            {
                _logger = __logger;
                _logger.Debug("ZipUpdater logger online");
                _logger.Debug("Path to this ZipUpdater lib: " + AssemblyDirectory);
            }
            else
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            Values.WorkFile = Path.Combine(AssemblyDirectory, Values.WorkFileName);
            LoadWorkFile();
            string _ZipCodeFileUrl = string.Empty;

            if (ConfigurationManager.AppSettings["ZipCodeFileUrl"] != null)
            {
                _ZipCodeFileUrl = ConfigurationManager.AppSettings["ZipCodeFileUrl"].ToString().Trim();
                _logger.Debug($"ZipUpdater - download URL = {_ZipCodeFileUrl}");
            }

            int _ZipCodeDownloadDaysBetweenUpdates = 0;
            if (ConfigurationManager.AppSettings["ZipCodeDownloadDaysBetweenUpdates"] != null)
            {
                int.TryParse(ConfigurationManager.AppSettings["ZipCodeDownloadDaysBetweenUpdates"].ToString().Trim(), out _ZipCodeDownloadDaysBetweenUpdates);
                _logger.Debug($"ZipUpdater - ZipCodeDownloadDaysBetweenUpdates set to = {_ZipCodeDownloadDaysBetweenUpdates}");
            }

            if (_ZipCodeDownloadDaysBetweenUpdates > 0)
            {
                Values.ZipCodeDownloadDaysBetweenUpdates = _ZipCodeDownloadDaysBetweenUpdates;
            }
            _logger.Debug($"ZipUpdater - ZipCodeDownloadDaysBetweenUpdates running value = {Values.ZipCodeDownloadDaysBetweenUpdates}");

            if (!string.IsNullOrEmpty(_ZipCodeFileUrl))
            {
                Values.ZipCodeFileUrl = _ZipCodeFileUrl;
                IsReady = true;
                _logger.Debug($"ZipUpdater - ZipUpdater is ready");
            }
            _logger.Debug($"ZipUpdater - ZipCodeFileUrl running value = {Values.ZipCodeFileUrl}");

            SaveWorkFile();
            _logger.Debug($"Init complete. Last Checked: {Values.LastCheck.ToString()} - Will do work? {Values.DoWork.ToString()}");
        }

        public void DoWork(bool skipDL = false)
        {

            _logger.Debug($"ZipUpdater - DoWork called - Ready? = {IsReady.ToString()}");

            if (!IsReady) return;
            if (Values.DoWork)
            {
                try
                {
                    Download(skipDL);
                    UpdateZipCodes();
                    // clean up
                    if (Directory.Exists(Values.TempFolderName))
                    {
                        _logger.Debug($"ZipUpdater - Cleaning up, removing  {Values.TempFolderName}");
                        Directory.Delete(Values.TempFolderName, true);
                    }
                    Values.LastCheck = DateTime.Now;
                    SaveWorkFile();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    throw;
                }
            }
            else
            {
                _logger.Debug("Nothing done at this time");
            }
        }

        public void LoadWorkFile()
        {
            //if (!IsReady) return;
            try
            {
                string _workingFileValues = string.Empty;

                if (!File.Exists(Values.WorkFile))
                {
                    //retval = JsonConvert.SerializeObject(RenderSettings, Formatting.Indented);
                    //pdfRenderSettings_temp = JsonConvert.DeserializeObject<PdfRenderSettings>(ConfigJson);
                    SaveWorkFile();
                }
                // load the file
                _logger.Debug($"ZipUpdater - Loading workfile {Values.WorkFile}");

                _workingFileValues = File.ReadAllText(Values.WorkFile);
                Values = JsonConvert.DeserializeObject<WorkingFileValues>(_workingFileValues);
            }
            catch (Exception ex)
            {
                LogError(ex);
                this.IsReady = false;
            }
        }

        public void SaveWorkFile()
        {
            _logger.Debug($"ZipUpdater - Saving workfile {Values.WorkFile}");
            string _workingFileValues = JsonConvert.SerializeObject(Values, Formatting.Indented);
            File.WriteAllText(Values.WorkFile, _workingFileValues);
        }

        public void Download(bool skipDL = false)
        {
            if (!IsReady) return;
            _logger.Debug($"ZipUpdater - Will Download");

            string _zipfile = Path.Combine(AssemblyDirectory, Values.ZipFileName);

            if (!skipDL)
            {
                if (File.Exists(_zipfile)) File.Delete(_zipfile);
                using (var client = new WebClient())
                {
                    _logger.Debug($"ZipUpdater - Downloading {_zipfile} from {Values.ZipCodeFileUrl}");
                    client.DownloadFile(Values.ZipCodeFileUrl, _zipfile);
                }
            }

            Values.TempFolderName = Path.Combine(AssemblyDirectory, Path.GetFileName(Path.GetTempFileName()));
            if (Directory.Exists(Values.TempFolderName)) Directory.Delete(Values.TempFolderName);
            System.IO.Compression.ZipFile.ExtractToDirectory(_zipfile, Values.TempFolderName);
            _logger.Debug($"ZipUpdater - {_zipfile} extracted to  {Values.TempFolderName}");

            Values.DataFile = Path.Combine(Values.TempFolderName, Values.DataFileName);
            SaveWorkFile();
            _logger.Debug($"ZipUpdater - {Values.DataFile} Downloaded");
        }

        public void UpdateZipCodes()
        {
            try
            {
                _logger.Debug($"ZipUpdater - Comparing to database");

                List<ZipRow> ziprows = new List<ZipRow>();
                List<string> fileZips = new List<string>();
                string _data = File.ReadAllText(Values.DataFile);
                foreach (string _line in _data.Split(new[] { '\r', '\n' }).ToList())
                {
                    ZipRow zipRow = new ZipRow(_line);
                    //if (ziprows.Where(x=>x.Zip.Equals(zipRow.Zip,StringComparison.InvariantCultureIgnoreCase)).Count() < 1)
                    //{
                    ziprows.Add(zipRow);

                    //}
                    //fileZips.Add(zipRow.Zip);
                }
                // get all the zip codes
                List<ZipCodeDataRow> zipCodeDataRows = backendLogic.GetZipCodes();
                // go through them, and add any missing
                string[] _dataZips = ziprows.Select(x => x.Zip).Distinct().ToArray();
                string[] _dbZips = zipCodeDataRows.Select(x => x.zip).Distinct().ToArray();
                string[] _exceptZips = _dataZips.Except(_dbZips).ToArray();
                if (_exceptZips.Length > 0)
                {
                    _logger.Debug($"ZipUpdater - {_exceptZips.Length} new zip codes found");

                    List<ZipRow> _zipAdds = ziprows.Where(z => _exceptZips.Contains(z.Zip)).ToList();
                    List<ZipCodeDataRow> _zipCodeDataRows = new List<ZipCodeDataRow>();

                    foreach (ZipRow zipRow in _zipAdds)
                    {
                        _zipCodeDataRows.Add(new ZipCodeDataRow()
                        {
                            zip = zipRow.Zip,
                            latitude = zipRow.latitude,
                            longitude = zipRow.longitude,
                            city = zipRow.City,
                            state = zipRow.StateABBR,
                            type = "UDATE",
                            tz_dst = 0,
                            tz_std = 0,
                        });
                    }
                    _logger.Debug($"ZipUpdater - Inserting {_exceptZips.Length} new zip codes");

                    backendLogic.InsertZipCodes(_zipCodeDataRows);
                }
                // TODO Update if long lat is very different
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.InnerException.ToString());
                throw;
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private void LogError(Exception ex)
        {
            _logger.Error(ex.Message);
            _logger.Error(ex.InnerException.ToString());
        }
    }

    public class WorkingFileValues
    {
        public DateTime LastCheck { get; set; } = DateTime.MinValue;
        public string WorkFile { get; set; } = String.Empty;
        public string ZipFileName { get; set; } = "zipfile.zip";
        public string DataFileName { get; set; } = "US.txt";
        public string DataFile { get; set; } = "US.txt";
        public string WorkFileName { get; set; } = "ZipUpdater.json";
        public int ZipCodeDownloadDaysBetweenUpdates { get; set; } = 7;
        public string ZipCodeFileUrl { get; set; } = String.Empty;
        public string TempFolderName { get; set; } = String.Empty;

        public bool DoWork
        {
            get
            {
                return (int)((TimeSpan)(DateTime.Now - this.LastCheck)).TotalDays >= ZipCodeDownloadDaysBetweenUpdates;
            }
        }
    }

    /*

Readme for GeoNames Postal Code files :

allCountries.zip: all countries, for the UK only the outwards codes, the UK total codes are in GB_full.csv.zip
GB_full.csv.zip the full codes for the UK, ca 1.7 mio rows
<iso countrycode>: country specific subset also included in allCountries.zip
This work is licensed under a Creative Commons Attribution 3.0 License.
This means you can use the dump as long as you give credit to geonames (a link on your website to www.geonames.org is ok)
see http://creativecommons.org/licenses/by/3.0/
UK (GB_full.csv.zip): Contains Royal Mail data Royal Mail copyright and database right 2017.
The Data is provided "as is" without warranty or any representation of accuracy, timeliness or completeness.

This readme describes the GeoNames Postal Code dataset.
The main GeoNames gazetteer data extract is here: http://download.geonames.org/export/dump/

For many countries lat/lng are determined with an algorithm that searches the place names in the main geonames database
using administrative divisions and numerical vicinity of the postal codes as factors in the disambiguation of place names.
For postal codes and place name for which no corresponding toponym in the main geonames database could be found an average
lat/lng of 'neighbouring' postal codes is calculated.
Please let us know if you find any errors in the data set. Thanks

For Canada we have only the first letters of the full postal codes (for copyright reasons)

For Chile we have only the first digits of the full postal codes (for copyright reasons)

For Ireland we have only the first letters of the full postal codes (for copyright reasons)

For Malta we have only the first letters of the full postal codes (for copyright reasons)

The Argentina data file contains the first 5 positions of the postal code.

For Brazil only major postal codes are available (only the codes ending with -000 and the major code per municipality).

The data format is tab-delimited text in utf8 encoding, with the following fields :

country code      : iso country code, 2 characters
postal code       : varchar(20)
place name        : varchar(180)
admin name1       : 1. order subdivision (state) varchar(100)
admin code1       : 1. order subdivision (state) varchar(20)
admin name2       : 2. order subdivision (county/province) varchar(100)
admin code2       : 2. order subdivision (county/province) varchar(20)
admin name3       : 3. order subdivision (community) varchar(100)
admin code3       : 3. order subdivision (community) varchar(20)
latitude          : estimated latitude (wgs84)
longitude         : estimated longitude (wgs84)
accuracy          : accuracy of lat/lng from 1=estimated, 4=geonameid, 6=centroid of addresses or shape

        US	76207	Denton	Texas	TX	Denton	121			33.2285	-97.1813	4
        US	76208	Denton	Texas	TX	Denton	121			33.2117	-97.0612	4
        US	76209	Denton	Texas	TX	Denton	121			33.2346	-97.1131	4
        US	76210	Denton	Texas	TX	Denton	121			33.1428	-97.0727	4
        US	76226	Argyle	Texas	TX	Denton	121			33.1062	-97.16	4
        US	76227	Aubrey	Texas	TX	Denton	121			33.292	-96.9879	4
    */

    public class ZipRow
    {
        //public string Country { get { return _vals[ZipRowIDs.Country.ToString()]; } }
        //public string Zip { get { return _vals[ZipRowIDs.Zip.ToString()]; } }
        //public string City { get { return _vals[ZipRowIDs.City.ToString()]; } }
        //public string State { get { return _vals[ZipRowIDs.State.ToString()]; } }
        //public string StateABBR { get { return _vals[ZipRowIDs.StateABBR.ToString()]; } }
        //public string County { get { return _vals[ZipRowIDs.County.ToString()]; } }
        //public string CountyABBR { get { return _vals[ZipRowIDs.CountyABBR.ToString()]; } }
        //public string Community { get { return _vals[ZipRowIDs.Community.ToString()]; } }
        //public string CommunityABBR { get { return _vals[ZipRowIDs.CommunityABBR.ToString()]; } }
        //public string latitude { get { return _vals[ZipRowIDs.latitude.ToString()]; } }
        //public string longitude { get { return _vals[ZipRowIDs.longitude.ToString()]; } }
        //public string accuracy { get { return _vals[ZipRowIDs.accuracy.ToString()]; } }
        public string Country { get; set; }

        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateABBR { get; set; }
        public string County { get; set; }
        public string CountyABBR { get; set; }
        public string Community { get; set; }
        public string CommunityABBR { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string accuracy { get; set; }
        private Dictionary<string, string> _vals = new Dictionary<string, string>();

        public ZipRow(string _row = null)
        {
            if (!string.IsNullOrEmpty(_row))
            {
                Fill(_row);
            }
        }

        public void Fill(string _row)
        {
            string[] _values = _row.Split('\t');
            if (_values.Length < 12)
            {
                var x = "uh oh";
            }
            Country = _values[(int)ZipRowIDs.Country];
            Zip = _values[(int)ZipRowIDs.Zip];
            City = _values[(int)ZipRowIDs.City];
            State = _values[(int)ZipRowIDs.State];
            StateABBR = _values[(int)ZipRowIDs.StateABBR];
            County = _values[(int)ZipRowIDs.County];
            CountyABBR = _values[(int)ZipRowIDs.CountyABBR];
            Community = _values[(int)ZipRowIDs.Community];
            CommunityABBR = _values[(int)ZipRowIDs.CommunityABBR];
            latitude = _values[(int)ZipRowIDs.latitude];
            longitude = _values[(int)ZipRowIDs.longitude];
            accuracy = _values[(int)ZipRowIDs.accuracy];

            //foreach (var _enumItem in System.Enum.GetValues(typeof(ZipRowIDs)))
            //{
            //    _vals.Add(_enumItem.ToString(), _values[(int)_enumItem].ToString());
            //}
        }
    }

    public enum ZipRowIDs
    {
        [Description("Country")]
        Country = 0,

        [Description("Zip")]
        Zip = 1,

        [Description("City")]
        City = 2,

        [Description("State")]
        State = 3,

        [Description("StateABBR")]
        StateABBR = 4,

        [Description("County")]
        County = 5,

        [Description("CountyABBR")]
        CountyABBR = 6,

        [Description("Community")]
        Community = 7,

        [Description("CommunityABBR")]
        CommunityABBR = 8,

        [Description("latitude")]
        latitude = 9,

        [Description("longitude")]
        longitude = 10,

        [Description("accuracy")]
        accuracy = 11
    }
}