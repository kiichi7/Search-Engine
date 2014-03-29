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


namespace nSearch.DebugShow
{

    /// <summary>
    /// 调试信息显示  nSearch.DebugShow.ClassDebugShow.WriteLine("");
    /// </summary>
    public static class ClassDebugShow
    {

        private static StringBuilder show_string = new StringBuilder();

        /// <summary>
        /// 是否显示
        /// </summary>
        public static  bool IsShow = true;


        /// <summary>
        /// 是否显示
        /// </summary>
        public static void SetIsShow(bool isShowB)
        {
             IsShow = isShowB;
        }

        /// <summary>
        /// 读取显示缓冲区
        /// </summary>
        /// <returns></returns>
        public static string showf()
        {
            if (IsShow == false) { return ""; }

            string show_string2 = show_string.ToString();
            show_string.Remove(0, show_string.Length);


            return show_string2;
        }

        /// <summary>
        /// 设定要显示的数据到缓存区
        /// </summary>
        /// <param name="dat"></param>
        public static void WriteLine(string dat)
        {
            if (IsShow == false) { return  ; }

           

       //     System.Diagnostics.Debug.WriteLine(dat );

       //     Console.WriteLine(dat);


            if (show_string.Length > 1024 * 128)
            {//缓存区未能及时读取的话 进行清理
                show_string.Remove(0, show_string.Length);
            }

            show_string.AppendLine(dat+"   "+ Environment.TickCount.ToString());
            


        }

        /// <summary>
        /// 设定要显示的数据到缓存区
        /// </summary>
        /// <param name="dat"></param>
        public static void WriteLineF(string dat)
        {
             //   System.Diagnostics.Debug.WriteLine(dat );

             //    Console.WriteLine(dat);

            if (IsShow == false) { return  ; }

            if (show_string.Length > 1024 * 128)
            {   //缓存区未能及时读取的话 进行清理
                show_string.Remove(0, show_string.Length);
            }

                show_string.AppendLine(dat);
              //  System.Diagnostics.Debug.WriteLine(dat);
           


        }

    }
}
