using SurPath.Data;
using SurPath.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business
{
    public class ThirdPartyBL : BusinessObject
    {
        private ThirdPartyDao thirdPartyDao = new ThirdPartyDao();

        public int Save(ThirdParty thirdParty)
        {
            if (thirdParty.ThirdPartyId == 0)
            {
                return thirdPartyDao.Insert(thirdParty);
            }
            else
            {
                return thirdPartyDao.Update(thirdParty);
            }
        }

        public int Delete(int thirdPartyId, string currentUserName)
        {
            return thirdPartyDao.Delete(thirdPartyId, currentUserName);
        }

        public ThirdParty Get(int thirdPartyId)
        {
            try
            {
                ThirdParty thirdParty = thirdPartyDao.Get(thirdPartyId);

                return thirdParty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ThirdParty> GetList()
        {
            try
            {
                DataTable dtThirdParty = thirdPartyDao.GetList();

                List<ThirdParty> thirdPartyList = new List<ThirdParty>();
                foreach (DataRow dr in dtThirdParty.Rows)
                {
                    ThirdParty thirdParty = new ThirdParty();
                    thirdParty.DonorId = (int)dr["DonorId"];
                    thirdParty.ThirdPartyId = (int)dr["ThirdPartyId"];
                    thirdParty.ThirdPartyFirstName = dr["ThirdPartyFirstName"].ToString();
                    thirdParty.ThirdPartyLastName = dr["ThirdPartyLastName"].ToString();
                    thirdParty.ThirdPartyAddress1 = dr["ThirdPartyAddress1"].ToString();
                    thirdParty.ThirdPartyAddress2 = dr["ThirdPartyAddress2"].ToString();
                    thirdParty.ThirdPartyCity = dr["ThirdPartyCity"].ToString();
                    thirdParty.ThirdPartyState = dr["ThirdPartyState"].ToString();
                    thirdParty.ThirdPartyZip = dr["ThirdPartyZip"].ToString();
                    thirdParty.ThirdPartyPhone = dr["ThirdPartyPhone"].ToString();
                    thirdParty.ThirdPartyFax = dr["ThirdPartyFax"].ToString();
                    thirdParty.ThirdPartyEmail = dr["ThirdPartyEmail"].ToString();
                    thirdParty.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    thirdParty.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    thirdParty.CreatedOn = (DateTime)dr["CreatedOn"];
                    thirdParty.CreatedBy = (string)dr["CreatedBy"];
                    thirdParty.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    thirdParty.LastModifiedBy = (string)dr["LastModifiedBy"];

                    thirdPartyList.Add(thirdParty);
                }

                return thirdPartyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ThirdParty> GetList(int donorId)
        {
            try
            {
                DataTable dtThirdParty = thirdPartyDao.GetList(donorId);

                List<ThirdParty> thirdPartyList = new List<ThirdParty>();
                foreach (DataRow dr in dtThirdParty.Rows)
                {
                    ThirdParty thirdParty = new ThirdParty();
                    thirdParty.DonorId = (int)dr["DonorId"];
                    thirdParty.ThirdPartyId = (int)dr["ThirdPartyId"];
                    thirdParty.ThirdPartyFirstName = dr["ThirdPartyFirstName"].ToString();
                    thirdParty.ThirdPartyLastName = dr["ThirdPartyLastName"].ToString();
                    thirdParty.ThirdPartyAddress1 = dr["ThirdPartyAddress1"].ToString();
                    thirdParty.ThirdPartyAddress2 = dr["ThirdPartyAddress2"].ToString();
                    thirdParty.ThirdPartyCity = dr["ThirdPartyCity"].ToString();
                    thirdParty.ThirdPartyState = dr["ThirdPartyState"].ToString();
                    thirdParty.ThirdPartyZip = dr["ThirdPartyZip"].ToString();
                    thirdParty.ThirdPartyPhone = dr["ThirdPartyPhone"].ToString();
                    thirdParty.ThirdPartyFax = dr["ThirdPartyFax"].ToString();
                    thirdParty.ThirdPartyEmail = dr["ThirdPartyEmail"].ToString();
                    thirdParty.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    thirdParty.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    thirdParty.CreatedOn = (DateTime)dr["CreatedOn"];
                    thirdParty.CreatedBy = (string)dr["CreatedBy"];
                    thirdParty.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    thirdParty.LastModifiedBy = (string)dr["LastModifiedBy"];

                    thirdPartyList.Add(thirdParty);
                }

                return thirdPartyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ThirdParty> GetList(int donorId, Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtThirdParty = thirdPartyDao.GetList(donorId, searchParam);

                List<ThirdParty> thirdPartyList = new List<ThirdParty>();
                foreach (DataRow dr in dtThirdParty.Rows)
                {
                    ThirdParty thirdParty = new ThirdParty();

                    thirdParty.ThirdPartyId = (int)dr["ThirdPartyId"];
                    thirdParty.ThirdPartyFirstName = dr["ThirdPartyFirstName"].ToString();
                    thirdParty.ThirdPartyLastName = dr["ThirdPartyLastName"].ToString();
                    thirdParty.ThirdPartyAddress1 = dr["ThirdPartyAddress1"].ToString();
                    thirdParty.ThirdPartyAddress2 = dr["ThirdPartyAddress2"].ToString();
                    thirdParty.ThirdPartyCity = dr["ThirdPartyCity"].ToString();
                    thirdParty.ThirdPartyState = dr["ThirdPartyState"].ToString();
                    thirdParty.ThirdPartyZip = dr["ThirdPartyZip"].ToString();
                    thirdParty.ThirdPartyPhone = dr["ThirdPartyPhone"].ToString();
                    thirdParty.ThirdPartyFax = dr["ThirdPartyFax"].ToString();
                    thirdParty.ThirdPartyEmail = dr["ThirdPartyEmail"].ToString();
                    thirdParty.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    thirdParty.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    thirdParty.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    thirdParty.CreatedOn = (DateTime)dr["CreatedOn"];
                    thirdParty.CreatedBy = (string)dr["CreatedBy"];
                    thirdParty.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    thirdParty.LastModifiedBy = (string)dr["LastModifiedBy"];

                    thirdPartyList.Add(thirdParty);
                }

                return thirdPartyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ThirdParty> Sorting(int donorId, string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtThirdParty = thirdPartyDao.sorting(donorId, searchparam, active, getInActive);

                List<ThirdParty> thirdPartyList = new List<ThirdParty>();

                foreach (DataRow dr in dtThirdParty.Rows)
                {
                    ThirdParty thirdParty = new ThirdParty();

                    thirdParty.ThirdPartyId = (int)dr["ThirdPartyId"];
                    thirdParty.ThirdPartyFirstName = dr["ThirdPartyFirstName"].ToString();
                    thirdParty.ThirdPartyLastName = dr["ThirdPartyLastName"].ToString();
                    thirdParty.ThirdPartyAddress1 = dr["ThirdPartyAddress1"].ToString();
                    thirdParty.ThirdPartyAddress2 = dr["ThirdPartyAddress2"].ToString();
                    thirdParty.ThirdPartyCity = dr["ThirdPartyCity"].ToString();
                    thirdParty.ThirdPartyState = dr["ThirdPartyState"].ToString();
                    thirdParty.ThirdPartyZip = dr["ThirdPartyZip"].ToString();
                    thirdParty.ThirdPartyPhone = dr["ThirdPartyPhone"].ToString();
                    thirdParty.ThirdPartyFax = dr["ThirdPartyFax"].ToString();
                    thirdParty.ThirdPartyEmail = dr["ThirdPartyEmail"].ToString();
                    thirdParty.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    thirdParty.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    thirdParty.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    thirdParty.CreatedOn = (DateTime)dr["CreatedOn"];
                    thirdParty.CreatedBy = (string)dr["CreatedBy"];
                    thirdParty.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    thirdParty.LastModifiedBy = (string)dr["LastModifiedBy"];

                    thirdPartyList.Add(thirdParty);
                }

                return thirdPartyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}