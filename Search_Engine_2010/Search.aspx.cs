/*
 * Copyright 2005 huson1020
 * 
 * Licensed under the Apache License, Version 2.1.0 (the "License");
 * 
 * 2010年3月26日
 */
#region 引用空间
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

using Lucene.Net;
using Lucene.Net.Demo;


using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

using Indexer;
using Highlighter = Lucene.Net.Highlight.Highlighter;
using QueryScorer = Lucene.Net.Highlight.QueryScorer;
using SimpleHTMLFormatter = Lucene.Net.Highlight.SimpleHTMLFormatter;
using SimpleFragmenter = Lucene.Net.Highlight.SimpleFragmenter;
#endregion

public partial class Search : System.Web.UI.Page
{
    #region 定义字段
    /// <summary>
    /// 搜索结果DataTable
    /// </summary>
    protected DataTable Results = new DataTable();

    /// <summary>
    /// 搜索后返回的全部 项。
    /// </summary>
    private int total;

    /// <summary>
    /// 第一个查找到的项.
    /// </summary>
    private int startAt;

    /// <summary>
    /// 搜索所需要的时间
    /// </summary>
    private TimeSpan duration;

    /// <summary>
    /// 每页显示10条数据 
    /// </summary>
    private readonly int maxResults = 10;

    /// <summary>
    /// 查询到的第一“项”
    /// </summary>
    private int fromItem;

    /// <summary>
    /// 查询到的最后一“项”
    /// </summary>
    private int toItem;


    /// <summary>
    /// 定义分词器
    /// </summary>
    Analyzer analyzer = new StandardAnalyzer();
    #endregion

    #region 定义用于“搜索”的方法
    /// <summary>
    /// 定义用于“搜索”的方法
    /// </summary>
    private void search()
    {
        DateTime start = DateTime.Now;

        //try
        //{
        // }
        //catch (Exception e)
        //{

        //    Response.Write("<script type='text/javascript'>window.alert(' " + e.ToString() + " ');</script>");

        //}

        // 创建搜索
        //索引放在“index”目录下
        string indexDirectory1 = Server.MapPath("./")+"index\\1.5\\";
        IndexSearcher searcher1 = new IndexSearcher(indexDirectory1);

       string indexDirectory2 = Server.MapPath("./") + "index\\1.4\\";
        IndexSearcher searcher2 = new IndexSearcher(indexDirectory2);

        //System.String index3 = @"\\192.168.1.130\index\1.5";
        //Lucene.Net.Index.IndexReader reader3;
        Lucene.Net.Search.ParallelMultiSearcher searcher;

       /* if (System.IO.Directory.Exists(index3))//是否存在目录
        {
            reader3 = Lucene.Net.Index.IndexReader.Open(index3);//读取索引读取器的实例，该处为索引读取器reader读取文件名为index的文件夹(目录)
            IndexSearcher searcher3 = new IndexSearcher(reader3);


            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher3,searcher1, searcher2 });
        }
        else
        {
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1, searcher2 });
        }*/
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1, searcher2 });

        //====================>(1)单域查询……<==============================================
        //System.String field = "text";//用于保存查找成功后文件所在的目录 
        //QueryParser parser = new QueryParser(field, new StandardAnalyzer());//创建查询解析器，指定了field(这里是指contents即目录), analyzer（标准分析器）


        //====================>(2)多域查询……<==============================================
        string[] strs = new string[] { "text", "path","fullpath","keywords","description","title" };
        Lucene.Net.QueryParsers.QueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(strs, new StandardAnalyzer());
        parser.SetDefaultOperator(Lucene.Net.QueryParsers.QueryParser.OR_OPERATOR);

        // 创建结果集
        this.Results.Columns.Add("link", typeof(string));
        this.Results.Columns.Add("title", typeof(string));
        this.Results.Columns.Add("sample", typeof(string));
        this.Results.Columns.Add("path", typeof(string));

        // 搜索 
        Query query = parser.Parse(this.Query);//建立一个查询器（指定查询条件Query_condition，即是 this.Query_txt.Text）
        Hits hits = searcher.Search(query);

        this.total = hits.Length();

        // 创建 高亮的关键字,默认是<b>..</b>   
      // 用这个指定<read>..</read>   
      SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<B style='color:Red;'>", "</B>");   
      Highlighter highlighter = new Highlighter(simpleHTMLFormatter, new QueryScorer(query));
      // 这个一般等于你要返回的，高亮的数据长度   
      // 如果太小，则只有数据的开始部分被解析并高亮，且返回的数据也少    太大，有时太浪费了。   
      highlighter.SetTextFragmenter(new SimpleFragmenter(100));  

        // initialize startAt
        this.startAt = initStartAt();

        // 显示搜索到的“项”的数目
        int resultsCount = smallerOf(total, this.maxResults + this.startAt);



        for (int i = startAt; i < resultsCount; i++)
        {
            // 拿到命中的文档
            Document doc = hits.Doc(i);

            //添加结尾，保证结尾特殊符号不被过滤
            string title = doc.Get("title") + "  ";
            // 拿到文件内容
            System.String text =Search.CutString( doc.Get("text"),480);
            // 拿到文件名、正确路径
            string path = doc.Get("path");
            string orpath = doc.Get("fullpath");

            Lucene.Net.Analysis.TokenStream titkeStream = analyzer.TokenStream("title", new System.IO.StringReader(title));//题目
            Lucene.Net.Analysis.TokenStream tokenStream = analyzer.TokenStream("text", new System.IO.StringReader(text));//摘要
            Lucene.Net.Analysis.TokenStream pathStream = analyzer.TokenStream("path", new System.IO.StringReader(path));//显示的路径

            System.String result = highlighter.GetBestFragments(tokenStream, text, 2, "...");
            string tresult = highlighter.GetBestFragments(titkeStream, title, 2, "..");
            string pathwords = highlighter.GetBestFragments(pathStream, path, 2, ".."); //路径有时显示

            // 建立一行来显示搜索到的结果
            DataRow row = this.Results.NewRow();
            if (tresult == "")
            {
                row["title"] = title;
            }
            else {
                row["title"] = tresult;

            }
            if (getpath(row, System.IO.Path.GetFileName(path.Replace("\\", "/"))))
            {
               row["link"]=getFullpath( System.IO.Path.GetFileName(doc.Get("path")));
            }
            else {
                row["link"] =orpath;
                if (pathwords=="")
                {
                    row["path"] = orpath;
                    
                }
                else
                {
                    row["path"] = pathwords.Replace("\\", "/");
                    
                } 
                
            }

            if (result == ""){
                row["sample"] = text;

            }
            else {
                row["sample"] = result;               

            }

            this.Results.Rows.Add(row);           
        }
        searcher.Close();

        // 结果信息
        this.duration = DateTime.Now - start;
        this.fromItem = startAt + 1;
        this.toItem = smallerOf(startAt + maxResults, total);
        
    }
    #endregion

    #region 保存“页面链接”的方法
    /// <summary>
    /// 页面链接，DataTable
    /// Page links. DataTable might be overhead but there used to be more fields in previous version so I'm keeping it for now.
    /// </summary>
    protected DataTable Paging
    {
        get
        {
            // pageNumber starts at 1
            int pageNumber = (startAt + maxResults - 1) / maxResults;

            DataTable dt = new DataTable();
            dt.Columns.Add("html", typeof(string));

            DataRow ar = dt.NewRow();
            ar["html"] = pagingItemHtml(startAt, pageNumber + 1, false);
            dt.Rows.Add(ar);

            int previousPagesCount = 4;
            for (int i = pageNumber - 1; i >= 0 && i >= pageNumber - previousPagesCount; i--)
            {
                int step = i - pageNumber;
                DataRow r = dt.NewRow();
                r["html"] = pagingItemHtml(startAt + (maxResults * step), i + 1, true);

                dt.Rows.InsertAt(r, 0);
            }

            int nextPagesCount = 4;
            for (int i = pageNumber + 1; i <= pageCount && i <= pageNumber + nextPagesCount; i++)
            {
                int step = i - pageNumber;
                DataRow r = dt.NewRow();
                r["html"] = pagingItemHtml(startAt + (maxResults * step), i + 1, true);

                dt.Rows.Add(r);
            }
            return dt;
        }
    }
    #endregion

    #region 解析HTML文件的方法
    /// <summary>
    /// 解析HTML文件
    /// Prepares HTML of a paging item (bold number for current page, links for others).
    /// </summary>
    /// <param name="start"></param>
    /// <param name="number"></param>
    /// <param name="active"></param>
    /// <returns></returns>
    private string pagingItemHtml(int start, int number, bool active)
    {

        if (active)
            return "<a href=\"Search.aspx?q=" + Server.UrlPathEncode(this.Query) + "&start=" + start + "\">" + number + "</a>";
        else
            return "<b>" + number + "</b>";
    }
    #endregion

    #region 定义string类型属性Summary（保存拽要信息）
    /// <summary>
    /// 拽要信息string
    /// Prepares the string with seach summary information.
    /// </summary>
    protected string Summary
    {
        get
        {
            if (total > 0)
                return "Results <b>" + this.fromItem + " - " + this.toItem + "</b> of <b>" + this.total + "</b> for <b>" + this.Query + "</b>. (" + this.duration.TotalSeconds + " seconds)";
            return "No results found";
        }
    }
    #endregion

    #region 定义string类型属性Query（保存查询的关键字）
    /// <summary>
    /// 返回查询或者为空.
    /// </summary>
    protected string Query
    {
        get
        {
            string query = this.Request.Params["q"];
            if (query == String.Empty)
                return null;
            return query;
        }
    }
    #endregion

    #region 定义int类型属性pageCount（保存查询结果的分页数）
    /// <summary>
    /// 结果分为多少页
    /// </summary>
    private int pageCount
    {
        get
        {
            return (total - 1) / maxResults; // floor
        }
    }
    #endregion

    #region 定义int类型属性lastPageStartsAt（保存查询结果的分页数的最后一页）
    /// <summary>
    /// 最后一页的“第一项”
    /// </summary>
    private int lastPageStartsAt
    {
        get
        {
            return pageCount * maxResults;
        }
    }
    #endregion

    #region 定义用于查询分页的initStartAt方法（）
    /// <summary>
    /// Initializes startAt value. Checks for bad values.
    /// </summary>
    /// <returns></returns>
    private int initStartAt()
    {
        try
        {
            int sa = Convert.ToInt32(this.Request.Params["start"]);

            // too small starting item, return first page
            if (sa < 0)
                return 0;

            // too big starting item, return last page
            if (sa >= total - 1)
            {
                return lastPageStartsAt;
            }

            return sa;
        }
        catch
        {
            return 0;
        }
    }
    #endregion

    #region 定义用于查询结果按10条项分多少页的smallerOf方法（）
    /// <summary>
    /// Returns the smaller value of parameters.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    private int smallerOf(int first, int second)
    {
        return first < second ? first : second;
    }
    #endregion

    #region 解析HTML文件的parseHtml方法（）
    /// <summary>
    /// 把HTML文件里的内容“<>”标志替换为空（""）
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private string parseHtml(string html)
    {
        string temp = Regex.Replace(html, "<[^>]*>", "");
        return temp.Replace("&nbsp;", " ");
    }
    #endregion

    #region 页面加载时候，调用的函数Page_Load（）。。。
    /// <summary>
    /// 页面加载时候，自动调用 indexer.AddDirectory（） 创建索引的函数。。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///    
    protected void Page_Load(object sender, EventArgs e){

        // 响应回车
        Page.RegisterHiddenField("__EVENTTARGET", "ButtonSearch");
        if (!IsPostBack)
        {
            if (this.Query != null)
            {
                search();
                DataBind();
            }
        }
    }
    #endregion

    #region 自定义查询方法HtmlQuery()。。。
    /// <summary>
    /// 查询HTML文件
    /// </summary>
    protected void HtmlQuery()
    {

    }
    #endregion

    /// <summary>
    /// 进行搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void query_btn_Click(object sender, EventArgs e)
    {
        if (this.query_txtbox.Text != null)
        {
            Response.Redirect("Search.aspx?q=" +Server.UrlPathEncode(this.query_txtbox.Text) ); 
        }

    }

    protected bool getpath(DataRow row ,string filename)
    {
    string newValue = string.Empty;
    bool flat = false;
    using (StreamReader read = new StreamReader(Server.MapPath("./") + @"SaveFullPath.txt", System.Text.Encoding.Default))
            {
                do{
                    newValue = read.ReadLine();

                    //Progress_label.Text += "\r\n" + newValue;
                    if (newValue!=null){

                        string[] st = newValue.Split('*');

                        if (st[0] == filename){
                            row["path"] = st[1].Replace("\\", "/");
                            flat=true;
                            break;}                   
                   } 

                }while (newValue != null);                   
            }
            return flat;
    }
 
    protected string getFullpath(string filename)
    {
        string newValue = string.Empty;
        string orfullpath;
        using (StreamReader read = new StreamReader(Server.MapPath("./") + @"SaveFullPath.txt", System.Text.Encoding.Default))
        { 
            do
            {
                newValue = read.ReadLine();
                if (newValue != null) {

                    string[] st = newValue.Split('*');

                    if (st[0] == filename)
                    {
                        orfullpath=st[1].Replace("\\", "/");
                        return orfullpath;
                        break;
                    } 
                }
            } while (newValue != null);
            return null;
        }
   }


    #region 字符串截取函数
    public static string CutString(string inputString, int len)
    {

        System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
        int tempLen = 0;
        string tempString = "";
        byte[] s = ascii.GetBytes(inputString);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }

            try
            {
                tempString += inputString.Substring(i, 1);
            }
            catch
            {
                break;
            }

            if (tempLen > len)
                break;
        }
        //如果截过则加上半个省略号
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
        if (mybyte.Length > len)
            tempString += "…";

        return tempString;
    }
    #endregion


}
