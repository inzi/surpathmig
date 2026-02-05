using SurPath.Data.Master;
using SurPath.Entity.Master;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business.Master
{
    public class TestingAuthorityBL : BusinessObject
    {
        private TestingAuthorityDao testingAuthorityDao = new TestingAuthorityDao();

        public int Save(TestingAuthority testingAuthority)
        {
            if (testingAuthority.TestingAuthorityId == 0)
            {
                return testingAuthorityDao.Insert(testingAuthority);
            }
            else
            {
                return testingAuthorityDao.Update(testingAuthority);
            }
        }

        public int Delete(int testingAuthorityId, string currentUserName)
        {
            return testingAuthorityDao.Delete(testingAuthorityId, currentUserName);
        }

        public TestingAuthority Get(int testingAuthorityId)
        {
            try
            {
                TestingAuthority testingAuthority = testingAuthorityDao.Get(testingAuthorityId);

                return testingAuthority;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByTestingAuthorityName(string testingAuthorityName)
        {
            try
            {
                DataTable testingAuthority = testingAuthorityDao.GetByTestingAuthorityName(testingAuthorityName);

                return testingAuthority;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestingAuthority> GetList(string getInActive = null)
        {
            try
            {
                DataTable dtTestingAuthority = testingAuthorityDao.GetList(getInActive);

                List<TestingAuthority> testingAuthorityList = new List<TestingAuthority>();
                foreach (DataRow dr in dtTestingAuthority.Rows)
                {
                    TestingAuthority testingAuthority = new TestingAuthority();

                    testingAuthority.TestingAuthorityId = (int)dr["TestingAuthorityId"];
                    testingAuthority.TestingAuthorityName = dr["TestingAuthorityName"].ToString();
                    testingAuthority.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testingAuthority.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testingAuthority.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testingAuthority.CreatedOn = (DateTime)dr["CreatedOn"];
                    testingAuthority.CreatedBy = (string)dr["CreatedBy"];
                    testingAuthority.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testingAuthority.LastModifiedBy = (string)dr["LastModifiedBy"];

                    testingAuthorityList.Add(testingAuthority);
                }

                return testingAuthorityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestingAuthority> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtTestingAuthority = testingAuthorityDao.GetList(searchParam);

                List<TestingAuthority> testingAuthorityList = new List<TestingAuthority>();

                foreach (DataRow dr in dtTestingAuthority.Rows)
                {
                    TestingAuthority testingAuthority = new TestingAuthority();

                    testingAuthority.TestingAuthorityId = (int)dr["TestingAuthorityId"];
                    testingAuthority.TestingAuthorityName = dr["TestingAuthorityName"].ToString();
                    testingAuthority.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testingAuthority.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testingAuthority.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testingAuthority.CreatedOn = (DateTime)dr["CreatedOn"];
                    testingAuthority.CreatedBy = (string)dr["CreatedBy"];
                    testingAuthority.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testingAuthority.LastModifiedBy = (string)dr["LastModifiedBy"];

                    testingAuthorityList.Add(testingAuthority);
                }

                return testingAuthorityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestingAuthority> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtTestingAuthority = testingAuthorityDao.sorting(searchparam, active, getInActive);

                List<TestingAuthority> testingAuthorityList = new List<TestingAuthority>();

                foreach (DataRow dr in dtTestingAuthority.Rows)
                {
                    TestingAuthority testingAuthority = new TestingAuthority();

                    testingAuthority.TestingAuthorityId = (int)dr["TestingAuthorityId"];
                    testingAuthority.TestingAuthorityName = dr["TestingAuthorityName"].ToString();
                    testingAuthority.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testingAuthority.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testingAuthority.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testingAuthority.CreatedOn = (DateTime)dr["CreatedOn"];
                    testingAuthority.CreatedBy = (string)dr["CreatedBy"];
                    testingAuthority.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testingAuthority.LastModifiedBy = (string)dr["LastModifiedBy"];

                    testingAuthorityList.Add(testingAuthority);
                }

                return testingAuthorityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}