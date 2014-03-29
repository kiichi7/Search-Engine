using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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

namespace nSearch.Spider
{

    /// <summary>
    /// 存储数据
    /// </summary>
    public struct UrlHDat
    {
        public string url;

        public string dat;
    
    }


    /// <summary>
    /// 蜘蛛  
    ///  
    ///   1 获取一个URL
    ///   2 获取一个文本形式网页
    ///   3 把失败的URL压回
    ///   4 蜘蛛自己从文件相同读取数据 过滤掉已经获取的数据页面
    /// </summary>
   public  class ClassSpider
    {
       ClassIsHave UrlIsHave = new ClassIsHave();

       Thread[] TT = new Thread[1];

       /// <summary>
       /// 蜘蛛的线程数目
       /// </summary>
       private int ThreadNum = 5;

       /// <summary>
       /// 是否停止
       /// </summary>
       private bool StopIt = false;

       /// <summary>
       /// 统计本次获得的数据条数
       /// </summary>
       private int InNum = 0;

       /// <summary>
       /// 开始的时间
       /// </summary>
       private int InStartTime = 0;

       /// <summary>
       /// 系统初始化
       /// </summary>      
       /// <param name="fspath">文件系统路径</param>
       /// <param name="urlpath">url系统路径</param>
       public void Init(string fspath, string urlpath)
       {
                  //文件系统初始化
           nSearch.FS.ClassFSMD.Init(fspath);
           nSearch.DebugShow.ClassDebugShow.WriteLine("文件系统初始化");


           //URL系统初始化
           nSearch.UrlMain.ClassSTURL.Init(urlpath);
           nSearch.DebugShow.ClassDebugShow.WriteLine("URL系统初始化");


           //得到文件系统中已有的URL
           ArrayList isHaveV = nSearch.FS.ClassFSMD.GetUrlList();
           nSearch.DebugShow.ClassDebugShow.WriteLine("得到文件系统中已有的URL");

           //压入去重判别模块
           UrlIsHave.InitData(isHaveV);
           nSearch.DebugShow.ClassDebugShow.WriteLine("压入去重判别模块");

       
       }


       /// <summary>
       /// 开始运行
       /// </summary>
       /// <param name="TNUM">运行的线程数目</param>

       public void StartRun(int TNUM)
       {


             /// <summary>
       /// 统计本次获得的数据条数
       /// </summary>
        InNum = 0;

       /// <summary>
       /// 开始的时间
       /// </summary>
        InStartTime = Environment.TickCount  ;


           ThreadNum =TNUM;

           nSearch.DebugShow.ClassDebugShow.WriteLine("蜘蛛线程总数 " + ThreadNum.ToString());

           Thread[] TT = new Thread[ThreadNum];

           StopIt = false;

           for (int ii = 0; ii < ThreadNum; ii++)
           {
               
               TT[ii] = new Thread(new ThreadStart(RunOne));
               TT[ii].Start();
               System.Threading.Thread.Sleep(100);
               nSearch.DebugShow.ClassDebugShow.WriteLine("  -->蜘蛛线程 "+ii.ToString());
           }
       
       }

       /// <summary>
       /// 关闭程序
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
                  //文件系统exit
           nSearch.FS.ClassFSMD.SaveExit();

           //URL系统exit
           nSearch.UrlMain.ClassSTURL.SaveExit();
       }

       /// <summary>
       /// 一个线程
       /// </summary>
       private void RunOne()
       {
           //结束出现的次数
           int END_ALL_NULL = 0;


           

           while (!StopIt)
           {




             

               UrlHDat xm = GetOneUrlHDat();


               if (xm.url == "END_ALL_NULL")
               {

                   END_ALL_NULL = END_ALL_NULL + 1;

                   if (END_ALL_NULL > 30)
                   {
                       //系统结束 URL库全部空

                       StopIt = true;

                       nSearch.DebugShow.ClassDebugShow.WriteLine("系统结束 URL库全部空");

                       return;
                   }
                   else
                   {
                       nSearch.DebugShow.ClassDebugShow.WriteLine("延时  URL库全部空");
                       //延时 减缓对网站造成DOS 攻击
                       System.Threading.Thread.Sleep(6000);
                   }
               }

               if (xm.url.Length == 0 | xm.dat.Length == 0)
               {
                   
               }
               else
               {
                   int xTime = Environment.TickCount;

                   //数据获取
                   //数据写入文件系统
                   nSearch.FS.ClassFSMD.PutOneDat(xm.url, xm.dat);

                   int xTime2 = Environment.TickCount - xTime ;

                   nSearch.DebugShow.ClassDebugShow.WriteLine("压入一个URL需要时间 -->>" + xTime2.ToString());

                   //数据量统计
                   InNum = InNum + 1;

                   int xTimeX = Environment.TickCount - InStartTime;

                   //得到速度
                   double xSHow = (double )InNum * 1000.0 / (double )xTimeX;

                   nSearch.DebugShow.ClassDebugShow.WriteLine("速度 -->>" + xSHow.ToString()+" 条/秒");

               }

               //延时 减缓对网站造成DOS 攻击
              // System.Threading.Thread.Sleep(200);

           }
       }


       /// <summary>
       /// 得到一个数据
       /// </summary>
       /// <returns></returns>
       public UrlHDat GetOneUrlHDat()
       {
           UrlHDat xm = new UrlHDat();


           int t_1 = Environment.TickCount;
           string xxx = nSearch.UrlMain.ClassSTURL.GetOneUrl();
           int t_2 = Environment.TickCount - t_1;
           nSearch.DebugShow.ClassDebugShow.WriteLine("请求一个UEL消耗时间 -> " + t_2.ToString());


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
                   nSearch.DebugShow.ClassDebugShow.WriteLine("得到URL -> " + xxx);


                   //系统结束
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
                   nSearch.DebugShow.ClassDebugShow.WriteLine("请求一个网页消耗时间 -> " + t_4.ToString());


                   if (StopIt == true)
                   {
                       xm.dat = "END_ALL_NULL";
                       xm.url = "END_ALL_NULL";
                       return xm;
                   }

                   if (xxxDat == null)
                   {
                       //请求错误  重新压回
                       nSearch.UrlMain.ClassSTURL.RePutOneUrl(xxx);
                       xm.dat = "";
                       xm.url = "";
                       return xm;
                   }
                   else
                   {
                       if (xxxDat.Length == 0)
                       {
                           //数据获取
                           //已经存储的数据压入已经存储的对比队列
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

                           //已经存储的数据压入已经存储的对比队列
                           UrlIsHave.SaveOneUrl(xxx);

                           if (StopIt == true)
                           {
                               xm.dat = "END_ALL_NULL";
                               xm.url = "END_ALL_NULL";
                               return xm;
                           }

                           //解析出URL
                           GetAddUrl(xxxDat, xxx);

                           nSearch.DebugShow.ClassDebugShow.WriteLine("得到URL数据 -> " + xxx);

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
       /// 得到数据中符合条件的URL
       /// </summary>
       /// <param name="data"></param>
       private void GetAddUrl(string HData, string SourceUrl)  //数据　　当前URL
       {
           //  变为同一层的标志
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

                       string xa1Val = a.Value.ToString().Trim().ToLower();    //得到URL      判断为该站点内部的URL

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
                                   // nSearch.DebugShow.ClassDebugShow.WriteLineF(" 压入 URL数据库 >> " + xa1);
                              
                           }
                       }
                   }

               }
           }


       }


       /// <summary>
       /// URL中的http链接进行分析,将相对路径转换为绝对路径
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
               return "";  //此为js  链接　无法处理
           }

           if (surl.ToLower().IndexOf("http://") != 0 | surl.Length < 11)
           {
               // 源不是url 返回错误  ./http://bt.joyyang.com/thread.php?fid=2
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
       /// 得到一个网页数据
       /// </summary>
       /// <param name="murl"></param>
       /// <returns>出错返回null 其它问题返回空 </returns>
       private string GetOneHTML(string murl)
       {

           string codeType ="gb2312";

           try
           {
               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(murl);

               request.Timeout = 60000;

               try
               {
                   //下面来看看如何处理HTML页面。首先要做的当然是下载HTML页面，这可以通过C#提供的HttpWebRequest类实现： 

                   // request = (HttpWebRequest)WebRequest.Create(murl);

                   WebResponse response = request.GetResponse();

                   Stream stream = response.GetResponseStream();
                   string buffer = "", line;
                   //接下来我们就从request创建一个stream流。在执行其他处理之前，我们要先确定该文件是二进制文件还是文本文件，不同的文件类型处理方式也不同。下面的代码确定该文件是否为二进制文件。 
                   //。如果是文本文件，首先从stream创建一个StreamReader，然后将文本文件的内容一行一行加入缓冲区。 

                   //  response.ContentType.
                   // Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");
                   //存放当前的应用的字符集
                   string NowCodeSet = "";

                   if (response.ContentLength < 1024 * 128)
                   {
                       //判断是否是网页格式
                       if (response.ContentType.ToLower().StartsWith("text/"))
                       {

                           //自动检测 UTF8
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
                               //自动检测GB2312
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
                                   //自动检测 不到时按照默认设置进行
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
                           //非网页格式 则返回
                           return null;

                       }


                       ///字符集为gb2312  而刚应用为utf-8 
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

                       ///字符集为utf-8 而刚应用为 gb2312 
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

                           return tmm;  //返回经过过滤得数据
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
       /// 编码转换
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
       /// 得到系统的状态值
       /// </summary>
       /// <returns></returns>
       public string GetShow()
       {
           return "000  "+ DateTime.Now.ToString();
       }
    }
}
