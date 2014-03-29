using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.SearchOne
{
   public  class StartClass
    {

       public string Set_IP = "";
       public int Set_Port = 0;


       Thread TT;

       /// <summary>
       /// ��ʼ
       /// </summary>
       public void StartRun()
       {
           TT = new Thread(new ThreadStart(StratV));
           TT.IsBackground = true;
           TT.Start();

       }

       /// <summary>
       /// ����
       /// </summary>
       public void StopRun()
       {

           nSearch.DebugShow.ClassDebugShow.WriteLineF(" ֹͣ ");

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
               nSearch.DebugShow.ClassDebugShow.WriteLineF("������ķ���������˿ںŸ�ʽ����ȷ�����������룡");
               return;
           }
       XF:

           //����˿ں�
           TcpListener tcplistener = new TcpListener(iep);

           nSearch.DebugShow.ClassDebugShow.WriteLineF("ALL-NUM-W");

           tcplistener.Start();

           //��¼�ܵķ��ʴ���
           int v = 0;

           //�����˿ں�
           while (true)
           {
               try
               {
                   Socket socket = tcplistener.AcceptSocket();
                   //����ȡ���ͺͽ������ݵ�Scoketʵ��
                   ServerXLClass CWebServer_one = new ServerXLClass(socket);

                   //��ʵ����
                   Thread T = new Thread(new ThreadStart(CWebServer_one.Run));
                
                   T.IsBackground = true;
                   //�����߳�
                   T.Start();
                   //�����߳�

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
