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

using Lucene.Net;
using Directory = Lucene.Net.Store.Directory;
using FSDirectory = Lucene.Net.Store.FSDirectory;
using IndexReader = Lucene.Net.Index.IndexReader;
using Term = Lucene.Net.Index.Term;
using StandardAnalyzer = Lucene.Net.Analysis.Standard.StandardAnalyzer;
using IndexWriter = Lucene.Net.Index.IndexWriter;
using Lucene.Net.Demo;
using Analyzer = Lucene.Net.Analysis.Analyzer;
using Document = Lucene.Net.Documents.Document;
using FilterIndexReader = Lucene.Net.Index.FilterIndexReader;
using QueryParser = Lucene.Net.QueryParsers.QueryParser;
using Hits = Lucene.Net.Search.Hits;
using IndexSearcher = Lucene.Net.Search.IndexSearcher;
using Query = Lucene.Net.Search.Query;
using Searcher = Lucene.Net.Search.Searcher;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e){
        
    }
    protected void BtnDownload_Click(object sender, EventArgs e){
        try
        {
            check(this.DownloadUri.ID, this.DownloadUri.Text);
            SaveFullPath(this.DownloadUri.Text);
            Response.Write("<script type='text/javascript'>window.alert(' 已经下载了网页文件!!! ');</script>");
            
        }
        catch (Exception ef)
        {
            //Response.Write("<script type='text/javascript'>window.alert('" + ef.ToString() + "dddddddd465465" + "');</script>");
            //Response.Write("<script type='text/javascript'>window.alert('");
            Response.Write(ef.ToString());
            //Response.Write("')</script>");

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="st">文本框ID</param>
    /// <param name="str">文本框的值</param>
    protected void check(string st,string str) {
        if (str != null){
            TextBox stID = Page.FindControl(st) as TextBox;
            SpiderLib.DownloadHtml don = new DownloadHtml(stID.Text);
        }
    
    }
    /// <summary>
    /// 存储全路径的方法
    /// </summary>
    /// <param name="fullpath">html文件路径</param>
    protected void SaveFullPath(string fullpath) {
        string path=Server.MapPath("../") + @"SaveFullPath.txt";
        //System.IO.FileStream fi=new FileInfo(path).Create();        
        //StreamWriter sw = new StreamWriter(fi,System.Text.Encoding.Default);

        StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.GetEncoding("GB2312"));


        //StreamWriter sw = new FileInfo(path).AppendText(); 
        sw.Write(System.IO.Path.GetFileName(fullpath) + "*" + fullpath + "\r\n");
        sw.Close();
          
       
    }

}
