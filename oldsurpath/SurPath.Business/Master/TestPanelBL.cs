using SurPath.Data.Master;
using SurPath.Entity.Master;
using SurPath.Enum;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurPath.Business.Master
{
    /// <summary>
    /// Test Panel related business process.
    /// </summary>
    ///
    public class TestPanelBL : BusinessObject
    {
        private TestPanelDao testPanelDao = new TestPanelDao();

        public int Save(TestPanel testPanel)
        {
            if (testPanel.TestPanelId == 0)
            {
                return testPanelDao.Insert(testPanel);
            }
            else
            {
                return testPanelDao.Update(testPanel);
            }
        }

        public int Delete(int testPanelId, string currentUserName)
        {
            return testPanelDao.Delete(testPanelId, currentUserName);
        }

        public int TestPanelActive(int testPanelId)
        {
            return testPanelDao.TestPanelActive(testPanelId);
        }

        public int UnmapTestPanel(TestPanel testPanel)
        {
            return testPanelDao.UnmapTestPanel(testPanel);
        }

        public TestPanel Get(int testPanelId)
        {
            try
            {
                TestPanel testPanel = testPanelDao.Get(testPanelId);

                return testPanel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByTestPanelCode(string PanelName)
        {
            try
            {
                DataTable testPanels = testPanelDao.GetByTestPanelCode(PanelName);

                return testPanels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestPanel> GetList()
        {
            try
            {
                DataTable dtTestPanel = testPanelDao.GetList();

                List<TestPanel> testPanelList = new List<TestPanel>();

                foreach (DataRow dr in dtTestPanel.Rows)
                {
                    TestPanel testPanel = new TestPanel();

                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = dr["TestPanelName"].ToString();
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNames = testPanelDao.GetDrugNameList(testPanel.TestPanelId);

                    if (dtDrugNames != null)
                    {
                        foreach (DataRow drDrugName in dtDrugNames.Rows)
                        {
                            testPanel.DrugNames.Add((int)drDrugName["DrugNameId"]);
                        }
                    }

                    testPanelList.Add(testPanel);
                }

                return testPanelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestPanel> GetListByCatgory(TestCategories testCategory)
        {
            try
            {
                DataTable dtTestPanel = testPanelDao.GetListByCatgory(testCategory);

                List<TestPanel> testPanelList = new List<TestPanel>();

                foreach (DataRow dr in dtTestPanel.Rows)
                {
                    TestPanel testPanel = new TestPanel();

                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = dr["TestPanelName"].ToString();
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNames = testPanelDao.GetDrugNameList(testPanel.TestPanelId);

                    if (dtDrugNames != null)
                    {
                        foreach (DataRow drDrugName in dtDrugNames.Rows)
                        {
                            testPanel.DrugNames.Add((int)drDrugName["DrugNameId"]);
                        }
                    }

                    testPanelList.Add(testPanel);
                }

                return testPanelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestPanel> GetListByUA()
        {
            try
            {
                DataTable dtTestPanel = testPanelDao.GetListByUA();

                List<TestPanel> testPanelList = new List<TestPanel>();

                foreach (DataRow dr in dtTestPanel.Rows)
                {
                    TestPanel testPanel = new TestPanel();

                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = dr["TestPanelName"].ToString();
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNames = testPanelDao.GetDrugNameList(testPanel.TestPanelId);

                    if (dtDrugNames != null)
                    {
                        foreach (DataRow drDrugName in dtDrugNames.Rows)
                        {
                            testPanel.DrugNames.Add((int)drDrugName["DrugNameId"]);
                        }
                    }

                    testPanelList.Add(testPanel);
                }

                return testPanelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestPanel> GetListByHair()
        {
            try
            {
                DataTable dtTestPanel = testPanelDao.GetListByHair();

                List<TestPanel> testPanelList = new List<TestPanel>();

                foreach (DataRow dr in dtTestPanel.Rows)
                {
                    TestPanel testPanel = new TestPanel();

                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = dr["TestPanelName"].ToString();
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNames = testPanelDao.GetDrugNameList(testPanel.TestPanelId);

                    if (dtDrugNames != null)
                    {
                        foreach (DataRow drDrugName in dtDrugNames.Rows)
                        {
                            testPanel.DrugNames.Add((int)drDrugName["DrugNameId"]);
                        }
                    }

                    testPanelList.Add(testPanel);
                }

                return testPanelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestPanel> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtTestPanel = testPanelDao.GetList(searchParam);

                List<TestPanel> testPanelList = new List<TestPanel>();

                foreach (DataRow dr in dtTestPanel.Rows)
                {
                    TestPanel testPanel = new TestPanel();

                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = dr["TestPanelName"].ToString();
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNames = testPanelDao.GetDrugNameList(testPanel.TestPanelId);

                    if (dtDrugNames != null)
                    {
                        foreach (DataRow drDrugName in dtDrugNames.Rows)
                        {
                            testPanel.DrugNames.Add((int)drDrugName["DrugNameId"]);
                        }
                    }

                    testPanelList.Add(testPanel);
                }

                return testPanelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TestPanel> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dttestpanel = testPanelDao.sorting(searchparam, active, getInActive);

                List<TestPanel> testPanelList = new List<TestPanel>();

                foreach (DataRow dr in dttestpanel.Rows)
                {
                    TestPanel testPanel = new TestPanel();

                    testPanel.TestPanelId = (int)dr["TestPanelId"];
                    testPanel.TestPanelName = dr["TestPanelName"].ToString();
                    testPanel.TestPanelDescription = dr["TestPanelDescription"].ToString();
                    testPanel.TestCategoryId = (int)dr["TestCategoryId"];
                    testPanel.TestCost = (double)dr["TestCost"];
                    testPanel.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    testPanel.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    testPanel.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    testPanel.CreatedOn = (DateTime)dr["CreatedOn"];
                    testPanel.CreatedBy = (string)dr["CreatedBy"];
                    testPanel.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    testPanel.LastModifiedBy = (string)dr["LastModifiedBy"];

                    DataTable dtDrugNames = testPanelDao.GetDrugNameList(testPanel.TestPanelId);

                    if (dtDrugNames != null)
                    {
                        foreach (DataRow drDrugName in dtDrugNames.Rows)
                        {
                            testPanel.DrugNames.Add((int)drDrugName["DrugNameId"]);
                        }
                    }

                    testPanelList.Add(testPanel);
                }

                return testPanelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}