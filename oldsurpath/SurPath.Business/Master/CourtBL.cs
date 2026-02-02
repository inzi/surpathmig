// ===============================================================================
// CourtBL.cs
//
// This file contains the methods to perform Court related business process.
// ===============================================================================
// Release history
// VERSION	DESCRIPTION
//
// ===============================================================================
// Copyright (C) 2014 SaaSWorks Technologies Pvt. Ltd.
// http://www.saasworksit.com
// All rights reserved.
// ==============================================================================

using SurPath.Data;
using SurPath.Data.Master;
using SurPath.Entity;
using SurPath.Entity.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace SurPath.Business.Master
{
    /// <summary>
    /// Court related business process.
    /// </summary>
    public class CourtBL : BusinessObject
    {
        private CourtDao courtDao = new CourtDao();
        private Regex regexEmail = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        public int Save(Court court)
        {
            if (court.CourtId == 0)
            {
                //Username / Email validation
                UserDao userDao = new UserDao();
                User user = userDao.GetByUsernameOrEmail(court.CourtUsername);
                if (user != null)
                {
                    throw new Exception("The Username already exists.");
                }
                //

                // RandomStringGenerator rsg = new RandomStringGenerator(true, false, true, false);
                //  court.CourtPassword = rsg.Generate(6, 8);
                string newPasswordEncrypted = UserAuthentication.Encrypt(court.CourtPassword, true);

                user = new User();

                user.Username = court.CourtUsername;
                user.UserPassword = newPasswordEncrypted;
                user.IsUserActive = true;
                user.UserFirstName = court.CourtName;
                user.UserLastName = null;
                user.UserPhoneNumber = null;
                user.UserFax = null;
                if (regexEmail.IsMatch(court.CourtUsername))
                {
                    user.UserEmail = court.CourtUsername;
                }
                else
                {
                    user.UserEmail = null;
                }
                user.ChangePasswordRequired = true;
                user.UserType = Enum.UserType.Court;
                user.DepartmentId = null;
                user.DonorId = null;
                user.ClientId = null;
                user.VendorId = null;
                user.AttorneyId = null;
                user.CourtId = null;
                user.JudgeId = null;
                user.CreatedBy = court.CreatedBy;
                user.LastModifiedBy = court.LastModifiedBy;

                int courtId = courtDao.Insert(court, user);

                //Send the mail with newPassword and other donor information
                if (court.CourtUsername != string.Empty)
                {
                    if (regexEmail.IsMatch(court.CourtUsername))
                    {
                        try
                        {
                            MailManager mailManager = new MailManager();
                            string mailBody = mailManager.SendRegistrationMail(user, "Registration");
                            if (court.CourtUsername != string.Empty)
                            {
                                mailManager.SendMail(court.CourtUsername, ConfigurationManager.AppSettings["RegistrationMailSubject"].ToString().Trim(), mailBody);
                            }
                        }
                        catch
                        {
                            throw new Exception("Not able to send mail.");
                        }
                    }
                }

                court.CourtId = courtId;
                return courtId;
            }
            else
            {
                return courtDao.Update(court);
            }
        }

        public int Delete(int courtId, string currentUserName)
        {
            return courtDao.Delete(courtId, currentUserName);
        }

        public Court Get(int courtId)
        {
            try
            {
                Court court = courtDao.Get(courtId);

                return court;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByCourtName(string courtName)
        {
            try
            {
                DataTable courts = courtDao.GetByCourtName(courtName);

                return courts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByEmail(string email)
        {
            try
            {
                DataTable courts = courtDao.GetByEmail(email);

                return courts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Court> GetList()
        {
            try
            {
                DataTable dtCourt = courtDao.GetList();

                List<Court> courtList = new List<Court>();
                foreach (DataRow dr in dtCourt.Rows)
                {
                    Court court = new Court();

                    court.CourtId = (int)dr["CourtId"];
                    court.CourtUsername = dr["CourtUsername"].ToString();
                    court.CourtName = dr["CourtName"].ToString();
                    court.CourtAddress1 = dr["CourtAddress1"].ToString();
                    court.CourtAddress2 = dr["CourtAddress2"].ToString();
                    court.CourtCity = dr["CourtCity"].ToString();
                    court.CourtState = dr["CourtState"].ToString();
                    court.CourtZip = dr["CourtZip"].ToString();
                    court.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    court.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    court.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    court.CreatedOn = (DateTime)dr["CreatedOn"];
                    court.CreatedBy = (string)dr["CreatedBy"];
                    court.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    court.LastModifiedBy = (string)dr["LastModifiedBy"];

                    courtList.Add(court);
                }

                return courtList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Court> GetList(Dictionary<string, string> searchParam)
        {
            try
            {
                DataTable dtCourt = courtDao.GetList(searchParam);

                List<Court> courtList = new List<Court>();
                foreach (DataRow dr in dtCourt.Rows)
                {
                    Court court = new Court();

                    court.CourtId = (int)dr["CourtId"];
                    court.CourtUsername = dr["CourtUsername"].ToString();
                    court.CourtName = dr["CourtName"].ToString();
                    court.CourtAddress1 = dr["CourtAddress1"].ToString();
                    court.CourtAddress2 = dr["CourtAddress2"].ToString();
                    court.CourtCity = dr["CourtCity"].ToString();
                    court.CourtState = dr["CourtState"].ToString();
                    court.CourtZip = dr["CourtZip"].ToString();
                    court.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    court.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    court.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    court.CreatedOn = (DateTime)dr["CreatedOn"];
                    court.CreatedBy = (string)dr["CreatedBy"];
                    court.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    court.LastModifiedBy = (string)dr["LastModifiedBy"];

                    courtList.Add(court);
                }

                return courtList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Court> Sorting(string searchparam, bool active, string getInActive = null)
        {
            try
            {
                DataTable dtCourt = courtDao.sorting(searchparam, active, getInActive);

                List<Court> courtList = new List<Court>();

                foreach (DataRow dr in dtCourt.Rows)
                {
                    Court court = new Court();

                    court.CourtId = (int)dr["CourtId"];
                    court.CourtUsername = dr["CourtUsername"].ToString();
                    court.CourtName = dr["CourtName"].ToString();
                    court.CourtAddress1 = dr["CourtAddress1"].ToString();
                    court.CourtAddress2 = dr["CourtAddress2"].ToString();
                    court.CourtCity = dr["CourtCity"].ToString();
                    court.CourtState = dr["CourtState"].ToString();
                    court.CourtZip = dr["CourtZip"].ToString();
                    court.IsActive = dr["IsActive"].ToString() == "1" ? true : false;
                    court.IsSynchronized = dr["IsSynchronized"].ToString() == "1" ? true : false;
                    court.IsArchived = dr["IsArchived"].ToString() == "1" ? true : false;
                    court.CreatedOn = (DateTime)dr["CreatedOn"];
                    court.CreatedBy = (string)dr["CreatedBy"];
                    court.LastModifiedOn = (DateTime)dr["LastModifiedOn"];
                    court.LastModifiedBy = (string)dr["LastModifiedBy"];

                    courtList.Add(court);
                }

                return courtList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}