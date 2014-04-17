<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Homepage.ascx.cs" Inherits="Homepage" %>

<link rel="stylesheet" type="text/css" href="FJAStyles.css" />
<%--<script type="text/jscript" src="FJAJScript.js"></script>--%>
<script language="JavaScript" type="text/JavaScript">
var count=1;
var s;
function ImageShow1()
{
var ss=document.getElementById('AnimationPix');
if(ss !=null){
document.getElementById('AnimationPix').src = "images/Homepage_animation_" + count + ".jpg";
if(count==10){count=0;}
count=count + 1
s=setTimeout("ImageShow1()",5000)}
}
</script>
<%--<br /><br />onload="ImageShow1();"--%>
<body >
<table style="width:100%; line-height: 18px;">
    <tr>
        <td valign="top">
            <div style="width: 325px; text-align: left;">
            <table class="homeleftlayer">
                <tr>
                    <td valign="top">
                        <div id="homeimage" align="left">
                            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
                                codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" 
                                width=275 height=275>
          <param name="movie" value="images/fjaFlash_home.swf" />
          <param name="quality" value="high" />
          <embed src="images/fjaFlash_home.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width=275 height=275 ></embed></object>
                                    </div>
                    </td>
                </tr>
                <%--<tr>
                    <td class="staticheader_hp">
                        What </td>
                </tr>
                <tr>
                    <td class="staticheader1_hp">
                        we propose to do</td>
                </tr>--%>
                <tr>
                    <td>
                        <div style="vertical-align: top">
                            <div class="staticheader_hp">What</div>
                            
                            <div class="staticheader1_hp" style="height: 30px">we propose to do</div></div>

                        Provide a common platform for identifying and developing talent
                    </td>
                </tr>
                <%--<tr>
                    <td class="staticheader_hp">
                        How</td>
                </tr>
                <tr>
                    <td class="staticheader1_hp">
                        we do it</td>
                </tr>--%>
                <tr>
                    <td><div style="vertical-align: top">
                        <div class="staticheader_hp">How</div>
                        <div class="staticheader1_hp" style="height: 30px">
                                                we do it</div></div>
                        Extract, analyze and process objective and valid information about job through <span style="text-decoration: underline; color: #0000FF">CODE-Informatics.</span> 
                        CareerJudge&#39;s state of-the-art technology. </td>
                </tr>
                <tr>
                    <td>
                        Assess and map abilities and skills using scientific tools and methods in order 
                        to meet organizational and individual requirements through <span style="text-decoration: underline; color: #0000FF">
                        TALENTSCOUT</span>, CJ&#39;s&nbsp; 
                        Assessment Engine <span style="text-decoration: underline; color: #0000FF">.</span> </td>
                </tr>
                <tr>
                    <td>
                        Guide young minds in choosing right careers and pursuing excellence through 
                        career profiling, counseling and training.</td>
                </tr>
                <tr>
                    <td>
                        <table style="width:330px; height: 100px; background-image: url('images/homepage_bgsmall_1.jpg'); background-repeat: no-repeat;">
                            <tr>
                                <td style="color: #FF0000; font-weight: bold; line-height: 25px; text-align: center;">
                                    &nbsp;
                                    Learn more about our<br />
                                    <img alt="" src="images/Home_fadedlogo.jpg" /></td>
                                <td style="color: #CC6600; font-weight: bold; line-height: 25px;">
                                    Corporate Assessment Solutions<br />
                                    Career Profiling Services                                     <br />
                                    Career Guidance and Training</td>
                            </tr>
                            </table>
                    </td>
                </tr>
                </table>
                </div>
        </td>
        <td valign="top">
            <div style="width: 375px; text-align: left;">
            <table class="homemiddlelayer">
                <%--<tr>
                    <td class="staticheader_hp">
                        &nbsp;</td>
                </tr>--%>
                <tr>
                    <td>
                        <br />
                        <div>
                            <div class="staticheader_hp_mov">Moving </div>                       
                                    <div class="staticheader1_hp_mov">to top Careers...</div></div>
                    </td>
                </tr>
                <%--<tr>
                    <td class="staticheader3_hp">
                        CITAT</td>
                </tr>--%>
                <tr>
                    <td><span class="staticheader3_hp">CITAT</span>
                        <br />
                        Today’s economy revolves around information technology. Careers in IT are 
                        lucrative and abounding. Test your aptitude for IT using our 
                        <span style="color: #FF9900"> Common IT Aptitude 
                        Test.</span> <span class="learnmore">Learn more …</span></td>
                </tr>
                <%--<tr>
                    <td class="staticheader3_hp">
                        BAAT</td>
                </tr>--%>
                <tr>
                    <td><span class="staticheader3_hp">BAAT</span>
                        <br />
                        Leading and managing business has always been a challenge. People with the right 
                        blend of attitude, abilities and skills become successful in managing business 
                        and people. Try our managerial aptitude test to learn how you are placed for 
                        managerial careers. <span style="color: #FF9900">Business Administration Aptitude Test.</span> 
                        <span class="learnmore"> Learn more …</span></td>
                </tr>
                <%--<tr>
                    <td class="staticheader3_hp">
                       
                        PAAT</td>
                </tr>--%>
                <tr>
                    <td><span class="staticheader3_hp">PAAT</span>
                        <br />
                        Power, status and social respect are important measures of success in career. 
                        Careers in public administration requires hard-work and perseverance and right 
                        personality disposition. Know your aptitude for careers in public/civil 
                        services. </td>
                </tr>
                <tr>
                    <td>
                        <span style="color: #FF9900">Public Administration Aptitude Test.</span> 
                        <span class="learnmore"> Learn more …</span></td>
                </tr>
                <tr>
                    <td>
                        <img alt="" src="images/homepage_photo.jpg" />
                        <div class="homephotoabel" style="height: 25px">
                        We have many more tests to offer. Check out our products for more information</div>
                    </td>
                </tr>
                <tr>
                    <td class="homephotoabel">
                        &nbsp;</td>
                </tr>
                </table>
                </div>
        </td>
        <td valign="top">
            <div style="width: 315px; text-align: left;">
            <table style="width:100%;">
                <tr>
                    <td style="text-align: left">
                        <table style="width: 300px;">
                            <tr>
                                <td background="images/client_login.gif"; style=" background-position: right; background-repeat: no-repeat; width: 306px; height: 35px; background-image: url('images/client_login.gif'); text-align: center;" 
                                    valign="middle"> <img src="images/spacer.gif" width="1" height="9"><img src="images/spacer.gif" align="absmiddle" width="19" height="1"><font class="titlesub3">Registered 
        Users Login Here </font><img src="images/arrow.gif" align="absmiddle" vspace="0" hspace="6" width="8" height="9"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div align="right" style="padding-top: 10px">
        Type your username and password below    
        <input id="txtUsername" style="width: 250px" type="text" runat="server" tabindex="1" /><img src="images/spacer.gif" width="1" height="5"><br>
                                                
        <input id="txtPassword" style="width: 250px" type="password" runat="server" tabindex="2" /><img src="images/spacer.gif" width="1" height="5"><br>
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="#FF3300" TabIndex="10"></asp:Label>
                                                <asp:ImageButton ID="imgBtnLogin" runat="server" 
            ImageUrl="~/images/submit.gif" onclick="imgBtnLogin_Click" TabIndex="3" />
        <%--<a href="/">--%> 
        
        <%--</a>--%></div></td>
                                    </tr>
                                    </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="center">
                        <table>
                            <tr>
                                <td align="center" 
                                    style="height: 375px; width: 250px; text-align: left;" 
                                    valign="top">
                                    <div>
                        <table style="width:247px; height: 370px; background-image: url('images/homepage_bgsmall_3.jpg'); background-repeat: no-repeat;">
                            <%--<tr>
                                <td class="staticheader2_hp">
                                    &nbsp;</td></tr>--%>
                                    <tr>
                                <td style="color: #CC6600; padding-top: 10px;" class="homeverticalimg">
                                <span class="staticheader2_hp">BPO-Call Center Readiness </span><span class="staticheader2_hp">Tests  </span>
                                                        
                                    <br />
                                    Voice English Assessment Test
                                    <br />
                                    (VEAT)
                                    <br />
                                    Non-Voice English Assessment Test (NEAT)</td>
                            </tr>
                            <%--<tr>
                                <td class="staticheader2_hp">
                                    &nbsp;</td>
                            </tr>--%>
                            <%--<tr>
                                <td height="5px">
                                    &nbsp;</td>
                            </tr>--%>
                            <tr>
                                <td style="color: #333300;" class="homeverticalimg">
                                
                                   <span class="staticheader2_hp">Know your Leader Potential</span>  <br />
                                    People Management Skills Test
                                    <br />
                                    (PMST) 
                                    <br />
                                    Test for Effective Communication Skills
                                    <br />
                                    (TECS)
                                    <br />
                                    Test for Assertiveness Skills
                                    <br />
                                    (TAS)
                                    <br />
                                    Test for Effective Presentation Skills<br />
                                    (TEPS)</td>
                            </tr>
                            <%--<tr>
                                <td class="staticheader2_hp">
                                    &nbsp;</td>
                            </tr>--%>
                            <%--<tr>
                                <td height="5px;">
                                    &nbsp;</td>
                            </tr>--%>
                            <tr>
                                <td style="color: #0000FF; padding-bottom: 10px;" class="homeverticalimg">
                               
                                   <span class="staticheader2_hp">Test your Artistic Abilities</span> <br />
                                    Visual Art Aptitude Test (VAAT) 
                                    <br />
                                    Plastic Art Aptitude Test (PLAAT)<br />
                                                                        Performing Art 
                                    Aptitude Test (PRAAT)</td>
                            </tr>
                        </table>
                                                </div>
                                    </td>
                                </tr>
                            </table>
                                </div>
                            </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table style="width:325px; height: 115px; background-image: url('images/homepage_bgsmall_2.jpg'); background-repeat: no-repeat;">
                            <tr>
                                <td valign="top" style="color: #FF0000; font-weight: bold; padding-left: 10px;">
                                    Corporate HR solutions</td>
                                <td style="color: #CC6600; font-weight: bold;" rowspan="2">
                                    Online Job Informatics<br />
                                    Competency Mapping<br />
                                    Diagnostic Gap Analysis<br />
                                    Job Evaluation<br />
                                    Performance 
                                    Benchmarking
                                    <br />
                                    Performance Appraisal</td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <img alt="" src="images/Home_fadedlogo.jpg" /></td>
                            </tr>
                            </table>
                    </td>
                </tr>
            </table>
                </div>
        </td>
    </tr>
    </table>
</body>