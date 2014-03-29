using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.ConfigX
{
    public static class ClassConfig
    {

        /// <summary>
        /// �ļ�ϵͳ·�� 
        /// </summary>
        public static string path_XLFS;
 
        /// <summary>
        /// ���ݷ���ϵͳ·��
        /// </summary>
        public static string path_Model;

        /// <summary>
        /// ����ϵͳ·��
        /// </summary>
        public static string path_Index;

        /// <summary>
        /// ����ָ��
        /// </summary>
        public static string path_AIDStart;

        /// <summary>
        /// ģ�巵��ҳ��
        /// </summary>
        public static string path_mHTML;

        /// <summary>
        /// TypeData����
        /// </summary>
        public static string path_TypeData;

        /// <summary>
        /// �����б�
        /// </summary>
        public static string path_T;

        /// <summary>
        /// URL����·��
        /// </summary>
        public static string path_UrlCent;

        /// <summary>
        /// E:\DATATEST\UrlCent\Start.txt
        /// ֩�������ļ�
        /// </summary>
        public static string path_StartTxt;

        /// <summary>
        /// ��ʼ����������  ����Ϊ TuDou.kc ���ڱ���·���� 
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






                // �ļ�ϵͳ·�� 

                //path_XLFS;
                if (a_one == "path_XLFS")
                {
                    path_XLFS = a.Value.ToString().Trim();
                }

                // ���ݷ���ϵͳ·��

                //path_Model;
                if (a_one == "path_Model")
                {
                    path_Model = a.Value.ToString().Trim();
                }

                // ����ϵͳ·��

                //path_Index;
                if (a_one == "path_Index")
                {
                    path_Index = a.Value.ToString().Trim();
                }

                // ����ָ��

                //path_AIDStart;
                if (a_one == "path_AIDStart")
                {
                    path_AIDStart = a.Value.ToString().Trim();
                }

                // ģ�巵��ҳ��

                //path_mHTML;
                if (a_one == "path_mHTML")
                {
                    path_mHTML = a.Value.ToString().Trim();
                }

                // TypeData����

                //path_TypeData;
                if (a_one == "path_TypeData")
                {
                    path_TypeData = a.Value.ToString().Trim();
                }

                // �����б�

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
        /// ��������
        /// </summary>
        /// <param name="sPath"></param>
        public static void SaveConfigData(string sPath)
        {
            string DDX = "";

              
        //�ļ�ϵͳ·�� 

            DDX = DDX + " path_XLFS=" + path_XLFS+"\r\n";
 
         
        //���ݷ���ϵͳ·��

            DDX = DDX + " path_Model=" + path_Model + "\r\n";

         
        //����ϵͳ·��

            DDX = DDX + " path_Index=" + path_Index + "\r\n";

         
        //����ָ��

            DDX = DDX + " path_AIDStart=" + path_AIDStart + "\r\n";

         
        //ģ�巵��ҳ��

            DDX = DDX + " path_mHTML=" + path_mHTML + "\r\n";

         
        //TypeData����

            DDX = DDX + " path_TypeData=" + path_TypeData + "\r\n";


            //�����б�

            DDX = DDX + " path_T=" + path_T + "\r\n";

            DDX = DDX + " path_UrlCent=" + path_UrlCent + "\r\n";

            DDX = DDX + " path_StartTxt=" + path_StartTxt + "\r\n";

            SaveFileData(sPath, DDX);


        }

        /// <summary>
        /// д�ļ�
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
