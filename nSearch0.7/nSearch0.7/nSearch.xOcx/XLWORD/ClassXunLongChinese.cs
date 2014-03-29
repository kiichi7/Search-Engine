using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

/*
      '       Ѹ�����ķ�����������  v0.6
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
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
    /// �ִʽ���洢�ṹ
    /// </summary>
    public  struct XunLongCNST
    {
        /// <summary>
        /// ��
        /// </summary>
        public string cWord;
        /// <summary>
        /// ����
        /// </summary>
        public string cType;
        
        /// <summary>
        /// ��ʼλ��  0
        /// </summary>
        public int cStart;

        /// <summary>
        /// ����
        /// </summary>
        public int cLength;
    
    }
    
    /// <summary>
    /// Ѹ���ִʽӿ�
    /// </summary>
     public static class ClassXunLongChinese
    {
    
         /// <summary>
         /// �趨�ִ���
         /// </summary>
      //  private static XwordClassLibrary.ClassXWord mXW = new XwordClassLibrary.ClassXWord();

         /// <summary>
         /// �ִʽӿ�
         /// </summary>
         private static nSearch.xOcx.xlcom xwordOne = new nSearch.xOcx.xlcom();

         /// <summary>
         /// �洢ֹͣ��
         /// </summary>
         public static ArrayList CnStopWord = new ArrayList();


         public static int  xxx =1;

         public static void SetRun(int ii)
         {
             xxx = ii;         
         
         }

         /// <summary>
         /// ���ķִ�
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

                 //������000 Ϊ��λ�и 
                 for (int i = xWN_400; i < a.Length; i++)
                 {
                     if (i % xWN_400 == 0)
                     {
                         //ȡ��500���ַ�
                         string one = a.Substring(i - xWN_400, xWN_400);
                         dat = dat + xwordOne.GetXword(one) + " ";

                         int u00 = 0;
                     }

                     int onen = a.Length - (a.Length % xWN_400);
                     //ȡ��500���ַ�
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
             int pNow = 0; //��ǰ��λ��
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
         /// ���ķִ�
         /// </summary>
         /// <param name="_incn"></param>
         /// <returns></returns>
         public static XunLongCNST[] ChineseIntfaceOLD(TextReader _incn)
         {

             string a = _incn.ReadToEnd();

             a = a.ToLower();

             //����
             char[] xx = { '!', '(', ')', '{', '}', ':', ';', '\'', '"', ',', '.', '?', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', ' ', '\n', '\r', '\t' };

             string[] xa = a.Split(xx);

             //  1 xa��Ϊ xxaa  ������
             string[] xxaa = new string[12048];
             int xxaaLen = 0;

             for (int i = 0; i < xa.Length; i++)
             {

                 if (xa[i].Length > 2)  //����С��2�� �ַ� �����зִ�
                 {

                     string cca = xwordOne.GetXword(xa[i]);

                     if (cca.Length > xa[i].Length)   //��ȷ�ִ�
                     {

                         xa[i] = cca;                 //�ִʽ���滻ԭ������

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

             int pNow = 0; //��ǰ��λ��

             XunLongCNST[] x = new XunLongCNST[12048];
             int pX = 0;


             //   aΪԭʼ����  xxaa Ϊ����ģ��  ����ƥ��
             for (int i = 0; i < xxaa.Length; i++)
             {
                 if (xxaa[i] != null)
                 {
                     //��ǰ����
                     string tmpOne = xxaa[i];

                     //����
                     string tmpType = "n";

                     //�ı�����λ��
                     int tmpStart = 0;

                     //����
                     int tmpLength = 0;

                     if (tmpOne.IndexOf('/') > 0)
                     {
                         //�����ִ� ˵��  // ���������
                         string[] tmpS = tmpOne.Split('/');

                         int new_tmps = tmpOne.LastIndexOf('/');

                         string new_1 = tmpOne.Substring(0, new_tmps);

                         string new_2 = tmpOne.Substring(new_tmps + 1, tmpOne.Length - new_tmps - 1);

                         tmpOne = new_1;
                         tmpType = new_2;


                         //�õ��ı�
                         //tmpOne = tmpS[0];

                         //�õ�����
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

                     //�����ı���ԭ�����е�λ��
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
         /// �Ƿ�Ӧ�ù��˵�
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


             //���˵�ֹͣ��
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


//���մ��Թ���

/*

Ag	������
a	���ݴ�
ad	���δ�
an	���δ�
Bg	��������
b	�����
c	����
Dg	������
d	����
e	̾��
f	��λ��
g	����
h	ǰ�ӳɷ�
i	����
j	������
k	��ӳɷ�
l	ϰ����
Mg	������
m	����
Ng	������
n	����
nr	����
ns	����
nt	��������
nx	�����ַ�
nz	����ר��
o	������
p	���
Qg	������
q	����
Rg	������
r	����
s	������
Tg	ʱ������
t	ʱ���
Ug	������
u	����
Vg	������
v	����
vd	������
vn	������
w	������
x	��������
Yg	��������
y	������
z	״̬��

 *
 * 
 * 
 * 
*/