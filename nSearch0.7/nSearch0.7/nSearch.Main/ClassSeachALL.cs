using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.Main
{

    /// <summary>
    /// 返回的结果集
    /// </summary>
    public struct RSK
    {
        /// <summary>
        /// 结果队列
        /// </summary>
        public ArrayList rs;

        /// <summary>
        /// 开始
        /// </summary>
        public int ANum;
        /// <summary>
        /// 结束
        /// </summary>
        public int BNum;

        /// <summary>
        /// 总共的数目
        /// </summary>
        public int ALLNum;

    }


    /// <summary>
    /// 一个结果条目
    /// </summary>
    public struct OneRs
    {
        /// <summary>
        /// 标题A
        /// </summary>
        public string A; // 
        /// <summary>
        /// 类聚B
        /// </summary>
        public string B; // 
        /// <summary>
        /// 简要显示C
        /// </summary>
        public string C; // 
        /// <summary>
        ///  快照显示D
        /// </summary>
        public string D; //

        // <summary>
        // 索引用的数据E
        // </summary>
        // public string E; // 
        //

        /// <summary>
        /// 单独的标题数据 
        /// </summary>
        public string T; // 
        /// <summary>
        /// 主类别
        /// </summary>
        public string M; // 
        /// <summary>
        /// 排序权值
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
        /// 编码类
        /// </summary>
        NewNxuEncoding.CNewNxuEncoding nCode = new NewNxuEncoding.CNewNxuEncoding();


        //多线程 把数据发送给大家 然后查询是否全部得到数据  如果全部得到 则结果聚合  出现错误的话 警报

/*
        Query Highlighting 
我们先创建一个高亮器对象highlighter，并将使用加黑（bold）字体来高亮显示（<B>查询词</B>）。
QueryHighlightExtractor highlighter = new      QueryHighlightExtractor(query, new StandardAnalyzer(), "<B>", "</B>");
通过对结果遍历，我们将载入原文中最相似的部分。
for (int i = 0; i < hits.Length(); i++) {    // ...    string plainText;    using (StreamReader sr = new StreamReader(doc.Get("filename"),                                   System.Text.Encoding.Default))    {        plainText = parseHtml(sr.ReadToEnd());    }    row["sample"] = highlighter.GetBestFragments(plainText, 80, 2, "...");    // ...}
        */


        /// <summary>
        /// 压缩过的字符串转换为RSK数据
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
        ///中文编码到BASE64// 1 先编码为GB2312 // 2 转换为byte // 3 编码为BASE64	
        /// </summary>
        /// <param name="DataX">字符串</param>
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
        ///	BASE64到中文// 1 解码为byte // 2 转换为2312
        /// </summary>
        /// <param name="DataX">字符串</param>
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
