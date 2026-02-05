using Surpath.ClearStar.BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surpath.ClearStar.BL
{
    public class BackendBCReport
    {
        public Tuple<string, byte[]> ViewProfileDocument(string sCustID, string profileId)
        {

            //Check Local File 

            var csreports = ConfigurationManager.AppSettings["ClearStarReports"].ToString().Trim();
            try
            {
                var exists = System.IO.File.Exists(csreports + profileId + ".pdf");
                if (!exists)
                {


                    var creds = DefaultCredentialsBL.GetCredentials();

                    string ProfileNo = profileId;

                    Surpath.ClearStar.BL.ProfileBL profile = new ProfileBL();

                    var result = profile.GetProfileReport(sCustID, ProfileNo);

                    var file = profile.SaveProfileReport(sCustID, ProfileNo, result);

                    if (System.IO.File.Exists(file))
                    {
                        var document = System.IO.File.GetAttributes(file);

                    }



                    var strType = System.IO.Path.GetExtension(file).Substring(1);
                    //Response.Clear();
                    //Response.ClearContent();
                    //Response.ClearHeaders();
                    //Response.AddHeader("content-disposition", "attachment; filename=" + file + "." + strType);
                    //Response.ContentType = strType;
                    //this.Response.BinaryWrite();
                    //this.Response.End();
                    return new Tuple<string, byte[]>(file, System.IO.File.ReadAllBytes(file));
                }
                else
                {
                    var file = csreports + profileId + ".pdf";
                    var strType = System.IO.Path.GetExtension(file).Substring(1);
                    //Response.Clear();
                    //Response.ClearContent();
                    //Response.ClearHeaders();
                    //Response.AddHeader("content-disposition", "attachment; filename=" + file + "." + strType);
                    //Response.ContentType = strType;
                    //this.Response.BinaryWrite(System.IO.File.ReadAllBytes(file));
                    //this.Response.End();
                    //return System.IO.File.ReadAllBytes(file);
                    return new Tuple<string, byte[]>(file, System.IO.File.ReadAllBytes(file));
                }
            }
            catch (Exception)
            { }

            return new Tuple<string, byte[]>("", new byte[] { });
        }
    }
}
