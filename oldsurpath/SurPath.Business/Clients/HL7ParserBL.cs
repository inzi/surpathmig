using SurPath.Data;
using SurPath.Entity;
using SurPath.Enum;
using System;
using System.Data;

namespace SurPath.Business
{
    public class HL7ParserBL : BusinessObject
    {
        private HL7ParserDao hl7Parser = new HL7ParserDao();

        public int GetReportType(string specimenId)
        {
            try
            {
                int rType = hl7Parser.DetermineLabReportType(specimenId);

                return rType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportInfo GetReportDetails(ReportType reportType, string specimenId, ReportInfo reportDetails)
        {
            try
            {
                
                ReportInfo reportInfo = hl7Parser.GetReportDetails(reportType, specimenId, reportDetails);

                return reportInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ReportInfo GetReportDetailsById(string reportId, ReportInfo reportDetails)
        //{
        //    try
        //    {
        //        ReportInfo reportInfo = hl7Parser.GetReportDetailsById(reportId, reportDetails);

        //        return reportInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetMismatchedData(string mismatchedIds)
        {
            try
            {
                DataTable dtMismatchedData = hl7Parser.GetMismatchedData(mismatchedIds);

                return dtMismatchedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}