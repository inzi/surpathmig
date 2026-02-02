using Surpath.CSTest.Document;
using Surpath.CSTest.Models;
using System.Xml;

namespace Surpath.ClearStar.BL
{
    public class DocumentBL
    {
        public XmlNode UploadProfileDocument3(DocumentModel doc, string CustId, string ProfileNo)
        {
            var cred = DefaultCredentialsBL.GetCredentials();
            Surpath.CSTest.Document.Document d = new Document();

            int iFileId = 0; //Todo: need values for the following.

            XmlNode docresp = d.UploadProfileDocument3(cred.UserName, cred.Password, cred.BoID, CustId, iFileId, ProfileNo, doc.OrderIdsDocCopiedTo, doc.FileType, doc.ContentType, doc.SecurityLevel, doc.Description, doc.IncludeInReport, doc.FileName, doc.bytes);
            return docresp;
        }
    }
}