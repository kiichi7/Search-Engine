using System;
using System.Collections.Generic;
using System.Text;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.SearchOne
{
   public  static  class ClassST
    {

       public static Lucene.Net.Analysis.Analyzer OneAnalyzer;

       public static object mSearch;

     


       /// <summary>
       /// 提前处理分词类
       /// </summary>
       public static void Init()      
       {
               /// <summary>
        /// 分词类
        /// </summary>
         OneAnalyzer = new Lucene.Net.Analysis.XunLongX.XunLongAnalyzer();


       /// <summary>
       /// 搜索类
       /// </summary>
        mSearch = new ClassSearch();

       }
        


    }
}
