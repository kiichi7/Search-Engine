using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.Main
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

    class ClassSeachALL
    {
        /// <summary>
        /// ������
        /// </summary>
        NewNxuEncoding.CNewNxuEncoding nCode = new NewNxuEncoding.CNewNxuEncoding();


        //���߳� �����ݷ��͸���� Ȼ���ѯ�Ƿ�ȫ���õ�����  ���ȫ���õ� �����ۺ�  ���ִ���Ļ� ����

/*
        Query Highlighting 
�����ȴ���һ������������highlighter������ʹ�üӺڣ�bold��������������ʾ��<B>��ѯ��</B>����
QueryHighlightExtractor highlighter = new      QueryHighlightExtractor(query, new StandardAnalyzer(), "<B>", "</B>");
ͨ���Խ�����������ǽ�����ԭ���������ƵĲ��֡�
for (int i = 0; i < hits.Length(); i++) {    // ...    string plainText;    using (StreamReader sr = new StreamReader(doc.Get("filename"),                                   System.Text.Encoding.Default))    {        plainText = parseHtml(sr.ReadToEnd());    }    row["sample"] = highlighter.GetBestFragments(plainText, 80, 2, "...");    // ...}
        */


        /// <summary>
        /// ѹ�������ַ���ת��ΪRSK����
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public RSK RTSTR2RSK(string xxx)
        {
          ///  string xxx = nCode.DeCompress(dat);

            int i0 = xxx.Length;
          //  int i1 = dat.Length;

            RSK gt = new RSK();         //(RSK) ObjDeserialize(dat,typeof(RSK) );

            string[] aa = xxx.Split('^');

            string a1 = nCode_CODE2CN(aa[0]);
            string a2 = nCode_CODE2CN(aa[1]);
            string a3 = nCode_CODE2CN(aa[2]);
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


                    OneRs abrs = new OneRs();
                    string[] ab = oip.Split('|');
                    /*
                    string A = nCode_CODE2CN(ab[0]);
                    string B = nCode_CODE2CN(ab[1]);
                    string C = nCode_CODE2CN(ab[2]);
                    string D = nCode_CODE2CN(ab[3]);
                    string M = nCode_CODE2CN(ab[4]);
                    string S = nCode_CODE2CN(ab[5]);
                    string T = nCode_CODE2CN(ab[6]);
                    string U = nCode_CODE2CN(ab[7]);

                 

                    abrs.A = A;
                    abrs.B = B;
                    abrs.C = C;
                    abrs.D = D;
                    abrs.M = M;
                    abrs.Score = float.Parse(S);
                    abrs.T = T;
                    abrs.url = U;
                    */
                    string S = nCode_CODE2CN(ab[5]);
                    abrs.A = ab[0];
                    abrs.B = ab[1];
                    abrs.C = ab[2];
                    abrs.D = ab[3];
                    abrs.M = ab[4];
                    abrs.Score = float.Parse(S);
                    abrs.T = ab[6];
                    abrs.url = ab[7];

                    Tuu.Add(abrs);

                }
            }

            gt.ALLNum = Int32.Parse(a1);
            gt.ANum = Int32.Parse(a2);
            gt.BNum = Int32.Parse(a3);
            gt.rs = Tuu;


            return gt;
        }

        /// <summary>
        ///���ı��뵽BASE64// 1 �ȱ���ΪGB2312 // 2 ת��Ϊbyte // 3 ����ΪBASE64	
        /// </summary>
        /// <param name="DataX">�ַ���</param>
        private String nCode_CN2CODE(String DataX)
        {

            Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");


            Byte[] dataSV = gbx.GetBytes(DataX);

            string base64String;

            base64String =
                   System.Convert.ToBase64String(dataSV,
                                                 0,
                                                 dataSV.Length);

            return base64String;


        }

        /// <summary>
        ///	BASE64������// 1 ����Ϊbyte // 2 ת��Ϊ2312
        /// </summary>
        /// <param name="DataX">�ַ���</param>
        private String nCode_CODE2CN(String DataX)
        {
            try
            {
                //Encoding gb = System.Text.Encoding.GetEncoding("Ansi");

                byte[] binaryData;

                binaryData =
                            System.Convert.FromBase64String(DataX);

                Encoding gb = System.Text.Encoding.GetEncoding("gb2312");

                string base64String;

                base64String = gb.GetString(binaryData, 0, binaryData.Length);

                return base64String;
            }
            catch
            {
                return DataX;
            }

        }



    }
}
