using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.TheSame
{
    class ClassTheSame
    {
       public  string fspath;
        
        public  string tspath;


        public void StartX( )
        {
            //�ļ�ϵͳ��ʼ��
            nSearch.FS.ClassFSMD.Init(fspath);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ļ�ϵͳ��ʼ��");

            ArrayList nnn = nSearch.FS.ClassFSMD.GetUrlList();

            //���һ������
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
                            //����һЩ���� ����֤�����Ķ�����



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
                        //����һЩ���� ����֤�����Ķ�����
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

                    //��ʾͬһ���������  
                    string NTOP = GMD5(AAA);

                    //END_WEN



                    for (int ii = 0; ii < 50;ii++ )
                    {

                        END_WEN.Add(NTOP + "\t" + tmp[ii].ToString());


                    }

                    tmp = null;

                   // listBox1.Items.Add(AAA);

                    nSearch.DebugShow.ClassDebugShow.WriteLineF("�õ�һ���������" + AAA);
                }

                if (nnn.Count > Num)
                {
                    goto StartIT;
                }
            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("�������");



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

            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ܹ��������� " + END.Count.ToString() + " ��");
          //  System.Threading.Thread.Sleep(1000);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ܹ��������� " + END.Count.ToString() + " ��");

        }






        /// <summary>
        /// �õ�MD5
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        public string GMD5(string dat)
        {
            return  System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(dat, "md5");
        }




        /// <summary>
        /// URL���ƶ�  ͬһվ�� ���� 10  ����1��ģ��  ���� 12
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
                RT = 10;  //��ͬһվ��
            }
            else
            {
                return 0;
            }

            if (ax1.Length != ax2.Length & ax1[2] == ax2[2])
            {

                return 10; //������ͬ1վ�� 
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





            // ����Ƿ��� ? ����
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
                    // û�в��� �����չ���Ƿ���ͬ
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
            //�ļ�ϵͳ��ʼ��
            nSearch.FS.ClassFSMD.Init(fspath);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ļ�ϵͳ��ʼ��");

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
                            //����һЩ���� ����֤�����Ķ�����
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
                        //����һЩ���� ����֤�����Ķ�����
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

                    nSearch.DebugShow.ClassDebugShow.WriteLineF("�õ�һ���������" + AAA);
                }

                if (nnn.Count > Num)
                {
                    goto StartIT;
                }
            }

            nSearch.DebugShow.ClassDebugShow.WriteLineF("�������");



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

            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ܹ��������� " + END.Count.ToString() + " ��");
          //  System.Threading.Thread.Sleep(1000);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("�ܹ��������� " + END.Count.ToString() + " ��");

        }


 
 */