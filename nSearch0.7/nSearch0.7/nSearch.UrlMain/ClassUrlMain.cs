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

namespace nSearch.UrlMain
{

    



    /// <summary>
    /// ����URL
    ///  1 ѹ��һ��URL
    ///  2 ���һ��URL
    ///  3 ������������URL
    ///  4 ����ϵ�
    /// </summary>
   class ClassUrlMain
    {


        //�洢�ṹ���� MD5 ��ͷһ����ĸ �����ָ������� a-z 0-9 ѭ������ĳ����ͷ��URL MD5 ���ڴ���Χ�ڵĴ洢���ļ���
        //
        //

        /// <summary>
        /// �洢����
        /// </summary>
        Hashtable MainBataBase = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Start.  ��ʼ������  ���Ҹ������Ʒ�Χ �� ÿ���������»�ȡ�������� ÿ������ ���ȼ��� 
        /// </summary>
        private string StartTxtPath;


        /// <summary>
        /// ѡ�� ��ǰ�����
        /// </summary>
        private char NowChar=' ';

       /// <summary>
       /// ÿѹ��60000������  �������ƶ�һ��ָ�� �ڴ��е����ݸ��ɹ���
       /// </summary>
       private int UrlAddClearCache = 0;

        /// <summary>
        /// ѡ����
        /// </summary>
        char[] sn = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };


        /// <summary>
        /// ���ô������չ���б�
        /// </summary>
        private string[] FILEEX ={"css",  "js",  "zip",  "avi", "rar",  "exe", "dat",  "png", "jpg", "gif", "mp3","rm","rmvb","doc",
                                   "xsl","pdf","asf","wav" ,"wmv","mpeg","mp4","txt","gz","tar","torrent","swf","ppt","mdb",
                                   "iso","bin","dll","obj","svg","xml","mov","pps","ico","iuc","bak","pps","gz"};


        /// <summary>
        /// ԭʼ��URL
        /// </summary>
        ArrayList MainURLBase = ArrayList.Synchronized(new ArrayList());

        ArrayList errUrl = ArrayList.Synchronized(new ArrayList());

        ArrayList a = ArrayList.Synchronized(new ArrayList());
        ArrayList b = ArrayList.Synchronized(new ArrayList());
        ArrayList c = ArrayList.Synchronized(new ArrayList());
        ArrayList d = ArrayList.Synchronized(new ArrayList());
        ArrayList e = ArrayList.Synchronized(new ArrayList());
        ArrayList f = ArrayList.Synchronized(new ArrayList());
        ArrayList g = ArrayList.Synchronized(new ArrayList());
        ArrayList h = ArrayList.Synchronized(new ArrayList());
        ArrayList i = ArrayList.Synchronized(new ArrayList());
        ArrayList j = ArrayList.Synchronized(new ArrayList());
        ArrayList k = ArrayList.Synchronized(new ArrayList());
        ArrayList l = ArrayList.Synchronized(new ArrayList());
        ArrayList m = ArrayList.Synchronized(new ArrayList());
        ArrayList n = ArrayList.Synchronized(new ArrayList());
        ArrayList o = ArrayList.Synchronized(new ArrayList());
        ArrayList p = ArrayList.Synchronized(new ArrayList());
        ArrayList q = ArrayList.Synchronized(new ArrayList());
        ArrayList r = ArrayList.Synchronized(new ArrayList());
        ArrayList s = ArrayList.Synchronized(new ArrayList());
        ArrayList t = ArrayList.Synchronized(new ArrayList());
        ArrayList u = ArrayList.Synchronized(new ArrayList());
        ArrayList v = ArrayList.Synchronized(new ArrayList());
        ArrayList w = ArrayList.Synchronized(new ArrayList());
        ArrayList x = ArrayList.Synchronized(new ArrayList());
        ArrayList y = ArrayList.Synchronized(new ArrayList());
        ArrayList z = ArrayList.Synchronized(new ArrayList());
        ArrayList n0 = ArrayList.Synchronized(new ArrayList());
        ArrayList n1 = ArrayList.Synchronized(new ArrayList());
        ArrayList n2 = ArrayList.Synchronized(new ArrayList());
        ArrayList n3 = ArrayList.Synchronized(new ArrayList());
        ArrayList n4 = ArrayList.Synchronized(new ArrayList());
        ArrayList n5 = ArrayList.Synchronized(new ArrayList());
        ArrayList n6 = ArrayList.Synchronized(new ArrayList());
        ArrayList n7 = ArrayList.Synchronized(new ArrayList());
        ArrayList n8 = ArrayList.Synchronized(new ArrayList());
        ArrayList n9 = ArrayList.Synchronized(new ArrayList());

        /// <summary>
        /// ��ʼ������
        /// </summary>
        public void  InitClassUrlMain(string StartTxtPathX)
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine("��ʼ������");

            if (StartTxtPathX[StartTxtPathX.Length - 1] != '\\')
            {
                StartTxtPathX = StartTxtPathX + "\\";
            }
            StartTxtPath = StartTxtPathX;


            MainBataBase.Clear();

            //����
            MainBataBase.Add('a', a);
            MainBataBase.Add('b', b);
            MainBataBase.Add('c', c);
            MainBataBase.Add('d', d);
            MainBataBase.Add('e', e);
            MainBataBase.Add('f', f);
            MainBataBase.Add('g', g);
            MainBataBase.Add('h', h);
            MainBataBase.Add('i', i);
            MainBataBase.Add('j', j);
            MainBataBase.Add('k', k);
            MainBataBase.Add('l', l);
            MainBataBase.Add('m', m);
            MainBataBase.Add('n', n);
            MainBataBase.Add('o', o);
            MainBataBase.Add('p', p);
            MainBataBase.Add('q', q);
            MainBataBase.Add('r', r);
            MainBataBase.Add('s', s);
            MainBataBase.Add('t', t);
            MainBataBase.Add('u', u);
            MainBataBase.Add('v', v);
            MainBataBase.Add('w', w);
            MainBataBase.Add('x', x);
            MainBataBase.Add('y', y);
            MainBataBase.Add('z', z);
            MainBataBase.Add('0', n0);
            MainBataBase.Add('1', n1);
            MainBataBase.Add('2', n2);
            MainBataBase.Add('3', n3);
            MainBataBase.Add('4', n4);
            MainBataBase.Add('5', n5);
            MainBataBase.Add('6', n6);
            MainBataBase.Add('7', n7);
            MainBataBase.Add('8', n8);
            MainBataBase.Add('9', n9);
      
            //������������Start.txt  ��������

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(StartTxtPath+"Start.txt", System.Text.Encoding.GetEncoding("gb2312"));
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    line=line.Trim();
                    if (line.Length > 0)
                    {
                        if (MainURLBase.Contains(line) == false)
                        {
                            if (line.IndexOf("//") != 0)   //������ע��
                            {
                                MainURLBase.Add(line);
                            }
                        }                    
                    }
                }
                reader.Close();
            }
            catch (IOException em)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(em.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            //������������Start.txt  
            LoadOneTxt(StartTxtPath+"Start.txt");


            //�ƶ��洢��
           // MoveNowChar();

            //Ĭ��ѡ��
            NowChar = 'a';

            //����ϵͳ
            string Tmp = GetOneUrl();

            PutOneUrl(Tmp);

        }

        /// <summary>
        /// ����һ�������ļ�
        /// </summary>
        /// <param name="path">�����ļ�·��</param>
        private void LoadOneTxt(string path)
        {


            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path, System.Text.Encoding.GetEncoding("gb2312"));
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    line = line.Trim();
                    if (line.Length > 0)
                    {
                        string Md5X = getMD5name(line).ToLower();
                        ArrayList newOne = (ArrayList)MainBataBase[Md5X[0]];

                        if (newOne.Contains(line) == false)
                        {
                            newOne.Add(line);
                            MainBataBase[Md5X[0]] = newOne;
                        }
                        newOne = null;
                    }
                }
                reader.Close();
            }
            catch (IOException e)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

        }



        public string GetTongji()
        {

            string iii = NowChar.ToString();


            return iii;
        
        }

        /// <summary>
        /// �õ�URL��MD5��
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string getMD5name(string url)
        {
            string strMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(url, "md5");
            return strMd5;
        }



        //����һ������
        private static void saveOne(string ppath, string dat)
        {

            nSearch.DebugShow.ClassDebugShow.WriteLine("����һ������");

            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(ppath, true, System.Text.Encoding.GetEncoding("gb2312"));
                writer.WriteLine(dat);
                writer.Close();
            }
            catch (IOException e)
            {
                if (writer != null)
                    writer.Close();
            }

        }

       /// <summary>
       /// �����˳�
       /// </summary>
       public void ExitSave()
       {
           nSearch.DebugShow.ClassDebugShow.WriteLine(" �����˳�");

           Hashtable xxxHashtable = (Hashtable)MainBataBase.Clone();
           //�����������ݵ����ݻ����ļ�

           foreach (DictionaryEntry xn in xxxHashtable)
           {
               //�洢����
               ArrayList newOne = (ArrayList)xn.Value;
               string nname = xn.Key.ToString();

               if (newOne.Count > 0)
               {

                   StreamWriter writer = null;
                   writer = new StreamWriter(StartTxtPath + nname + ".txt", true, System.Text.Encoding.GetEncoding("gb2312"));
                   foreach (string one in newOne)
                   {
                       writer.WriteLine(one);
                   }
                   writer.Close();
               }
               newOne = null;
           }
       }

        /// <summary>
        /// �ƶ�һ����ǰ�Ĵ洢ѡ�� -> a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9 
        /// </summary>
        private void MoveNowChar()
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine("�ƶ�һ����ǰ�Ĵ洢ѡ�� -> a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9");

                //�����ĩβ ���ƶ�����ͷ
                if (NowChar == '9' | NowChar == ' ')
                {
                    NowChar = 'a';

                }
                else
                {
                    //�������ĩβ ������ƶ�һ��
                    for (int ii = 0; ii < sn.Length -1; ii++)
                    {
                        if (sn[ii] == NowChar)
                        {
                            NowChar = sn[ii + 1];
                            break;
                        }

                    }
                }

                Hashtable xxxHashtable = (Hashtable)MainBataBase.Clone();

            //�����������ݵ����ݻ����ļ�

            foreach (DictionaryEntry xn in xxxHashtable)
            { 
                //�洢����
                ArrayList newOne = (ArrayList)xn.Value;
                string nname = xn.Key.ToString();

                if (newOne.Count > 100)  //�������100ʱ���浽�ļ�
                {
                    StreamWriter writer = null;
                    writer = new StreamWriter(StartTxtPath + nname + ".txt", true, System.Text.Encoding.GetEncoding("gb2312"));
                    foreach (string one in newOne)
                    {
                        writer.WriteLine(one);
                    }
                    writer.Close();
                    newOne = null;
                }
            }



            if (a.Count > 100) { a.Clear(); }
            if (b.Count > 100) {  b.Clear();}
            if (c.Count > 100) {   c.Clear();}
            if (d.Count > 100) {  d.Clear();}
            if (e.Count > 100) {   e.Clear();}
            if (f.Count > 100) {  f.Clear();}
            if (g.Count > 100) {  g.Clear();}
            if (h.Count > 100) {   h.Clear();}
            if (i.Count > 100) {    i.Clear();}
            if ( j.Count > 100) {    j.Clear();}
            if ( k.Count > 100) {    k.Clear();}
            if ( l.Count > 100) {     l.Clear();}
            if ( m.Count > 100) {    m.Clear();}
            if ( n.Count > 100) {   n.Clear();}
            if ( o.Count > 100) {   o.Clear();}
            if ( p.Count > 100) {    p.Clear();}
            if ( q.Count > 100) {    q.Clear();}
            if ( r.Count > 100) {    r.Clear();}
            if ( s.Count > 100) {    s.Clear();}
            if ( t.Count > 100) {    t.Clear();}
            if ( u.Count > 100) {     u.Clear();}
            if ( v.Count > 100) {    v.Clear();}
            if ( w.Count > 100) {   w.Clear();}
            if ( x.Count > 100) {     x.Clear();}
            if ( y.Count > 100) {      y.Clear();}
            if ( z.Count > 100) {    z.Clear();}
            if ( n0.Count > 100) {     n0.Clear();}
            if ( n1.Count > 100) {    n1.Clear();}
            if ( n2.Count > 100) {     n2.Clear();}
            if ( n3.Count > 100) {    n3.Clear();}
            if ( n4.Count > 100) {     n4.Clear();}
            if ( n5.Count > 100) {    n5.Clear();}
            if ( n6.Count > 100) {    n6.Clear();}
            if ( n7.Count > 100) {    n7.Clear();}
            if ( n8.Count > 100) {     n8.Clear();}
            if (n9.Count > 100) { n9.Clear(); }

            MainBataBase.Clear();
            MainBataBase.Add('a', a);
            MainBataBase.Add('b', b);
            MainBataBase.Add('c', c);
            MainBataBase.Add('d', d);
            MainBataBase.Add('e', e);
            MainBataBase.Add('f', f);
            MainBataBase.Add('g', g);
            MainBataBase.Add('h', h);
            MainBataBase.Add('i', i);
            MainBataBase.Add('j', j);
            MainBataBase.Add('k', k);
            MainBataBase.Add('l', l);
            MainBataBase.Add('m', m);
            MainBataBase.Add('n', n);
            MainBataBase.Add('o', o);
            MainBataBase.Add('p', p);
            MainBataBase.Add('q', q);
            MainBataBase.Add('r', r);
            MainBataBase.Add('s', s);
            MainBataBase.Add('t', t);
            MainBataBase.Add('u', u);
            MainBataBase.Add('v', v);
            MainBataBase.Add('w', w);
            MainBataBase.Add('x', x);
            MainBataBase.Add('y', y);
            MainBataBase.Add('z', z);
            MainBataBase.Add('0', n0);
            MainBataBase.Add('1', n1);
            MainBataBase.Add('2', n2);
            MainBataBase.Add('3', n3);
            MainBataBase.Add('4', n4);
            MainBataBase.Add('5', n5);
            MainBataBase.Add('6', n6);
            MainBataBase.Add('7', n7);
            MainBataBase.Add('8', n8);
            MainBataBase.Add('9', n9);


            //���ص�ǰ������  ��ȡ���� ��ǰ��Ҫ�����URL����
            string nnamex =StartTxtPath   + NowChar.ToString()+ ".txt";

                 nSearch.DebugShow.ClassDebugShow.WriteLine("�ƶ�һ����ǰ�Ĵ洢ѡ�� -> " + NowChar.ToString());

            if (System.IO.File.Exists(nnamex) == false)
            { 
            
            }
            else
            {

                ArrayList newOne2 = (ArrayList)MainBataBase[NowChar];

                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(nnamex, System.Text.Encoding.GetEncoding("gb2312"));
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        line = line.Trim();
                        if (line.Length > 0)
                        {
                            if (newOne2.Contains(line) == false)
                            {
                                newOne2.Add(line);
                            }
                        }
                    }
                    reader.Close();
                }
                catch (IOException en)
                {
                    nSearch.DebugShow.ClassDebugShow.WriteLineF(en.Message);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                MainBataBase[NowChar] = newOne2;

                newOne2 = null;

                if (System.IO.File.Exists(nnamex) == true)
                {
                    System.IO.File.Delete(nnamex);
                }
            }
        }

        /// <summary>
        /// ��ȡһ��URL
        /// </summary>
        /// <returns></returns>
        public string GetOneUrl()
        {

            nSearch.DebugShow.ClassDebugShow.WriteLine("��ȡһ��URL");

            int iTime = 0; //�ظ�36�� �򷵻� ��END_ALL_NULL�� ȫ������

            GXXX:

            ArrayList newOne = (ArrayList)MainBataBase[NowChar];

            // ����Ϊ�� �� ת��ָ��  ���� �Ƴ���һ��
            if (newOne.Count == 0)
            {
                //�ʵ�����ʱ  ��ֹ����һ���Ӷ�ȡӲ���ٶȼӿ�
                System.Threading.Thread.Sleep(200);

                MoveNowChar();

                iTime = iTime + 1;

                if (iTime > 36)
                { 
                   return  "END_ALL_NULL";
                }

                goto GXXX;

            }
            else
            {
                string one = newOne[0].ToString();
                newOne.RemoveAt(0);
                MainBataBase[NowChar] = newOne;
                newOne = null;

                return one;

            }

            return "";
        }


        /// <summary>
        /// �Ƿ񱣴��URL  �ж��Ƿ��뼯���е���ͬһվ��
        /// </summary>
        /// <param name="ourl"></param>
        /// <returns></returns>
        private bool isSave(string ourl)
        {

            foreach (string urlT in MainURLBase)
            {
                if (Url2Url(urlT, ourl) == true)
                {
                    return true;  //�����Ʒ�Χ֮��
                }         
            }

            return false;


        }

        /// <summary>
        /// ѹ��1 ��URL
        /// </summary>
        /// <returns></returns>
        public void PutOneUrl(string purl)
        {
          //  nSearch.DebugShow.ClassDebugShow.WriteLine("ѹ��1 ��URL");

            purl = purl.Trim();

            //����������ļ�����
            if (isOKFile(purl) == false)
            {

                return;
            }

            //�����Ʒ�Χ�������һ�Ų���
            if (isSave(purl) == false)
            {
                return;
            }



            string Md5X = getMD5name(purl).ToLower();
            ArrayList newOne = (ArrayList)MainBataBase[Md5X[0]];

            //��������� �򲻹�
            if (newOne.Contains(purl) == false)
            {

               


                newOne.Add(purl);
                MainBataBase[Md5X[0]] = newOne;

                //ÿѹ��60000 ������ ���ƶ�һ������ָ��
                if (UrlAddClearCache > 60000)
                {
                    MoveNowChar();
                }

            }

            newOne = null;
        
        }

        /// <summary>
        /// �ٴ�ѹ��һ��URL �ظ�2�� ���Զ�ȡ��  �����һ�γ��� ���¼ ����ѹ��url
        /// </summary>
        public void RePutOneUrl(string murl)
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine("�ٴ�ѹ��һ��URL �ظ�2�� ���Զ�ȡ��  �����һ�γ��� ���¼ ����ѹ��url");

            if (errUrl.Contains(murl) == true)
            {
                errUrl.Remove(murl);
                return;
            }
            else
            {
                errUrl.Add(murl);
                PutOneUrl(murl);
            }
        
        }

        /// <summary>
        /// URL���ƶ�  �Ƿ�ͬһվ��  
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        private bool  Url2Url(string url1, string url2)
        {
            url1 = url1.ToLower().Trim();
            url2 = url2.ToLower().Trim();

            if (url1 + "/" == url2 | url1 == url2 + "/")
            {
                return true ;
            }          

            if (url1.IndexOf("http://") != 0 | url2.IndexOf("http://") != 0)
            {
                return false;
            }

            string[] ax1 = url1.Split('/');
            string[] ax2 = url2.Split('/');

            if (ax1[2] == ax2[2])
            {
                return true; //��ͬһվ��
            }
            else
            {
                return false ;
            }

          
        }



        /// <summary>
        /// �����ļ���չ���Ƿ�������Ҫ�ġ�ccs  js  zip  avi rar  exe dat  png jpg gif mp3 �Ȳ�ץȡ
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool isOKFile(string url)
        {
            if (url.IndexOf('?') > 0)
            {
                return true;
            }

            if (url.Length < 4)
            {
                return true;
            }

            //ȡ�����λ�����������չ�����򷵻ء�F
            string tmp_one = url.Substring(url.Length - 4, 4).ToLower();

            int ll = tmp_one.Length;

            foreach (string a_o in FILEEX)
            {
                int intU = tmp_one.LastIndexOf("." + a_o);

                if (intU > -1 & intU == ll - a_o.Length - 1)
                {
                    return false;
                }

            }

            return true;

        }




    }
}
