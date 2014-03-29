/*
 * Copyright 2005 huson1020
 * 
 * Licensed under the Apache License, Version 2.1.0 (the "License");
 * 
 * 2010��3��26��
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

/* ����INDEX_DIR�����ڽ���IndexWriterʵ��������ʵ���Ĳ�����Ŀ¼Directory��,�൱�ڽ���һ��Ŀ¼�����ֽ�index������ֻ������!!
         ��Ŀ¼���ڱ��潨���������ļ�
         */
    System.IO.FileInfo INDEX_DIR = new System.IO.FileInfo("index\\File\\");
    Lucene.Net.Store.Directory ramdir = new Lucene.Net.Store.RAMDirectory();
    IndexWriter fswriter;
    IndexWriter writer;

#region   //���·������ڱ����ļ�������Ŀ¼����Ŀ¼���ļ���,��Ҫ���õݹ�ķ�����������,�����ڱ�����ʱ��˳����ļ���һЩ����д�������ĵ���
public void ListFiles(FileSystemInfo fileinfo)
   {

       if (!fileinfo.Exists) return;

       DirectoryInfo dirinfo = fileinfo as DirectoryInfo;

       //if (dirinfo == null) return; //����Ŀ¼ 

       FileSystemInfo[] files = dirinfo.GetFileSystemInfos();

       for (int i = 0; i < files.Length; i++)    //����Ŀ¼�������ļ�����Ŀ¼ 
       {
           FileInfo file = files[i] as FileInfo;

           if (file != null) // ���ļ� 
           {
               Document dot = new Document();//����һ���ĵ��������������� 
               dot.Add(new Lucene.Net.Documents.Field("title", files[i].Name, Lucene.Net.Documents.Field.Store.YES,
                       Lucene.Net.Documents.Field.Index.TOKENIZED));//���ĵ����ӽ�һ��field��,�ļ�����title�����ɷִʣ�TOKENIZED��
               dot.Add(new Lucene.Net.Documents.Field("path", files[i].FullName, Lucene.Net.Documents.Field.Store.YES,
                       Lucene.Net.Documents.Field.Index.UN_TOKENIZED));//���ĵ����ӽ�һ��field������·����path�������ɷִʣ�UN_TOKENIZED��
               //����һ���洢�����ݡ�[contens]����
               dot.Add(new Lucene.Net.Documents.Field("contents", new System.IO.StreamReader(files[i].FullName, System.Text.Encoding.Default)));
               writer.AddDocument(dot);

               Response.Write("<script type='text/javascript'>window.alert("+"��ǰд�������ĵ����ļ�Ϊ��   " + file.Name +")</script>");

           }

           else //��Ŀ¼ 
           {
               ListFiles(files[i]); //����Ŀ¼���еݹ���� 
           }
       }

   }
public void ListFiles2(FileSystemInfo fileinfo)
    {

        if (!fileinfo.Exists) return;

        DirectoryInfo dirinfo = fileinfo as DirectoryInfo;

        //if (dirinfo == null) return; //����Ŀ¼ 

        FileSystemInfo[] files = dirinfo.GetFileSystemInfos();

        for (int i = 0; i < files.Length; i++)    //����Ŀ¼�������ļ�����Ŀ¼ 
        {
            FileInfo file = files[i] as FileInfo;

            if (file != null) // ���ļ� 
            {
                Document dot = new Document();//����һ���ĵ��������������� 
                dot.Add(new Lucene.Net.Documents.Field("title", files[i].Name, Lucene.Net.Documents.Field.Store.YES,
                        Lucene.Net.Documents.Field.Index.TOKENIZED));//���ĵ����ӽ�һ��field��,�ļ�����title�����ɷִʣ�TOKENIZED��
                dot.Add(new Lucene.Net.Documents.Field("path", files[i].FullName, Lucene.Net.Documents.Field.Store.YES,
                        Lucene.Net.Documents.Field.Index.UN_TOKENIZED));//���ĵ����ӽ�һ��field������·����path�������ɷִʣ�UN_TOKENIZED��
                //����һ���洢�����ݡ�[contens]����
                dot.Add(new Lucene.Net.Documents.Field("contents", new System.IO.StreamReader(files[i].FullName, System.Text.Encoding.Default)));
                fswriter.AddDocument(dot);

          Response.Write("<script type='text/javascript'>window.alert(" +"��ǰд�������ĵ����ļ�Ϊ��   " + file.Name + ")</script>");

            }

            else //��Ŀ¼ 
            {
                ListFiles(files[i]); //����Ŀ¼���еݹ���� 
            }
        }

    }
#endregion

#region  ҳ���Page_Load������Ҳ��web��վ������õĿ�ʼ�ĵط�~
    DirectoryInfo docDir; DirectoryInfo docDir2;
protected void Page_Load(object sender, EventArgs e)
    {

         docDir = new System.IO.DirectoryInfo(Server.MapPath("./")+@"connect_bak");
        // docDir2 = new System.IO.DirectoryInfo(Server.MapPath("./") + @"1.4");
         docDir2 = new System.IO.DirectoryInfo(Server.MapPath("./") + @"1.5");

        this.result_TxtBox.Text = "����·���� " + "\n";
        this.result_TxtBox.Text += docDir.FullName + "\r\n" ;
        this.result_TxtBox.Text += "��������������������������������������������������������������";
        this.result_TxtBox.Text += "\n"+ docDir2.FullName;
       
    }
#endregion  

protected void  query_btn_Click(object sender, EventArgs e)
{
    this.Query();
}

#region  ���ڲ�ѯ�ķ���
public void Query()
    {

        //������ѯ��Ҫ��ʵ������
        //==================================>(1)������������:  File ��File2<==============================
        //Lucene.Net.Index.MultiReader reader = new Lucene.Net.Index.MultiReader(new IndexReader[] { IndexReader.Open(@"\\172.16.32.189\File"), IndexReader.Open(Server.MapPath("./") + @"index\\File\\") });


        Lucene.Net.Search.ParallelMultiSearcher searcher;

        System.String index1 = Server.MapPath("./") + "index\\File\\";
        IndexReader reader1 = IndexReader.Open(index1);//��ȡ������ȡ����ʵ�����ô�Ϊ������ȡ��reader��ȡ�ļ���Ϊindex���ļ���(Ŀ¼)
        Searcher searcher1 = new IndexSearcher(reader1);//ָ��searcher(����)�� IndexReader(������ȡ��)
        Analyzer analyzer = new StandardAnalyzer();//��׼������

        System.String index2 = @"\\172.16.32.189\File";
        //System.String index = Server.MapPath("./") + "index\\File2\\";//�������ڵ�Ŀ¼��            

        if (new System.IO.FileInfo(index2).Exists)
        {
            IndexReader reader2 = Lucene.Net.Index.IndexReader.Open(index2);//��ȡ������ȡ����ʵ�����ô�Ϊ������ȡ��reader��ȡ�ļ���Ϊindex���ļ���(Ŀ¼)
            IndexSearcher searcher2 = new IndexSearcher(reader2);            
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1, searcher2 });
        }
        else
        {
            searcher = new Lucene.Net.Search.ParallelMultiSearcher(new Lucene.Net.Search.Searchable[] { searcher1 });
        }

        //==================================>(2)�����ڴ�����<==============================
        //Lucene.Net.Search.IndexSearcher searcher = new IndexSearcher(ramdir);


        //��ѯǰ���ѽ������ʾ���
        this.result_TxtBox.Text = "";
        //�����ѯ����
        if (this.query_txtbox.Text.Trim(new char[] { ' ' }) == String.Empty)
            return;

        System.String Query_condition = this.query_txtbox.Text.Trim();



        //====================>(1)�����ѯ����<==============================================
       //System.String field= "title";//���ڱ�����ҳɹ����ļ����ڵ�Ŀ¼ 
       //QueryParser parser = new QueryParser("field", analyzer);//������ѯ��������ָ����field(������ָcontents��Ŀ¼), analyzer����׼��������

      
       //====================>(2)�����ѯ����<==============================================
       string[] strs = new string[] { "title", "contents" };
       Lucene.Net.QueryParsers.QueryParser parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(strs, new StandardAnalyzer());
       parser.SetDefaultOperator(Lucene.Net.QueryParsers.QueryParser.OR_OPERATOR);


       Query query = parser.Parse(Query_condition);//����һ����ѯ����ָ����ѯ����Query_condition������ this.Query_txt.Text��
       //���д���
        Hits hits = searcher.Search(query);//����hits��������Search������)query���õ����ļ�
        if (hits.Length()==0)
        {
            this.result_TxtBox.Text += "==========������˼����ѯ����������Ĺؼ��֡�^+^===========" + "\n";
         }
       else{       
       for (int i = 0; i < hits.Length(); i++)
        {
            // �õ����е��ĵ�
            Document doc = hits.Doc(i);


            //string filename = doc.Get("path");//�õ������ļ���������Ŀ¼·��  
            string result_path = doc.Get("path");//�õ����е��ļ�Ŀ¼��

            //string folder = Path.GetDirectoryName(result_path);

            string filename = Path.GetFileName(result_path);//�õ������ļ���  

            //DirectoryInfo di = new DirectoryInfo(folder);

            this.result_TxtBox.Text += "======================================================" + "\n";
            this.result_TxtBox.Text += "��ѯ������ڵ��ļ�:   "+filename + "\n";            
            //this.result_TxtBox.Text += "��ѯ������ڵ�Ŀ¼:   " + di.FullName + "\n";

        }
    }
       //reader.Close(); 
       searcher.Close();
   
   }
#endregion

#region IndexDocs()�������ڰ������ļ�д������
   internal void IndexDocs(IndexWriter writer, System.IO.FileInfo file)
    {
        // do not try to index files that cannot be read
        // if (file.canRead())  // {{Aroush}} what is canRead() in C#?
        try
        {
            #region
            //����ڴ����ϴ�����ѡ���ļ�file��Ŀ¼�����õ��ļ�����Ŀ¼�������жϲ�Ϊ�պ�ѭ��д�������ļ���
            if (System.IO.Directory.Exists(file.FullName))
            {
                //�����ļ�����Ŀ¼
                System.String[] files = System.IO.Directory.GetFileSystemEntries(file.FullName);
                // an IO error could occur
                if (files != null)
                {
                    //����ļ����ǿյģ���ѭ�����ļ���Ϣд������writer
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
                    writer.AddDocument(FileDocument.Document(file));//����һ���������ļ�
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
            // ��Ӧ�س�
            System.Console.Out.WriteLine("�������ڸ�Ŀ¼�� '" + INDEX_DIR + "'...");
            // IndexDocs����������ѭ��Ϊȫ���ļ����������ķ���

            this.result_TxtBox.Text = "";//check_listBoxΪ��ʾ��������

            //==================================>(1)������������<==============================
             writer = new IndexWriter(INDEX_DIR, new StandardAnalyzer(), true);//���һ������Ϊtrue��������һ���µ������ļ���������ԭ����
            //��ΪFALSEʱ������������������д��ԭ���������ĵ�

           //IndexDocs(writer, docDir);
           ListFiles(docDir);
           System.Console.Out.WriteLine("�Ż���...");
           writer.Optimize();

            //==================================>(2)�����ڴ�����<==============================
            fswriter = new IndexWriter("index\\File2\\", new StandardAnalyzer(), true);
            ListFiles2(docDir2);
            //������������ļ����Ҷ�д�������ļ����Ż����ٹرգ�
            fswriter.Optimize();
            //FSDirectory[] fs = { FSDirectory.GetDirectory("index\\File2\\", false) };
   
            //fswriter.AddIndexes(fs);

            writer.Close();//���ĵ���д�������󣬼ǵùرա������ڲ�ѯʱ���������в��ҵ�����Ϊ�գ���δд��ȥ��
            fswriter.Close();

            this.result_TxtBox.Text = "��ɽ�����������������  "+"\r\n";
        }
        catch (System.IO.IOException ie)
        {
            System.Console.Out.WriteLine(" caught a " + ie.GetType() + "\n with message: " + ie.Message);
        }
 
    }
}
