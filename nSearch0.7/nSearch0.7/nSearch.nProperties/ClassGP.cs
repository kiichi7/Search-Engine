using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.nProperties
{
    public class ClassGP
    {


        IndexSearcher searcher;

        string SavePAth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexpath"></param>
        public void Init(string indexpath, string Sp)
        {
            searcher = new IndexSearcher(indexpath);

            if (Sp[Sp.Length - 1] == '\\')
            { }
            else
            {
                Sp = Sp + "\\";
            }

            SavePAth = Sp;

            nSearch.DebugShow.ClassDebugShow.WriteLineF("--->>  " + SavePAth);

        }

        /// <summary>
        /// �õ��б�
        ///   1 ������������  �õ�������б�  ���ɼ�ÿ�����10000��
        /// 
        /// </summary>
        public void SearchGet()
        {

            nSearch.DebugShow.ClassDebugShow.WriteLineF("��ʼ���������б�");

            Hashtable main = new Hashtable();

            for (int iu = 0; iu < searcher.MaxDoc(); iu++)
            {
                Document doc = searcher.Doc(iu);
                string M = doc.Get("M");
                string a1 = doc.Get("A1");
                string a2 = doc.Get("A2");

                if (main.Contains(M) == false)
                {
                    ArrayList x = new ArrayList();
                    x.Add(a1 + "\t" + a2);
                    main.Add(M, (ArrayList)x.Clone());
                    x = null;
                }
                else
                {
                    ArrayList x = (ArrayList)main[M];
                    if (x.Count < 10000)
                    {
                        x.Add(a1 + "\t" + a2);
                        main[M]=x;

                        if (x.Count%1000 == 5)
                        {
                            nSearch.DebugShow.ClassDebugShow.WriteLineF("-->" + M + " > " + x.Count.ToString());
                        }


                        x = null;



                    }
                }

            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("���ݲɼ����");

            //��۵õ�����
            foreach (DictionaryEntry cc in main)
            {
                string ff = cc.Key.ToString();
                ArrayList x = (ArrayList)cc.Value;

                ArrayList a1 = new ArrayList();
                ArrayList a2 = new ArrayList();

                StringBuilder vvv1 = new StringBuilder();
                StringBuilder vvv2 = new StringBuilder();

                nSearch.DebugShow.ClassDebugShow.WriteLineF("��� " + ff);

                foreach (string ww in x)
                {
                    string[] xcd = ww.Split('\t');

                    if (a1.Contains(xcd[0]) == false)
                    {
                        a1.Add(xcd[0]);
                        vvv1.Append("a1\t" + xcd[0] + "\r\n");
                    }

                    if (a2.Contains(xcd[1]) == false)
                    {
                        a2.Add(xcd[1]);
                        vvv2.Append("a2\t" + xcd[1] + "\r\n");
                    }
                }


                SaveFileData(SavePAth + ff + ".txt", vvv1.ToString() + "\r\n" + vvv2.ToString());
                nSearch.DebugShow.ClassDebugShow.WriteLineF(" == �����ļ� " + SavePAth + ff + ".txt");



            }



        }

        /// <summary>
        /// д�ļ�
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        private void SaveFileData(string filename, string data)
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


        /// <summary>
        /// ���ļ�
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string getFileData(string filename)
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
        private static void SaveFileDataX(string filename, string data)
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


