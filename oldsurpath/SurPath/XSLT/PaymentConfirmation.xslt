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

                                      <img align="center" alt="" src="@Model.HeaderLogo" width="193" style="max-width:193px; padding-bottom: 0; display: inline !important; vertical-align: bottom;" class="mcnImage" />
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
    <p> Please find your payment details below </p>
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
                                      <em>Copyright � 2018 Surscan, All rights reserved.</em><br />
                                      <br />
                                      <strong>Our mailing address is:</strong><br />
                                      clientservices@surscan.com<br />
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
                                                          Hello <xsl:value-of select="Root/DonorName" /> ,
                                                        </p>
                                                        <p>
                                                          <xsl:call-template name="Middle" />
                                                        </p>
                                                        <p>
                                                          <table border="1" cellpadding="0" cellspacing="0" width="100%">
                                                            <tbody>
                                                              <tr>
                                                                <th> Program Name </th>
                                                                <th> Total Price </th>
                                                                <th> Payment Type</th>
                                                              </tr>
                                                              <tr align="center">
                                                                <td>
                                                                  <xsl:value-of select="Root/ClientDepartmentName" /> </td>

                                                                <td>
                                                                  <xsl:value-of select="Root/PaymentMethod" /> </td>
                                                              </tr>
                                                            </tbody>
                                                          </table>
                                                        </p>
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