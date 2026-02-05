using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SurPath.Entity
{
    public class IntegrationHandOffDTO
    {
        public int donor_id { get; set; } = 0; // Surpath internal ID
        public string donor_email { get; set; } = string.Empty; // Donor Email
        public string donor_password { get; set; } = string.Empty; // Surpath use only, interal donor Password
        public string partner_client_code { get; set; } = string.Empty; // YOUR code for the client. You can set this to whatever you want.
        public string integration_id { get; set; } = string.Empty; // YOUR internal ID for the donor. Always treated as a string, convert / cast as needed.
        public string auth { get; set; } = string.Empty; // AUTH is the security failsafe on the object, must be properlly formatted
        public string donor_first_name { get; set; } = string.Empty; // When donor registers, they must fill out contact info, this value will be pre-populated for the donor
        public string donor_mi { get; set; } = string.Empty; // Prepopulate donor's middle initial during registration
        public string donor_last_name { get; set; } = string.Empty; // prepopulate donor's last name
        public string donor_suffix { get; set; } = string.Empty; // Prepopulate donor's suffix (JR, SR, etc.)
        public string donor_city { get; set; } = string.Empty; // prepopulate donor's city
        public string donor_state { get; set; } = string.Empty; // prepopulate donor's state
        public string donor_zip { get; set; } = string.Empty; // prepopulate donor's zip
        public string donor_phone_1 { get; set; } = string.Empty; // prepopulate donor's phone
        public string donor_phone_2 { get; set; } = string.Empty; // prepopulate donor's alternative phone
        public string donor_address_1 { get; set; } = string.Empty; // prepopulate donor's address (street)
        public string donor_address_2 { get; set; } = string.Empty; // prepopulate donor's address (Apt#, suite #, etc.)
        public bool email_changed { get; set; } = false;

    }

    public class SurpathHandoffURLGenerator
    {
        private string _SurpathKey { get; set; } = string.Empty;
        private string _CryptoKey { get; set; } = string.Empty;
        private string _BaseUrl { get; set; } = string.Empty;
        private string _ClientGuid { get; set; } = string.Empty;
        private bool _AllowHttp { get; set; }

        public SurpathHandoffURLGenerator(string SurpathKey, string CryptoKey, string ClientGuid, string BaseUrl = "", bool allowHttp = false)
        {
            try
            {
                _SurpathKey = SurpathKey;
                _CryptoKey = CryptoKey;
                _AllowHttp = allowHttp;
                _ClientGuid = ClientGuid;
                _BaseUrl = BaseUrl;
                if (string.IsNullOrEmpty(BaseUrl))
                {
                    _BaseUrl = "https://stage.surpath.com/registration/handoff/";
                }
                if (!validURL(_BaseUrl, _AllowHttp))
                {
                    throw new Exception("BaseUrl is not a valid URL");

                }
                if (string.IsNullOrEmpty(_SurpathKey))
                {
                    throw new Exception("SurpathKey is required");
                }

                if (string.IsNullOrEmpty(_CryptoKey))
                {
                    throw new Exception("CryptoKey is required");
                }

                if (string.IsNullOrEmpty(_ClientGuid))
                {
                    throw new Exception("ClientGuid is required");
                }

                _BaseUrl = EnsureEndsWith(_BaseUrl, "/");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Generate a handoff URI for a DTO object
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 
        /*
         * 
You can leave donor_id as 0, that’s our internal ID.
Donor_email is self explanatory.
Donor_password is ignored for hand offs, the auth field is used instead.
partner_client_code is *your* code for the client. (you can this via the API) (required)
The integration_id is *your* integration ID for the donor. You can put your internal identifier here. (required)
 
Auth property should be: partner_client_code + donor_email encrypted with your *ID*.
This is to ensure uniqueness. This will be used as the password for the donor account
For identifying donors, we use a combination of email, password and the integration_id as a personal identifier associated with Project Concert.
This is to ensure if the email is re-used by the school, there’s still uniqueness to associated records.
They will not get an email to activate their account, or anything like that. They’ll be created after they complete the registration.
         * 
         */
        public Uri GetHandoffURL(IntegrationHandOffDTO dto)
        {
            Uri uriResult = new Uri(_BaseUrl);

            try
            {


                if (string.IsNullOrEmpty(dto.integration_id)) throw new Exception("An internal ID for this donor is required");
                if (string.IsNullOrEmpty(dto.partner_client_code)) throw new Exception("A client code is required to associate this donor with an existing department test");

                /*
                 * 
 
                Auth property should be: partner_client_code + donor_email encrypted with your *ID* (SurpathKey).
                This is to ensure uniqueness. This will be used as the password for the donor account
                For identifying donors, we use a combination of email, password and the integration_id as a personal identifier associated with Project Concert.
                This is to ensure if the email is re-used by the school, there’s still uniqueness to associated records.
                * 
                 */
                string _authvalue = string.Empty;

                _authvalue = dto.partner_client_code + dto.donor_email;

                dto.auth = EncryptWithKey(_authvalue, _SurpathKey);


                string result = string.Empty;

                _BaseUrl = EnsureEndsWith(_BaseUrl, "/");
                result = EnsureEndsWith(result + this._BaseUrl, "/");

                // Build the URL
                //// use the client GUID to identify the program we're handing off to
                //string _b64ClientGuid = Base64UrlEncoder.Encode(_ClientGuid);
                //result = EnsureEndsWith(result + _b64ClientGuid, "/");

                string _b64partner_client_code = Base64UrlEncoder.Encode(dto.partner_client_code);
                result = EnsureEndsWith(result + _b64partner_client_code, "/");

                // Next, we need to encrypt the json object to a url safe base 64 string

                var _json = JsonConvert.SerializeObject(dto);
                var encobj = EncryptWithKey(_json, _CryptoKey);
                var base64dto = Base64UrlEncoder.Encode(encobj);
                result = result + base64dto;

                if (validURL(result, _AllowHttp))
                {

                    if (!(Uri.TryCreate(result, UriKind.Absolute, out uriResult)
                       && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
                    {
                        throw new Exception("Unable to generate new URI! Something went wrong");
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }
            return uriResult;
        }

        private bool validURL(string url, bool allowHttp = false)
        {
            try
            {
                Uri uriResult;
                bool isvalid = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttps);
                if (allowHttp)
                {
                    isvalid = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                }
                    
                return isvalid;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public string EnsureEndsWith(string _what, string _with)
        {
            try
            {
                if (!(_what.EndsWith(_with))) _what = _what + _with;
            }
            catch (Exception ex)
            {

                throw;
            }

            return _what;

        }
        public string ToJson(IntegrationHandOffDTO dto)
        {
            string json = string.Empty;
            try
            {
                json = new JavaScriptSerializer().Serialize(dto);
            }
            catch (Exception ex)
            {

                throw;
            }
            return json;
        }
        public static string EncryptWithKey(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string DecryptWithKey(string cipherString, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }


    public class SurpathHandoffURLGeneratorExample
    {
        public void howto()
        {
            var key = Guid.NewGuid().ToString();
            var crypto = Guid.NewGuid().ToString();
            var partner_client_code = Guid.NewGuid().ToString();

            SurpathHandoffURLGenerator surpathHandoffURLGenerator =
                new SurpathHandoffURLGenerator(key, crypto, partner_client_code);

            IntegrationHandOffDTO integrationHandOffDTO = new IntegrationHandOffDTO();
            integrationHandOffDTO.donor_email = "chris@inzi.com";
            integrationHandOffDTO.partner_client_code = partner_client_code;
            integrationHandOffDTO.donor_first_name = "Chris";
            integrationHandOffDTO.donor_last_name = "Norman";
            integrationHandOffDTO.donor_phone_1 = "2148019441";
            integrationHandOffDTO.donor_state = "TX";
            integrationHandOffDTO.integration_id = "integration_id";

            var URL = surpathHandoffURLGenerator.GetHandoffURL(integrationHandOffDTO).ToString();

        }
    }
}