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
    public  struct XunLongCNST
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


         public static int  xxx =1;

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
             string  a = aOLD.Trim();

             a =a.Replace("---"," ");
             a =a.Replace("==="," ");
             a =a.Replace("   "," ");
             a =a.Replace("  "," ");



             if (a == null | a.Length == 0)
             {
                 return null;
             
             }

             string dat = "";

             if (a.IndexOf(' ') > 0)
             {
                 string[] NewAS = a.Split(' ');
                 int nal =a.Length / 5;

                 if (NewAS.Length >= nal)
                 {
                     dat = "";
                     for (int c = 0; c < NewAS.Length; c++)
                     {
                         dat = dat + NewAS[c];
                     }
                     goto X2;
                 }
             }


             int xWN_400 =512;

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
                 if (TOne.Length > 0  & pNow <= aOLD.Length  )
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
         /// 中文分词
         /// </summary>
         /// <param name="_incn"></param>
         /// <returns></returns>
         public static XunLongCNST[] ChineseIntfaceOLD(TextReader _incn)
         {

             string a = _incn.ReadToEnd();

             a = a.ToLower();

             //隔开
             char[] xx = { '!', '(', ')', '{', '}', ':', ';', '\'', '"', ',', '.', '?', '！', '（', '）', '：', '；', '‘', '“', '，', '。', '？', ' ', '\n', '\r', '\t' };

             string[] xa = a.Split(xx);

             //  1 xa变为 xxaa  的数组
             string[] xxaa = new string[12048];
             int xxaaLen = 0;

             for (int i = 0; i < xa.Length; i++)
             {

                 if (xa[i].Length > 2)  //长度小于2的 字符 不进行分词
                 {

                     string cca = xwordOne.GetXword(xa[i]);

                     if (cca.Length > xa[i].Length)   //正确分词
                     {

                         xa[i] = cca;                 //分词结果替换原来数据

                     }

                 }


                 if (xa[i].IndexOf(' ') > -1)
                 {
                     string[] tmp = xa[i].Split(' ');
                     for (int j = 0; j < tmp.Length; j++)
                     {
                         if (tmp[j] != null & tmp[j].Length > 0)
                         {
                             xxaa[xxaaLen] = tmp[j];
                             xxaaLen = xxaaLen + 1;
                         }
                     }
                 }
                 else
                 {
                     xxaa[xxaaLen] = xa[i];
                     xxaaLen = xxaaLen + 1;
                 }

             }

             int pNow = 0; //当前的位置

             XunLongCNST[] x = new XunLongCNST[12048];
             int pX = 0;


             //   a为原始数据  xxaa 为数据模板  进行匹配
             for (int i = 0; i < xxaa.Length; i++)
             {
                 if (xxaa[i] != null)
                 {
                     //当前数据
                     string tmpOne = xxaa[i];

                     //类型
                     string tmpType = "n";

                     //文本串中位置
                     int tmpStart = 0;

                     //长度
                     int tmpLength = 0;

                     if (tmpOne.IndexOf('/') > 0)
                     {
                         //包含分词 说明  // 分离出类型
                         string[] tmpS = tmpOne.Split('/');

                         int new_tmps = tmpOne.LastIndexOf('/');

                         string new_1 = tmpOne.Substring(0, new_tmps);

                         string new_2 = tmpOne.Substring(new_tmps + 1, tmpOne.Length - new_tmps - 1);

                         tmpOne = new_1;
                         tmpType = new_2;


                         //得到文本
                         //tmpOne = tmpS[0];

                         //得到长度
                         tmpLength = tmpOne.Length;

                         /*
                         if (tmpS.Length > 1)
                         {
                             if (tmpS[1].Length > 0)
                             {
                                 tmpType = tmpS[1];
                             }
                         }
                         */


                     }

                     //检测出文本在原数据中的位置
                     int tmpxx = a.IndexOf(tmpOne);

                     if (tmpxx > -1)
                     {
                         tmpStart = tmpxx + pNow;

                         a = a.Substring(tmpxx + tmpLength, a.Length - tmpLength - tmpxx);

                         pNow = pNow + tmpxx + tmpLength - 1;

                         x[pX].cWord = tmpOne;
                         x[pX].cType = tmpType;
                         x[pX].cStart = tmpStart;
                         x[pX].cLength = tmpLength;
                     }
                     pX = pX + 1;
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

             return true ;
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