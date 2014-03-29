using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.TheSame
{
    class ClassTheSame
    {
       public  string fspath;
        
        public  string tspath;


        public void StartX( )
        {
            //文件系统初始化
            nSearch.FS.ClassFSMD.Init(fspath);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("文件系统初始化");

            ArrayList nnn = nSearch.FS.ClassFSMD.GetUrlList();

            //存放一个备份
            ArrayList nnn2 = (ArrayList)nnn.Clone();


            ArrayList END = new ArrayList();
            ArrayList END_WEN = new ArrayList();


            int Num =50;

        StartIT:

            if (nnn.Count > Num)
            {
                string AAA = nnn[0].ToString();
                nnn.RemoveAt(0);

                ArrayList tmp = new ArrayList();

                tmp.Clear();
                 


                for (int i = 0; i < nnn.Count-1; i++)
                {
                    string AAA2 =  nnn[i].ToString();

                    int GT = Url2Url(AAA,AAA2);

                    if (GT > 10)
                    {

                        if ( i % (Num/2) == 7)
                        {
                            //空余一些数据 来保证采样的多样性



                        }
                        else
                        {

                            if (tmp.Count > Num)
                            { }
                            else
                            {
                                tmp.Add(AAA2);
                            }



                            nnn.RemoveAt(i);

                        }

                    }
                }

              
                /*
                foreach (string atmp in tmp)
                {
                    if (nnn.Count %77 == 6)
                    {
                        //空余一些数据 来保证采样的多样性
                    }
                    else
                    {
                        nnn.Remove(atmp);
                    }
                }
                */

                if (tmp.Count > Num)
                {
                    END.Add(AAA);

                    //标示同一个采样结果  
                    string NTOP = GMD5(AAA);

                    //END_WEN



                    for (int ii = 0; ii < 50;ii++ )
                    {

                        END_WEN.Add(NTOP + "\t" + tmp[ii].ToString());


                    }

                    tmp = null;

                   // listBox1.Items.Add(AAA);

                    nSearch.DebugShow.ClassDebugShow.WriteLineF("得到一个采样结果" + AAA);
                }

                if (nnn.Count > Num)
                {
                    goto StartIT;
                }
            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("采样完成");



            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(tspath, false, System.Text.Encoding.GetEncoding("gb2312"));

                foreach (string iuy in END_WEN)
                {
                    writer.WriteLine(iuy);

                }


                writer.Close();
            }
            catch (IOException e0)
            {
                if (writer != null)
                    writer.Close();
            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("总共采样到： " + END.Count.ToString() + " 条");
          //  System.Threading.Thread.Sleep(1000);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("总共采样到： " + END.Count.ToString() + " 条");

        }






        /// <summary>
        /// 得到MD5
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public string GMD5(string dat)
        {
            return  System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dat, "md5");
        }




        /// <summary>
        /// URL相似度  同一站点 返回 10  属于1组模版  返回 12
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public int Url2Url(string url1, string url2)
        {
            url1 = url1.ToLower().Trim();
            url2 = url2.ToLower().Trim();


            if (url1 + "/" == url2 | url1 == url2 + "/")
            {
                return 12;
            }

            int RT = 0;

            if (url1.IndexOf("http://") != 0 | url2.IndexOf("http://") != 0)
            {
                return 0;
            }


            string[] ax1 = url1.Split('/');
            string[] ax2 = url2.Split('/');

            if (ax1[2] == ax2[2])
            {
                RT = 10;  //在同一站点
            }
            else
            {
                return 0;
            }

            if (ax1.Length != ax2.Length & ax1[2] == ax2[2])
            {

                return 10; //两者在同1站点 
            }


            for (int i = 2; i < ax1.Length - 1; i++)
            {
                if (ax1[i] != ax2[i])
                {
                    return 10;
                }

            }



            string url1file = ax1[ax1.Length - 1];
            string url2file = ax2[ax2.Length - 1];





            // 检查是否有 ? 出现
            if (url1file.IndexOf('?') > -1)
            {
                if (url2file.IndexOf('?') > -1)
                {
                    string[] kzms1 = url1file.Split('?');
                    string[] kzms2 = url2file.Split('?');

                    if (kzms1[0] == kzms2[0])
                    {
                        return 12;
                    }
                    else
                    {
                        return 10;
                    }
                }
                else
                {
                    return 10;
                }
            }
            else
            {
                if (url2file.IndexOf('?') > -1)
                {
                    return 10;
                }
                else
                {
                    // 没有参数 检测扩展名是否相同
                    if (url1file.IndexOf('.') == -1 | url2file.IndexOf('.') == -1)
                    {
                        return 10;
                    }
                    else
                    {
                        string[] kzm1 = url1file.Split('.');
                        string[] kzm2 = url2file.Split('.');

                        if (kzm1[kzm1.Length - 1] == kzm2[kzm2.Length - 1])
                        {
                            return 12;
                        }
                        else
                        {
                            return 10;
                        }
                    }
                }

            }


        }


    }
}

/*
 
 
        public void StartX( )
        {
            //文件系统初始化
            nSearch.FS.ClassFSMD.Init(fspath);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("文件系统初始化");

            ArrayList nnn = nSearch.FS.ClassFSMD.GetUrlList();

            ArrayList END = new ArrayList();

            int Num = 16;

        StartIT:

            if (nnn.Count > Num)
            {
                string AAA = nnn[0].ToString();

                ArrayList tmp = new ArrayList();

                tmp.Clear();

                nnn.RemoveAt(0);

                for (int i = 0; i < nnn.Count; i++)
                {
                    int GT = Url2Url(AAA, nnn[i].ToString());

                    if ( GT >  10)
                    {

                        if (tmp.Count % 17 == 6  & i % 3==1 )
                        {
                            //空余一些数据 来保证采样的多样性
                        }
                        else
                        {
                            tmp.Add(nnn[i].ToString());
                        }
                      
                    }
                }

              

                foreach (string atmp in tmp)
                {
                    if (nnn.Count % 27 == 6)
                    {
                        //空余一些数据 来保证采样的多样性
                    }
                    else
                    {
                        nnn.Remove(atmp);
                    }
                }

                if (tmp.Count > Num)
                {
                    END.Add(AAA);
                   // listBox1.Items.Add(AAA);

                    nSearch.DebugShow.ClassDebugShow.WriteLineF("得到一个采样结果" + AAA);
                }

                if (nnn.Count > Num)
                {
                    goto StartIT;
                }
            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("采样完成");



            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(tspath, false, System.Text.Encoding.GetEncoding("gb2312"));

                foreach (string iuy in END)
                {
                    writer.WriteLine(iuy);

                }


                writer.Close();
            }
            catch (IOException e0)
            {
                if (writer != null)
                    writer.Close();
            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("总共采样到： " + END.Count.ToString() + " 条");
          //  System.Threading.Thread.Sleep(1000);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("总共采样到： " + END.Count.ToString() + " 条");

        }


 
 */