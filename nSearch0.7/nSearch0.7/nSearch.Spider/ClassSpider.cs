using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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

namespace nSearch.Spider
{

    /// <summary>
    /// �洢����
    /// </summary>
    public struct UrlHDat
    {
        public string url;

        public string dat;
    
    }


    /// <summary>
    /// ֩��  
    ///  
    ///   1 ��ȡһ��URL
    ///   2 ��ȡһ���ı���ʽ��ҳ
    ///   3 ��ʧ�ܵ�URLѹ��
    ///   4 ֩���Լ����ļ���ͬ��ȡ���� ���˵��Ѿ���ȡ������ҳ��
    /// </summary>
   public  class ClassSpider
    {
       ClassIsHave UrlIsHave = new ClassIsHave();

       Thread[] TT = new Thread[1];

       /// <summary>
       /// ֩����߳���Ŀ
       /// </summary>
       private int ThreadNum = 5;

       /// <summary>
       /// �Ƿ�ֹͣ
       /// </summary>
       private bool StopIt = false;

       /// <summary>
       /// ͳ�Ʊ��λ�õ���������
       /// </summary>
       private int InNum = 0;

       /// <summary>
       /// ��ʼ��ʱ��
       /// </summary>
       private int InStartTime = 0;

       /// <summary>
       /// ϵͳ��ʼ��
       /// </summary>      
       /// <param name="fspath">�ļ�ϵͳ·��</param>
       /// <param name="urlpath">urlϵͳ·��</param>
       public void Init(string fspath, string urlpath)
       {
                  //�ļ�ϵͳ��ʼ��
           nSearch.FS.ClassFSMD.Init(fspath);
           nSearch.DebugShow.ClassDebugShow.WriteLine("�ļ�ϵͳ��ʼ��");


           //URLϵͳ��ʼ��
           nSearch.UrlMain.ClassSTURL.Init(urlpath);
           nSearch.DebugShow.ClassDebugShow.WriteLine("URLϵͳ��ʼ��");


           //�õ��ļ�ϵͳ�����е�URL
           ArrayList isHaveV = nSearch.FS.ClassFSMD.GetUrlList();
           nSearch.DebugShow.ClassDebugShow.WriteLine("�õ��ļ�ϵͳ�����е�URL");

           //ѹ��ȥ���б�ģ��
           UrlIsHave.InitData(isHaveV);
           nSearch.DebugShow.ClassDebugShow.WriteLine("ѹ��ȥ���б�ģ��");

       
       }


       /// <summary>
       /// ��ʼ����
       /// </summary>
       /// <param name="TNUM">���е��߳���Ŀ</param>

       public void StartRun(int TNUM)
       {


             /// <summary>
       /// ͳ�Ʊ��λ�õ���������
       /// </summary>
        InNum = 0;

       /// <summary>
       /// ��ʼ��ʱ��
       /// </summary>
        InStartTime = Environment.TickCount  ;


           ThreadNum =TNUM;

           nSearch.DebugShow.ClassDebugShow.WriteLine("֩���߳����� " + ThreadNum.ToString());

           Thread[] TT = new Thread[ThreadNum];

           StopIt = false;

           for (int ii = 0; ii < ThreadNum; ii++)
           {
               
               TT[ii] = new Thread(new ThreadStart(RunOne));
               TT[ii].Start();
               System.Threading.Thread.Sleep(100);
               nSearch.DebugShow.ClassDebugShow.WriteLine("  -->֩���߳� "+ii.ToString());
           }
       
       }

       /// <summary>
       /// �رճ���
       /// </summary>
       public void StopRun()
       {
           StopIt = true;




           for (int ii = 0; ii < ThreadNum; ii++)
           {
               try
               {
                   TT[ii].Suspend();
               }
               catch
               { }

             //  System.Threading.Thread.Sleep(100);

               try
               {
                   TT[ii].Abort();
               }
               catch
               {  }



           }

           StopIt = true;
                  //�ļ�ϵͳexit
           nSearch.FS.ClassFSMD.SaveExit();

           //URLϵͳexit
           nSearch.UrlMain.ClassSTURL.SaveExit();
       }

       /// <summary>
       /// һ���߳�
       /// </summary>
       private void RunOne()
       {
           //�������ֵĴ���
           int END_ALL_NULL = 0;


           

           while (!StopIt)
           {




             

               UrlHDat xm = GetOneUrlHDat();


               if (xm.url == "END_ALL_NULL")
               {

                   END_ALL_NULL = END_ALL_NULL + 1;

                   if (END_ALL_NULL > 30)
                   {
                       //ϵͳ���� URL��ȫ����

                       StopIt = true;

                       nSearch.DebugShow.ClassDebugShow.WriteLine("ϵͳ���� URL��ȫ����");

                       return;
                   }
                   else
                   {
                       nSearch.DebugShow.ClassDebugShow.WriteLine("��ʱ  URL��ȫ����");
                       //��ʱ ��������վ���DOS ����
                       System.Threading.Thread.Sleep(6000);
                   }
               }

               if (xm.url.Length == 0 | xm.dat.Length == 0)
               {
                   
               }
               else
               {
                   int xTime = Environment.TickCount;

                   //���ݻ�ȡ
                   //����д���ļ�ϵͳ
                   nSearch.FS.ClassFSMD.PutOneDat(xm.url, xm.dat);

                   int xTime2 = Environment.TickCount - xTime ;

                   nSearch.DebugShow.ClassDebugShow.WriteLine("ѹ��һ��URL��Ҫʱ�� -->>" + xTime2.ToString());

                   //������ͳ��
                   InNum = InNum + 1;

                   int xTimeX = Environment.TickCount - InStartTime;

                   //�õ��ٶ�
                   double xSHow = (double )InNum * 1000.0 / (double )xTimeX;

                   nSearch.DebugShow.ClassDebugShow.WriteLine("�ٶ� -->>" + xSHow.ToString()+" ��/��");

               }

               //��ʱ ��������վ���DOS ����
              // System.Threading.Thread.Sleep(200);

           }
       }


       /// <summary>
       /// �õ�һ������
       /// </summary>
       /// <returns></returns>
       public UrlHDat GetOneUrlHDat()
       {
           UrlHDat xm = new UrlHDat();


           int t_1 = Environment.TickCount;
           string xxx = nSearch.UrlMain.ClassSTURL.GetOneUrl();
           int t_2 = Environment.TickCount - t_1;
           nSearch.DebugShow.ClassDebugShow.WriteLine("����һ��UEL����ʱ�� -> " + t_2.ToString());


           if (xxx == null | xxx == "")
           {
               System.Threading.Thread.Sleep(1000);

               xm.dat = "";
               xm.url = "";
               return xm;
           }
           else
           {
               if (UrlIsHave.isHave(xxx) == false)
               {
                   nSearch.DebugShow.ClassDebugShow.WriteLine("�õ�URL -> " + xxx);


                   //ϵͳ����
                   if (xxx == "END_ALL_NULL")
                   {
                       xm.dat = "END_ALL_NULL";
                       xm.url = "END_ALL_NULL";
                       return xm;

                   }

                      

                   if (StopIt == true)
                   {
                       xm.dat = "END_ALL_NULL";
                       xm.url = "END_ALL_NULL";
                       return xm;
                   }

                   int t_3 = Environment.TickCount;
                   string xxxDat = GetOneHTML(xxx);
                   int t_4 = Environment.TickCount - t_3;
                   nSearch.DebugShow.ClassDebugShow.WriteLine("����һ����ҳ����ʱ�� -> " + t_4.ToString());


                   if (StopIt == true)
                   {
                       xm.dat = "END_ALL_NULL";
                       xm.url = "END_ALL_NULL";
                       return xm;
                   }

                   if (xxxDat == null)
                   {
                       //�������  ����ѹ��
                       nSearch.UrlMain.ClassSTURL.RePutOneUrl(xxx);
                       xm.dat = "";
                       xm.url = "";
                       return xm;
                   }
                   else
                   {
                       if (xxxDat.Length == 0)
                       {
                           //���ݻ�ȡ
                           //�Ѿ��洢������ѹ���Ѿ��洢�ĶԱȶ���
                           UrlIsHave.SaveOneUrl(xxx);
                           xm.dat = "";
                           xm.url = "";
                           return xm;
                       }
                       else
                       {

                           if (StopIt == true)
                           {
                               xm.dat = "END_ALL_NULL";
                               xm.url = "END_ALL_NULL";
                               return xm;
                           }

                           //�Ѿ��洢������ѹ���Ѿ��洢�ĶԱȶ���
                           UrlIsHave.SaveOneUrl(xxx);

                           if (StopIt == true)
                           {
                               xm.dat = "END_ALL_NULL";
                               xm.url = "END_ALL_NULL";
                               return xm;
                           }

                           //������URL
                           GetAddUrl(xxxDat, xxx);

                           nSearch.DebugShow.ClassDebugShow.WriteLine("�õ�URL���� -> " + xxx);

                           xm.dat = xxxDat;
                           xm.url = xxx;
                           return xm;
                       }
                   }
               }
           }

           xm.dat = "";
           xm.url = "";
           return xm;
       }


       /// <summary>
       /// �õ������з���������URL
       /// </summary>
       /// <param name="data"></param>
       private void GetAddUrl(string HData, string SourceUrl)  //���ݡ�����ǰURL
       {
           //  ��Ϊͬһ��ı�־
           int xgl = SourceUrl.LastIndexOf('/');
           string TMPurl = SourceUrl.Substring(0, xgl + 1);

           string[] FastrD = SourceUrl.Split('/');
           string fastUrl = "http://" + FastrD[2];

           HTMParse.ParseHTML parse = new HTMParse.ParseHTML();
           parse.Source = HData;

           while (!parse.Eof())
           {
               char ch = parse.Parse();
               if (ch == 0)
               {

                   HTMParse.Attribute a = parse.GetTag()["HREF"];

                   if (a != null)
                   {

                       HTMParse.Attribute c = parse.GetTag()["HREF"];

                       string xa1Val = a.Value.ToString().Trim().ToLower();    //�õ�URL      �ж�Ϊ��վ���ڲ���URL

                       if (xa1Val.Length == 0)
                       {
                           int PP_PP = 0;

                           PP_PP = PP_PP + 1;
                       }
                       else
                       {
                           string xa1 = Data2Url(SourceUrl, xa1Val);
                           xa1 = xa1.Trim().ToString();

                           if ((xa1.Length > 7) & (xa1.Length < 160)) //== "http://")
                           {                            
                                   try
                                   {
                                       //Environment.TickCount;
                                       // string New_CANSHU = System.Web.HttpUtility.UrlDecode(xa1, System.Text.Encoding.GetEncoding("GB2312"));
                                       string New_CANSHU = xa1;

                                       nSearch.UrlMain.ClassSTURL.PutOneUrl(New_CANSHU); // nUrlDB.putOneUrl(a);
                                   }
                                   catch
                                   {
                                       Console.Write("->E");
                                   }
                                   // nSearch.DebugShow.ClassDebugShow.WriteLineF(" ѹ�� URL���ݿ� >> " + xa1);
                              
                           }
                       }
                   }

               }
           }


       }


       /// <summary>
       /// URL�е�http���ӽ��з���,�����·��ת��Ϊ����·��
       /// </summary>
       /// <param name="surl"></param>
       /// <param name="nurlt"></param>
       /// <returns></returns>
       private  string Data2Url(string surl, string nurlt)
       {
           surl = surl.Trim();
           nurlt = nurlt.Trim();

           if (nurlt.IndexOf("http://") == 0)
           {
               return nurlt;
           }

           //if (nurlt.IndexOf('#') > -1 | nurlt.IndexOf("javascript:") > -1 | nurlt.IndexOf("mailto:") > -1 | nurlt == null)
           if (nurlt.IndexOf('#') > -1 | nurlt.IndexOf("javascript:") > -1 | nurlt == null)
           {
               return "";  //��Ϊjs  ���ӡ��޷�����
           }

           if (surl.ToLower().IndexOf("http://") != 0 | surl.Length < 11)
           {
               // Դ����url ���ش���  ./http://bt.joyyang.com/thread.php?fid=2
               return "";
           }

           nurlt = nurlt.Trim();
           nurlt = nurlt.Replace("\r", "");
           nurlt = nurlt.Replace("\n", "");
           if (nurlt.Length == 0 | nurlt == "." | nurlt == "/" | nurlt == "./")
           {
               return "";
           }

           try
           {
               Uri baseUri = new Uri(surl);
               Uri absoluteUri = new Uri(baseUri, nurlt);

               return absoluteUri.ToString();   //   http://www.enet.com.cn/enews/test.html     
           }
           catch
           {
               return "";
           }
       }

       /// <summary>
       /// �õ�һ����ҳ����
       /// </summary>
       /// <param name="murl"></param>
       /// <returns>������null �������ⷵ�ؿ� </returns>
       private string GetOneHTML(string murl)
       {

           string codeType ="gb2312";

           try
           {
               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(murl);

               request.Timeout = 60000;

               try
               {
                   //������������δ���HTMLҳ�档����Ҫ���ĵ�Ȼ������HTMLҳ�棬�����ͨ��C#�ṩ��HttpWebRequest��ʵ�֣� 

                   // request = (HttpWebRequest)WebRequest.Create(murl);

                   WebResponse response = request.GetResponse();

                   Stream stream = response.GetResponseStream();
                   string buffer = "", line;
                   //���������Ǿʹ�request����һ��stream������ִ����������֮ǰ������Ҫ��ȷ�����ļ��Ƕ������ļ������ı��ļ�����ͬ���ļ����ʹ���ʽҲ��ͬ������Ĵ���ȷ�����ļ��Ƿ�Ϊ�������ļ��� 
                   //��������ı��ļ������ȴ�stream����һ��StreamReader��Ȼ���ı��ļ�������һ��һ�м��뻺������ 

                   //  response.ContentType.
                   // Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");
                   //��ŵ�ǰ��Ӧ�õ��ַ���
                   string NowCodeSet = "";

                   if (response.ContentLength < 1024 * 128)
                   {
                       //�ж��Ƿ�����ҳ��ʽ
                       if (response.ContentType.ToLower().StartsWith("text/"))
                       {

                           //�Զ���� UTF8
                           if ((response.ContentType.ToLower().IndexOf("utf-8") > -1) | (response.ContentType.ToLower().IndexOf("unicode") > -1))
                           {
                               StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                               NowCodeSet = "utf-8";
                               buffer = "";
                               while ((line = reader.ReadLine()) != null)
                               {
                                   buffer += line + "\r\n";
                               }
                               reader.Close();
                               stream.Close();
                               response.Close();
                               buffer = Str2Str(buffer);
                           }
                           else
                           {
                               //�Զ����GB2312
                               if ((response.ContentType.ToLower().IndexOf("gb2312") > -1) | (response.ContentType.ToLower().IndexOf("gbk") > -1))
                               {
                                   StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("GB2312"));
                                   NowCodeSet = "gb2312";
                                   buffer = "";
                                   while ((line = reader.ReadLine()) != null)
                                   {
                                       buffer += line + "\r\n";
                                   }
                                   reader.Close();
                                   stream.Close();
                                   response.Close();
                               }
                               else
                               {
                                   //�Զ���� ����ʱ����Ĭ�����ý���
                                   if (codeType.ToLower().IndexOf("utf") > -1)
                                   {
                                       StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                                       NowCodeSet = "utf-8";
                                       buffer = "";
                                       while ((line = reader.ReadLine()) != null)
                                       {
                                           buffer += line + "\r\n";
                                       }
                                       reader.Close();
                                       stream.Close();
                                       response.Close();
                                       buffer = Str2Str(buffer);
                                   }
                                   else
                                   {
                                       StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("GB2312"));
                                       NowCodeSet = "gb2312";
                                       buffer = "";
                                       while ((line = reader.ReadLine()) != null)
                                       {
                                           buffer += line + "\r\n";
                                       }
                                       reader.Close();
                                       stream.Close();
                                       response.Close();
                                   }

                               }

                           }

                       }
                       else
                       {
                           //����ҳ��ʽ �򷵻�
                           return null;

                       }


                       ///�ַ���Ϊgb2312  ����Ӧ��Ϊutf-8 
                       if ((buffer.ToLower().IndexOf("gb2312") > -1) & (NowCodeSet == "utf-8"))
                       {
                           HttpWebRequest requestX = (HttpWebRequest)WebRequest.Create(murl);
                           WebResponse responseX = requestX.GetResponse();
                           Stream streamX = responseX.GetResponseStream();
                           StreamReader readerX = new StreamReader(streamX, System.Text.Encoding.GetEncoding("GB2312"));
                           buffer = "";
                           while ((line = readerX.ReadLine()) != null)
                           {
                               buffer += line + "\r\n";
                           }
                           readerX.Close();
                           streamX.Close();
                           responseX.Close();
                       }

                       ///�ַ���Ϊutf-8 ����Ӧ��Ϊ gb2312 
                       if ((buffer.ToLower().IndexOf("utf-8") > -1) & (NowCodeSet == "gb2312"))
                       {
                           HttpWebRequest requestY = (HttpWebRequest)WebRequest.Create(murl);
                           WebResponse responseY = requestY.GetResponse();
                           Stream streamY = responseY.GetResponseStream();
                           StreamReader readerY = new StreamReader(streamY, System.Text.Encoding.UTF8);

                           buffer = "";
                           while ((line = readerY.ReadLine()) != null)
                           {
                               buffer += line + "\r\n";
                           }
                           readerY.Close();
                           streamY.Close();
                           responseY.Close();
                           buffer = Str2Str(buffer);
                       }


                       //   string tmm =clearHTMLDB(buffer);
                       string tmm = buffer;

                       if (tmm.Length > 0)
                       {
                           nSearch.DebugShow.ClassDebugShow.WriteLineF("GUrlData : --> " + murl);
                       }

                       if (tmm.Length > 1024 * 128)
                       {
                           return "";
                       }
                       else
                       {

                           return tmm;  //���ؾ������˵�����
                       }
                   }
                   else
                   {
                       return "";
                   }
               }
               catch
               {
                   request.Abort();

                   nSearch.DebugShow.ClassDebugShow.WriteLineF("Err : --> " + murl);
                   return null;
               }


           }
           catch
           {
               Console.Write("->E");
               return null;
           }
       }

       /// <summary>
       /// ����ת��
       /// </summary>
       /// <param name="data"></param>
       /// <returns></returns>
       private string Str2Str(string data)
       {
           string gb2312info = string.Empty;

           Encoding utf8 = Encoding.UTF8;
           Encoding gb2312 = Encoding.GetEncoding("gb2312");

           // Convert the string into a byte[].
           byte[] unicodeBytes = utf8.GetBytes(data);
           // Perform the conversion from one encoding to the other.
           byte[] asciiBytes = Encoding.Convert(utf8, gb2312, unicodeBytes);

           // Convert the new byte[] into a char[] and then into a string.
           // This is a slightly different approach to converting to illustrate
           // the use of GetCharCount/GetChars.
           char[] asciiChars = new char[gb2312.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
           gb2312.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
           gb2312info = new string(asciiChars);

           return gb2312info;
       }

       /// <summary>
       /// �õ�ϵͳ��״ֵ̬
       /// </summary>
       /// <returns></returns>
       public string GetShow()
       {
           return "000  "+ DateTime.Now.ToString();
       }
    }
}
