using SurPath.Data;
using SurPath.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business.Master
{
    public class ZipCodeBL : BusinessObject
    {
        private ZipCodeDao zipCodeDao = new ZipCodeDao();

        public int Save(ZipCode zipCode)
        {
            return zipCodeDao.Insert(zipCode);
        }

        public List<ZipCode> GetZip()
        {
            try
            {
                DataTable dtZip = zipCodeDao.GetZip();

                List<ZipCode> zipList = new List<ZipCode>();

                foreach (DataRow drZip in dtZip.Rows)
                {
                    ZipCode zipCode = new ZipCode();

                    zipCode.ZipId = (int)drZip["ZipId"];
                    zipCode.Zip = drZip["Zip"].ToString();
                    zipCode.City = drZip["City"].ToString();
                    zipCode.State = drZip["State"].ToString();

                    zipList.Add(zipCode);
                }
                return zipList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ZipCode> GetState()
        {
            try
            {
                DataTable dtState = zipCodeDao.GetState();

                List<ZipCode> stateList = new List<ZipCode>();

                foreach (DataRow drState in dtState.Rows)
                {
                    ZipCode zipCode = new ZipCode();

                    zipCode.ZipId = (int)drState["ZipId"];
                    zipCode.Zip = drState["Zip"].ToString();
                    zipCode.City = drState["City"].ToString();
                    zipCode.State = drState["State"].ToString();

                    stateList.Add(zipCode);
                }
                return stateList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ZipCode> GetZipcodeForState()
        {
            try
            {
                DataTable dtZip = zipCodeDao.GetZipcodeForState();

                List<ZipCode> zipList = new List<ZipCode>();

                foreach (DataRow drZip in dtZip.Rows)
                {
                    ZipCode zipCode = new ZipCode();

                    zipCode.ZipId = (int)drZip["ZipId"];
                    zipCode.Zip = drZip["Zip"].ToString();
                    zipCode.City = drZip["City"].ToString();
                    zipCode.State = drZip["State"].ToString();

                    zipList.Add(zipCode);
                }
                return zipList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetZipcodeId(int ID)
        {
            try
            {
                DataTable dtZip = zipCodeDao.GetZipcodeById(ID);

                return dtZip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ZipCode> GetZipcodeById(int ID)
        {
            try
            {
                DataTable dtZip = zipCodeDao.GetZipcodeById(ID);

                List<ZipCode> zipList = new List<ZipCode>();

                foreach (DataRow drZip in dtZip.Rows)
                {
                    ZipCode zipCode = new ZipCode();

                    zipCode.ZipId = (int)drZip["ZipId"];
                    zipCode.Zip = drZip["Zip"].ToString();
                    zipCode.City = drZip["City"].ToString();
                    zipCode.State = drZip["State"].ToString();

                    zipList.Add(zipCode);
                }
                return zipList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ZipCode Get(int zipid)
        {
            try
            {
                ZipCode ZipCodes = zipCodeDao.Get(zipid);

                return ZipCodes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NearestZipCode LoadZip(string donorzip)
        {
            try
            {
                NearestZipCode nearestZipCode = zipCodeDao.LoadZip(donorzip);

                return nearestZipCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}