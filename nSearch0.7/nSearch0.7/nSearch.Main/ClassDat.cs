using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search.Highlight;

using System.Net;
using System.Net.Sockets;

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

namespace nSearch.Main
{
    class ServerXLClass
    {
        /// <summary>
        /// �������
        /// </summary>
        NewNxuEncoding.CNewNxuEncoding newz_code = new NewNxuEncoding.CNewNxuEncoding();

        Encoding gbx = System.Text.Encoding.GetEncoding("gb2312");

        Encoding utf8 = System.Text.Encoding.UTF8 ;// .GetEncoding("utf8");


        Socket clientSocket;

        System.Text.Encoding gb = System.Text.Encoding.GetEncoding("gb2312");

        public ServerXLClass(Socket socket )
        {

            this.clientSocket = socket;

        

        }

        /// <summary>
        /// ��¼�������
        /// </summary>
        string[] nRSD = new string[1];
        int[] nRSDX = new int[1];


        /// <summary>
        /// ��¼��ǰ�������̺߳�
        /// </summary>
        int iTT = -1;

        /// <summary>
        /// ��¼�ڵ���Ϣ
        /// </summary>
        ArrayList VBNM;

        /// <summary>
        /// ��¼���������
        /// </summary>
        string urlxcf;


        /// <summary>
        /// DATA
        /// </summary>
              string A_WD ;
        /// <summary>
        /// ��ʼ���
        /// </summary>
                int A_WS;
        /// <summary>
        /// ����
        /// </summary>
        int A_WL; 


        /// <summary>
        /// ��󷵻ص�����
        /// </summary>
        string ABACK = "";






        public void one()
        {
            int xone = iTT;

            //��iTT��Ϊ��д״̬
            iTT = -1;

          

            if (urlxcf == "")
            {
                return;

                string dat = "URL����";
                byte[] x = gb.GetBytes(dat);
                string sBuffer = "";
                sBuffer = "HTTP/1.1 200 OK" + "\r\n";
                sBuffer = sBuffer + "Server: KC\r\n";
                sBuffer = sBuffer + "Content-Type: " + "text/html" + "\r\n";
                sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
                sBuffer = sBuffer + "Content-Length: " + x.Length.ToString() + "\r\n\r\n";
              //  return sBuffer + dat + "\r\n\r\n";
            }
            else
            {

              //  string UrlXXNow = "http://" + VBNM[xone].ToString() + "/" + newz_code.CN2CODE( "s?wd=" +urlxcf + "&ws=0&wl=0&bb="+Environment.TickCount.ToString());

                try
                {
                   // WebClient wc = new WebClient();

                    nSearch.DebugShow.ClassDebugShow.WriteLine("  --> ����[ " + urlxcf + " ] ");

                 //   byte[] x = wc.DownloadData(UrlXXNow);


              

                    string aapp = VBNM[xone].ToString();

                    int xxp = aapp.IndexOf(':');

                    string aapp1 = aapp.Substring(0, xxp);
                    string aapp2 = aapp.Substring(xxp + 1, aapp.Length - xxp - 1);
                    
                    ///////////////////////////////////////////////////////////

                    GetOne4Data("/" + newz_code.CN2CODE("s?wd=" + urlxcf + "&ws=0&wl=0&bb=" + Environment.TickCount.ToString()), aapp1, Int32.Parse(aapp2), xone);


                    return;
                    /*
                         if (buffer.Length == 0)
                         {
                             nSearch.DebugShow.ClassDebugShow.WriteLine("  --> ����[ " + urlxcf + " ] ERR ");
                             return;
                         }
                         else
                         {
                             //string d1 = "\r\n\r\n";


                             nRSDX[xone] = 1;
                             nRSD[xone] = buffer;


                             nSearch.DebugShow.ClassDebugShow.WriteLine("  --> ����[ " + urlxcf + " ] OK ");
                             return;
                         }
                    */
                    /*
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlXXNow);

                                request.Timeout = 6000;

                                WebResponse response = request.GetResponse();

                                Stream stream = response.GetResponseStream();

                                StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("GB2312"));

                                buffer = reader.ReadToEnd();

                                try
                                {
                                    reader.Close();
                                  
                                }
                                catch
                                { }


                                try
                                {
                                  
                                    stream.Close();
                                    
                                }
                                catch
                                { }

                                try
                                {
                                   
                                    response.Close();
                                  
                                }
                                catch
                                { }

                                try
                                {
                                   
                                    request.Abort();
                                }
                                catch
                                { }



                                if (buffer.Length == 0)
                                {
                                    nSearch.DebugShow.ClassDebugShow.WriteLine("  --> ����[ " + urlxcf + " ] ERR ");
                                    return;
                                }
                                else
                                {
                                    nRSD[xone] = buffer;
                                    nSearch.DebugShow.ClassDebugShow.WriteLine("  --> ����[ " + urlxcf + " ] OK ");
                                    return;
                                }

                            byte[] x = gbx.GetBytes(buffer);
                    //////////////////////////////////////////////////////////



                    nSearch.DebugShow.ClassDebugShow.WriteLine("  --> ����[ " + urlxcf + " ] OK ");

                 //   wc.Dispose();

                    string str = gb.GetString(x);

                    nRSD[xone] = str;

                    return;

                    string sBuffer = "";
                    sBuffer = "HTTP/1.1 200 OK" + "\r\n";
                    sBuffer = sBuffer + "Server: KC\r\n";
                    sBuffer = sBuffer + "Content-Type: " + "text/html" + "\r\n";
                    sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
                    sBuffer = sBuffer + "Content-Length: " + x.Length.ToString() + "\r\n\r\n";

                    */
                  //  return sBuffer + str + "\r\n\r\n";
                }
                catch
                {
                    return;

                    /*
                    string dat = "������æ�����Ժ����ԣ�";
                    byte[] x = gb.GetBytes(dat);
                    string sBuffer = "";
                    sBuffer = "HTTP/1.1 200 OK" + "\r\n";
                    sBuffer = sBuffer + "Server: KC\r\n";
                    sBuffer = sBuffer + "Content-Type: " + "text/html" + "\r\n";
                    sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
                    sBuffer = sBuffer + "Content-Length: " + x.Length.ToString() + "\r\n\r\n";
                    */

                   // return sBuffer + dat + "\r\n\r\n";
                }
            }
        }


        public void Run()
        {


            int X = 0;

            string time = DateTime.Now.Millisecond.ToString();

            string clientmessage = " ";
            //������Կͻ��˵�HTTP�����ַ���
            string URL = " ";
            Byte[] read = new byte[2048];

            try
            {
                while (true)
                {
                    X = X + 1;

                    //��Ž�������ַ������Ϣ
                    int bytes = clientSocket.Receive(read, 2048, 0);

                    if (bytes == 0)
                    {
                        clientSocket.Close();
                        return;
                    }


                    nSearch.DebugShow.ClassDebugShow.WriteLineF("��ʼ��Ӧһ��IEԭʼ����    RT���߳� " + time + " ���������" + X);

                    string str;
                    nRSD = new string[1];
                    nRSDX = new int[1];

                    string str1 = utf8.GetString(read, 0, bytes);
                     string    str2 = gb.GetString(read, 0, bytes);

                     if (str1.IndexOf("Firefox") == -1)
                     {

                         str = str2;
                     }
                     else
                     {
                         str = str1;
                     }


                    //�õ�����
                    string newurl = GetOneUrl(str);

                    //�趨����
                    urlxcf = newurl;


                    A_WD = urlxcf;

                    nSearch.DebugShow.ClassDebugShow.WriteLineF(time + "> " + "��ʼ��Ӧһ��IEԭʼ����    RT���߳� " + " S --> ���� " + A_WD);
                   

                    //�õ��ڵ��б�
                     VBNM = (ArrayList)ClassST.vbn.Clone();
             
                    int  VBNM_Count =  VBNM.Count;                     
                    //�������

                    nRSDX = new int[VBNM_Count];

                     nRSD = new string[VBNM_Count];
 
                     iTT = -1;


                    Thread[] TT = new Thread[VBNM_Count];

                    for (int iii = 0; iii < VBNM_Count; iii++)
                    {                
                        if (iTT != -1)
                        {  //�����־λû�б��ÿ� ��ȴ�һ��
                            System.Threading.Thread.Sleep(10);
                        }

                        iTT=iii;
                        //����һ������
                        TT[iii] =  new Thread(new ThreadStart(one));
                        TT[iii].IsBackground = true;

                        TT[iii].Start();
                      
                        nRSD[iii] = "";
                        nRSDX[iii] = 0;
                    }

                    //�ȴ��ڵ㴦�������Ϣ���ݷ���
                 //   System.Threading.Thread.Sleep(1000);




                    //�ܹ����õ�ѭ������ ��������ʱ
                    for (int iiiLoop = 0; iiiLoop < 80; iiiLoop++)
                    {
                        try
                        {
                            for (int iii = 0; iii < VBNM_Count; iii++)
                            {
                                if (nRSDX[iii] == 0)
                                {
                                    goto XVSS;
                                }
                            }

                            //�Ѿ��õ�ȫ������
                            goto XVSS1;
                        }
                        catch { }


                    XVSS: ;

                        System.Threading.Thread.Sleep(100);
                    }

                    //���ݴ���   ĳ���ڵ㳬ʱ 
                    //���ؽ��

                    ABACK = GetHttp("�ڵ����");

                    goto  END_BACK ;



                XVSS1:

                    string A2 = GetALL_BACKDAT();
                    ABACK = GetHttp(A2);


                END_BACK: ;


                    nSearch.DebugShow.ClassDebugShow.WriteLine(  time+"> "+ A_WD + " ��IE��������" );

                    byte[] message = gbx.GetBytes(ABACK);

                  clientSocket.Send(message);


                    for (int iii = 0; iii < VBNM.Count; iii++)
                    {
                        try
                        {
                          TT[iii].Abort();
                           // TT[iii].Suspend();
                          //  TT[iii].Join();
                            

                        }
                        catch
                        { }
                    }



                    clientSocket.Shutdown(SocketShutdown.Both);
                   clientSocket.Close();
                   

                 //   nSearch.DebugShow.ClassDebugShow.WriteLine(" S --> ���� OK" + A_WD);

                    // System.Threading.Thread.Sleep(500);


                    nSearch.DebugShow.ClassDebugShow.WriteLine(time + "> " + A_WD + " ��IE�����������");

  

                 //   nSearch.DebugShow.ClassDebugShow.WriteLineF(" ���߳� " + time + " ���������" + X);


                    return;
                }
            }
            catch
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF("SERR �߳� " + time + "�˳� ���������" + X);


                return;
                try
                { clientSocket.Close(); }
                catch
                { }
            }

        }

        /// <summary>
        /// ���ݵõ��������жϳ� ��Ҫ�����Ǹ�������
        /// </summary>
        /// <param name="dat"></param>
        /// <returns>������ȷ��url</returns>
        private string GetOneUrl(string dat)
        {

            try
            {

                int x = dat.IndexOf("\r");

                string a_1 = dat.Substring(0, x);

                string[] a_2 = a_1.Split(' ');

                string a_3 = a_2[1].Substring(1, a_2[1].Length - 1);

                a_3 = System.Web.HttpUtility.UrlDecode(a_3, System.Text.Encoding.GetEncoding("GB2312"));


                //ȡ��������ж� s?wd=<�ؼ���>����</�ؼ���>&czd=���1  127.0.0.1:81/s?wd=����


                //http://127.0.0.1:8080/s?wd=�й�&ws=0&wl=0

                int i_w = a_3.IndexOf("s?");

                if (i_w == -1)
                {
                    return "�޷��ҵ����";
                }

                // wd=KJ&ws=0&wl=0
                string NewX = a_3.Substring(2, a_3.Length - 2);

                string[] U_P = NewX.Split('&');

                string[] wd = U_P[0].Split('=');
                string[] ws = U_P[1].Split('=');
                string[] wl = U_P[1].Split('=');

                string A_WD = wd[1];
                 A_WS = Int32.Parse(ws[1]);
                 A_WL = Int32.Parse(wl[1]);

                 if (A_WS < 0)
                 {
                     A_WS = 0;


                 }

                 if (A_WL < 1)
                 {
                     A_WL = 10;
                 }

                return A_WD;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        private string GetALL_BACKDAT()
        {

            ClassSeachALL nClass = new ClassSeachALL();

            //��������
            RSK ALL = new RSK();

            ArrayList cNew = new ArrayList();

            //���������ڵ�õ������� �ϲ���� ������������  ���������ҳ��  ����һ�����
            foreach (string aabb in nRSD)
            {
                RSK cc = nClass.RTSTR2RSK(aabb);

                //���ݺϲ���ALL

                ALL.ALLNum = ALL.ALLNum + cc.ALLNum;

                foreach (OneRs ncb in cc.rs)
                {
                    cNew.Add(ncb);
                }

            }

            ALL.ANum = 0;
            ALL.BNum = 0;
            ALL.rs = (ArrayList)cNew.Clone();
            //�õ����ݵĸ���

            //�õ����������� 
            int all_a_num = ALL.ALLNum;

            //��ʼ
            int all_a_ws = A_WS;
            //����
            int ALL_a_wl = A_WL;


            //���HTML��ʽ�����ݿ�    �滻ģ���е����ݿռ�

            int CL = A_WS + A_WL;

            if (CL > ALL.rs.Count)
            {
                CL = ALL.rs.Count;
            }
            // "/s?wd=TYPE|title|body|type1|type2&ws=0&wl=10";
            string[] FEFE = A_WD.Split('|');



            QueryParser parser = new QueryParser("E", ClassST.OneAnalyzer);
            Query query = parser.Parse(FEFE[1] + " " + FEFE[2]);

            //Highlighter highlighter = new Highlighter(new QueryScorer(query));

            //  Highlighter highlighter = new Highlighter();
            Highlighter highlighter = new Highlighter(new SimpleHTMLFormatter("<font color=\"red\">", "</font>"), new QueryScorer(query));
            highlighter.TextFragmenter = new SimpleFragmenter(20);



            StringBuilder GHX = new StringBuilder();

            for (int p = A_WS; p < CL; p++)
            {
                OneRs ncb = (OneRs)ALL.rs[p];
                StringBuilder Tmp = new StringBuilder();

                string N_D = newz_code.CODE2CN(ncb.D);
                string N_Url = newz_code.CODE2CN(ncb.url);
                string N_A = newz_code.CODE2CN(ncb.A);

                TokenStream tokenStream = ClassST.OneAnalyzer.TokenStream("D", new System.IO.StringReader(N_D));

                System.String result = highlighter.GetBestFragments(tokenStream, N_D, 100, "...");

                // string   result = N_D;

                Tmp.Append("  <tr>\r\n");
                Tmp.Append("   <td height=\"66\" scope=\"col\"><table width=\"95%\" height=\"78\" border=\"0\" cellpadding=\"0\" cellspacing=\"1\">\r\n");
                Tmp.Append("    <tr>\r\n");
                Tmp.Append("        <td scope=\"col\"><div align=\"left\"><a href=\"" + N_Url + "\" target=\"_blank\"><span class=\"STYLE17\">" + N_A + "</span></a></div></td>\r\n");
                Tmp.Append("    </tr>\r\n");
                Tmp.Append("    <tr>\r\n");
                Tmp.Append("        <td><span class=\"STYLE18\">");
                Tmp.Append(result);
                Tmp.Append("</span></td>\r\n");
                Tmp.Append("    </tr>\r\n");
                Tmp.Append("    <tr>\r\n");
                Tmp.Append("        <td><a href=\"" + N_Url + "\" target=\"_blank\"><span class=\"STYLE19\">" + N_Url + "</span></a></td>\r\n");
                Tmp.Append("    </tr>\r\n");
                Tmp.Append("    </table><p>&nbsp;</p></td>\r\n");
                // Tmp.Append("  <p> </p>\r\n");
                Tmp.Append("  </tr>\r\n");

                GHX.Append(Tmp.ToString());
            }


            string NewTable = "<table width=\"95%\" height=\"152\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n";
            NewTable = NewTable + GHX.ToString();
            NewTable = NewTable + "</table>\r\n";


            //������
            string WebHtml = ClassST.ModelHTM.Replace("<%HTTP://ZD4004.BLOG.163.COM%>", NewTable);

            //   // "/s?wd=TYPE|title|body|type1|type2&ws=0&wl=10";
            //string[] FEFE = A_WD.Split('|');

            //ѡ����  
            WebHtml = WebHtml.Replace("<%http://blog.163.com/zd4004/%>", ClassST.GetMainTypeListHtm(FEFE[0], FEFE[2]));

            //������
            WebHtml = WebHtml.Replace("<%HTTP://BLOG.163.COM/ZD4004_1%>", ClassST.GetBoxListDat1(FEFE[0], FEFE[3], FEFE[4]));
            WebHtml = WebHtml.Replace("<%HTTP://BLOG.163.COM/ZD4004_2%>", ClassST.GetBoxListDat2(FEFE[0], FEFE[3], FEFE[4]));

            //ҳ����ʾ
            WebHtml = WebHtml.Replace("<%HTTP://BLOG.163.COM/ZD4004_URL%>", ClassST.GetPageNumHTML("/s?wd=" + A_WD + "&", all_a_num, all_a_ws, ALL_a_wl));

            //���������Ŀ 

            string KPP = "�������� ";


            if (all_a_ws + ALL_a_wl < all_a_num)
            {
                int ssrr = all_a_ws + ALL_a_wl;
                int all_a_wsn = all_a_ws + 1;
                KPP = KPP + "" + all_a_num.ToString() + " ����� ���� " + all_a_wsn.ToString() + " - " + ssrr.ToString() + " ��";
            }
            else
            {
                int ssrr = all_a_num;
                int all_a_wsn = all_a_ws + 1;
                KPP = KPP + "" + all_a_num.ToString() + " ����� ���� " + all_a_wsn.ToString() + " - " + ssrr.ToString() + " ��";
            }

            WebHtml = WebHtml.Replace("<%HTTP://BLOG.163.COM/ZD4004_NUM%>", KPP);

            return WebHtml;
        }




        /// <summary>
        /// ����ת��ΪHTTP
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private string GetHttp(string dat)
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



        /// <summary>
        /// �ӽڵ�õ�һ������
        /// </summary>
        /// <param name="url">����</param>
        /// <param name="ips">IP 127.0.0.1</param>
        /// <param name="port">�˿�</param>
        /// <param name="iRsd">�ڽ�������е����</param>
        /// <returns>����</returns>
        private void GetOne4Data(string url, string ips, int port,int iRsd)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ips);
                //��ϳ�Զ���ս��
                IPEndPoint hostEP = new IPEndPoint(ip, port);
                //����Socket ʵ��
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {

                    socket.Connect(hostEP);
                }
                catch (Exception se)
                {
                    nSearch.DebugShow.ClassDebugShow.WriteLineF("���Ӵ���" + se.Message);
                }

                string sendStr = "GET " + url + " HTTP/1.1\r\nHost: " + ips + "\r\nConnection: Close\r\n\r\n";

                byte[] bytesSendStr = new byte[1024];
                //�����������ַ���ת�����ֽ�byte����
                bytesSendStr = System.Text.Encoding.ASCII.GetBytes(sendStr); // gbx.GetBytes(sendStr);
                try
                {

                    socket.Send(bytesSendStr, bytesSendStr.Length, 0);//��������������
                }
                catch (Exception ce)
                {
                    nSearch.DebugShow.ClassDebugShow.WriteLineF("���ʹ���:" + ce.Message);
                }
                //�����ֽ����飬һ�ν������ݵĳ���Ϊ1024�ֽ�
                byte[] recvBytes = new byte[1024];

                //����ʵ�ʽ������ݵ��ֽ���
                int bytes = 0;
                //ѭ����ȡ��ֱ����������������

                StringBuilder tmpStr = new StringBuilder();

                while (true)
                {
                    bytes = socket.Receive(recvBytes, recvBytes.Length, 0);
                    //��ȡ��ɺ��˳�ѭ��
                    if (bytes <= 0)
                        break;


                    tmpStr.Append(System.Text.Encoding.ASCII.GetString(recvBytes, 0, bytes));


                    //����ȡ���ֽ���ת��Ϊ�ַ���
                }


                //������ȡ���ַ���ת��Ϊ�ֽ�����

                //����Socket
                socket.Shutdown(SocketShutdown.Both);
                //�ر�Socket
                socket.Close();



                nRSDX[iRsd] = 1;
                nRSD[iRsd] = tmpStr.ToString();
                return;
            }
            catch
            {


                nRSDX[iRsd] = 0;
                nRSD[iRsd] = "";
                return;



                
            }
        }



    }
}
