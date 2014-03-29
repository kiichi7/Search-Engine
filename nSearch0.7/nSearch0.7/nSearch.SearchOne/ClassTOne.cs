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


    class ServerXLClass
    {




       


        /// <summary>
        /// 编码解码
        /// </summary>
        NewNxuEncoding.CNewNxuEncoding newz_code = new NewNxuEncoding.CNewNxuEncoding();

        Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");

        Encoding utf8 = System.Text.Encoding.UTF8;// .GetEncoding("utf8");

        Socket clientSocket;

        System.Text.Encoding gb = System.Text.Encoding.GetEncoding("gb2312");

        public ServerXLClass(Socket socket)
        {

            this.clientSocket = socket;
        }

        public string one(string dat)
        {
                   
                byte[] x = gb.GetBytes(dat);
                string sBuffer = "";
                sBuffer = "HTTP/1.1 200 OK" + "\r\n";
                sBuffer = sBuffer + "Server: KC\r\n";
                sBuffer = sBuffer + "Content-Type: " + "text/html" + "\r\n";
                sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
                sBuffer = sBuffer + "Content-Length: " + x.Length.ToString() + "\r\n\r\n";
                return sBuffer + dat + "\r\n\r\n";
            
        }


        public void Run()
        {
            int X = 0;

            string time = DateTime.Now.Millisecond.ToString();

            string clientmessage = " ";
            //存放来自客户端的HTTP请求字符串
            string URL = " ";
            Byte[] read = new byte[2048];

            try
            {
                while (true)
                {
                    X = X + 1;

                    //存放解析出地址请求信息
                    int bytes = clientSocket.Receive(read, 2048, 0);

                    if (bytes == 0)
                    {
                        clientSocket.Close();
                        return;
                    }

                    string str = System.Text.Encoding.ASCII.GetString(read, 0, bytes);
                  


                    string newurl = GetOneUrl(str);



                 //   string newdat = one(newurl);



                    byte[] message = System.Text.Encoding.ASCII.GetBytes(newurl);


                    clientSocket.Send(message);


                    nSearch.DebugShow.ClassDebugShow.WriteLineF("发送完成 本线程 " + time + " 请求次数：" + X);

                   // nSearch.DebugShow.ClassDebugShow.WriteLine("  --> 请求 [ Send ok]");

                    //  System.Threading.Thread.Sleep(200);
                    // 



                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                    nSearch.DebugShow.ClassDebugShow.WriteLineF("关闭 本线程 " + time + " 请求次数：" + X);
                    return;
                }
            }
            catch
            {

                return;

                nSearch.DebugShow.ClassDebugShow.WriteLineF(" ERR线程 " + time + "退出 请求次数：" + X);

                try
                { clientSocket.Close(); }
                catch
                { }
            }

        }

        /// <summary>
        /// 得到一个串行化好的搜索结果
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private string GetOneUrl(string dat)
        {

            int x = dat.IndexOf("\r");

            string a_1 = dat.Substring(0, x);

            string[] a_2 = a_1.Split(' ');

            string a_3 = a_2[1].Substring(1, a_2[1].Length - 1);

            a_3 = newz_code.CODE2CN(a_3);

            a_3 = System.Web.HttpUtility.UrlDecode(a_3, System.Text.Encoding.GetEncoding("GB2312"));



            //http://127.0.0.1:8080/s?wd=中国&ws=0&wl=0

            int i_w = a_3.IndexOf("s?");

            if (i_w == -1)
            {
                return "无法找到结果";
            }

            // wd=KJ&ws=0&wl=0
            string NewX = a_3.Substring(2, a_3.Length - 2);


            nSearch.DebugShow.ClassDebugShow.WriteLine( NewX+ "  --> 得到请求  ");

            string[] U_P = NewX.Split('&');

            string[] wd = U_P[0].Split('=');
            string[] ws = U_P[1].Split('=');
            string[] wl = U_P[1].Split('=');

            string A_WD = wd[1];
            int A_WS = Int32.Parse(ws[1]);
            int A_WL = Int32.Parse(wl[1]);


        //    nSearch.DebugShow.ClassDebugShow.WriteLine("  --> 请求[ " + A_WD + " ] ");

            ClassSearch nSearchTmp = (ClassSearch)nSearch.SearchOne.ClassST.mSearch;

            nSearch.SearchOne.RSK xRs = nSearchTmp.GetRS(A_WD, A_WS, A_WL);

            string NewD = nSearchTmp.RTRSK2STR(xRs);

            nSearch.DebugShow.ClassDebugShow.WriteLine(NewX + "  --> 得到数据  ");


            return NewD;


        }




    }


}
