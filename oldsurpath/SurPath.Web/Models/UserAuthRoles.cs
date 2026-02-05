using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SurPathWeb.Models
{
    public class UserAuthRole
    {
        public int UserId { get; set; }
        public int AuthRuleId { get; set; }
        public string AuthRuleName { get; set; }
        public string AuthRuleInternalName { get; set; }
        public int AuthRuleCategoryId { get; set; }
        public string AuthRuleCategoryName { get; set; }
        public string AuthRuleCategoryInternalName { get; set; }


    }
    public class UserAuthRoles
    {
        public List<UserAuthRole> Roles { get; set; }
        public void FromDataTable(DataTable _dt)
        {
            this.Roles = new List<UserAuthRole>();

            try
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    this.Roles.Add(new UserAuthRole() {
                        UserId = dr.Field<int>("UserId"),
                        AuthRuleId = dr.Field<int>("AuthRuleId"),
                        AuthRuleCategoryId = dr.Field<int>("AuthRuleCategoryId"),
                        AuthRuleName = dr.Field<string>("AuthRuleName"),
                        AuthRuleCategoryInternalName = dr.Field<string>("AuthRuleCategoryInternalName"),
                        AuthRuleCategoryName = dr.Field<string>("AuthRuleCategoryName"),
                        AuthRuleInternalName = dr.Field<string>("AuthRuleInternalName"),

                    });
                }
            }
            catch (Exception)
            {

                
            }
        }
        public bool HasRole(string _role)
        {
            bool retval = false;
            try
            {
                retval = this.Roles.Exists(r => r.AuthRuleInternalName.Equals(_role, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {

                
            }
            return retval;
        }
    }
}