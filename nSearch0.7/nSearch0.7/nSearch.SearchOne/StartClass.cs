using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.SearchOne
{
   public  class StartClass
    {

       public string Set_IP = "";
       public int Set_Port = 0;


       Thread TT;

       /// <summary>
       /// 开始
       /// </summary>
       public void StartRun()
       {
           TT = new Thread(new ThreadStart(StratV));
           TT.IsBackground = true;
           TT.Start();

       }

       /// <summary>
       /// 结束
       /// </summary>
       public void StopRun()
       {

           nSearch.DebugShow.ClassDebugShow.WriteLineF(" 停止 ");

           try
           {
               TT.Suspend();
               TT.Abort();
           }
           catch
           { }
       
       }

       private  void StratV()
       {

           IPAddress myIP;
           IPEndPoint iep;
           try
           {
               myIP = IPAddress.Parse(Set_IP);
               iep = new IPEndPoint(myIP, Set_Port);
           }
           catch
           {
               nSearch.DebugShow.ClassDebugShow.WriteLineF("你输入的服务器名或端口号格式不正确，请重新输入！");
               return;
           }
       XF:

           //定义端口号
           TcpListener tcplistener = new TcpListener(iep);

           nSearch.DebugShow.ClassDebugShow.WriteLineF("ALL-NUM-W");

           tcplistener.Start();

           //记录总的访问次数
           int v = 0;

           //侦听端口号
           while (true)
           {
               try
               {
                   Socket socket = tcplistener.AcceptSocket();
                   //并获取传送和接收数据的Scoket实例
                   ServerXLClass CWebServer_one = new ServerXLClass(socket);

                   //类实例化
                   Thread T = new Thread(new ThreadStart(CWebServer_one.Run));
                
                   T.IsBackground = true;
                   //创建线程
                   T.Start();
                   //启动线程

                   v = v + 1;

                   nSearch.DebugShow.ClassDebugShow.WriteLineF("Start ALL-NUM-Web-GATE:" + v.ToString() + "   Time:" + DateTime.Now.ToString());
               }
               catch
               {
                   goto XF;
               }
           }
       
       
       
       
       
       }


    }
}
