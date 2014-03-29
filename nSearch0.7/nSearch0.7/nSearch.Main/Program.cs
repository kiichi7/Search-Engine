using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// http://127.0.0.1:81/s?wd=中国
        /// 
        ///    每个节点取出前100个 然后 合并 项  重新排序
        /// 
        ///    记录每个节点 总共的个数  节点有一个顺序 
        ///           如果 请求的个数 大于 节点数*100   然后读取节点的数据              
        ///     
        /// 
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            nSearch.ConfigX.ClassConfig.InitConfigData("Config.txt");
            Application.Run(new FormMain());
        }
    }
}