<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:key name="Mail" match="Root" use="@ID" />

  <xsl:output method="xml" indent="yes" />

  <xsl:template name="BeginTemplate">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
      <tr>
        <td align="center" valign="top">
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="/">
    <html>
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>SurScan</title>
        <link href="xsl.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
          body, #bodyTable, #bodyCell {
          height: 100% !important;
          margin: 0;
          padding: 0;
          width: 100% !important;
          }

          table {
          border-collapse: collapse;
          }

          img, a img {
          border: 0;
          outline: none;
          text-decoration: none;
          }

          h1, h2, h3, h4, h5, h6 {
          margin: 0;
          padding: 0;
          }

          p {
          margin: 1em 0;
          padding: 0;
          }

          a {
          word-wrap: break-word;
          }

          .ReadMsgBody {
          width: 100%;
          }

          .ExternalClass {
          width: 100%;
          }

          .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div {
          line-height: 100%;
          }

          table, td {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
          }

          #outlook a {
          padding: 0;
          }

          img {
          -ms-interpolation-mode: bicubic;
          }

          body, table, td, p, a, li, blockquote {
          -ms-text-size-adjust: 100%;
          -webkit-text-size-adjust: 100%;
          }

          #bodyCell {
          padding: 0;
          }

          .mcnImage {
          vertical-align: bottom;
          }

          .mcnTextContent img {
          height: auto !important;
          }

          a.mcnButton {
          display: block;
          }

          body, #bodyTable {
          background-color: #F5F5F5;
          }

          #bodyCell {
          border-top: 0;
          }

          h1 {
          color: #202020 !important;
          display: block;
          font-family: Helvetica;
          font-size: 34px;
          font-style: normal;
          font-weight: bold;
          line-height: 125%;
          letter-spacing: normal;
          margin: 0;
          text-align: center;
          }

          h2 {
          color: #FFFFFF !important;
          display: block;
          font-family: Helvetica;
          font-size: 26px;
          font-style: normal;
          font-weight: bold;
          line-height: 125%;
          letter-spacing: normal;
          margin: 0;
          text-align: left;
          }

          h3 {
          color: #404040 !important;
          display: block;
          font-family: Helvetica;
          font-size: 18px;
          font-style: normal;
          font-weight: bold;
          line-height: 125%;
          letter-spacing: normal;
          margin: 0;
          text-align: left;
          }

          h4 {
          color: #606060 !important;
          display: block;
          font-family: Helvetica;
          font-size: 16px;
          font-style: normal;
          font-weight: bold;
          line-height: 125%;
          letter-spacing: normal;
          margin: 0;
          text-align: left;
          }

          #templatePreheader {
          background-color: #F5F5F5;
          border-top: 0;
          border-bottom: 0;
          }

          #preheaderBackground {
          background-color: #F5F5F5;
          border-top: 0;
          border-bottom: 0;
          }

          .preheaderContainer .mcnTextContent, .preheaderContainer .mcnTextContent p {
          color: #FFFFFF;
          font-family: Helvetica;
          font-size: 10px;
          line-height: 125%;
          text-align: left;
          }

          .preheaderContainer .mcnTextContent a {
          color: #FFFFFF;
          font-weight: normal;
          text-decoration: underline;
          }

          #templateHeader {
          background-color: #F5F5F5;
          border-top: 0;
          border-bottom: 0;
          }

          #headerBackground {
          background-color: #FFFFFF;
          border-top: 0;
          border-bottom: 0;
          }

          .headerContainer .mcnTextContent, .headerContainer .mcnTextContent p {
          color: #202020;
          font-family: Helvetica;
          font-size: 16px;
          line-height: 150%;
          text-align: left;
          }

          .headerContainer .mcnTextContent a {
          color: #EE4343;
          font-weight: normal;
          text-decoration: underline;
          }

          #templateBody {
          background-color: #F5F5F5;
          border-top: 0;
          border-bottom: 0;
          }

          #bodyBackground {
          background-color: #FFFFFF;
          border-top: 0;
          border-bottom: 0;
          }

          .bodyContainer .mcnTextContent, .bodyContainer .mcnTextContent p {
          color: #202020;
          font-family: Helvetica;
          font-size: 18px;
          line-height: 150%;
          text-align: left;
          }

          .bodyContainer .mcnTextContent a {
          color: #EE4343;
          font-weight: normal;
          text-decoration: underline;
          }

          #templateFooter {
          background-color: #F5F5F5;
          border-top: 0;
          border-bottom: 0;
          }

          #footerBackground {
          background-color: #FFFFFF;
          border-top: 0;
          border-bottom: 0;
          }

          .footerContainer .mcnTextContent, .footerContainer .mcnTextContent p {
          color: #606060;
          font-family: Helvetica;
          font-size: 10px;
          line-height: 125%;
          text-align: center;
          }

          .footerContainer .mcnTextContent a {
          color: #606060;
          font-weight: normal;
          text-decoration: underline;
          }

          p.MsoNormal, li.MsoNormal, div.MsoNormal
          {margin:0in;
          margin-bottom:.0001pt;
          font-size:12.0pt;
          font-family:"Calibri",sans-serif;}

          span.apple-style-span
          {mso-style-name:apple-style-span;}

          span.apple-converted-space
          {mso-style-name:apple-converted-space;}

          .MsoChpDefault
          {font-family:"Calibri",sans-serif;}

          @page WordSection1
          {size:8.5in 11.0in;
          margin:1.0in 1.0in 1.0in 1.0in;}
          div.WordSection1
          {page:WordSection1;}
        </style>
      </head>
      <body leftmargin="0" marginwidth="0" topmargin="0" marginheight="0" offset="0">
        <center>

          <p class="MsoNormal" align="center" style='text-align:center'>
            <span style='font-size:16.0pt;font-family:"Arial",sans-serif;color:black'>
              Hello <xsl:value-of select="Root/Recipient" />
            </span>
          </p>

          <p class="MsoNormal" align="center" style='text-align:center'>
            <b>
              <span style='font-size:16.0pt;font-family:"Arial",sans-serif;color:black'>CALL LOCATION TO CONFIRM HOURS</span>
            </b>
          </p>

          <p class='MsoNormal' align='center' style='text-align:center'>
            <b>
              <span style='font-size:20.0pt;font-family:"Gulim",sans-serif;color:black'> </span>
            </b>
          </p>

          <p class='MsoNormal'>
            <span style='font-family:"Arial",sans-serif;color:black'>Important - This email contains detailed instructions for both you and the defined collection site you will be tested.</span>
          </p>

          <p class='MsoNormal'>
            <span style='font-family:"Gulim",sans-serif;color:black'> </span>
          </p>

          <p class='MsoNormal'>
            <span style='font-family:"Arial",sans-serif;color:red'>
              <xsl:value-of select="Root/Program" />
            </span>
            <span style='font-family:"Arial",sans-serif;color:black'> stipulates that the date selected for your test must be completely </span>
            <b>
              <span style='font-size:18.0pt;font-family:"Arial",sans-serif;color:#FD1E13'>&quot;RANDOM&quot;</span>
            </b>
            <span style='font-size:18.0pt;font-family:"Arial",sans-serif;color:black'>.</span>
            <span style='font-family:"Arial",sans-serif;color:black'>

              <b>
                <i>
                  This means you are not given advance notice; you cannot specify when you can be tested.
                </i>
              </b>
            </span>
            <b>
              <i>
                <span style='font-family:"Arial",sans-serif; color:red'>
                  The date and time to have the test <u>completed</u> is in the attachment at the top
                </span>
              </i>
            </b>
            <b>
              <i>
                <span style='font-family:"Arial",sans-serif; color:black'> </span>
              </i>
            </b>
            <b>
              <i>
                <span style='font-family:"Arial",sans-serif; color:red'>of your testing order.</span>
              </i>
            </b>
          </p>

          <p class='MsoNormal'>
            <b>
              <i>
                <span style='font-family:"Arial",sans-serif; color:black'> </span>
              </i>
            </b>
          </p>

          <p class='MsoNormal'>
            <u>
              <span style='font-size:16.0pt;font-family:"Arial",sans-serif; color:red'>Please see attached notification for your time and location of your screening.</span>
            </u>
          </p>
        </center>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>