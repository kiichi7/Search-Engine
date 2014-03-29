using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using System.IO;
using System.Threading;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.Index 
{
    class ClassIndex
    {

        private Lucene.Net.Index.IndexWriter writer;

        /// <summary>
        /// 分词类
        /// </summary>
        private static Lucene.Net.Analysis.Analyzer OneAnalyzer;

        /// <summary>
        /// 索引目录
        /// </summary>
        private string indexDirectory;

        //设置起始的ID

        Thread TT;

        // 读取一个文件 取得记录的ID ID断点


        //起始的文件系统ID
        int XX_AIDStart = 0;
        //加载模板

        /// <summary>
        /// 索引文件的ID 指针
        /// </summary>
        private string idp_filename;


        //开始索引 

        public void InitRum(string fpath, string npath,string indexp,string idp)
        {

            idp_filename = idp;


            nSearch.DebugShow.ClassDebugShow.WriteLineF("文件系统初始化开始");
            //文件系统初始化
            nSearch.FS.ClassFSMD.Init(fpath);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("文件系统初始化完成");


            nSearch.ModelData.ClassModelData.Init(npath);


           
            if (indexp[indexp.Length - 1] == '\\')
            { }
            else
            {
                indexp = indexp + "\\";
            }

            indexDirectory = indexp;

            OneAnalyzer = new Lucene.Net.Analysis.XunLongX.XunLongAnalyzer();

            TT = new Thread(new ThreadStart(RunStart));
            TT.Start();




        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {



            nSearch.DebugShow.ClassDebugShow.WriteLineF(" 停止索引");

            try
            {
                TT.Suspend();
                TT.Abort();
            }
            catch
            { }


            try
            {
                writer.Optimize();

                writer.Close();

            }
            catch
            { }

            if (XX_AIDStart > 0)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF("保存当前索引的文件位置指针-> " + XX_AIDStart.ToString());
                putFileData(idp_filename, XX_AIDStart.ToString());
            }
            nSearch.DebugShow.ClassDebugShow.WriteLineF(" 停止索引[ok]");
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        private void RunStart()
        {

            nSearch.DebugShow.ClassDebugShow.WriteLineF(" 初始化索引");



            //已经存在的话为增加
            if (System.IO.File.Exists(indexDirectory + "segments") == true)
            {

                writer = new IndexWriter(indexDirectory, OneAnalyzer, false);
            }
            else
            {
                writer = new IndexWriter(indexDirectory, OneAnalyzer, true);
            }


            string AIDStart = getFileData(idp_filename);
            if (AIDStart.Length > 0)
            {
                XX_AIDStart = Int32.Parse(AIDStart);
            }
            else
            {
                XX_AIDStart = 0;
            }


            writer.SetUseCompoundFile(true);


            writer.SetMaxBufferedDocs(100);

            writer.SetMergeFactor(100);


            nSearch.DebugShow.ClassDebugShow.WriteLineF(" 初始化索引[OK]");


            //文件ID对照表
            //ArrayList mID = nSearch.FS.ClassFSMD.GetUrlList();

            int iii = nSearch.FS.ClassFSMD.GetFileNum();


            nSearch.ClassLibraryHTML.ClassHTML myTag = new nSearch.ClassLibraryHTML.ClassHTML();

            //循环

            for (int i = XX_AIDStart; i < iii; i++)
            {
                
                if (i == 19)
                {

                    int uu = 0;
                }
                
                nSearch.FS.oneHtmDat  newDat = nSearch.FS.ClassFSMD.GetOneDat(i);

                nSearch.DebugShow.onePage mmm = (nSearch.DebugShow.onePage)myTag.GetOnePage(newDat.HtmDat, newDat.url);

                Lucene.Net.Documents.Document doc = nSearch.ModelData.ClassModelData.GetOneDoc(newDat.url,    mmm.Title, mmm.Body,0);
                nSearch.DebugShow.ClassDebugShow.WriteLineF("==索引-> " + i.ToString());
                writer.AddDocument(doc);

                nSearch.DebugShow.ClassDebugShow.WriteLineF("++索引-> "+i.ToString());

                XX_AIDStart = i;

                if (i % 200 == 46)
                {
                    nSearch.DebugShow.ClassDebugShow.WriteLineF("保存当前索引的文件位置指针-> " + i.ToString());
                    putFileData(idp_filename, XX_AIDStart.ToString());
                }

            }
            XX_AIDStart = XX_AIDStart + 1;
            nSearch.DebugShow.ClassDebugShow.WriteLineF("保存当前索引的文件位置指针-> " + XX_AIDStart.ToString());
            putFileData(idp_filename, XX_AIDStart.ToString());

            writer.Optimize();

            writer.Close();


            nSearch.DebugShow.ClassDebugShow.WriteLineF(" 索引[OK]");

        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string getFileData(string filename)
        {

            StreamReader reader = null;
            string data = string.Empty;
            try
            {
                reader = new StreamReader(filename, System.Text.Encoding.GetEncoding("gb2312"));

                data = reader.ReadToEnd();

                reader.Close();
                return data;
            }
            catch (IOException e)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return "";


        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        private void putFileData(string filename, string data)
        {


            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("gb2312"));
                writer.Write(data);
                writer.Close();
            }
            catch (IOException e)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }


    }
}
/*
  属性 默认值 说明 
 * 
 * mergeFactor 越大，系统会用更多的内存，更少磁盘处理
 * 
mergeFactor org.apache.lucene.mergeFactor 10 控制index的大小和频率,两个作用 1.一个段有多少document  2.多少个段合成一个大段 
maxMergeDocs org.apache.lucene.maxMergeDocs Integer.MAX_VALUE 限制一个段中的document数目 
minMergeDocs org.apache.lucene.minMergeDocs 10 缓存在内存中的document数目，超过他以后会写入到磁盘 
maxFieldLength  1000 一个Field中最大Term数目，超过部分忽略，不会index到field中，所以自然也就搜索不到 

 
 
 
 Query query=MultiFieldQueryParser.parse("索引",new String[]{"title","content"},analyzer);

        Query mquery=new WildcardQuery(new Term("sender","bluedavy*");

        TermQuery tquery=new TermQuery(new Term("name","jerry");

        

        BooleanQuery bquery=new BooleanQuery();

        bquery.add(query,true,false);

        bquery.add(mquery,true,false);

        bquery.add(tquery,true,false);

        

        Searcher searcher=new IndexSearcher(indexFilePath);

        Hits hits=searcher.search(bquery);

 
 
 
 
 
 */