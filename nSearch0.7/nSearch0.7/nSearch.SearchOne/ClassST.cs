using System;
using System.Collections.Generic;
using System.Text;
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
   public  static  class ClassST
    {

       public static Lucene.Net.Analysis.Analyzer OneAnalyzer;

       public static object mSearch;

     


       /// <summary>
       /// ��ǰ����ִ���
       /// </summary>
       public static void Init()      
       {
               /// <summary>
        /// �ִ���
        /// </summary>
         OneAnalyzer = new Lucene.Net.Analysis.XunLongX.XunLongAnalyzer();


       /// <summary>
       /// ������
       /// </summary>
        mSearch = new ClassSearch();

       }
        


    }
}
