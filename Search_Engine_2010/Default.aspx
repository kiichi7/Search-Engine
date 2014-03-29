<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <title>IM071_Search Engine_IM071</title>
    <link  type="text/css" href="css/main.css" rel="stylesheet"/>
<style type="text/css"> 
<!-- 
html{filter:gray;}
<%--BODY { 
filter:progid:DXImageTransform.Microsoft.BasicImage(grayscale=1); 
} --%>
--> 
</style> 
<script type="text/javascript">
  function Focus(){
  if(window.event.keycode==32||window.event.keycode==13){
   document.getElementById("query_btn").focus();
   document.getElementById("query_btn").click();
   document.getElementById("query_btn").style.backgroundColor="red";   
  }
  }
  </script>
</head>
<body text="#000000"  link="#0000cc" vlink="#551a8b"alink="#ff0000" topmargin="3" marginheight="3" >
<center>
    <Form id="Form1" runat=server  action=""> 
   <div id="guser" width="100%">
   <nobr><a href="" onclick="this.style.behavior='url(#default#homepage)';this.setHomePage('http://www.baidu.com/');"  >设置 IM071 为首页</a> |<a href="#" onclick="javascript:window.external.addFavorite('http://www.baidu.com/', '收藏网站 IM071搜索引擎');" >收藏网站</a>| <a href="Search.aspx">网页搜索</a>
</nobr>
    </div>    
 <!--，以下两个DIV为隔离上面右上角的连接而设置
 结束使用-->
    <div class="gbh" style="left: 0px; top: 24px;">
    </div>
    <div class="gbh" style="right: -7px; top: 0px;">
    </div>
        <img src="images/logo.png" width="276" height="110" border="0" alt="IM071" title="IM071"
            id="logo">
        <table cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td width="25%" style="height: 70px">
                    <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Size="Large" OnClick="Button1_Click"
                        Text="建立索引" Width="126px" /></td>
                <td align="center" nowrap="nowrap" style="height: 70px">                           
          <asp:TextBox ID="query_txtbox" runat="server" Width="468px" Font-Bold="False" Font-Names="宋体" Font-Size="X-Large" Height="27px"></asp:TextBox><br>
                    <asp:Button ID="query_btn" runat="server" OnClick="query_btn_Click" Text="IM071 非html文件搜索" Font-Bold="True" Font-Size="Large" Height="33px"  />                     
                </td>
                <td nowrap width="25%" align="left" style="height: 70px">
                    <span style="font-size: 13px">&nbsp;</span></td>
            </tr>
        </table>      
        <br />
        <asp:TextBox ID="result_TxtBox" runat="server" Width="579px" TextMode="MultiLine" Height="294px"></asp:TextBox>  
        <br />        
        <br />       
        <br />
            <p>
                <span style="font-size: 13px">©2010 - <a href="#">IM071搜索
此内容系IM071根据您的指令自动搜索的结果，不代表IM071赞成被搜索网站的内容或立场</a> - <a href="#" target="_blank">
                    ICP证合字B2-20100004号</a></span></p>
         </Form>  
         </center>
</body>
</html>
