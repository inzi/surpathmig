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

  <xsl:template name="PreHeader">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" id="templatePreheader">
      <tr>
        <td align="center" valign="top" style="padding-right:10px; padding-left:10px;">
          <table border="0" cellpadding="0" cellspacing="0" width="600" class="templateContainer">
            <tr>
              <td align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="preheaderBackground">
                  <tr>
                    <td valign="top" class="preheaderContainer">
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnDividerBlock">
                        <tbody class="mcnDividerBlockOuter">
                          <tr>
                            <td class="mcnDividerBlockInner" style="padding: 18px;">
                              <table class="mcnDividerContent" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-top-width: 0px;">
                                <tbody>
                                  <tr>
                                    <td>
                                      <span></span>
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="BeginHeader">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" id="templateHeader">
      <tr>
        <td align="center" valign="top" style="padding-right:10px; padding-left:10px;">
          <table border="0" cellpadding="0" cellspacing="0" width="600" class="templateContainer">
            <tr>
              <td align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="headerBackground">
                  <tr>
                    <td valign="top" class="headerContainer">
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnImageBlock">
                        <tbody class="mcnImageBlockOuter">
                          <tr>
                            <td valign="top" style="padding:9px" class="mcnImageBlockInner">
                              <table align="left" width="100%" border="0" cellpadding="0" cellspacing="0" class="mcnImageContentContainer">
                                <tbody>
                                  <tr>
                                    <td class="mcnImageContent" valign="top" style="padding-right: 9px; padding-left: 9px; padding-top: 0; padding-bottom: 0; text-align:center;">

                                      <img align="center" alt="" src="https://gallery.mailchimp.com/e91a0088b8a88248a709d2dbb/images/d84fcfe6-77f8-400a-88ce-1fdc6349153b.png" width="193" style="max-width:193px; padding-bottom: 0; display: inline !important; vertical-align: bottom;" class="mcnImage" />
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnDividerBlock">
                        <tbody class="mcnDividerBlockOuter">
                          <tr>
                            <td class="mcnDividerBlockInner" style="padding: 18px;">
                              <table class="mcnDividerContent" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-top-width: 1px;border-top-style: solid;border-top-color: #BBBBBB;">
                                <tbody>
                                  <tr>
                                    <td>
                                      <span></span>
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="Middle">
    <p> Thanks for your pre-registration .</p>
  </xsl:template>

  <xsl:template name="Description">
    <p> To activate your SurScan account please click on the activation button. Please find your temporary password above .</p>
  </xsl:template>

  <xsl:template name="Footer">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" id="templateFooter">
      <tr>
        <td align="center" valign="top" style="padding-right:10px; padding-left:10px;">
          <table border="0" cellpadding="0" cellspacing="0" width="600" class="templateContainer">
            <tr>
              <td align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="footerBackground">
                  <tr>
                    <td valign="top" class="footerContainer">
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnTextBlock">
                        <tbody class="mcnTextBlockOuter">
                          <tr>
                            <td valign="top" class="mcnTextBlockInner">
                              <table align="left" border="0" cellpadding="0" cellspacing="0" width="600" class="mcnTextContentContainer">
                                <tbody>
                                  <tr>
                                    <td valign="top" class="mcnTextContent" style="padding-top:9px; padding-right: 18px; padding-bottom: 9px; padding-left: 18px;">
                                      <em>Copyright � 2014 Surscan, All rights reserved.</em><br />
                                      <br />
                                      <strong>Our mailing address is:</strong><br />
                                      Info@surscan.com<br />
                                      &#xA0;
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnDividerBlock">
                        <tbody class="mcnDividerBlockOuter">
                          <tr>
                            <td class="mcnDividerBlockInner" style="padding: 18px;">
                              <table class="mcnDividerContent" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-top-width: 0px;">
                                <tbody>
                                  <tr>
                                    <td>
                                      <span></span>
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
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
        </style>
      </head>
      <body leftmargin="0" marginwidth="0" topmargin="0" marginheight="0" offset="0">
        <center>
          <table align="center" border="0" cellpadding="0" cellspacing="0" height="100%" width="100%" id="bodyTable">
            <tr>
              <td align="center" valign="top" id="bodyCell" style="padding-bottom:40px;">
                <!-- BEGIN TEMPLATE // -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                  <tr>
                    <!-- BEGIN PREHEADER // -->
                    <td align="center" valign="top">
                      <p>
                        <xsl:call-template name="PreHeader" />
                      </p>
                    </td>
                    <!-- // END PREHEADER -->
                  </tr>
                  <tr>
                    <!-- BEGIN HEADER // -->
                    <td align="center" valign="top">
                      <p>
                        <xsl:call-template name="BeginHeader" />
                      </p>
                    </td>
                    <!-- // END HEADER -->
                  </tr>
                  <tr>
                    <!-- BEGIN BODY // -->
                    <td align="center" valign="top">
                      <table border="0" cellpadding="0" cellspacing="0" width="100%" id="templateBody">
                        <tr>
                          <td align="center" valign="top" style="padding-right:10px; padding-left:10px;">
                            <table border="0" cellpadding="0" cellspacing="0" width="600" class="templateContainer">
                              <tr>
                                <td align="center" valign="top">
                                  <table border="0" cellpadding="0" cellspacing="0" width="100%" id="bodyBackground">
                                    <tr>
                                      <td valign="top" class="bodyContainer">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnTextBlock">
                                          <tbody class="mcnTextBlockOuter">
                                            <tr>
                                              <td valign="top" class="mcnTextBlockInner">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="600" class="mcnTextContentContainer">
                                                  <tbody>
                                                    <tr>
                                                      <td valign="top" class="mcnTextContent" style="padding-top:9px; padding-right: 18px; padding-bottom: 9px; padding-left: 18px;">
                                                        <p>
                                                          Hello <xsl:value-of select="Root/Name" /> ,
                                                        </p>
                                                        <p>
                                                          <xsl:call-template name="Middle" />
                                                        </p>
                                                        <p>
                                                          Temporary password:<span style="color:#A20200"><xsl:value-of select="Root/Password" />
                                                          </span>
                                                        </p>
                                                        <p>
                                                          <xsl:call-template name="Description" />
                                                        </p>
                                                      </td>
                                                    </tr>
                                                  </tbody>
                                                </table>
                                              </td>
                                            </tr>
                                          </tbody>
                                        </table>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnButtonBlock">
                                          <tbody class="mcnButtonBlockOuter">
                                            <tr>
                                              <td style="padding-top:0; padding-right:18px; padding-bottom:18px; padding-left:18px;" valign="top" align="center" class="mcnButtonBlockInner">
                                                <table border="0" cellpadding="0" cellspacing="0" class="mcnButtonContentContainer" style="border-collapse: separate !important;border: 2px solid #707070;border-radius: 4px;background-color: #202020;">
                                                  <tbody>
                                                    <tr>
                                                      <td align="center" valign="middle" class="mcnButtonContent" style="font-family: Arial; font-size: 16px; padding: 20px;">
                                                        <a class="mcnButton " title="Activate Account" href="@Model.ActivationLink" target="_blank" style="font-weight: bold;letter-spacing: normal;line-height: 100%;text-align: center;text-decoration: none;color: #FFFFFF;">Activate Account</a>
                                                      </td>
                                                    </tr>
                                                  </tbody>
                                                </table>
                                              </td>
                                            </tr>
                                          </tbody>
                                        </table>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="mcnDividerBlock">
                                          <tbody class="mcnDividerBlockOuter">
                                            <tr>
                                              <td class="mcnDividerBlockInner" style="padding: 18px;">
                                                <table class="mcnDividerContent" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-top-width: 1px;border-top-style: solid;border-top-color: #999999;">
                                                  <tbody>
                                                    <tr>
                                                      <td>
                                                        <span></span>
                                                      </td>
                                                    </tr>
                                                  </tbody>
                                                </table>
                                              </td>
                                            </tr>
                                          </tbody>
                                        </table>
                                      </td>
                                    </tr>
                                  </table>
                                </td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                      </table>
                    </td>
                    <!-- // END BODY -->
                  </tr>
                  <tr>
                    <!-- BEGIN FOOTER // -->
                    <td align="center" valign="top">
                      <p>
                        <xsl:call-template name="Footer" />
                      </p>
                    </td>
                    <!-- // END FOOTER -->
                  </tr>
                </table>
                <!-- // END TEMPLATE -->
              </td>
            </tr>
          </table>
        </center>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>