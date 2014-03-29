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

//存放一些公共的基础结构
namespace nSearch.ClassLibraryStruct
{
    /// <summary>
    /// 还原 过滤器
    /// </summary>
    public struct auto2dat
    {

        /// <summary>
        /// 类别一  模板匹配后进入 类别一的类聚数据 值   暂时不使用自动类聚 所以采用固定的地方
        /// </summary>
        public string A_TYPE_1;

        /// <summary>
        /// 类别2 模板匹配后进入 类别二的类聚数据 值   暂时不使用自动类聚 所以采用固定的地方
        /// </summary>
        public string A_TYPE_2;



        /// <summary>
        /// 标题A
        /// </summary>
        public string A;
        /// <summary>
        /// 类聚B
        /// </summary>
        public string B;
        /// <summary>
        /// 简要显示C
        /// </summary>
        public string C;
        /// <summary>
        /// 快照显示D
        /// </summary>
        public string D;
        /// <summary>
        /// 索引用的数据E
        /// </summary>
        public string E;
        /// <summary>
        /// 采样结果列表F
        /// </summary>
        public string F;

        /// <summary>
        /// 单独的标题数据 
        /// </summary>
        public string T;

        /// <summary>
        /// 自动生成 过滤器H
        /// </summary>
        public string H;

        /// <summary>
        /// 主类别
        /// </summary>
        public string M;

        /// <summary>
        /// 临时使用的BODY数据
        /// </summary>
        public string TmpBody;

        /// <summary>
        /// 存放临时的统计数据
        /// </summary>
        public int isXnum;

        /// <summary>
        /// 临时的排序遍历
        /// </summary>
        public bool isSORTIT;


                /// <summary>
        /// 临时的排序遍历
        /// </summary>
        public bool isOK;

        /// <summary>
        /// 临时存放模板的名称
        /// </summary>
        public string TmpName;
    }

    /// <summary>
    /// 一个处理的网页  去掉其它的
    /// </summary>
    public struct onePage
    {
        /// <summary>
        /// 原始标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 原始主体
        /// </summary>
        public string Body;

        /// <summary>
        /// 网页排名得分
        /// </summary>
        public int Num;


    
    }



}
