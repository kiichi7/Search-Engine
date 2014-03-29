using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.IO;
using SpiderLib;


public partial class FtpDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //SpiderLib.DownloadFtp ftl = new DownloadFtp("ftp://172.16.32.189:21/123.txt", "imyang", "");
        //Response.Write("<br/><br/><br/><strong>已经下载了FTP返回信息FtpWebResponse</strong>");

        SpiderLib.FTPClient ft = new FTPClient("172.16.32.158", "/", "Anonymous", "", 21);
        ft.Connect();
        foreach (string str in ft.Dir("*.rar"))
        {
            Response.Write(str.ToString() + "<br/>");
        }

    }
}
