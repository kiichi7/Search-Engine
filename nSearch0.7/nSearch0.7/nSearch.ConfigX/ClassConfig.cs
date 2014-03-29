using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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

namespace nSearch.ConfigX
{
    public static class ClassConfig
    {

        /// <summary>
        /// 文件系统路径 
        /// </summary>
        public static string path_XLFS;
 
        /// <summary>
        /// 数据分析系统路径
        /// </summary>
        public static string path_Model;

        /// <summary>
        /// 索引系统路径
        /// </summary>
        public static string path_Index;

        /// <summary>
        /// 索引指针
        /// </summary>
        public static string path_AIDStart;

        /// <summary>
        /// 模板返回页面
        /// </summary>
        public static string path_mHTML;

        /// <summary>
        /// TypeData类型
        /// </summary>
        public static string path_TypeData;

        /// <summary>
        /// 采样列表
        /// </summary>
        public static string path_T;

        /// <summary>
        /// URL中心路径
        /// </summary>
        public static string path_UrlCent;

        /// <summary>
        /// E:\DATATEST\UrlCent\Start.txt
        /// 蜘蛛配置文件
        /// </summary>
        public static string path_StartTxt;

        /// <summary>
        /// 初始化配置数据  名称为 TuDou.kc 放在本地路径下 
        /// </summary>
        /// <param name="path"></param>
        public static void InitConfigData(string sPath)
        {
            Hashtable c = new Hashtable();

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(sPath, System.Text.Encoding.GetEncoding("gb2312"));
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if ((line.Length > 0) & (line.IndexOf('=') > 0))
                    {
                        string[] x = line.Split('=');

                        if (x.Length == 2)
                        {
                            c.Add(x[0].Trim(), x[1].Trim());
                        }
                    }
                }
                reader.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            foreach (DictionaryEntry a in c)
            {

                string a_one = a.Key.ToString().Trim();
                a_one = a_one.Replace("\t", "");
                a_one = a_one.Replace(" ", "");
                a_one = a_one.Replace("\r", "");
                a_one = a_one.Replace("\n", "");






                // 文件系统路径 

                //path_XLFS;
                if (a_one == "path_XLFS")
                {
                    path_XLFS = a.Value.ToString().Trim();
                }

                // 数据分析系统路径

                //path_Model;
                if (a_one == "path_Model")
                {
                    path_Model = a.Value.ToString().Trim();
                }

                // 索引系统路径

                //path_Index;
                if (a_one == "path_Index")
                {
                    path_Index = a.Value.ToString().Trim();
                }

                // 索引指针

                //path_AIDStart;
                if (a_one == "path_AIDStart")
                {
                    path_AIDStart = a.Value.ToString().Trim();
                }

                // 模板返回页面

                //path_mHTML;
                if (a_one == "path_mHTML")
                {
                    path_mHTML = a.Value.ToString().Trim();
                }

                // TypeData类型

                //path_TypeData;
                if (a_one == "path_TypeData")
                {
                    path_TypeData = a.Value.ToString().Trim();
                }

                // 采样列表

                //path_T;
                if (a_one == "path_T")
                {
                    path_T = a.Value.ToString().Trim();
                }


                // path_UrlCent;

                if (a_one == "path_UrlCent")
                {
                    path_UrlCent = a.Value.ToString().Trim();
                }


                // E:\DATATEST\UrlCent\Start.txt
                // public static string path_StartTxt;
                if (a_one == "path_StartTxt")
                {
                    path_StartTxt = a.Value.ToString().Trim();
                }




            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sPath"></param>
        public static void SaveConfigData(string sPath)
        {
            string DDX = "";

              
        //文件系统路径 

            DDX = DDX + " path_XLFS=" + path_XLFS+"\r\n";
 
         
        //数据分析系统路径

            DDX = DDX + " path_Model=" + path_Model + "\r\n";

         
        //索引系统路径

            DDX = DDX + " path_Index=" + path_Index + "\r\n";

         
        //索引指针

            DDX = DDX + " path_AIDStart=" + path_AIDStart + "\r\n";

         
        //模板返回页面

            DDX = DDX + " path_mHTML=" + path_mHTML + "\r\n";

         
        //TypeData类型

            DDX = DDX + " path_TypeData=" + path_TypeData + "\r\n";


            //采样列表

            DDX = DDX + " path_T=" + path_T + "\r\n";

            DDX = DDX + " path_UrlCent=" + path_UrlCent + "\r\n";

            DDX = DDX + " path_StartTxt=" + path_StartTxt + "\r\n";

            SaveFileData(sPath, DDX);


        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        private static void SaveFileData(string filename, string data)
        {


            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("gb2312"));
                writer.Write(data);
                writer.Close();
            }
            catch (IOException e)
            {
               // nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }
    }
}
