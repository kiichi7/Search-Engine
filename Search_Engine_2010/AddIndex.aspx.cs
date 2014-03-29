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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;

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

public partial class AddIndex : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void AddIndexbtn_Click(object sender, EventArgs e)
    {
        Lucene.Net.Store.Directory ramdir = new Lucene.Net.Store.RAMDirectory();

        Console.WriteLine("Indexing...");
        DateTime start = DateTime.Now;

        string path4 = Server.MapPath("./") + @"1.4\\";
        if (System.IO.Directory.Exists(path4))//是否存在目录
        {
            Indexer.IntranetIndexer indexer4 = new Indexer.IntranetIndexer(Server.MapPath("index\\1.4\\"));
            indexer4.AddDirectory(new System.IO.DirectoryInfo(path4), "*.*");
            indexer4.Close(); 
        }
        //IntranetIndexer indexer = new IntranetIndexer(ramdir);//把索引写进内存

        string path5 = Server.MapPath("./") + @"1.5\\";
        if (System.IO.Directory.Exists(path5))
        { 
             Indexer.IntranetIndexer indexer5 = new Indexer.IntranetIndexer(Server.MapPath("index\\1.5\\"));
             indexer5.AddDirectory(new System.IO.DirectoryInfo(path5), "*.*");
             indexer5.Close();

        }



        Console.WriteLine("Done. Took " + (DateTime.Now - start));
        Response.Write("<script type='text/javascript'>window.alert(' 创建索引成功，并已经优化!!! ');</script>");
    }   
}
