using System;
using System.Collections.Generic;
using System.Text;

using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;

/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.ModelData
{
    /// <summary>
    /// ����Ӧģ�����
    /// </summary>
    public static class ClassModelData
    {

        //���ý�����
        private  static nSearch.ClassLibraryStruct.ClassUserModel xMod = new nSearch.ClassLibraryStruct.ClassUserModel();

        /// <summary>
        /// ģ��ƥ����
        /// </summary>
        private static nSearch.ClassLibraryStruct.ClassUserModel mxWeb = new nSearch.ClassLibraryStruct.ClassUserModel();

        /// <summary>
        /// ��������
        /// </summary>
        private static nSearch.ClassLibraryHTML.ClassHTML vclear = new nSearch.ClassLibraryHTML.ClassHTML();

        /// <summary>
        /// ��ʼ��ģ��
        /// </summary>
        /// <param name="mpath"></param>
        public static void Init(string mpath)
        {
            nSearch.DebugShow.ClassDebugShow.WriteLineF("����ϵͳ��ʼ��");

            mxWeb.init(mpath);

            nSearch.DebugShow.ClassDebugShow.WriteLineF("����ϵͳ��ʼ��[ok]");

        }

        /// <summary>
        /// ����һ��ϵͳ��Ӧ���ĵ���
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static Lucene.Net.Documents.Document GetOneDoc(string url,string title, string htmldat,int Num)
        {

            Lucene.Net.Documents.Document oneDoc = new Lucene.Net.Documents.Document();



            nSearch.ClassLibraryStruct.auto2dat k = mxWeb.getTagAndData(htmldat);

            //doc.Add(Field.Text("ID", id)); // ����

            if (k.isOK == true)
            {
                oneDoc.Add(new Field("A", k.A, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// ����A

                nSearch.DebugShow.ClassDebugShow.WriteLineF("ģ��ƥ��ɹ�");
            }
            else
            {
                oneDoc.Add(new Field("A",title, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// ����A


                nSearch.DebugShow.ClassDebugShow.WriteLineF("ģ��ƥ��ʧ��");
            }

            if (k.B.Length == 0)
            {
                k.B = "kc";
            }
            oneDoc.Add(new Field("B", k.B, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// ���B
            if (k.C.Length == 0)
            {
                k.C = "kc";
            }
            oneDoc.Add(new Field("C", k.C, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// ��Ҫ��ʾC
            if (k.D.Length == 0)
            {
                k.D = "kc";
            }
            oneDoc.Add(new Field("D", k.D, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// ������ʾD

            //*
            oneDoc.Add(new Field("E", vclear.GetClearCode( k.E,true), Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));// �����õ�����E
            if (k.T.Length == 0)
            {
                k.T = "kc";
            }
            oneDoc.Add(new Field("T", k.T, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// �����ı�������
            if (k.M.Length == 0)
            {
                k.M = "kc";
            }
            oneDoc.Add(new Field("M", k.M, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// �����
            oneDoc.Add(new Field("U", url, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// url

            //*
            if (k.A_TYPE_1.Length == 0)
            {
                k.B = "kc";
            }
            oneDoc.Add(new Field("A1", k.A_TYPE_1, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// url
            if (k.A_TYPE_2.Length == 0)
            {
                k.B = "kc";
            }
            oneDoc.Add(new Field("A2", k.A_TYPE_2, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// url

            return oneDoc;
        }


    }


}

/*
 * 
 * Field.Index���ĸ����ԣ��ֱ��ǣ�
Field.Index.TOKENIZED���ִ�����
Field.Index.UN_TOKENIZED���ִʽ����������������������ڵȣ�Rod Johnson����Ϊһ���ʣ�������Ҫ�ִʡ�
Field.Index����������������Ų��ܱ��������������ĵ���һЩ�����������ĵ�����, URL�ȡ�
Field.Index.NO_NORMS���� 
Field.StoreҲ���������ԣ��ֱ��ǣ�
Field.Store.YES�������ļ�����ֻ�洢��������, ����ƽ�ԭ������ֱ��Ҳ�洢�������ļ��У����ĵ��ı��⡣
Field.Store.NO��ԭ�Ĳ��洢�������ļ��У�����������к��ٸ������������������ļ���Path�����ݿ�������ȣ��������Ӵ�ԭ�ģ��ʺ�ԭ�����ݽϴ�������
Field.Store.COMPRESS ѹ���洢�� 
termVector��Lucene 1.4.3���������ṩһ����������������ģ����ѯ,�����á� 
 * 
 * 
 * 
 * 
 * 
         /// <summary>
        /
        /// </summary>
        public string A;
        /// <summary>
        /
        /// </summary>
        public string B;
        /// <summary>
        /
        /// </summary>
        public string C;
        /// <summary>
        /
        /// </summary>
        public string D;
        /// <summary>
        /
        /// </summary>
        public string E;
        /// <summary>
        /// ��������б�F
        /// </summary>
        public string F;

        /// <summary>
        /
        /// </summary>
        public string T;

        /// <summary>
        /// �Զ����� ������H
        /// </summary>
        public string H;

        /// <summary>
        /
        /// </summary>
        public string M;
 */