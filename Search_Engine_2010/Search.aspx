<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" enableViewState="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" ontent="text/html; charset=UTF-8" />
    <title>IM071_Search Engine_IM071</title>
    <link  type="text/css" href="css/main.css" rel="stylesheet"/>
</head>
<body style="background-color: White" text="#000000" link="#0000cc" vlink="#551a8b"alink="#ff0000" topmargin="3" marginheight="3">

   <Form id="Form1" runat=server  action=""> 
   <div id="guser" width="100%">
   <nobr><a href="" onclick="this.style.behavior='url(#default#homepage)';this.setHomePage('http://www.baidu.com/');"  >设置 IM071 为首页</a> |<a href="#" onclick="javascript:window.external.addFavorite('http://www.baidu.com/', '收藏网站 IM071搜索引擎');" >收藏网站</a>| <a href="AddIndex.aspx">索引管理</a>
</nobr>
    </div>    
 <!--，以下两个DIV为隔离上面右上角的连接而设置
 结束使用-->
    <div class="gbh" style="left: 0px; top: 24px;">
    </div>
    <div class="gbh" style="right: -7px; top: 0px;">
    </div>

        <table cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td width="25%" style="height: 70px">
                 <img src="images/logo.png" width="276" height="110" border="0" alt="IM071" title="IM071" id="logo">
                </td>
                <td align="center" nowrap="nowrap" style="height: 70px"> 
                <br /><br />
<asp:TextBox ID="query_txtbox" runat="server" Width="468px"  Font-Bold="False" Font-Names="宋体" Font-Size="X-Large" Height="27px" Text="<%# Query %>"></asp:TextBox>
<br /><span style="float:left;"><b ><font color="blue" size="2">&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;测试： </font></b><a href="Search.aspx?q=html">搜索关键字“html”</a></span>                                      
                </td>
                <td nowrap width="25%" align="left" style="height: 70px">
<br /><br />  <asp:Button ID="query_btn" runat="server" OnClick="query_btn_Click" Text="IM071 搜索" Font-Bold="False" Font-Size="X-Large" Height="38px"  />   </td>
            </tr>
        </table>      
        <br />
        <!-- 搜索结果的显示 简介和链接-->
        
 <p >
    <asp:repeater id=Repeater1 runat="server" DataSource="<%# Results %>"  >
		<ItemTemplate>
		<p style="padding-left:13px;">
		<a  class="link" href='<%# DataBinder.Eval(Container.DataItem, "link")  %>' target="_blank"><%# DataBinder.Eval(Container.DataItem, "title")  %></a><br>
							<span class="sample"><%# DataBinder.Eval(Container.DataItem, "sample")  %></span>
							<br>
							<span class="path"><%# DataBinder.Eval(Container.DataItem, "path")  %></span>
							
		</p>
					</ItemTemplate>
				</asp:repeater>				
</P>
<!-- 搜索结果的显示-->

 <P id="pagecount" class="paging"><font color="red" size="4">Result page:</font> 
				        <asp:repeater id="Repeater2" runat="server" DataSource="<%# Paging %>">
					        <ItemTemplate>
						        <%# DataBinder.Eval(Container.DataItem, "html") %>
					        </ItemTemplate>
				        </asp:repeater>
                   </p>
        <br />         
<p style="text-align:center;"><span style="font-size: 13px"><font size="2" >©2010 - <a href="#">IM071 &nbsp;此内容系IM071根据您的指令自动搜索的结果，不代表IM071赞成被搜索网站的内容或立场</a><br /> - <a href="#" target="_blank">ICP证合字B2-20100004号</a></font></span></p>
         </Form>  

</body>
</html>
