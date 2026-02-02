using MySql.Data.MySqlClient;
using Serilog;
using SurPath.Business;
using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;

//using SurPath.MySQLHelper;
using SurpathBackend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HL7ParserService
{
    public class HL7Stage
    {
        public ILogger _logger;
        public string ConnectionString;
        public MySqlConnection conn;
        public List<StageHelper> stagehelpers = new List<StageHelper>();
        public string StageFilesFolder;
        public string LabReportFilePath;
        public string MROReportFileInboundPath;

        public HL7Stage(ILogger __logger)
        {
            _logger = __logger;
            this.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString().Trim();
            this.conn = new MySqlConnection(this.ConnectionString);
            this.StageFilesFolder = ConfigurationManager.AppSettings["StageFilesFolder"].ToString().Trim();
            if (!this.StageFilesFolder.EndsWith("/")) this.StageFilesFolder = this.StageFilesFolder + "/";
            this.MROReportFileInboundPath = ConfigurationManager.AppSettings["MROReportFileInboundPath"].ToString().Trim();
            if (!this.MROReportFileInboundPath.EndsWith("/")) this.MROReportFileInboundPath = this.MROReportFileInboundPath + "/";
            this.LabReportFilePath = ConfigurationManager.AppSettings["LabReportFilePath"].ToString().Trim();
            if (!this.LabReportFilePath.EndsWith("/")) this.LabReportFilePath = this.LabReportFilePath + "/";
        }

        public bool Gen(int? specificDonorId = null)
        {
            _logger.Debug($"Generating for testing" + (specificDonorId.HasValue ? $" for donor {specificDonorId}" : ""));

            _logger.Debug($"Getting testing donors");
            if (!GetDonors(specificDonorId)) return false;

            _logger.Debug($"Generating testing lab reports");
            GenLabReports();

            _logger.Debug($"Generating testing MRO reports");
            GenMROReports();

            _logger.Debug($"Releasing testing MRO reports");
            ReleaseDocs();

            return true;
        }

        public bool GetDonors(int? specificDonorId = null)
        {
            _logger.Debug("Getting donor(s) in-queue");

            if (specificDonorId.HasValue)
            {
                _logger.Debug($"Getting specific donor {specificDonorId.Value}");
            }
            else
            {
                _logger.Debug("Getting all donors in-queue");
            }
            // Client dept id is 0 -
            //            var sqlQuery = @"
            //select d.donor_id, ip.backend_integration_partners_pidtype, cd.lab_code from donors d
            //inner join donor_test_info dti on d.donor_id = dti.donor_id
            //inner join client_departments cd on cd.client_department_id = dti.client_department_id
            //inner join clients c on c.client_id = cd.client_id
            //inner join backend_integration_partner_client_map cm on cm.client_id = c.client_id   -- cm.client_department_id = dti.client_department_id
            //inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id

            //where
            //cm.backend_integration_partner_client_map_id is not null
            //and dti.test_status = 4
            //";

            var sqlQuery = @"
select d.donor_id, ip.backend_integration_partners_pidtype, cd.lab_code from donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
inner join client_departments cd on cd.client_department_id = dti.client_department_id
inner join clients c on c.client_id = cd.client_id
inner join backend_integration_partner_client_map cm on cm.client_id = c.client_id  and cm.client_department_id = dti.client_department_id
inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id

where
cm.backend_integration_partner_client_map_id is not null
and dti.test_status = 4
" + (specificDonorId.HasValue ? $"and d.donor_id = {specificDonorId.Value}\n" : "") + ";";
            ParamHelper param = new ParamHelper();
            RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
            rsg.NumericCharacters = "0123456789".ToCharArray();
            rsg.UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
            {
                using (var sqlQueryResult = cmd.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        while (sqlQueryResult.Read())
                        {
                            int _donor_id = sqlQueryResult.GetInt32(sqlQueryResult.GetOrdinal("donor_id"));
                            int _backend_integration_partners_pidtype = sqlQueryResult.GetInt32(sqlQueryResult.GetOrdinal("backend_integration_partners_pidtype"));
                            string lab_code = sqlQueryResult.GetString(sqlQueryResult.GetOrdinal("lab_code"));

                            stagehelpers.Add(new StageHelper() { donor_id = _donor_id, lab_code = lab_code, pidtypevalue = new PIDTypeValue() { PIDType = _backend_integration_partners_pidtype } });
                        }
                    }
            }
            ;

            if (stagehelpers.Count() < 1) return false;

            // we have donors, go get them.
            _logger.Debug("Found donor(s): ", stagehelpers.Count());
            DonorBL donorBL = new DonorBL(_logger);
            List<StageHelper> newstagehelpers = new List<StageHelper>();
            foreach (StageHelper _stageh in stagehelpers)
            {
                Donor donor = donorBL.Get(_stageh.donor_id, "stage");
                Random random = new Random();
                var negpos = random.Next(100);
                var mropos = random.Next(100);
                var SPECSPECSPEC = rsg.Generate(10);
                var SAMPSAMPSAMP = rsg.Generate(8);
                var ispos = negpos > 50;
                var ismropos = mropos > 50;
                if (donor.PidTypeValues.Exists(dp => dp.PIDType == _stageh.pidtypevalue.PIDType))
                {
                    //newstagehelpers.Add(new StageHelper() { donor = donor, pos = negpos > 20, mropos = mropos > 20, sample_id=SAMPSAMPSAMP, specimen_id=SPECSPECSPEC, pidtypevalue = _stageh.pidtypevalue, donor_id = _stageh.donor_id, lab_code = _stageh.lab_code });
                    newstagehelpers.Add(new StageHelper() { donor = donor, pos = ispos, mropos = ismropos, sample_id = SAMPSAMPSAMP, specimen_id = SPECSPECSPEC, pidtypevalue = _stageh.pidtypevalue, donor_id = _stageh.donor_id, lab_code = _stageh.lab_code });
                }
            }
            stagehelpers = newstagehelpers;

            bool setToProcessing = false;

            if (setToProcessing)
            {
                // change the test status to be Processing so we don't regen stage files
                _logger.Debug($"setToProcessing is true, so change the test status to be Processing so we don't regen stage files");
                sqlQuery = @"
update donors d
inner join donor_test_info dti on d.donor_id = dti.donor_id
inner join backend_integration_partner_client_map cm on cm.client_department_id = dti.client_department_id
inner join backend_integration_partners ip on cm.backend_integration_partner_id = ip.backend_integration_partner_id
inner join client_departments cd on cd.client_department_id = dti.client_department_id
set dti.test_status = 6
where
cm.backend_integration_partner_client_map_id is not null
and dti.test_status = 4;

";
                if (this.conn.State == ConnectionState.Closed) conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
                {
                    int _rowcount = cmd.ExecuteNonQuery();
                    _logger.Debug($"{_rowcount} records set to processing by stage");
                }
                ;
            }

            return true;
        }

        public bool GenLabReports()
        {
            foreach (StageHelper stageh in stagehelpers)
            {
                if (stageh.pos == false)
                {
                    _logger.Debug($"Generating negative lab report for {stageh.donor.DonorId} - {stageh.specimen_id}");
                }
                else
                {
                    _logger.Debug($"Generating positive lab report for {stageh.donor.DonorId} - {stageh.specimen_id}");
                }
                generateLabReport(stageh);
            }

            return true;
        }

        public bool GenMROReports()
        {
            foreach (StageHelper stageh in stagehelpers.Where(s => s.pos == true).ToList())
            {
                if (stageh.mropos == false)
                {
                    _logger.Debug($"Generating negative MRO report for {stageh.donor.DonorId} - {stageh.specimen_id}");
                }
                else
                {
                    _logger.Debug($"Generating positive MRO report for {stageh.donor.DonorId} - {stageh.specimen_id}");
                }
                generateMROReport(stageh);
            }
            return true;
        }

        public bool ReleaseDocs()
        {
            // flag all positive reports as released

            var sqlQuery = @"
UPDATE surpathlive.backend_integration_partner_release
SET released = 1, last_modified_by = 'HL7Stage', released_by = 'HL7Stage'
WHERE released = 0
;

";
            sqlQuery = @"
UPDATE surpathlive.backend_integration_partner_release h1
inner join (select max(h2.backend_integration_partner_release_id) as maxid, h2.donor_test_info_id from backend_integration_partner_release h2) h3 on h3.maxid = h1.backend_integration_partner_release_id
SET h1.released = 1, h1.last_modified_by = 'HL7Stage', h1.released_by = 'HL7Stage' ;

";

            if (this.conn.State == ConnectionState.Closed) conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
            {
                int _rowcount = cmd.ExecuteNonQuery();
                _logger.Debug($"{_rowcount} records released by staging");
            }
            ;

            return true;
        }

        public bool ReleaseNegative(StageHelper stageh)
        {
            return true;
        }

        public void generateMROReport(StageHelper stageh)
        {
            var file_name = this.StageFilesFolder + "mro_template.rpt";
            var mro_pos_file_name = this.StageFilesFolder + "mro_pos_template.pdf";
            var mro_neg_file_name = this.StageFilesFolder + "mro_neg_template.pdf";

            if (!File.Exists(file_name))
            {
                _logger.Error($"MRO template file {file_name} does not exist.");
                return;
            }

            var result = File.ReadAllText(file_name);

            result = fixuprpt(stageh, result, stageh.mropos);

            var new_file_name = this.MROReportFileInboundPath + $"{stageh.specimen_id}.rpt";
            File.WriteAllText(new_file_name, result);
            _logger.Debug($"MRO File {new_file_name} created");

            // generate a PDF
            var new_mro_file_name = this.MROReportFileInboundPath + $"{stageh.specimen_id}.pdf";
            if (stageh.pos == true)
            {
                File.Copy(mro_pos_file_name, new_mro_file_name);
            }
            else
            {
                File.Copy(mro_neg_file_name, new_mro_file_name);
            }
        }

        public void generateLabReport(StageHelper stageh)
        {
            var file_name = this.StageFilesFolder + "lab_template.rpt";

            // PIDPIDPID
            // FNAMEFNAMEFNAME
            // LNAMELNAMELNAME

            if (!File.Exists(file_name))
            {
                _logger.Error($"Lab template file {file_name} does not exist.");
                return;
            }

            var result = File.ReadAllText(file_name);

            result = fixuprpt(stageh, result, stageh.pos);

            var new_file_name = this.LabReportFilePath + $"{stageh.specimen_id}.rpt";
            File.WriteAllText(new_file_name, result);
            _logger.Debug($"Lab file {new_file_name} created");
        }

        public string fixuprpt(StageHelper stageh, string result, bool posneg)
        {
            var RESRESRES = "NEGATIVE";
            if (posneg == true)
            {
                RESRESRES = "POSITIVE";
            }

            result = result.Replace(labreplacekeys.PIDPIDPID.ToString(), stageh.donor.PidTypeValues.Where(p => p.PIDType == stageh.pidtypevalue.PIDType).First().PIDValue);
            result = result.Replace(labreplacekeys.FNAMEFNAMEFNAME.ToString(), stageh.donor.DonorFirstName);
            result = result.Replace(labreplacekeys.LNAMELNAMELNAME.ToString(), stageh.donor.DonorLastName);
            result = result.Replace(labreplacekeys.MIMIMI.ToString(), stageh.donor.DonorMI);
            result = result.Replace(labreplacekeys.LABLABLAB.ToString(), stageh.lab_code);
            //var SPECSPECSPEC = rsg.Generate(10);
            result = result.Replace(labreplacekeys.SPECSPECSPEC.ToString(), stageh.specimen_id);
            //var SAMPSAMPSAMP = rsg.Generate(8);
            result = result.Replace(labreplacekeys.SAMPSAMPSAMP.ToString(), stageh.sample_id);
            //var RESRESRES = "NEGATIVE";
            //if (stageh.pos == true)
            //{
            //    RESRESRES = "POSITIVE";
            //}
            result = result.Replace(labreplacekeys.RESRESRES.ToString(), RESRESRES);
            result = result.Replace(labreplacekeys.RES2RES2RES2.ToString(), RESRESRES.Substring(0, 3));
            result = result.Replace(labreplacekeys.DATEDATEDATE.ToString(), DateTime.Now.ToString("MM/dd/yy"));
            result = result.Replace(labreplacekeys.DATE2DATE2DATE2.ToString(), DateTime.Now.ToString("dd-MMM-yyyy").ToUpper());
            result = result.Replace(labreplacekeys.DATE3DATE3DATE3.ToString(), DateTime.Now.ToString("yyyyMMddHHmm").ToUpper());

            return result;
        }
    }

    public enum labreplacekeys
    {
        [Description("PIDPIDPID")]
        PIDPIDPID,

        [Description("FNAMEFNAMEFNAME")]
        FNAMEFNAMEFNAME,

        [Description("LNAMELNAMELNAME")]
        LNAMELNAMELNAME,

        [Description("MIMIMI")]
        MIMIMI,

        [Description("SPECSPECSPEC")]
        SPECSPECSPEC,

        [Description("SAMPSAMPSAMP")]
        SAMPSAMPSAMP,

        [Description("RESRESRES")]
        RESRESRES,

        [Description("RES2RES2RES2")]
        RES2RES2RES2,

        [Description("DATEDATEDATE")] //08/06/19
        DATEDATEDATE,

        [Description("DATE2DATE2DATE2")] // 08-Aug-2019
        DATE2DATE2DATE2,

        [Description("DATE3DATE3DATE3")] // 202106221231
        DATE3DATE3DATE3,

        [Description("LABLABLAB")] // LAB CODE
        LABLABLAB
    }

    public class StageHelper
    {
        public Donor donor { get; set; }
        public int donor_id { get; set; }
        public int donor_test_info_id { get; set; }
        public bool pos { get; set; } = false;
        public bool mropos { get; set; } = false;
        public string lab_code { get; set; }
        public string specimen_id { get; set; }
        public string sample_id { get; set; }
        public PIDTypeValue pidtypevalue;
    }
}