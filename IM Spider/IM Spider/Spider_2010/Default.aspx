<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center>
    <br /><br /><br /><br /><br /><br /><br />
     <font>网页文件名字：</font>
    <asp:TextBox ID="DownloadUri" runat="server" Width="406px"></asp:TextBox>
    <asp:Button ID="BtnDownload" runat="server" Text="下载网页" Font-Bold="True" OnClick="BtnDownload_Click"/>
    <br /><br />
    <font color="red">网页文件名字，应为全名，结尾为html、asp、aspx、php、jar、do、action等等</font>    
    </center>
    
    </div>
    </form>
</body>
</html>
