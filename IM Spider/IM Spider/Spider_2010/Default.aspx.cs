using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.IO;
using SpiderLib;


public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void BtnDownload_Click(object sender, EventArgs e)
    {
        SpiderLib.DownloadHtml don = new DownloadHtml(this.DownloadUri.Text);

        Response.Write("<script type='text/javascript'>window.alert(' 已经下载了网页文件!!! ');</script>");


    }
}
