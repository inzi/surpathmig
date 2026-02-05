Imports Microsoft.IdentityModel.Tokens
Imports Newtonsoft.Json
Imports System
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web.Script.Serialization

Namespace SurPath.Entity


    Public Class SurpathHandoffURLGeneratorVB
        Private Property _SurpathKey As String = String.Empty
        Private Property _CryptoKey As String = String.Empty
        Private Property _BaseUrl As String = String.Empty
        Private Property _ClientGuid As String = String.Empty
        Private Property _AllowHttp As Boolean

        Public Sub New(ByVal SurpathKey As String, ByVal CryptoKey As String, ByVal ClientGuid As String, ByVal Optional BaseUrl As String = "", ByVal Optional allowHttp As Boolean = False)
            Try
                _SurpathKey = SurpathKey
                _CryptoKey = CryptoKey
                _AllowHttp = allowHttp
                _ClientGuid = ClientGuid
                _BaseUrl = BaseUrl

                If String.IsNullOrEmpty(BaseUrl) Then
                    _BaseUrl = "https://stage.surpath.com/registration/handoff/"
                End If

                If Not validURL(_BaseUrl, _AllowHttp) Then
                    Throw New Exception("BaseUrl is not a valid URL")
                End If

                If String.IsNullOrEmpty(_SurpathKey) Then
                    Throw New Exception("SurpathKey is required")
                End If

                If String.IsNullOrEmpty(_CryptoKey) Then
                    Throw New Exception("CryptoKey is required")
                End If

                If String.IsNullOrEmpty(_ClientGuid) Then
                    Throw New Exception("ClientGuid is required")
                End If

                _BaseUrl = EnsureEndsWith(_BaseUrl, "/")
            Catch ex As Exception
                Throw
            End Try
        End Sub


        ''' <summary>
        ''' Generate a handoff URI for a DTO object
        ''' 
        ''' </summary>
        ''' <paramname="dto"></param>
        ''' <returns></returns>
        ''' 
        ' 
        '  
        ' You can leave donor_id as 0, that’s our internal ID.
        ' Donor_email is self explanatory.
        ' Donor_password is ignored for hand offs, the auth field is used instead.
        ' partner_client_code is *your* code for the client. (you can this via the API) (required)
        ' The integration_id is *your* integration ID for the donor. You can put your internal identifier here. (required)
        ' 
        ' Auth property should be: partner_client_code + donor_email encrypted with your *ID*.
        ' This is to ensure uniqueness. This will be used as the password for the donor account
        ' For identifying donors, we use a combination of email, password and the integration_id as a personal identifier associated with Project Concert.
        ' This is to ensure if the email is re-used by the school, there’s still uniqueness to associated records.
        ' They will not get an email to activate their account, or anything like that. They’ll be created after they complete the registration.
        '  
        ' 
        Public Function GetHandoffURL(ByVal dto As IntegrationHandOffDTO) As Uri
            Dim uriResult As Uri = New Uri(_BaseUrl)

            Try
                If String.IsNullOrEmpty(dto.integration_id) Then Throw New Exception("An internal ID for this donor is required")
                If String.IsNullOrEmpty(dto.partner_client_code) Then Throw New Exception("A client code is required to associate this donor with an existing department test")

                ' 
                '  
                ' 
                ' Auth property should be: partner_client_code + donor_email encrypted with your *ID* (SurpathKey).
                ' This is to ensure uniqueness. This will be used as the password for the donor account
                ' For identifying donors, we use a combination of email, password and the integration_id as a personal identifier associated with Project Concert.
                ' This is to ensure if the email is re-used by the school, there’s still uniqueness to associated records.
                '  
                ' 
                Dim _authvalue As String = String.Empty
                _authvalue = dto.partner_client_code & dto.donor_email
                dto.auth = EncryptWithKey(_authvalue, _SurpathKey)
                Dim result As String = String.Empty
                _BaseUrl = EnsureEndsWith(_BaseUrl, "/")
                result = EnsureEndsWith(result & _BaseUrl, "/")

                ' Build the URL
                ''' use the client GUID to identify the program we're handing off to
                'string _b64ClientGuid = Base64UrlEncoder.Encode(_ClientGuid);
                'result = EnsureEndsWith(result + _b64ClientGuid, "/");

                Dim _b64partner_client_code As String = Base64UrlEncoder.Encode(dto.partner_client_code)
                result = EnsureEndsWith(result & _b64partner_client_code, "/")

                ' Next, we need to encrypt the json object to a url safe base 64 string

                Dim _json = JsonConvert.SerializeObject(dto)
                Dim encobj = EncryptWithKey(_json, _CryptoKey)
                Dim base64dto = Base64UrlEncoder.Encode(encobj)
                result = result & base64dto

                If validURL(result, _AllowHttp) Then

                    If Not (Uri.TryCreate(result, UriKind.Absolute, uriResult) AndAlso (Equals(uriResult.Scheme, Uri.UriSchemeHttp) OrElse Equals(uriResult.Scheme, Uri.UriSchemeHttps))) Then
                        Throw New Exception("Unable to generate new URI! Something went wrong")
                    End If
                End If

            Catch ex As Exception
                Throw
            End Try

            Return uriResult
        End Function

        Private Function validURL(ByVal url As String, ByVal Optional allowHttp As Boolean = False) As Boolean
            Try
                Dim uriResult As Uri
                Dim isvalid As Boolean = Uri.TryCreate(url, UriKind.Absolute, uriResult) AndAlso Equals(uriResult.Scheme, Uri.UriSchemeHttps)

                If allowHttp Then
                    isvalid = Uri.TryCreate(url, UriKind.Absolute, uriResult) AndAlso (Equals(uriResult.Scheme, Uri.UriSchemeHttp) OrElse Equals(uriResult.Scheme, Uri.UriSchemeHttps))
                End If

                Return isvalid
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function EnsureEndsWith(ByVal _what As String, ByVal _with As String) As String
            Try
                If Not _what.EndsWith(_with) Then _what = _what & _with
            Catch ex As Exception
                Throw
            End Try

            Return _what
        End Function

        Public Function ToJson(ByVal dto As IntegrationHandOffDTO) As String
            Dim json As String = String.Empty

            Try
                json = New JavaScriptSerializer().Serialize(dto)
            Catch ex As Exception
                Throw
            End Try

            Return json
        End Function

        Public Shared Function EncryptWithKey(ByVal toEncrypt As String, ByVal key As String) As String
            Dim keyArray As Byte()
            Dim toEncryptArray As Byte() = Encoding.UTF8.GetBytes(toEncrypt)
            Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key))
            hashmd5.Clear()
            Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
            tdes.Key = keyArray
            tdes.Mode = CipherMode.ECB
            tdes.Padding = PaddingMode.PKCS7
            Dim cTransform As ICryptoTransform = tdes.CreateEncryptor()
            Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
            tdes.Clear()
            Return Convert.ToBase64String(resultArray, 0, resultArray.Length)
        End Function

        Public Shared Function DecryptWithKey(ByVal cipherString As String, ByVal key As String) As String
            Dim keyArray As Byte()
            Dim toEncryptArray As Byte() = Convert.FromBase64String(cipherString)
            Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key))
            hashmd5.Clear()
            Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
            tdes.Key = keyArray
            tdes.Mode = CipherMode.ECB
            tdes.Padding = PaddingMode.PKCS7
            Dim cTransform As ICryptoTransform = tdes.CreateDecryptor()
            Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
            tdes.Clear()
            Return Encoding.UTF8.GetString(resultArray)
        End Function
    End Class


End Namespace
