using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackendHelpers
{
    public class PIDHelper
    {

        public static string JsonParserMasks;
        public HelperMaskList maskList = new HelperMaskList();
        public ILogger _logger;
        public bool Verbose = false;

        public bool ShowPidInLog = false;

        public string libPath { get; set; }

        public PIDHelper(ILogger __logger = null)
        {
            if (__logger == null)
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            else
            {
                _logger = __logger;
            }

            this.libPath = Path.GetDirectoryName(
                     Assembly.GetAssembly(typeof(PIDHelper)).CodeBase);

            _logger.Debug($"PIDHelper lib path: {this.libPath}");
        }

            public List<PidMask> Evaluate(string pidString)
        {

            if (ShowPidInLog==true)
            {
                _logger.Debug($"PIDHelper Evaluate {pidString}");
            }

            if (maskList.PidMasks.Count < 1) LoadPidMasks();
            if (Verbose==true) _logger.Debug($"PIDHelper evaluating against {maskList.PidMasks.Count} expressions");
            List<PidMask> retList = new List<PidMask>();
            Regex regex;
            foreach(PidMask p in maskList.PidMasks)
            {
                if (Verbose == true) _logger.Debug($"PIDHelper Testing {p.description.FirstOrDefault().ToString()}");

                regex = new Regex(p.rule);
                if (regex.IsMatch(pidString))
                {
                    if (Verbose == true) _logger.Debug($"PIDHelper {pidString} matches {p.description.FirstOrDefault().ToString()}");

                    retList.Add(p);
                }

            }

            return retList;
        }

        public void LoadPidMasks()
        {
            if (Verbose == true) _logger.Debug("PIDHelper loading masks");
            if (String.IsNullOrEmpty(JsonParserMasks))
            {
                if (ConfigKeyExists("AllFilesFolder") && ConfigKeyExists("JsonParserMasks"))
                {
                    string folder_name = ConfigurationManager.AppSettings["AllFilesFolder"].ToString().Trim();
                    string file_name = ConfigurationManager.AppSettings["JsonParserMasks"].ToString().Trim();
                   // file_name = "PIDTypes-original.json";
                    folder_name = folder_name.TrimEnd('\\') + '\\';
                    file_name = folder_name + file_name;
                    if (Verbose == true) _logger.Debug($"PIDHelper loading file {file_name}");

                    if (!File.Exists(file_name))
                    {
                        _logger.Debug($"PIDHelper {file_name} NOT FOUND in folder {Path.GetFullPath(folder_name)} [setting of: {folder_name} ]");


                        return;
                    }

                    string _JsonParserMasks = File.ReadAllText(file_name);
                    if (IsValidJson(_JsonParserMasks))
                    {
                        JsonParserMasks = _JsonParserMasks;
                    }
                }
            }
            maskList = JsonConvert.DeserializeObject<HelperMaskList>(JsonParserMasks);
            //JObject pidMasks = JObject.Parse(JsonParserMasks);
            //MaskList maskList = new MaskList();
            //foreach (dynamic x in pidMasks)
            //{
            //    PidMask p = new PidMask();
            //    p.Name = x.Key;
            //    p.rule = x.Value["rule"].ToString();
            //    foreach (string de in ((JArray)pidMasks[x.Key]["description"]))
            //    {
            //        p.description.Add(de.ToString());
            //    }

            //    p.Type = HelperPidTypes.DL;
            //    maskList.PidMasks.Add(p);
            //}

            JsonSerializer jsonSerializer = new JsonSerializer();
            string maskJson = JsonConvert.SerializeObject(maskList);
            if (Verbose == true) _logger.Debug($"PIDHelper {maskList.PidMasks.Count.ToString()} Loaded Masks");

            // TODO put into database?

        }
        public static bool ConfigKeyExists(string _configkey)
        {
            return ConfigurationManager.AppSettings[_configkey] != null;
        }
        public bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }


    [Serializable]
    public class PidMask
    {
        //      "DLAL": {
        //"rule": "^[0-9]{1,8}$",
        //"description": [
        //  "1-8 Numeric"
        //]
        //}, 
        public string Name;
        public PidTypes Type;
        //public PidTypes PidType;
        public string rule;
        public List<string> description = new List<string>();
    }

    public class HelperMaskList
    {
        public List<PidMask> PidMasks { get; set; } = new List<PidMask>();
    }

    //public enum HelperPidTypes
    //{
    //    [Description("Unknown")]
    //    None = 0,

    //    [Description("SSN")]
    //    SSN = 1,

    //    [Description("DL")]
    //    DL = 2,

    //    [Description("Passport")]
    //    Passport = 3,

    //    [Description("Other")]
    //    Other = 4,

    //    [Description("SampleID")]
    //    SampleID = 5,

    //    [Description("TaxPayer ID")]
    //    TaxPayerID = 6,

    //    [Description("Quest ID")]
    //    QuestID = 7,

    //    [Description("Donor ID")]
    //    DonorID = 7,
    //}

}
