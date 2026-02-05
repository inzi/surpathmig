Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SurPath.Entity

<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub testURLBuilderHandOff()

        Dim key = "242575a2-d320-11eb-b1e0-7c4c58ced981"
        Dim crypto = "9edef9be-4ec6-ad3f-75e9-45d031d51741"
        Dim partner_client_code = "pcclientcode1" ' Guid.NewGuid().ToString();
        Dim surpathHandoffURLGenerator As SurpathHandoffURLGenerator = New SurpathHandoffURLGenerator(key, crypto, partner_client_code, "http://localhost:1481/registration/handoff", True)
        Dim integrationHandOffDTO As IntegrationHandOffDTO = New IntegrationHandOffDTO()
        integrationHandOffDTO.donor_email = "chris@inzi.com"
        integrationHandOffDTO.partner_client_code = partner_client_code
        integrationHandOffDTO.donor_first_name = "Chris"
        integrationHandOffDTO.donor_last_name = "Norman"
        integrationHandOffDTO.donor_phone_1 = "2148019441"
        integrationHandOffDTO.donor_state = "TX"
        integrationHandOffDTO.integration_id = "pcid55"
        Dim URL = surpathHandoffURLGenerator.GetHandoffURL(integrationHandOffDTO).ToString()

        ' split the url into a list
        Dim _parts As List(Of String) = URL.Split("/"c).ToList()
        ' get rid of empty values.
        _parts = _parts.Where(Function(p) String.IsNullOrEmpty(p) = False).ToList()
        ' get the index of 'handoff'
        Dim _handoffindex = _parts.IndexOf("handoff")
        Dim base64clientcode = _parts(_handoffindex + 1)
        Dim decodedClientCode = B64Utils.Base64URLDecode(base64clientcode)
        Assert.IsTrue(partner_client_code = decodedClientCode)


    End Sub
    ' 

    <TestMethod()> Public Sub testURLBuilderHandOffVB()

        'Dim key = "242575a2-d320-11eb-b1e0-7c4c58ced981"
        'Dim crypto = "9edef9be-4ec6-ad3f-75e9-45d031d51741"
        'Dim partner_client_code = "pcclientcode1" ' Guid.NewGuid().ToString();
        'Dim surpathHandoffURLGeneratorVB As SurpathHandoffURLGeneratorVB = New SurpathHandoffURLGeneratorVB(key, crypto, partner_client_code, "http://localhost:1481/registration/handoff", True)
        'Dim integrationHandOffDTO As IntegrationHandOffDTO = New IntegrationHandOffDTO()
        'integrationHandOffDTO.donor_email = "chris@inzi.com"
        'integrationHandOffDTO.partner_client_code = partner_client_code
        'integrationHandOffDTO.donor_first_name = "Chris"
        'integrationHandOffDTO.donor_last_name = "Norman"
        'integrationHandOffDTO.donor_phone_1 = "2148019441"
        'integrationHandOffDTO.donor_state = "TX"
        'integrationHandOffDTO.integration_id = "pcid55"
        'Dim URL = SurpathHandoffURLGeneratorVB.GetHandoffURL(integrationHandOffDTO).ToString()

        '' split the url into a list
        'Dim _parts As List(Of String) = URL.Split("/"c).ToList()
        '' get rid of empty values.
        '_parts = _parts.Where(Function(p) String.IsNullOrEmpty(p) = False).ToList()
        '' get the index of 'handoff'
        'Dim _handoffindex = _parts.IndexOf("handoff")
        'Dim base64clientcode = _parts(_handoffindex + 1)
        'Dim decodedClientCode = B64Utils.Base64URLDecode(base64clientcode)
        'Assert.IsTrue(partner_client_code = decodedClientCode)


    End Sub

End Class

Public Module B64Utils
    Public Function Base64EncodeBytes(ByVal bytes As Byte()) As String
        'var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        Return Convert.ToBase64String(bytes)
    End Function

    Public Function Base64Encode(ByVal plainText As String) As String
        Dim plainTextBytes = Encoding.UTF8.GetBytes(plainText)
        Return Convert.ToBase64String(plainTextBytes)
    End Function

    'Decode
    Public Function Base64Decode(ByVal base64EncodedData As String) As String
        Dim base64EncodedBytes = Convert.FromBase64String(base64EncodedData)
        Return Encoding.UTF8.GetString(base64EncodedBytes)
    End Function

    Public Function Base64URLDecode(ByVal base64URLEncodedData As String) As String
        ' Add needed characters for URL encoded (which is always missing = at end)
        ' https://stackoverflow.com/questions/1228701/code-for-decoding-encoding-a-modified-base64-url

        base64URLEncodedData = base64URLEncodedData.PadRight(base64URLEncodedData.Length + (4 - base64URLEncodedData.Length Mod 4) Mod 4, "="c)
        Dim base64EncodedBytes = Convert.FromBase64String(base64URLEncodedData)
        Return Encoding.UTF8.GetString(base64EncodedBytes)
    End Function
End Module
