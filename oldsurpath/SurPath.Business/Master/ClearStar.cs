using Surpath.ClearStar.BL;
using Surpath.CSTest.Models;

namespace SurPath.Business.Master
{
    public class ClearStar
    {
        public string CreateCSClient()
        {
            var newId = string.Empty;
            var creds = DefaultCredentialsBL.GetCredentials();

            Surpath.ClearStar.BL.CustomerBL service = new CustomerBL();

            CustomerModel cust = new CustomerModel();
            cust.FullName = "Mike Kearl";
            cust.City = "McKinney";
            cust.State = "TX";
            cust.Zip = "75071";
            cust.Phone = "801-404-9676";
            cust.Address1 = "4805 Carolina Circle";
            cust.Comments = "Total Stud";
            cust.Email = "mike.kearl@gmail.com";

            //cust.ShortName = "";
            //Todo: we may want to pass surscancustid in here .. right now im generating it.

            CustomerModel newcust = service.CreateCustomer(cust, "tester", "test");

            return newcust.CustomerId;
        }
    }
}