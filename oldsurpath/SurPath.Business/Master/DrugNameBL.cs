// ===============================================================================
// DrugNameBL.cs
//
// This file contains the methods to perform Drug Name related business process.
// ===============================================================================
// Release history
// VERSION	DESCRIPTION
//
// ===============================================================================
// Copyright (C) 2014 SaaSWorks Technologies Pvt. Ltd.
// http://www.saasworksit.com
// All rights reserved.
// ==============================================================================

using SurPath.Data.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business.Master
{
    /// <summary>
    /// Drug Name related business process.
    /// </summary>
    public class DrugNameBL : BusinessObject
    {
        private DrugNameDao drugNameDao = new DrugNameDao();

        /// <summary>
        /// Save the Drug Name information to the database.
        /// </summary>
        /// <param name="drugName">Drug Name information which one need to be added to the database.</param>
        /// <returns>Returns DrugNameId, the auto increament value.</returns>
        public int Save(DrugName drugName)
        {
            if (drugName.DrugNameId == 0)
            {
                return drugNameDao.Insert(drugName);
            }
            else
            {
                return drugNameDao.Update(drugName);
            }
        }

        /// <summary>
        /// Deletes the Drug Name information from database.
        /// </summary>
        /// <param name="drugNameIdList">DrugNameId list using comma sepertor.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int Delete(int drugNameId, string currentUserName)
        {
            return drugNameDao.Delete(drugNameId, currentUserName);
        }

        /// <summary>
        /// Get the Drug Name information by DrugNameId
        /// </summary>
        /// <param name="drugNameId">DrugNameId which one need to be get from the database.</param>
        /// <returns>Returns Drug Name information.</returns>
        public DrugName Get(int drugNameId)
        {
            try
            {
                DrugName drugName = drugNameDao.Get(drugNameId);

                return drugName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the Drug information by Drug Name
        /// </summary>
        /// <param name="drugName">Drug Name which one need to be get from the database.</param>
        /// <returns>Returns Drug information.</returns>
        public DataTable GetByDrugName(string drugName)
        {
            try
            {
                DataTable drugNames = drugNameDao.GetByDrugName(drugName);

                return drugNames;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Drug Name informations.
        /// </summary>
        /// <returns>Returns Drug Name information list.</returns>
        public List<DrugName> GetList()
        {
            try
            {
                DataTable dtDrugNames = drugNameDao.GetList();

                List<DrugName> drugNameList = new List<DrugName>();

                foreach (DataRow dr in dtDrugNames.Rows)
                {
                    DrugName drugName = new DrugName();

                    drugName.DrugNameId = (int)dr["DrugNameId"];
                    drugName.DrugNameValue = dr["DrugNameValue"].ToString();
                    if (dr["DrugCodeValue"].ToString() == string.Empty)
                    {
                        drugName.DrugCodeValue = string.Empty;
                    }
                    else
                    {
                        drugName.DrugCodeValue = dr["DrugCodeValue"].ToString();
                    }
                    if (dr["UAScreenValue"].ToString() != string.Empty)
                    {
                        drugName.UAScreenValue = dr["UAScreenValue"].ToString() + " " + dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAScreenValue = string.Empty;
                    }
                    if (dr["UAConfirmationValue"].ToString() != string.Empty)
                    {
                        drugName.UAConfirmationValue = dr["UAConfirmationValue"].ToString() + " " + dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAConfirmationValue = string.Empty;
                    }
                    if (dr["HairScreenValue"].ToString() != string.Empty)
                    {
                        drugName.HairScreenValue = dr["HairScreenValue"].ToString() + " " + dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairScreenValue = string.Empty;
                    }
                    if (dr["HairConfirmationValue"].ToString() != string.Empty)
                    {
                        drugName.HairConfirmationValue = dr["HairConfirmationValue"].ToString() + " " + dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairConfirmationValue = string.Empty;
                    }

                    if (dr["UAUnitOfMeasurement"].ToString() != string.Empty)
                    {
                        drugName.UAUnitOfMeasurement = dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAUnitOfMeasurement = string.Empty;
                    }
                    if (dr["HairUnitOfMeasurement"].ToString() != string.Empty)
                    {
                        drugName.HairUnitOfMeasurement = dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairUnitOfMeasurement = string.Empty;
                    }
                    // drugName.HairUnitOfMeasurement = dr["UnitOfMeasurement"].ToString();
                    drugName.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    drugName.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    drugName.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    drugName.CreatedOn = (DateTime)dr["CreatedOn"];
                    drugName.CreatedBy = (string)dr["CreatedBy"];
                    drugName.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    drugName.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNameTestCatgories = drugNameDao.GetDrugNameTestCategories(drugName.DrugNameId);
                    if (dtDrugNameTestCatgories != null)
                    {
                        foreach (DataRow drTestCategory in dtDrugNameTestCatgories.Rows)
                        {
                            if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                drugName.IsUA = true;
                            }
                            else if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                drugName.IsHair = true;
                            }
                        }
                    }
                    else
                    {
                        drugName.IsUA = false;
                        drugName.IsHair = false;
                    }

                    drugNameList.Add(drugName);
                }

                return drugNameList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Drug Name informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Drug Name information list.</returns>
        public List<DrugName> GetList(SurPath.Enum.TestCategories testCategory)
        {
            try
            {
                DataTable dtDrugNames = drugNameDao.GetList(testCategory);

                List<DrugName> drugNameList = new List<DrugName>();

                foreach (DataRow dr in dtDrugNames.Rows)
                {
                    DrugName drugName = new DrugName();

                    drugName.DrugNameId = (int)dr["DrugNameId"];
                    drugName.DrugNameValue = dr["DrugNameValue"].ToString();
                    drugName.DrugCodeValue = (string)dr["DrugCodeValue"];
                    drugName.UAScreenValue = dr["UAScreenValue"].ToString();
                    drugName.UAConfirmationValue = dr["UAConfirmationValue"].ToString();
                    drugName.HairScreenValue = dr["HairScreenValue"].ToString();
                    drugName.HairConfirmationValue = dr["HairConfirmationValue"].ToString();
                    drugName.UAUnitOfMeasurement = dr["UAUnitOfMeasurement"].ToString();
                    drugName.HairUnitOfMeasurement = dr["HairUnitOfMeasurement"].ToString();
                    drugName.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    drugName.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    drugName.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    drugName.CreatedOn = (DateTime)dr["CreatedOn"];
                    drugName.CreatedBy = (string)dr["CreatedBy"];
                    drugName.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    drugName.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNameTestCatgories = drugNameDao.GetDrugNameTestCategories(drugName.DrugNameId);
                    if (dtDrugNameTestCatgories != null)
                    {
                        foreach (DataRow drTestCategory in dtDrugNameTestCatgories.Rows)
                        {
                            if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                drugName.IsUA = true;
                            }
                            else if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                drugName.IsHair = true;
                            }
                        }
                    }
                    else
                    {
                        drugName.IsUA = false;
                        drugName.IsHair = false;
                    }

                    drugNameList.Add(drugName);
                }

                return drugNameList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Drug Name informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Drug Name information list.</returns>
        public List<DrugName> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtDrugNames = drugNameDao.GetList(searchParam);

                List<DrugName> drugNameList = new List<DrugName>();

                foreach (DataRow dr in dtDrugNames.Rows)
                {
                    DrugName drugName = new DrugName();

                    drugName.DrugNameId = (int)dr["DrugNameId"];
                    drugName.DrugNameValue = dr["DrugNameValue"].ToString();
                    drugName.DrugCodeValue = (string)dr["DrugCodeValue"];
                    if (dr["UAScreenValue"].ToString() != string.Empty)
                    {
                        drugName.UAScreenValue = dr["UAScreenValue"].ToString() + " " + dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAScreenValue = string.Empty;
                    }
                    if (dr["UAConfirmationValue"].ToString() != string.Empty)
                    {
                        drugName.UAConfirmationValue = dr["UAConfirmationValue"].ToString() + " " + dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAConfirmationValue = string.Empty;
                    }
                    if (dr["HairScreenValue"].ToString() != string.Empty)
                    {
                        drugName.HairScreenValue = dr["HairScreenValue"].ToString() + " " + dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairScreenValue = string.Empty;
                    }
                    if (dr["HairConfirmationValue"].ToString() != string.Empty)
                    {
                        drugName.HairConfirmationValue = dr["HairConfirmationValue"].ToString() + " " + dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairConfirmationValue = string.Empty;
                    }

                    drugName.UAUnitOfMeasurement = dr["UAUnitOfMeasurement"].ToString();
                    drugName.HairUnitOfMeasurement = dr["HairUnitOfMeasurement"].ToString();
                    drugName.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    drugName.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    drugName.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    drugName.CreatedOn = (DateTime)dr["CreatedOn"];
                    drugName.CreatedBy = (string)dr["CreatedBy"];
                    drugName.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    drugName.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNameTestCatgories = drugNameDao.GetDrugNameTestCategories(drugName.DrugNameId);
                    if (dtDrugNameTestCatgories != null)
                    {
                        foreach (DataRow drTestCategory in dtDrugNameTestCatgories.Rows)
                        {
                            if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                drugName.IsUA = true;
                            }
                            else if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                drugName.IsHair = true;
                            }
                        }
                    }
                    else
                    {
                        drugName.IsUA = false;
                        drugName.IsHair = false;
                    }

                    drugNameList.Add(drugName);
                }

                return drugNameList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DrugName> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtDrugs = drugNameDao.sorting(searchparam, active, getInActive);

                List<DrugName> drugList = new List<DrugName>();

                foreach (DataRow dr in dtDrugs.Rows)
                {
                    DrugName drugName = new DrugName();
                    drugName.DrugNameId = (int)dr["DrugNameId"];
                    drugName.DrugNameValue = dr["DrugNameValue"].ToString();
                    if (dr["DrugCodeValue"].ToString() == string.Empty)
                    {
                        drugName.DrugCodeValue = string.Empty;
                    }
                    else
                    {
                        drugName.DrugCodeValue = dr["DrugCodeValue"].ToString();
                    }
                    if (dr["UAScreenValue"].ToString() != string.Empty)
                    {
                        drugName.UAScreenValue = dr["UAScreenValue"].ToString() + " " + dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAScreenValue = string.Empty;
                    }
                    if (dr["UAConfirmationValue"].ToString() != string.Empty)
                    {
                        drugName.UAConfirmationValue = dr["UAConfirmationValue"].ToString() + " " + dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAConfirmationValue = string.Empty;
                    }
                    if (dr["HairScreenValue"].ToString() != string.Empty)
                    {
                        drugName.HairScreenValue = dr["HairScreenValue"].ToString() + " " + dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairScreenValue = string.Empty;
                    }
                    if (dr["HairConfirmationValue"].ToString() != string.Empty)
                    {
                        drugName.HairConfirmationValue = dr["HairConfirmationValue"].ToString() + " " + dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairConfirmationValue = string.Empty;
                    }

                    if (dr["UAUnitOfMeasurement"].ToString() != string.Empty)
                    {
                        drugName.UAUnitOfMeasurement = dr["UAUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.UAUnitOfMeasurement = string.Empty;
                    }
                    if (dr["HairUnitOfMeasurement"].ToString() != string.Empty)
                    {
                        drugName.HairUnitOfMeasurement = dr["HairUnitOfMeasurement"].ToString();
                    }
                    else
                    {
                        drugName.HairUnitOfMeasurement = string.Empty;
                    }
                    // drugName.HairUnitOfMeasurement = dr["UnitOfMeasurement"].ToString();
                    drugName.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    drugName.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    drugName.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    drugName.CreatedOn = (DateTime)dr["CreatedOn"];
                    drugName.CreatedBy = (string)dr["CreatedBy"];
                    drugName.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    drugName.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNameTestCatgories = drugNameDao.GetDrugNameTestCategories(drugName.DrugNameId);
                    if (dtDrugNameTestCatgories != null)
                    {
                        foreach (DataRow drTestCategory in dtDrugNameTestCatgories.Rows)
                        {
                            if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.UA).ToString())
                            {
                                drugName.IsUA = true;
                            }
                            else if (drTestCategory["TestCategoryId"].ToString() == ((int)TestCategories.Hair).ToString())
                            {
                                drugName.IsHair = true;
                            }
                        }
                    }
                    else
                    {
                        drugName.IsUA = false;
                        drugName.IsHair = false;
                    }

                    drugList.Add(drugName);
                }

                return drugList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}