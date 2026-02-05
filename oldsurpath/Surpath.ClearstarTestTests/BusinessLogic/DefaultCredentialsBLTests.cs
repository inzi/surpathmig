using Microsoft.VisualStudio.TestTools.UnitTesting;

using Surpath.ClearStar.BL;

namespace Surpath.CSTest.Tests
{
    [TestClass]
    public class DefaultCredentialsBLTests
    {
        [TestMethod]
        public void GetCredentialsBOID()
        {
            var creds = DefaultCredentialsBL.GetCredentials();

            Assert.AreEqual(763, creds.BoID);
        }

        [TestMethod]
        public void GetCredentialsUserName()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            var name = creds.UserName;

            Assert.AreEqual("jonjackson", name);
        }

        [TestMethod]
        public void GetCredentialsPassword()
        {
            var creds = DefaultCredentialsBL.GetCredentials();
            var password = creds.Password;

            Assert.AreEqual("Password01!", password);
        }
    }
}