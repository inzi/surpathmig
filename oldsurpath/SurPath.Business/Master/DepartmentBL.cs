// ===============================================================================
// DepartmentBL.cs
//
// This file contains the methods to perform Department related business process.
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
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business.Master
{
    /// <summary>
    /// Department related business process.
    /// </summary>
    ///

    public class DepartmentBL : BusinessObject
    {
        private DepartmentDao departmentDao = new DepartmentDao();

        /// <summary>
        /// Save the Department information to the database.
        /// </summary>
        /// <param name="department">Department information which one need to be added to the database.</param>
        /// <returns>Returns DepartmentId, the auto increament value.</returns>
        public int Save(Department department)
        {
            if (department.DepartmentId == 0)
            {
                return departmentDao.Insert(department);
            }
            else
            {
                return departmentDao.Update(department);
            }
        }

        /// <summary>
        /// Deletes the Department information from database.
        /// </summary>
        /// <param name="departmentId">DepartmentId list using comma sepertor.</param>
        /// <returns>Returns number of records deleted from the database.</returns>
        public int Delete(int departmentId, string currentUserName)
        {
            return departmentDao.Delete(departmentId, currentUserName);
        }

        /// <summary>
        /// Get the Department information by DepartmentId
        /// </summary>
        /// <param name="departmentId">DepartmentId which one need to be get from the database.</param>
        /// <returns>Returns Department information.</returns>
        public Department Get(int departmentId)
        {
            try
            {
                Department department = departmentDao.Get(departmentId);

                return department;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the Department information by Department Name
        /// </summary>
        /// <param name="departmentName">Department Name which one need to be get from the database.</param>
        /// <returns>Returns Department information.</returns>
        public DataTable GetByDepartmentName(string departmentName)
        {
            try
            {
                DataTable department = departmentDao.GetByDepartmentName(departmentName);

                return department;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Department informations.
        /// </summary>
        /// <returns>Returns Department information list.</returns>
        public List<Department> GetList()
        {
            try
            {
                DataTable dtDepartments = departmentDao.GetList();

                List<Department> departmentList = new List<Department>();
                foreach (DataRow dr in dtDepartments.Rows)
                {
                    Department department = new Department();

                    department.DepartmentId = (int)dr["DepartmentId"];
                    department.DepartmentNameValue = dr["DepartmentNameValue"].ToString();
                    department.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    department.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    department.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    department.CreatedOn = (DateTime)dr["CreatedOn"];
                    department.CreatedBy = (string)dr["CreatedBy"];
                    department.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    department.LastModifiedBy = (string)dr["LastModifiedBy"];

                    departmentList.Add(department);
                }

                return departmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all the Department informations based on the search criteria.
        /// </summary>
        /// <param name="searchParam">Collection of search criteria</param>
        /// <returns>Returns Department information list.</returns>
        public List<Department> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtDepartments = departmentDao.GetList(searchParam);

                List<Department> departmentList = new List<Department>();

                foreach (DataRow dr in dtDepartments.Rows)
                {
                    Department department = new Department();

                    department.DepartmentId = (int)dr["DepartmentId"];
                    department.DepartmentNameValue = dr["DepartmentNameValue"].ToString();
                    department.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    department.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    department.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    department.CreatedOn = (DateTime)dr["CreatedOn"];
                    department.CreatedBy = (string)dr["CreatedBy"];
                    department.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    department.LastModifiedBy = (string)dr["LastModifiedBy"];

                    departmentList.Add(department);
                }

                return departmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Department> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtDepartment = departmentDao.sorting(searchparam, active, getInActive);

                List<Department> departmentList = new List<Department>();

                foreach (DataRow dr in dtDepartment.Rows)
                {
                    Department department = new Department();

                    department.DepartmentId = (int)dr["DepartmentId"];
                    department.DepartmentNameValue = dr["DepartmentNameValue"].ToString();
                    department.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    department.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    department.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    department.CreatedOn = (DateTime)dr["CreatedOn"];
                    department.CreatedBy = (string)dr["CreatedBy"];
                    department.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    department.LastModifiedBy = (string)dr["LastModifiedBy"];

                    departmentList.Add(department);
                }

                return departmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}