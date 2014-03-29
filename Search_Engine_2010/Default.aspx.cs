/*
 * Copyright 2005 huson1020
 * 
 * Licensed under the Apache License, Version 2.1.0 (the "License");
 * 
 * 2010年3月26日
 */
using System;
using System.Data;
using System.Configuration;
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


public  partial class _Default : System.Web.UI.Page 
{   

/* 变量INDEX_DIR用于在建立IndexWriter实例（索引实例的参数：目录Directory）,相当于建了一个目录，名字叫index，且有只读属性!!
         该目录用于保存建立的索引文件
         */
    System.IO.FileInfo INDEX_DIR = new System.IO.FileInfo("index\\File\\");
    Lucene.Net.Store.Directory ramdir = new Lucene.Net.Store.RAMDirectory();
    IndexWriter fswriter;
    IndexWriter writer;

#region   //以下方法用于遍历文件（包括目录、子目录的文件）,主要是用递归的方法来遍历的,并且在遍历的时候，顺便把文件的一些属性写进索引文档。
public void ListFiles(FileSystemInfo fileinfo)
   {

       if (!fileinfo.Exists) return;

       DirectoryInfo dirinfo = fileinfo as DirectoryInfo;

       //if (dirinfo == null) return; //不是目录 

       FileSystemInfo[] files = dirinfo.GetFileSystemInfos();

       for (int i = 0; i < files.Length; i++)    //遍历目录下所有文件、子目录 
       {
           FileInfo file = files[i] as FileInfo;

           if (file != null) // 是文件 
           {
               Document dot = new Document();//定义一个文档集，用来保存域 
               dot.Add(new Lucene.Net.Documents.Field("title", files[i].Name, Lucene.Net.Documents.Field.Store.YES,
                       Lucene.Net.Documents.Field.Index.TOKENIZED));//向文档集加进一个field域,文件名（title），可分词（TOKENIZED）
               dot.Add(new Lucene.Net.Documents.Field("path", files[i].FullName, Lucene.Net.Documents.Field.Store.YES,
                       Lucene.Net.Documents.Field.Index.UN_TOKENIZED));//向文档集加进一个field域，完整路径（path），不可分词（UN_TOKENIZED）
               //增加一个存储“内容”[contens]的域
               dot.Add(new Lucene.Net.Documents.Field("contents", new System.IO.StreamReader(files[i].FullName, System.Text.Encoding.Default)));
               writer.AddDocument(dot);

               Response.Write("<script type='text/javascript'>window.alert("+"当前写进索引文档的文件为：   " + file.Name +")</script>");

           }

           else //是目录 
           {
               ListFiles(files[i]); //对子目录进行递归调用 
           }
       }

   }
public void ListFiles2(FileSystemInfo fileinfo)
    {

        if (!fileinfo.Exists) return;

        DirectoryInfo dirinfo = fileinfo as DirectoryInfo;

        //if (dirinfo == null) return; //不是目录 

        FileSystemInfo[] files = dirinfo.GetFileSystemInfos();

        for (int i = 0; i < files.Length; i++)    //遍历目录下所有文件、子目录 
        {
            FileInfo file = files[i] as FileInfo;

            if (file != null) // 是文件 
            {
                Document dot = new Document();//定义一个文档集，用来保存域 
                dot.Add(new Lucene.Net.Documents.Field("title", files[i].Name, Lucene.Net.Documents.Field.Store.YES,
                        Lucene.Net.Documents.Field.Index.TOKENIZED));//向文档集加进一个field域,文件名（title），可分词（TOKENIZED）
                dot.Add(new Lucene.Net.Documents.Field("path", files[i].FullName, Lucene.Net.Documents.Field.Store.YES,
                        Lucene.Net.Documents.Field.Index.UN_TOKENIZED));//向文档集加进一个field域，完整路径（path），不可分词（UN_TOKENIZED）
                //增加一个存储“内容”[contens]的域
                dot.Add(new Lucene.Net.Documents.Field("contents", new System.IO.StreamReader(files[i].FullName, System.Text.Encoding.Default)));
                fswriter.AddDocument(dot);

          Response.Write("<script type='text/javascript'>window.alert(" +"当前写进索引文档的文件为：   " + file.Name + ")</script>");

            }

            else //是目录 
            {
                ListFiles(files[i]); //对子目录进行递归调用 
            }
        }

    }
#endregion

#region  页面的Page_Load方法！也是web网站程序调用的开始的地方~
    DirectoryInfo docDir; DirectoryInfo docDir2;
protected void Page_Load(object sender, EventArgs e)
    {

         docDir = new System.IO.DirectoryInfo(Server.MapPath("./")+@"connect_bak");
        // docDir2 = new System.IO.DirectoryInfo(Server.MapPath("./") + @"1.4");
         docDir2 = new System.IO.DirectoryInfo(Server.MapPath("./") + @"1.5");

        this.result_TxtBox.Text = "查找路径： " + "\n";
        this.result_TxtBox.Text += docDir.FullName + "\r\n" ;
        this.result_TxtBox.Text += "———————————————————————————————";
        this.result_TxtBox.Text += "\n"+ docDir2.FullName;
       
    }
#endregion  

protected void  query_btn_Click(object sender, EventArgs e)
{
    this.Query();
}

#region  用于查询的方法
public void Query()
    {

        //建立查询需要的实例对象
        //==================================>(1)搜索磁盘索引:  File 、File2<==============================
        //Lucene.Net.Index.MultiReader reader = new Lucene.Net.Index.MultiReader(new IndexReader[] { IndexReader.Open(@"\\172.16.32.189\File"), IndexReader.Open(Server.MapPath("./") + @"index\\File\\") });


        Lucene.Net.Search.ParallelMultiSearcher searcher;

        System.String index1 = Server.MapPath("./") + "index\\File\\";
        IndexReader reader1 = IndexReader.Open(index1);//读取索引读取器的实例，该处为索引读取器reader读取文件名为index的文件夹(目录)
        Searcher searcher1 = new IndexSearcher(reader1);//指定searcher(搜索)的 IndexReader(索引读取器)
        Analyzer analyzer = new StandardAnalyzer();//标准分析器

        System.String index2 = @"\\172.16.32.189\File";
        //System.String index = Server.MapPath("./") + "index\\File2\\";//索引所在的目录名            

        if (new System.IO.FileInfo(index2).Exists)
        {
            IndexReader reader2 = Lucene.Net.Index.IndexReader.Open(index2);//读取索引读取器的实例，该处为索引读取器reader读取文件名为index的文件夹(目录)
            IndexSearcher searcher2 = new IndexSearcher(reader2);            
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1, searcher2 });
        }
        else
        {
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1 });
        }

        //==================================>(2)搜索内存索引<==============================
        //Lucene.Net.Search.IndexSearcher searcher = new IndexSearcher(ramdir);


        //查询前，把结果的显示清空
        this.result_TxtBox.Text = "";
        //处理查询条件
        if (this.query_txtbox.Text.Trim(new char[] { ' ' }) == String.Empty)
            return;

        System.String Query_condition = this.query_txtbox.Text.Trim();



        //====================>(1)单域查询……<==============================================
       //System.String field= "title";//用于保存查找成功后文件所在的目录 
       //QueryParser parser = new QueryParser("field", analyzer);//创建查询解析器，指定了field(这里是指contents即目录), analyzer（标准分析器）

      
       //====================>(2)多域查询……<==============================================
       string[] strs = new string[] { "title", "contents" };
       Lucene.Net.QueryParsers.QueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(strs, new StandardAnalyzer());
       parser.SetDefaultOperator(Lucene.Net.QueryParsers.QueryParser.OR_OPERATOR);


       Query query = parser.Parse(Query_condition);//建立一个查询器（指定查询条件Query_condition，即是 this.Query_txt.Text）
       //命中处理
        Hits hits = searcher.Search(query);//定义hits变量保存Search（搜索)query，得到的文件
        if (hits.Length()==0)
        {
            this.result_TxtBox.Text += "==========不好意思，查询不到你输入的关键字。^+^===========" + "\n";
         }
       else{       
       for (int i = 0; i < hits.Length(); i++)
        {
            // 拿到命中的文档
            Document doc = hits.Doc(i);


            //string filename = doc.Get("path");//拿到命中文件名的完整目录路径  
            string result_path = doc.Get("path");//拿到命中的文件目录名

            //string folder = Path.GetDirectoryName(result_path);

            string filename = Path.GetFileName(result_path);//拿到命中文件名  

            //DirectoryInfo di = new DirectoryInfo(folder);

            this.result_TxtBox.Text += "======================================================" + "\n";
            this.result_TxtBox.Text += "查询结果所在的文件:   "+filename + "\n";            
            //this.result_TxtBox.Text += "查询结果所在的目录:   " + di.FullName + "\n";

        }
    }
       //reader.Close(); 
       searcher.Close();
   
   }
#endregion

#region IndexDocs()方法用于把物理文件写进索引
   internal void IndexDocs(IndexWriter writer, System.IO.FileInfo file)
    {
        // do not try to index files that cannot be read
        // if (file.canRead())  // {{Aroush}} what is canRead() in C#?
        try
        {
            #region
            //如果在磁盘上存在你选择文件file的目录，则拿到文件名的目录，并且判断不为空后，循环写进索引文件中
            if (System.IO.Directory.Exists(file.FullName))
            {
                //返回文件名的目录
                System.String[] files = System.IO.Directory.GetFileSystemEntries(file.FullName);
                // an IO error could occur
                if (files != null)
                {
                    //如果文件不是空的，则循环把文件信息写进索引writer
                    for (int i = 0; i < files.Length; i++)
                    {
                        IndexDocs(writer, new System.IO.FileInfo(files[i]));

                    }
                }
            }
            else
            {

                System.Console.Out.WriteLine("adding:" + file);

                try
                {
                    writer.AddDocument(FileDocument.Document(file));//增加一个索引的文件
                }
                catch (System.IO.FileNotFoundException fnfe)
                {
                    ;
                }
            }
            #endregion

        }
        catch (System.UnauthorizedAccessException ue)
        {
            ;
        }




    }
   #endregion
   /* private void LoadDocumnt() 
    {
        Done de = new Done();
        de.ToString();
        write_test we = new write_test("st");
        //System.Net.HttpWebResponse ht = new HttpWebResponse();
        //System.Net.HttpWebRequest hp = new HttpWebRequest();
        WebClient wc = new WebClient();
    }*/
    protected void BuildIndexBtn_Click(object sender, EventArgs e)
    {
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            // 响应回车
            System.Console.Out.WriteLine("索引放在该目录： '" + INDEX_DIR + "'...");
            // IndexDocs方法是用于循环为全部文件建立索引的方法

            this.result_TxtBox.Text = "";//check_listBox为显示索引进度

            //==================================>(1)创建磁盘索引<==============================
             writer = new IndexWriter(INDEX_DIR, new StandardAnalyzer(), true);//最后一个参数为true，即创建一个新的索引文件，并覆盖原来的
            //当为FALSE时，即不创建，把索引写进原来的索引文档

           //IndexDocs(writer, docDir);
           ListFiles(docDir);
           System.Console.Out.WriteLine("优化中...");
           writer.Optimize();

            //==================================>(2)创建内存索引<==============================
            fswriter = new IndexWriter("index\\File2\\", new StandardAnalyzer(), true);
            ListFiles2(docDir2);
            //在上面遍历完文件、且都写进索引文件后，优化后再关闭！
            fswriter.Optimize();
            //FSDirectory[] fs = { FSDirectory.GetDirectory("index\\File2\\", false) };
   
            //fswriter.AddIndexes(fs);

            writer.Close();//把文档集写进索引后，记得关闭。否则，在查询时从索引文中查找的数据为空（即未写进去）
            fswriter.Close();

            this.result_TxtBox.Text = "完成建立索引！！！……  "+"\r\n";
        }
        catch (System.IO.IOException ie)
        {
            System.Console.Out.WriteLine(" caught a " + ie.GetType() + "\n with message: " + ie.Message);
        }
 
    }
}
