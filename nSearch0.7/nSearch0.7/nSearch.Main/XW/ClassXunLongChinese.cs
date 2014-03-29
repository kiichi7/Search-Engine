using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

/*
      '       迅龙中文分类搜索引擎  v0.6
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */


namespace Lucene.Net.Analysis.XunLongX
{


    /// <summary>
    /// <p>
    /// 
    /// @author Zhang, Dong
    ///
    /// </summary>


    /// <summary>
    /// 分词结果存储结构
    /// </summary>
    public struct XunLongCNST
    {
        /// <summary>
        /// 词
        /// </summary>
        public string cWord;
        /// <summary>
        /// 词性
        /// </summary>
        public string cType;

        /// <summary>
        /// 开始位置  0
        /// </summary>
        public int cStart;

        /// <summary>
        /// 长度
        /// </summary>
        public int cLength;

    }

    /// <summary>
    /// 迅龙分词接口
    /// </summary>
    public static class ClassXunLongChinese
    {

        /// <summary>
        /// 设定分词类
        /// </summary>
        //  private static XwordClassLibrary.ClassXWord mXW = new XwordClassLibrary.ClassXWord();

        /// <summary>
        /// 分词接口
        /// </summary>
        private static nSearch.xOcx.xlcom xwordOne = new nSearch.xOcx.xlcom();

        /// <summary>
        /// 存储停止词
        /// </summary>
        public static ArrayList CnStopWord = new ArrayList();


        public static int xxx = 1;

        public static void SetRun(int ii)
        {
            xxx = ii;

        }

        /// <summary>
        /// 中文分词
        /// </summary>
        /// <param name="_incn"></param>
        /// <returns></returns>
        public static XunLongCNST[] ChineseIntface(TextReader _incn)
        {
            string aOLD = _incn.ReadToEnd().ToLower();
            string a = aOLD.Trim();



            a = a.Replace("---", " ");
            a = a.Replace("===", " ");

            a = a.Replace("--", "-");
            a = a.Replace("==", "=");

            //去掉数字 和 数字空格 连续的情况


            a = a.Replace(" 1 ", " ");
            a = a.Replace(" 2 ", " ");
            a = a.Replace(" 3 ", " ");
            a = a.Replace(" 4 ", " ");
            a = a.Replace(" 5 ", " ");
            a = a.Replace(" 6 ", " ");
            a = a.Replace(" 7 ", " ");
            a = a.Replace(" 8 ", " ");
            a = a.Replace(" 9 ", " ");
            a = a.Replace(" 0 ", " ");


            a = a.Replace(" 00 ", " ");

            a = a.Replace(" 11 ", " ");
            a = a.Replace(" 12 ", " ");
            a = a.Replace(" 13 ", " ");
            a = a.Replace(" 14 ", " ");
            a = a.Replace(" 15 ", " ");
            a = a.Replace(" 16 ", " ");
            a = a.Replace(" 17 ", " ");
            a = a.Replace(" 18 ", " ");
            a = a.Replace(" 19 ", " ");
            a = a.Replace(" 20 ", " ");

            a = a.Replace(" 21 ", " ");
            a = a.Replace(" 22 ", " ");
            a = a.Replace(" 23 ", " ");
            a = a.Replace(" 24 ", " ");
            a = a.Replace(" 25 ", " ");
            a = a.Replace(" 26 ", " ");
            a = a.Replace(" 27 ", " ");
            a = a.Replace(" 28 ", " ");
            a = a.Replace(" 29 ", " ");
            a = a.Replace(" 30 ", " ");

            a = a.Replace(" 31 ", " ");

            a = a.Replace("   ", " ");
            a = a.Replace("  ", " ");


            if (a == null | a.Length == 0)
            {
                return null;

            }

            string dat = "";

            if (a.IndexOf(' ') > 0)
            {
                string[] NewAS = a.Split(' ');
                int nal = a.Length / 5;

                if (NewAS.Length >= nal)
                {
                    dat = "";
                    for (int c = 0; c < NewAS.Length; c++)
                    {
                        dat = dat +" " +NewAS[c];
                    }
                    goto X2;
                }
            }










            int xWN_400 = 512;

            if (a.Length < xWN_400)
            {


                dat = xwordOne.GetXword(a);
            }
            else
            {

                //数据以000 为单位切割开 
                for (int i = xWN_400; i < a.Length; i++)
                {
                    if (i % xWN_400 == 0)
                    {
                        //取得500个字符
                        string one = a.Substring(i - xWN_400, xWN_400);
                        dat = dat + xwordOne.GetXword(one) + " ";

                        int u00 = 0;
                    }

                    int onen = a.Length - (a.Length % xWN_400);
                    //取得500个字符
                    if (i == onen)
                    {
                        string one = a.Substring(i, (a.Length % xWN_400));
                        dat = dat + xwordOne.GetXword(one);
                        break;
                    }

                }
            }


        X2:

            dat = dat.Trim();

            XunLongCNST[] x = new XunLongCNST[1];

            if (dat.IndexOf(' ') == -1)
            {
                if (dat.IndexOf('/') == -1)
                {
                    x = new XunLongCNST[1];
                    x[0].cStart = 0;
                    x[0].cLength = aOLD.Length;
                    x[0].cType = "n";
                    x[0].cWord = aOLD;
                    return x;
                }
                else
                {
                    x = new XunLongCNST[1];
                    string[] newtmp = dat.Split('/');
                    x[0].cStart = 0;
                    x[0].cLength = aOLD.Length;
                    if (newtmp[1].Length == 0)
                    {
                        x[0].cType = "n";
                    }
                    else
                    {
                        x[0].cType = newtmp[1];
                    }
                    x[0].cWord = aOLD;
                    return x;
                }
            }

            string[] tmps = dat.Split(' ');
            x = new XunLongCNST[tmps.Length];
            int pX = 0;
            int pNow = 0; //当前的位置
            for (int i = 0; i < tmps.Length; i++)
            {
                string TOne = tmps[i];
                if (TOne.Length > 0 & pNow <= aOLD.Length)
                {
                    if (TOne.IndexOf('/') == -1)
                    {
                        int nn = aOLD.IndexOf(TOne, pNow);
                        if (nn > -1)
                        {
                            pNow = nn + 1;
                            x[pX].cWord = TOne;
                            x[pX].cType = "n";
                            x[pX].cStart = nn;
                            x[pX].cLength = TOne.Length;
                            pX = pX + 1;
                        }
                        else
                        { }
                    }
                    else
                    {
                        string[] onet = TOne.Split('/');
                        string onet0 = onet[0];
                        int nn = aOLD.IndexOf(onet0, pNow);
                        if (nn > -1)
                        {
                            pNow = nn + 1;
                            x[pX].cWord = onet0;
                            if (onet.Length == 2)
                            {
                                if (onet[1].Length == 0)
                                {
                                    x[pX].cType = "n";
                                }
                                else
                                {
                                    x[pX].cType = onet[1];
                                }
                            }
                            else
                            {
                                x[pX].cType = "n";
                            }
                            x[pX].cStart = nn;
                            x[pX].cLength = onet0.Length;
                            pX = pX + 1;
                        }
                        else
                        { }
                    }
                }
            }

            return x;
        }



        /// <summary>
        /// 是否应该过滤掉
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool ChineseFilterIt(XunLongCNST a)
        {

            if (a.cWord == null)
            {
                return true;
            }


            if (a.cWord == null & a.cType == null)
            {
                return true;
            }


            //过滤掉停止词
            if (CnStopWord.Contains(a.cWord) == true)
            {
                return true;
            }

            if (a.cWord != null & a.cType == null)
            {
                return false;
            }

            string x = a.cType;

            if (x.IndexOf('n') > -1 | x.IndexOf('v') > -1 | x.IndexOf('i') > -1 | x.IndexOf('j') > -1 | x.IndexOf('l') > -1 | x.IndexOf('s') > -1)
            {
                return false;
            }

            return true;
        }

    }
}


//按照词性过滤

/*

Ag	形语素
a	形容词
ad	副形词
an	名形词
Bg	区别语素
b	区别词
c	连词
Dg	副语素
d	副词
e	叹词
f	方位词
g	语素
h	前接成分
i	成语
j	简略语
k	后接成分
l	习用语
Mg	数语素
m	数词
Ng	名语素
n	名词
nr	人名
ns	地名
nt	机构团体
nx	外文字符
nz	其它专名
o	拟声词
p	介词
Qg	量语素
q	量词
Rg	代语素
r	代词
s	处所词
Tg	时间语素
t	时间词
Ug	助语素
u	助词
Vg	动语素
v	动词
vd	副动词
vn	名动词
w	标点符号
x	非语素字
Yg	语气语素
y	语气词
z	状态词

 *
 * 
 * 
 * 
*/