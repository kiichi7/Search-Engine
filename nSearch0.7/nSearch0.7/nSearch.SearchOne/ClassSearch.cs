using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search.Highlight;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.SearchOne
{


    /// <summary>
    /// ���صĽ����
    /// </summary>
    public struct RSK
    {
        /// <summary>
        /// �������
        /// </summary>
        public ArrayList rs;

        /// <summary>
        /// ��ʼ
        /// </summary>
        public int ANum;
        /// <summary>
        /// ����
        /// </summary>
        public int BNum;

        /// <summary>
        /// �ܹ�����Ŀ
        /// </summary>
        public int ALLNum;

    }


    /// <summary>
    /// һ�������Ŀ
    /// </summary>
    public struct OneRs
    {
        /// <summary>
        /// ����A
        /// </summary>
        public string A; // 
        /// <summary>
        /// ���B
        /// </summary>
        public string B; // 
        /// <summary>
        /// ��Ҫ��ʾC
        /// </summary>
        public string C; // 
        /// <summary>
        ///  ������ʾD
        /// </summary>
        public string D; //

        // <summary>
        // �����õ�����E
        // </summary>
       // public string E; // 
       //
       
        /// <summary>
        /// �����ı������� 
        /// </summary>
        public string T; // 
        /// <summary>
        /// �����
        /// </summary>
        public string M; // 
        /// <summary>
        /// ����Ȩֵ
        /// </summary>
        public float Score; //

        /// <summary>
        /// url
        /// </summary>
        public string url;
    }


    /// <summary>
    /// ���������� 
    /// 
    /// 
    /// 
    /// </summary>
    class ClassSearch
    {

        IndexSearcher searcher ;


        /// <summary>
        /// ������
        /// </summary>
        NewNxuEncoding.CNewNxuEncoding nCode = new NewNxuEncoding.CNewNxuEncoding();

        /// <summary>
        /// �ִ���
        /// </summary>
        private static Lucene.Net.Analysis.Analyzer OneAnalyzer;

        /// <summary>
        /// �ִ���
        /// </summary>
        private static Lucene.Net.Analysis.Analyzer OneAnalyzer2 = new Lucene.Net.Analysis.Standard.StandardAnalyzer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexpath"></param>
        public void Init(string indexpath)
        {
            searcher = new IndexSearcher(indexpath);

            OneAnalyzer = ClassST.OneAnalyzer;
        }


        //

        /// <summary>
        /// Ĭ�� ���ǰ 100 ��
        /// 
        /// ���� ����  ����  ���һ  ����
        /// </summary>
        /// <param name="word"></param>
        /// <param name="ipage"></param>
        /// <param name="npaeg"></param>
        /// <returns></returns>
        public RSK GetRS(string word, int iStart, int iLen)
        {


            //word ʹ�� | ��� 
            //  �����|title|word|���һ|����

            string[] gh = word.Split('|');




            //  Serialize;
            RSK MM = new RSK();

            ArrayList UU = new ArrayList();

            BooleanQuery all_query = new BooleanQuery();

            if (gh[0].Trim().Length > 0)
            {
                //�����
                QueryParser parser1 = new QueryParser("M", OneAnalyzer);
                Query query1 = parser1.Parse(gh[0].Trim());
                all_query.Add(query1, BooleanClause.Occur.MUST);
            }


            if (gh[1].Trim().Length > 0)
            {
            //����
            QueryParser parser2 = new QueryParser("A", OneAnalyzer);
            Query query2 = parser2.Parse(gh[1].Trim());
            all_query.Add(query2, BooleanClause.Occur.MUST);
            }

            if (gh[2].Trim().Length > 0)
            {
            //����
            QueryParser parser3 = new QueryParser("E", OneAnalyzer);
            Query query3 = parser3.Parse(gh[2].Trim());
            all_query.Add(query3, BooleanClause.Occur.MUST);
            }

            if (gh[3].Trim().Length > 0)
            {
                //���һ
                QueryParser parser4 = new QueryParser("A1", OneAnalyzer);
                Query query4 = parser4.Parse(gh[3].Trim());
                all_query.Add(query4, BooleanClause.Occur.MUST);
            }

            if (gh[4].Trim().Length > 0)
            {
                //����
                QueryParser parser5 = new QueryParser("A2", OneAnalyzer);
                Query query5 = parser5.Parse(gh[4].Trim());
                all_query.Add(query5, BooleanClause.Occur.MUST);
            }











            Hits hits = searcher.Search(all_query);
          


            MM.ALLNum = hits.Length();

         

            //����ǳ�ʼ�� ����
            if (iStart == 0 & iLen == 0)
            {
                int iuo = hits.Length();

                //����������100��
                if (iuo > 50)
                {
                    iuo = 50;
                }

                iStart = 0;
                iLen = iuo;
            }


       

           
             
     /*
            for (int i = 0; i < hits.Length(); i++)
            {
                string FIELD_NAME = "D";
                System.String text = hits.Doc(i).Get(FIELD_NAME);
                TokenStream tokenStream = OneAnalyzer.TokenStream(FIELD_NAME, new System.IO.StringReader(text));
                System.String result = highlighter.GetBestFragment(tokenStream, text);
                System.Console.Out.WriteLine("\t" + result);
            }

            */

          // Highlighter highlighter = new Highlighter(new QueryScorer(query));
          

            for (int i = iStart; i <iStart+ iLen; i++)
            {
                Document doc = hits.Doc(i);

                OneRs nit = new OneRs();

                nit.A = doc.Get("A");
                nit.B = doc.Get("B");
                nit.C = doc.Get("C");


                string Tmptext = doc.Get("D");
             //   string Tmptext = doc.Get("E");
                /*
                TokenStream tokenStream = OneAnalyzer.TokenStream("D", new System.IO.StringReader(Tmptext));
               // System.String result = highlighter.GetBestFragment(tokenStream, Tmptext);
                System.String result = highlighter.GetBestFragments( tokenStream, Tmptext, 200, "...");
                */

                if (Tmptext.Length > 500)
                {
                    nit.D = Tmptext.Substring(0,500);  
                }
                else
                {
                    nit.D = Tmptext;            
                }


                


                nit.M = doc.Get("M");
                nit.T = doc.Get("T");
                nit.url = doc.Get("U");
                nit.Score = hits.Score(i);


                UU.Add(nit);
            }

          
            MM.ANum = 0;
            MM.BNum = 100;
            MM.rs = UU;


            //string dd = RTRSK2STR(MM);

          
           // RSK cc = RTSTR2RSK(dd);



            return MM;

        }


        /// <summary>
        /// RSK����ת��Ϊѹ�������ַ���
        /// </summary>
        /// <param name="oi"></param>
        /// <returns></returns>
        public string RTRSK2STR(RSK oi)
        {

            string a1 =nCode.CN2CODE(  oi.ALLNum.ToString());
            string a2 = nCode.CN2CODE(oi.ANum.ToString());
            string a3= nCode.CN2CODE(oi.BNum.ToString());

           StringBuilder  a4 = new StringBuilder("");

            //�������
            foreach (nSearch.SearchOne.OneRs one in oi.rs)
            {
                string a4_1 = nCode.CN2CODE(one.A) + "|" + nCode.CN2CODE(one.B) + "|" + nCode.CN2CODE(one.C) + "|" + nCode.CN2CODE(one.D) + "|" + nCode.CN2CODE(one.M) + "|" + nCode.CN2CODE(one.Score.ToString()) + "|" + nCode.CN2CODE(one.T) + "|" + nCode.CN2CODE(one.url);

                if (a4.Length == 0)
                {
                    a4.Append(a4_1);
                }
                else
                {
                    a4.Append("~"+ a4_1);
                }
            }


          //  string xxx =  a1+"^"+a2+"^"+a3+"^"+a4 ;

            StringBuilder xcxx = new StringBuilder("");

            xcxx.Append(a1);
            xcxx.Append("^");
            xcxx.Append(a2);
            xcxx.Append("^");
            xcxx.Append(a3);
            xcxx.Append("^");
            xcxx.Append(a4);
            //��ѹ��
          //  string nxxx = nCode.Compress(xxx);

         //   int i0 = xxx.Length;
         //   int i1 = nxxx.Length;


            return xcxx.ToString();
        }

        /// <summary>
        /// ѹ�������ַ���ת��ΪRSK����
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public RSK RTSTR2RSK(string dat)
        {
            //����ѹ
            string xxx = dat  ; //nCode.DeCompress(dat);

            int i0 = xxx.Length;
            int i1 = dat.Length;

            RSK gt = new RSK();         //(RSK) ObjDeserialize(dat,typeof(RSK) );

            string[] aa = xxx.Split('^');

            string a1 = nCode.CODE2CN(aa[0]);
            string a2 = nCode.CODE2CN(aa[1]);
            string a3 = nCode.CODE2CN(aa[2]);
            string a4 = aa[3];



            string[] a4_all = a4.Split('~');

            ArrayList Tuu = new ArrayList();
            Tuu.Clear();


            if (a4.Length == 0)
            {

            }
            else
            {

                foreach (string oip in a4_all)
                {



                    string[] ab = oip.Split('|');

                    string A = nCode.CODE2CN(ab[0]);
                    string B = nCode.CODE2CN(ab[1]);
                    string C = nCode.CODE2CN(ab[2]);
                    string D = nCode.CODE2CN(ab[3]);
                    string M = nCode.CODE2CN(ab[4]);
                    string S = nCode.CODE2CN(ab[5]);
                    string T = nCode.CODE2CN(ab[6]);
                    string U = nCode.CODE2CN(ab[7]);

                    nSearch.SearchOne.OneRs abrs = new OneRs();

                    abrs.A = A;
                    abrs.B = B;
                    abrs.C = C;
                    abrs.D = D;
                    abrs.M = M;
                    abrs.Score = float.Parse(S);
                    abrs.T = T;
                    abrs.url = U;

                    Tuu.Add(abrs);

                }
            }

            gt.ALLNum = Int32.Parse(a1);
            gt.ANum = Int32.Parse(a2);
            gt.BNum = Int32.Parse(a3);
            gt.rs = Tuu;


            return gt;
        }







    }
}


/*
 
 
        /// <summary>
        /// ���л��ĺ���
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string ObjSerializer(object obj)
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(mem, Encoding.Default);
            ser.Serialize(writer, obj);
            writer.Close();
            return Encoding.Default.GetString(mem.ToArray());
        }


        /// <summary>
        /// �����л��ĺ���
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public object ObjDeserialize(string str, Type t)
        {
            System.Xml.Serialization.XmlSerializer mySerializer = new System.Xml.Serialization.XmlSerializer(t);

            System.IO.StreamReader mem2 = new System.IO.StreamReader(new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(str)), System.Text.Encoding.Default);
            return mySerializer.Deserialize(mem2);
        }
 
 
 
 
 
 
 */

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