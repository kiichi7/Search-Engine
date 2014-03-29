/*
 * Copyright 2005 huson1020
 * 
 * Licensed under the Apache License, Version 2.1.0 (the "License");
 * 
 * 2010��3��26��
 */
#region ���ÿռ�
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
    #region �����ֶ�
    /// <summary>
    /// �������DataTable
    /// </summary>
    protected DataTable Results = new DataTable();

    /// <summary>
    /// �����󷵻ص�ȫ�� �
    /// </summary>
    private int total;

    /// <summary>
    /// ��һ�����ҵ�����.
    /// </summary>
    private int startAt;

    /// <summary>
    /// ��������Ҫ��ʱ��
    /// </summary>
    private TimeSpan duration;

    /// <summary>
    /// ÿҳ��ʾ10������ 
    /// </summary>
    private readonly int maxResults = 10;

    /// <summary>
    /// ��ѯ���ĵ�һ���
    /// </summary>
    private int fromItem;

    /// <summary>
    /// ��ѯ�������һ���
    /// </summary>
    private int toItem;


    /// <summary>
    /// ����ִ���
    /// </summary>
    Analyzer analyzer = new StandardAnalyzer();
    #endregion

    #region �������ڡ��������ķ���
    /// <summary>
    /// �������ڡ��������ķ���
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

        // ��������
        //�������ڡ�index��Ŀ¼��
        string indexDirectory1 = Server.MapPath("./")+"index\\1.5\\";
        IndexSearcher searcher1 = new IndexSearcher(indexDirectory1);

       string indexDirectory2 = Server.MapPath("./") + "index\\1.4\\";
        IndexSearcher searcher2 = new IndexSearcher(indexDirectory2);

        //System.String index3 = @"\\192.168.1.130\index\1.5";
        //Lucene.Net.Index.IndexReader reader3;
        Lucene.Net.Search.ParallelMultiSearcher searcher;

       /* if (System.IO.Directory.Exists(index3))//�Ƿ����Ŀ¼
        {
            reader3 = Lucene.Net.Index.IndexReader.Open(index3);//��ȡ������ȡ����ʵ�����ô�Ϊ������ȡ��reader��ȡ�ļ���Ϊindex���ļ���(Ŀ¼)
            IndexSearcher searcher3 = new IndexSearcher(reader3);


            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher3,searcher1, searcher2 });
        }
        else
        {
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1, searcher2 });
        }*/
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1, searcher2 });

        //====================>(1)�����ѯ����<==============================================
        //System.String field = "text";//���ڱ�����ҳɹ����ļ����ڵ�Ŀ¼ 
        //QueryParser parser = new QueryParser(field, new StandardAnalyzer());//������ѯ��������ָ����field(������ָcontents��Ŀ¼), analyzer����׼��������


        //====================>(2)�����ѯ����<==============================================
        string[] strs = new string[] { "text", "path","fullpath","keywords","description","title" };
        Lucene.Net.QueryParsers.QueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(strs, new StandardAnalyzer());
        parser.SetDefaultOperator(Lucene.Net.QueryParsers.QueryParser.OR_OPERATOR);

        // ���������
        this.Results.Columns.Add("link", typeof(string));
        this.Results.Columns.Add("title", typeof(string));
        this.Results.Columns.Add("sample", typeof(string));
        this.Results.Columns.Add("path", typeof(string));

        // ���� 
        Query query = parser.Parse(this.Query);//����һ����ѯ����ָ����ѯ����Query_condition������ this.Query_txt.Text��
        Hits hits = searcher.Search(query);

        this.total = hits.Length();

        // ���� �����Ĺؼ���,Ĭ����<b>..</b>   
      // �����ָ��<read>..</read>   
      SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<B style='color:Red;'>", "</B>");   
      Highlighter highlighter = new Highlighter(simpleHTMLFormatter, new QueryScorer(query));
      // ���һ�������Ҫ���صģ����������ݳ���   
      // ���̫С����ֻ�����ݵĿ�ʼ���ֱ��������������ҷ��ص�����Ҳ��    ̫����ʱ̫�˷��ˡ�   
      highlighter.SetTextFragmenter(new SimpleFragmenter(100));  

        // initialize startAt
        this.startAt = initStartAt();

        // ��ʾ�������ġ������Ŀ
        int resultsCount = smallerOf(total, this.maxResults + this.startAt);



        for (int i = startAt; i < resultsCount; i++)
        {
            // �õ����е��ĵ�
            Document doc = hits.Doc(i);

            //��ӽ�β����֤��β������Ų�������
            string title = doc.Get("title") + "  ";
            // �õ��ļ�����
            System.String text =Search.CutString( doc.Get("text"),480);
            // �õ��ļ�������ȷ·��
            string path = doc.Get("path");
            string orpath = doc.Get("fullpath");

            Lucene.Net.Analysis.TokenStream titkeStream = analyzer.TokenStream("title", new System.IO.StringReader(title));//��Ŀ
            Lucene.Net.Analysis.TokenStream tokenStream = analyzer.TokenStream("text", new System.IO.StringReader(text));//ժҪ
            Lucene.Net.Analysis.TokenStream pathStream = analyzer.TokenStream("path", new System.IO.StringReader(path));//��ʾ��·��

            System.String result = highlighter.GetBestFragments(tokenStream, text, 2, "...");
            string tresult = highlighter.GetBestFragments(titkeStream, title, 2, "..");
            string pathwords = highlighter.GetBestFragments(pathStream, path, 2, ".."); //·����ʱ��ʾ

            // ����һ������ʾ�������Ľ��
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

        // �����Ϣ
        this.duration = DateTime.Now - start;
        this.fromItem = startAt + 1;
        this.toItem = smallerOf(startAt + maxResults, total);
        
    }
    #endregion

    #region ���桰ҳ�����ӡ��ķ���
    /// <summary>
    /// ҳ�����ӣ�DataTable
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

    #region ����HTML�ļ��ķ���
    /// <summary>
    /// ����HTML�ļ�
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

    #region ����string��������Summary������קҪ��Ϣ��
    /// <summary>
    /// קҪ��Ϣstring
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

    #region ����string��������Query�������ѯ�Ĺؼ��֣�
    /// <summary>
    /// ���ز�ѯ����Ϊ��.
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

    #region ����int��������pageCount�������ѯ����ķ�ҳ����
    /// <summary>
    /// �����Ϊ����ҳ
    /// </summary>
    private int pageCount
    {
        get
        {
            return (total - 1) / maxResults; // floor
        }
    }
    #endregion

    #region ����int��������lastPageStartsAt�������ѯ����ķ�ҳ�������һҳ��
    /// <summary>
    /// ���һҳ�ġ���һ�
    /// </summary>
    private int lastPageStartsAt
    {
        get
        {
            return pageCount * maxResults;
        }
    }
    #endregion

    #region �������ڲ�ѯ��ҳ��initStartAt��������
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

    #region �������ڲ�ѯ�����10����ֶ���ҳ��smallerOf��������
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

    #region ����HTML�ļ���parseHtml��������
    /// <summary>
    /// ��HTML�ļ�������ݡ�<>����־�滻Ϊ�գ�""��
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private string parseHtml(string html)
    {
        string temp = Regex.Replace(html, "<[^>]*>", "");
        return temp.Replace("&nbsp;", " ");
    }
    #endregion

    #region ҳ�����ʱ�򣬵��õĺ���Page_Load����������
    /// <summary>
    /// ҳ�����ʱ���Զ����� indexer.AddDirectory���� ���������ĺ�������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///    
    protected void Page_Load(object sender, EventArgs e){

        // ��Ӧ�س�
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

    #region �Զ����ѯ����HtmlQuery()������
    /// <summary>
    /// ��ѯHTML�ļ�
    /// </summary>
    protected void HtmlQuery()
    {

    }
    #endregion

    /// <summary>
    /// ��������
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


    #region �ַ�����ȡ����
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
        //����ع�����ϰ��ʡ�Ժ�
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
        if (mybyte.Length > len)
            tempString += "��";

        return tempString;
    }
    #endregion


}
