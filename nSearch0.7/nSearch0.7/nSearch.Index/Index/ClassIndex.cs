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
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.Index 
{
    class ClassIndex
    {

        private Lucene.Net.Index.IndexWriter writer;

        /// <summary>
        /// �ִ���
        /// </summary>
        private static Lucene.Net.Analysis.Analyzer OneAnalyzer;

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        private string indexDirectory;

        //������ʼ��ID

        Thread TT;

        // ��ȡһ���ļ� ȡ�ü�¼��ID ID�ϵ�


        //��ʼ���ļ�ϵͳID
        int XX_AIDStart = 0;
        //����ģ��

        /// <summary>
        /// �����ļ���ID ָ��
        /// </summary>
        private string idp_filename;


        //��ʼ���� 

        public void InitRum(string fpath, string npath,string indexp,string idp)
        {

            idp_filename = idp;


            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ļ�ϵͳ��ʼ����ʼ");
            //�ļ�ϵͳ��ʼ��
            nSearch.FS.ClassFSMD.Init(fpath);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ļ�ϵͳ��ʼ�����");


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
        /// ֹͣ
        /// </summary>
        public void Stop()
        {



            nSearch.DebugShow.ClassDebugShow.WriteLineF(" ֹͣ����");

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
                nSearch.DebugShow.ClassDebugShow.WriteLineF("���浱ǰ�������ļ�λ��ָ��-> " + XX_AIDStart.ToString());
                putFileData(idp_filename, XX_AIDStart.ToString());
            }
            nSearch.DebugShow.ClassDebugShow.WriteLineF(" ֹͣ����[ok]");
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private void RunStart()
        {

            nSearch.DebugShow.ClassDebugShow.WriteLineF(" ��ʼ������");



            //�Ѿ����ڵĻ�Ϊ����
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


            nSearch.DebugShow.ClassDebugShow.WriteLineF(" ��ʼ������[OK]");


            //�ļ�ID���ձ�
            //ArrayList mID = nSearch.FS.ClassFSMD.GetUrlList();

            int iii = nSearch.FS.ClassFSMD.GetFileNum();


            nSearch.ClassLibraryHTML.ClassHTML myTag = new nSearch.ClassLibraryHTML.ClassHTML();

            //ѭ��

            for (int i = XX_AIDStart; i < iii; i++)
            {
                
                if (i == 19)
                {

                    int uu = 0;
                }
                
                nSearch.FS.oneHtmDat  newDat = nSearch.FS.ClassFSMD.GetOneDat(i);

                nSearch.DebugShow.onePage mmm = (nSearch.DebugShow.onePage)myTag.GetOnePage(newDat.HtmDat, newDat.url);

                Lucene.Net.Documents.Document doc = nSearch.ModelData.ClassModelData.GetOneDoc(newDat.url,    mmm.Title, mmm.Body,0);
                nSearch.DebugShow.ClassDebugShow.WriteLineF("==����-> " + i.ToString());
                writer.AddDocument(doc);

                nSearch.DebugShow.ClassDebugShow.WriteLineF("++����-> "+i.ToString());

                XX_AIDStart = i;

                if (i % 200 == 46)
                {
                    nSearch.DebugShow.ClassDebugShow.WriteLineF("���浱ǰ�������ļ�λ��ָ��-> " + i.ToString());
                    putFileData(idp_filename, XX_AIDStart.ToString());
                }

            }
            XX_AIDStart = XX_AIDStart + 1;
            nSearch.DebugShow.ClassDebugShow.WriteLineF("���浱ǰ�������ļ�λ��ָ��-> " + XX_AIDStart.ToString());
            putFileData(idp_filename, XX_AIDStart.ToString());

            writer.Optimize();

            writer.Close();


            nSearch.DebugShow.ClassDebugShow.WriteLineF(" ����[OK]");

        }

        /// <summary>
        /// ���ļ�
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
        /// д�ļ�
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
  ���� Ĭ��ֵ ˵�� 
 * 
 * mergeFactor Խ��ϵͳ���ø�����ڴ棬���ٴ��̴���
 * 
mergeFactor org.apache.lucene.mergeFactor 10 ����index�Ĵ�С��Ƶ��,�������� 1.һ�����ж���document  2.���ٸ��κϳ�һ����� 
maxMergeDocs org.apache.lucene.maxMergeDocs Integer.MAX_VALUE ����һ�����е�document��Ŀ 
minMergeDocs org.apache.lucene.minMergeDocs 10 �������ڴ��е�document��Ŀ���������Ժ��д�뵽���� 
maxFieldLength  1000 һ��Field�����Term��Ŀ���������ֺ��ԣ�����index��field�У�������ȻҲ���������� 

 
 
 
 Query query=MultiFieldQueryParser.parse("����",new String[]{"title","content"},analyzer);

        Query mquery=new WildcardQuery(new Term("sender","bluedavy*");

        TermQuery tquery=new TermQuery(new Term("name","jerry");

        

        BooleanQuery bquery=new BooleanQuery();

        bquery.add(query,true,false);

        bquery.add(mquery,true,false);

        bquery.add(tquery,true,false);

        

        Searcher searcher=new IndexSearcher(indexFilePath);

        Hits hits=searcher.search(bquery);

 
 
 
 
 
 */