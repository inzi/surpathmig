using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpathBackend.Classes
{
    public static class BackendStatics
    {
        static string Base64EncodeBytes(byte[] bytes)
        {
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(bytes);
        }
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //Decode
        static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static string Base64URLDecode(string base64URLEncodedData)
        {
            base64URLEncodedData = base64URLEncodedData.PadRight(base64URLEncodedData.Length + (4 - base64URLEncodedData.Length % 4) % 4, '=');
            var base64EncodedBytes = System.Convert.FromBase64String(base64URLEncodedData);

            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
