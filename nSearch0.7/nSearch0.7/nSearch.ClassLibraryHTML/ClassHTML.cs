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

namespace nSearch.ClassLibraryHTML
{




    public class ClassHTML
    {

        private  ClassTXT2IDAT mClassTXT2IDAT = new ClassTXT2IDAT();
        private ClassTagClear mClassTagClear = new ClassTagClear();



        /// <summary>
        /// 清除标点 Html标记 乱码 等 为干净文本
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public string GetClearCode(string dat,bool isClearBD)
        {
            string mct = mClassTXT2IDAT.GetOneGoodData2(dat, isClearBD);

            return mct;
        }


        /// <summary>
        /// 返回一个处理好的HTML数据 onePage 结构中 Title = 纯文本  Body = 干净的HTML数据   
        /// 
        /// 只取出　TITLE 和　BODY  中的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public nSearch.DebugShow.onePage GetOnePage(string data, string UrlX)
        {
          //  nSearch.ClassLibraryStruct.onePage VC = new nSearch.ClassLibraryStruct.onePage();

            nSearch.DebugShow.onePage VC = new nSearch.DebugShow.onePage();

            int a1 = data.IndexOf("<title>");
            int a2 = data.IndexOf("</title>");
            int a3 = data.IndexOf("<body");
            int a4 = data.IndexOf("</body>");

            int a5 = data.IndexOf(">", a3 + 1);

            string data1 = "";
            string data2 = "";

            try
            {

                if (a1 > 0 & a2 > 0 & a2 > a1)
                {
                    data1 = data.Substring(a1 + 7, a2 - a1 - 7);
                }

                if (a3 > 0 & a5 > 0 & a5 > a3)
                {
                    data2 = data.Substring(a5 + 1, a4 - a5 - 1);
                }

                data1 = data1.Replace("*", "#");
                data2 = data2.Replace("*", "#");

                VC.Title =mClassTXT2IDAT.GetOneGoodData2( data1,true);
                VC.Body = mClassTagClear.HTML2CLEAR2( data2);
                VC.Num = 0;
               
            }
            catch
            {

                VC.Title = "[]";
                VC.Body =mClassTagClear.HTML2CLEAR2( data);
                VC.Num = 0;
         
            }

            return VC;
        
        }




    }
}


/*
 
 
 
         /// <summary>
        /// 清除标签为干净的HTML  暂不考虑相对路径的URL问题
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public string GetClearTagX(string dat,string url)
        {
            string mct = mClassTagClear.HTML2CLEAR(dat);

            return mct;
                   
        }

 
 
 
 
 
 
 
 
 */