using System;
using System.Collections.Generic;
using System.Text;

using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;

/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.ModelData
{
    /// <summary>
    /// 自适应模板解析
    /// </summary>
    public static class ClassModelData
    {

        //调用解析类
        private  static nSearch.ClassLibraryStruct.ClassUserModel xMod = new nSearch.ClassLibraryStruct.ClassUserModel();

        /// <summary>
        /// 模板匹配类
        /// </summary>
        private static nSearch.ClassLibraryStruct.ClassUserModel mxWeb = new nSearch.ClassLibraryStruct.ClassUserModel();

        /// <summary>
        /// 数据清理
        /// </summary>
        private static nSearch.ClassLibraryHTML.ClassHTML vclear = new nSearch.ClassLibraryHTML.ClassHTML();

        /// <summary>
        /// 初始化模板
        /// </summary>
        /// <param name="mpath"></param>
        public static void Init(string mpath)
        {
            nSearch.DebugShow.ClassDebugShow.WriteLineF("过滤系统初始化");

            mxWeb.init(mpath);

            nSearch.DebugShow.ClassDebugShow.WriteLineF("过滤系统初始化[ok]");

        }

        /// <summary>
        /// 返回一个系统对应的文档项
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public static Lucene.Net.Documents.Document GetOneDoc(string url,string title, string htmldat,int Num)
        {

            Lucene.Net.Documents.Document oneDoc = new Lucene.Net.Documents.Document();



            nSearch.ClassLibraryStruct.auto2dat k = mxWeb.getTagAndData(htmldat);

            //doc.Add(Field.Text("ID", id)); // 名称

            if (k.isOK == true)
            {
                oneDoc.Add(new Field("A", k.A, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// 标题A

                nSearch.DebugShow.ClassDebugShow.WriteLineF("模板匹配成功");
            }
            else
            {
                oneDoc.Add(new Field("A",title, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// 标题A


                nSearch.DebugShow.ClassDebugShow.WriteLineF("模板匹配失败");
            }

            if (k.B.Length == 0)
            {
                k.B = "kc";
            }
            oneDoc.Add(new Field("B", k.B, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 类聚B
            if (k.C.Length == 0)
            {
                k.C = "kc";
            }
            oneDoc.Add(new Field("C", k.C, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 简要显示C
            if (k.D.Length == 0)
            {
                k.D = "kc";
            }
            oneDoc.Add(new Field("D", k.D, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 快照显示D

            //*
            oneDoc.Add(new Field("E", vclear.GetClearCode( k.E,true), Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));// 索引用的数据E
            if (k.T.Length == 0)
            {
                k.T = "kc";
            }
            oneDoc.Add(new Field("T", k.T, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// 单独的标题数据
            if (k.M.Length == 0)
            {
                k.M = "kc";
            }
            oneDoc.Add(new Field("M", k.M, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO));// 主类别
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
 * Field.Index有四个属性，分别是：
Field.Index.TOKENIZED：分词索引
Field.Index.UN_TOKENIZED：分词进行索引，如作者名，日期等，Rod Johnson本身为一单词，不再需要分词。
Field.Index：不进行索引，存放不能被搜索的内容如文档的一些附加属性如文档类型, URL等。
Field.Index.NO_NORMS：； 
Field.Store也有三个属性，分别是：
Field.Store.YES：索引文件本来只存储索引数据, 此设计将原文内容直接也存储在索引文件中，如文档的标题。
Field.Store.NO：原文不存储在索引文件中，搜索结果命中后，再根据其他附加属性如文件的Path，数据库的主键等，重新连接打开原文，适合原文内容较大的情况。
Field.Store.COMPRESS 压缩存储； 
termVector是Lucene 1.4.3新增的它提供一种向量机制来进行模糊查询,很少用。 
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
        /// 采样结果列表F
        /// </summary>
        public string F;

        /// <summary>
        /
        /// </summary>
        public string T;

        /// <summary>
        /// 自动生成 过滤器H
        /// </summary>
        public string H;

        /// <summary>
        /
        /// </summary>
        public string M;
 */