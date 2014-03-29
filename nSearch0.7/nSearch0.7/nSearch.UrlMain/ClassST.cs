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

namespace nSearch.UrlMain
{

    /// <summary>
    /// URL数据中心
    /// </summary>
    public static  class ClassSTURL
    {
        private static ClassUrlMain newUrl = new ClassUrlMain();


        /// <summary>
        /// 数据读取锁  锁定时 不允许其它读操作
        /// </summary>
        private static bool xl_lock_r = false;

        /// <summary>
        /// 数据写入锁  锁定时 不允许其它写操作
        /// </summary>
        private static bool xl_lock_w = false;






        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init( string path)
        {
            //锁定系统
            xl_lock_r = true;
            //锁定系统
            xl_lock_w = true;
          

            newUrl.InitClassUrlMain(path);



 
            //写完解锁
            xl_lock_r = false;
            //写完解锁
            xl_lock_w = false;

        }

        /// <summary>
        /// 读取一个URL
        /// </summary>
        public static string  GetOneUrl()
        {
            if (xl_lock_r == true)
            {
                return null;
            }

            xl_lock_r = true; //锁定

        

       




    string     btr =   newUrl.GetOneUrl();

     xl_lock_r = false; //解锁

    return btr;




        }


        /// <summary>
        /// 压入一个URL
        /// </summary>
        public static void PutOneUrl(string url)
        {

            //数据处于写锁定状态 不能写入
            if (xl_lock_w == true)
            {
                return ;
            }


            //锁定系统
            xl_lock_w = true;

             newUrl.PutOneUrl(url);        

            //写完解锁
            xl_lock_w = false;


        }

        /// <summary>
        /// 压入一个URL
        /// </summary>
        public static void RePutOneUrl(string url)
        {

            //数据处于写锁定状态 不能写入
            if (xl_lock_w == true)
            {
                return  ;
            }


            //锁定系统
            xl_lock_w = true;

              newUrl.RePutOneUrl(url);     

            //写完解锁
            xl_lock_w = false;


        }

        /// <summary>
        /// 得到内部情况统计
        /// </summary>
        /// <returns></returns>
        public static string GetTongji()
        {

            return newUrl.GetTongji();
        
        }

        /// <summary>
        /// 保存退出 
        /// </summary>
        public static void SaveExit()
        {
            newUrl.ExitSave();
        
        }


    }
}
