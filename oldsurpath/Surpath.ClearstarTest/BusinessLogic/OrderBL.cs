using Surpath.CSTest.Order;
using System.Xml;

namespace Surpath.ClearStar.BL
{
    public class OrderBL
    {
        /// <summary>
        /// Submit Order Notification Request if Status updates are desired for individual requests
        /// </summary>
        /// <param name="CustId"></param>
        /// <param name="ProfileNo"></param>
        /// <param name="OrderId"></param>
        public void NotificationRequest(string CustId, string ProfileNo, int OrderId)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Order.Order o = new Order();

            string sNotificationType = "";//Todo: need value here.
            string sNotificationValue = ""; //Todo: need value here.

            XmlNode propresp = o.OrderNotificationRequest(cred.UserName, cred.Password, cred.BoID, CustId, OrderId, sNotificationType, sNotificationValue, ProfileNo);
        }
    }
}