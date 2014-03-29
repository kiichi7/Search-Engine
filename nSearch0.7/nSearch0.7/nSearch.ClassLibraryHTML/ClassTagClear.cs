using System;
using System.Collections.Generic;
using System.Text;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.ClassLibraryHTML
{
    /// <summary>
    /// ����HTML����
    /// </summary>
    class ClassTagClear
    {


        /// <summary>
        /// ���ĳ�� ֵ 
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private string clearDat(string dat, string valc)
        {
            string du = dat;
            //href="http://www.baidu.com/"
            dat = dat.Trim();
            if (dat.IndexOf(valc) != 0)
            {
                return dat;
            }

            return "";
           

            int c1 = dat.IndexOf("\"");

            if (c1 + 1 >= dat.Length)
            {
                goto HGH;
            }

            int c2 = dat.LastIndexOf("\"");

            if ((c1 <= -1) | (c2 <= -1) | (c1 >= c2))
            {
                goto HGH;
            }
            // string s1 = dat.Substring(0, c1 + 1);
            // string s2 = dat.Substring(c2, dat.Length - c2);

            string s1 = dat.Substring(0, c1);
            string s2 = dat.Substring(c2 + 1, dat.Length - c2 - 1);

            //  dat = s1 + dat.Substring(c1 + 1, c2 - c1 - 1) + s2 + "\r\n" + s1 + "\r\n" + s2;
            dat = s1 + s2;


        HGH:
            return dat;
        }

        /// <summary>
        /// ������ԭʼ�ģ���� �õ�������ԭʼ��Url  ��ԭ������
        /// </summary>
        private string SourceURL = "";

        /// <summary>
        /// ת��ĳ�� ֵ 
        /// </summary>
        /// <param name="dat">����</param>
        /// <param name="valc">����</param>
        /// <returns></returns>
        private string RTclearDat(string dat, string valc)
        {
            // // href=""��href="">http://www.chinahr.com/</

            string du = dat;
            //href="http://www.baidu.com/"
            dat = dat.Trim();
            if (dat.IndexOf(valc) != 0 | dat.IndexOf("=") == -1)
            {
                return dat;
            }

            // return "href=''";
            //  return "k";

            int c1 = dat.IndexOf("\"");

            if (c1 == -1)
            {
                c1 = dat.IndexOf("'");
            }

            if (c1 + 1 >= dat.Length)
            {
                goto HGH;
            }

            int c2 = dat.LastIndexOf("\"");


            if (c2 == -1)
            {
                c2 = dat.IndexOf("'");
            }

            //��ʾ�����Բ��ǰ�����""�е�
            if (c1 == -1)
            {
                //  c1 = dat.IndexOf("=")+1;
                dat = dat.Replace(valc + "=", valc + "=\"");
                c1 = dat.IndexOf("\"");
            }

            if (c2 == -1)
            {
                dat = dat + "\"";
                c2 = dat.LastIndexOf("\"");
            }


            if ((c1 <= -1) | (c2 <= -1) | (c1 >= c2))
            {
                goto HGH;
            }
            // string s1 = dat.Substring(0, c1 + 1);
            // string s2 = dat.Substring(c2, dat.Length - c2);

            string s1 = dat.Substring(0, c1 - 1);



            string s2 = dat.Substring(c2 + 1, dat.Length - c2 - 1);

            string s3 = dat.Substring(c1 + 1, c2 - c1 - 1);  //http://www.baidu.com/

            //�Ѱ��Ʒ�ģ�����Ϊ����url
            string s3_2 = Data2Url(SourceURL, s3);
            //// href=""��href="">http://www.chinahr.com/</
            dat = s1 + s2 + ">" + s3_2 + "<X";

            if (s1 == null | s2 == null | s3_2.Length == 0)
            {
                return du;
            }

            if (s3_2.IndexOf("http://") != 0)
            {
                return du;
            }

            if (s3_2.IndexOf('<') > 0 | s3_2.IndexOf('>') > 0)
            {
                return du;
            }

            return dat;

        HGH:
            return du;
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
        /// ���ĳ����ǩ������ֵ
        /// </summary>
        /// <param name="tagn"></param>
        /// <param name="valc"></param>
        /// <returns></returns>
        private string clearOneTag(string data, string tagn, string valc)
        {
            int stN = 0;

        XDX:
            if (stN >= data.Length)
            {
                goto XUI;
            }

            int a1 = data.IndexOf("<" + tagn, stN);  //123456  23   56

            if (a1 + tagn.Length >= data.Length)
            {
                goto XUI;
            }

            int a2 = data.IndexOf(">", a1 + tagn.Length + 1);



            if ((a1 > -1) & (a2 > -1))
            {
                string s1 = data.Substring(0, a1 + tagn.Length + 1);
                string s2 = data.Substring(a2, data.Length - a2);
                string ss = data.Substring(a1 + tagn.Length + 1, a2 - a1 - tagn.Length - 1);// href="http://www.baidu.com/"

                ss = ss.Trim();

                if (ss.IndexOf(" ") == -1)
                {
                    ss = " " + clearDat(ss, valc);
                }
                else
                {
                    string newX = "";
                    string[] mcc = ss.Split(' ');
                    foreach (string a in mcc)
                    {
                        if (a.Length > 0)
                        {
                            newX = newX + " " + clearDat(a, valc);
                        }
                    }
                    ss = newX;
                }

                stN = s1.Length + ss.Length;
                data = s1 + ss + s2;

                goto XDX;
            }

        XUI: ;
            return data;
        }



        // <a href="http://www.chinahr.com/">��<a href="></">


        /// <summary>
        /// �滻ĳ����ǩ������ֵ��Ϊ����ʶ������ݡ�
        /// </summary>
        /// <param name="data">ԭʼ����</param>
        /// <param name="tagn">��ǩ</param>
        /// <param name="valc">������</param>
        /// <returns></returns>
        private string RTOneTag2New(string data, string tagn, string valc)
        {
            int stN = 0;

        XDX:
            if (stN >= data.Length)
            {
                goto XUI;
            }

            int a1 = data.IndexOf("<" + tagn, stN);  //123456  23   56

            if (a1 + tagn.Length >= data.Length)
            {
                goto XUI;
            }

            int a2 = data.IndexOf(">", a1 + tagn.Length + 1);



            if ((a1 > -1) & (a2 > -1))
            {
                string s1 = data.Substring(0, a1 + tagn.Length + 1);
                string s2 = data.Substring(a2, data.Length - a2);
                string ss = data.Substring(a1 + tagn.Length + 1, a2 - a1 - tagn.Length - 1);// href="http://www.baidu.com/"

                ss = ss.Trim();

                if (ss.IndexOf(" ") == -1)
                {
                    ss = " " + clearDat(ss, valc);
                }
                else
                {
                    string newX = "";
                    string[] mcc = ss.Split(' ');
                    foreach (string a in mcc)
                    {
                        if (a.Length > 0)
                        {
                            newX = newX + " " + RTclearDat(a, valc);
                        }
                    }
                    ss = newX;
                }

                stN = s1.Length + ss.Length;
                data = s1 + ss + s2;

                goto XDX;
            }

        XUI: ;
            return data;
        }


        /// <summary>
        /// ȥ��script
        /// </summary>
        /// <param name="idat"></param>
        /// <param name="b1">script</param>
        /// <param name="b2">script</param>
        /// <returns></returns>
        private string clearSP(string idat,string b1,string b2 )
        {
          //  string b1 = "<script";
           // string b2 = "</script>";

        XX:
            int a1 = idat.IndexOf(b1);

            if (a1 + b1.Length >= idat.Length)
            {
                return idat;
            }

            int a2 = idat.IndexOf(b2, a1 + b1.Length + 1);

            if (a1 > -1 & a2 > -1)
            {
                string s1 = idat.Substring(0, a1);
                string s2 = idat.Substring(a2 + b2.Length, idat.Length - a2 - b2.Length);

                idat = s1 + s2;
            }
            else
            {
                return idat;
            }

            goto XX;

            return idat;

        }

        /// <summary>
        /// ����������е�URL  ...
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string clearHREFAdd(string data)
        {
            //  <script   </script>
            //<input type="hidden" name="__VIEWSTATE" value="dDwtMikr" />
            // <a href="http://www.chinahr.com/">

            string dd = clearOneTag(data, "input", "value");

            dd = clearOneTag(dd, "a", "href");

            dd = clearOneTag(dd, "img", "src");

            //  dd = RTOneTag2New(data, "a", "href");
            //  dd = RTOneTag2New(dd, "img", "src");

            return dd;

        }

        /// <summary>
        /// ����ת��
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private  string Str2Str(string data)
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
        /// HTMLҳ����������
        /// </summary>
        /// <param name="HTMLDATA">HTML����</param>
        /// <returns>������HTML����</returns>
        public string HTML2CLEAR2(string HTMLDATA)
        {
            //����ű���
            //  string b1 = "<script";
            // string b2 = "</script>";
            string Data = clearSP(HTMLDATA, "<script", "</script>");

            Data = clearSP(Data, "<Script", "</Script>");

            Data = clearSP(Data, "<SCRIPT", "</SCRIPT>");

            Data = clearSP(Data, "<script", "</script>");

            //��������е�ĳЩ����ֵ
            Data = clearHREFAdd(Data);

            return Data;

        }











    }
}
