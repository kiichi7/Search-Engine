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

namespace nSearch.FS
{
    /// <summary>
    /// 文件系统  带锁
    /// </summary>
   public static  class ClassFSMD
    {

       private static ClassFS myFS = new ClassFS();

       /// <summary>
       /// 数据读取锁  锁定时 不允许其它读操作
       /// </summary>
       private static bool xl_lock_r =false;

       /// <summary>
       /// 数据写入锁  锁定时 不允许其它写操作
       /// </summary>
       private static bool xl_lock_w = false;

      

       /// <summary>
       /// 初始化数据系统
       /// </summary>
       /// <param name="path"></param>
       public static void Init(string path)
       {

           //锁定系统
           xl_lock_r = true;
           //锁定系统
           xl_lock_w = true;
           //初始化系统
           myFS.InitData(path);

           //写完解锁
           xl_lock_r = false;
           //写完解锁
           xl_lock_w = false;
       }

       /// <summary>
       /// 读取一条数据
       /// </summary>
       /// <param name="id">数据ID</param>
       /// <returns></returns>
       public static oneHtmDat GetOneDat(int id)
       {
           oneHtmDat myTmp = new oneHtmDat();

           if (xl_lock_r == true)
           {
               return myTmp;
           }

           xl_lock_r = true; //锁定

            myTmp = myFS.GetData(id);

           xl_lock_r = false; //解锁

           return myTmp;
       }

       /// <summary>
       /// 写入一条数据
       /// </summary>
       /// <param name="url"></param>
       /// <param name="dat"></param>
       /// <returns></returns>
       public static bool PutOneDat(string url, string dat)
       {
           //数据处于写锁定状态 不能写入
           if (xl_lock_w == true)
           {
               return false;
           }


           //锁定系统
           xl_lock_w = true;

           myFS.SaveData(url, dat); 

           //写完解锁
           xl_lock_w = false ;

           return true;
       
       }

       /// <summary>
       /// 得到ID 列表
       /// </summary>
       /// <returns></returns>
       public static ArrayList GetUrlList()
       {
           ArrayList nnnn = myFS.UrlNameIndex;

           return nnnn;
       }


       public static int GetFileNum()
       {

           return myFS.FileNum;
       
       }

       /// <summary>
       /// 保存退出
       /// </summary>
       public static void SaveExit()
       {

           myFS.SaveCache();
       
       }

    }
}
