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

namespace nSearch.Main
{
    class ClassMainS
    {
        Thread T;

        public   int Set_Port = 41;
      
        public string Set_IP = "";



        Thread TT;

        /// <summary>
        /// ��ʼ
        /// </summary>
        public void StartRun()
        {
            TT = new Thread(new ThreadStart(Start));
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

        public void Start()
        { 
           

          


            nSearch.DebugShow.ClassDebugShow.WriteLineF("�˿�: " + Set_Port.ToString());
           // nSearch.DebugShow.ClassDebugShow.WriteLineF("�ļ�·��: " + pathc);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("��IP: " + Set_IP);

            /// <summary>
            /// �ﶨ�ĵ�ַ ���� http://
            /// </summary>
          
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

                    nSearch.DebugShow.ClassDebugShow.WriteLineF("ALL-NUM-Web-GATE:"+ v.ToString() +"   Time:" + DateTime.Now.ToString() );
                }
                catch
                {

                    nSearch.DebugShow.ClassDebugShow.WriteLine("ERR ��ʼ�µĹ��� ");
                    goto XF;
                }
            }

        }



 
    }
}
