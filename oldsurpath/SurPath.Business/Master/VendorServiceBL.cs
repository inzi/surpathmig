using SurPath.Data.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business.Master
{
    public class VendorServiceBL : BusinessObject
    {
        private VendorServiceDao vendorServiceDao = new VendorServiceDao();

        public int Save(VendorService vendorService)
        {
            if (vendorService.VendorServiceId == 0)
            {
                return vendorServiceDao.Insert(vendorService);
            }
            else
            {
                return vendorServiceDao.Update(vendorService);
            }
        }

        public int Delete(int vendorServiceId, string currentUserName)
        {
            return vendorServiceDao.Delete(vendorServiceId, currentUserName);
        }

        public VendorService Get(int vendorServiceId)
        {
            try
            {
                VendorService vendorService = vendorServiceDao.Get(vendorServiceId);

                return vendorService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VendorService GetVendorServiceByCostParam(int vendorId, int testCategoryId, YesNo observedType, SpecimenFormType formType)
        {
            try
            {
                VendorService vendorService = vendorServiceDao.GetVendorServiceByCostParam(vendorId, testCategoryId, observedType, formType);

                return vendorService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByVendorServiceName(int vendorId, string vendorServiceName)
        {
            try
            {
                DataTable vendorServices = vendorServiceDao.GetByVendorServiceName(vendorId, vendorServiceName);

                return vendorServices;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorService> GetList(int vendorId)
        {
            try
            {
                DataTable dtVendorServices = vendorServiceDao.GetList(vendorId);

                List<VendorService> vendorServiceList = new List<VendorService>();

                foreach (DataRow dr in dtVendorServices.Rows)
                {
                    VendorService vendorService = new VendorService();

                    vendorService.VendorServiceId = (int)dr["VendorServiceId"];
                    vendorService.VendorServiceNameValue = dr["VendorServiceNameValue"].ToString();
                    vendorService.Cost = double.Parse(dr["Cost"].ToString());
                    vendorService.TestCategoryId = dr["TestCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["TestCategoryId"]) : 0;
                    vendorService.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    vendorService.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    vendorService.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    vendorService.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendorService.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendorService.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendorService.CreatedBy = (string)dr["CreatedBy"];
                    vendorService.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendorService.LastModifiedBy = (string)dr["LastModifiedBy"];

                    vendorServiceList.Add(vendorService);
                }

                return vendorServiceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorService> GetList(int vendorId, Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtVendorServices = vendorServiceDao.GetList(vendorId, searchParam);

                List<VendorService> vendorServiceList = new List<VendorService>();

                foreach (DataRow dr in dtVendorServices.Rows)
                {
                    VendorService vendorService = new VendorService();

                    vendorService.VendorServiceId = (int)dr["VendorServiceId"];
                    vendorService.VendorServiceNameValue = dr["VendorServiceNameValue"].ToString();
                    vendorService.Cost = double.Parse(dr["Cost"].ToString());
                    vendorService.TestCategoryId = dr["TestCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["TestCategoryId"]) : 0;
                    vendorService.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    vendorService.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    vendorService.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    vendorService.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendorService.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendorService.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendorService.CreatedBy = (string)dr["CreatedBy"];
                    vendorService.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendorService.LastModifiedBy = (string)dr["LastModifiedBy"];

                    vendorServiceList.Add(vendorService);
                }

                return vendorServiceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorService> Sorting(int vendorId, string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtVendorService = vendorServiceDao.sorting(vendorId, searchparam, active, getInActive);

                List<VendorService> vendorServiceList = new List<VendorService>();

                foreach (DataRow dr in dtVendorService.Rows)
                {
                    VendorService vendorService = new VendorService();

                    vendorService.VendorServiceId = (int)dr["VendorServiceId"];
                    vendorService.VendorServiceNameValue = dr["VendorServiceNameValue"].ToString();
                    vendorService.Cost = double.Parse(dr["Cost"].ToString());
                    vendorService.TestCategoryId = dr["TestCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["TestCategoryId"]) : 0;
                    vendorService.IsObserved = dr["IsObserved"] != DBNull.Value ? (YesNo)(Convert.ToInt32(dr["IsObserved"].ToString())) : YesNo.None;
                    vendorService.FormTypeId = dr["FormTypeId"] != DBNull.Value ? (SpecimenFormType)(Convert.ToInt32(dr["FormTypeId"].ToString())) : SpecimenFormType.None;
                    vendorService.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    vendorService.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    vendorService.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    vendorService.CreatedOn = (DateTime)dr["CreatedOn"];
                    vendorService.CreatedBy = (string)dr["CreatedBy"];
                    vendorService.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    vendorService.LastModifiedBy = (string)dr["LastModifiedBy"];

                    vendorServiceList.Add(vendorService);
                }

                return vendorServiceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}