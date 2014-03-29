using System;
using System.Collections.Generic;
using System.Text;
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

namespace nSearch.UrlMain
{

    



    /// <summary>
    /// 调度URL
    ///  1 压入一个URL
    ///  2 输出一个URL
    ///  3 处理请求错误的URL
    ///  4 处理断点
    /// </summary>
   class ClassUrlMain
    {


        //存储结构采用 MD5 的头一个字母 来区分各个数据 a-z 0-9 循环处理某个开头的URL MD5 不在处理范围内的存储到文件中
        //
        //

        /// <summary>
        /// 存储数据
        /// </summary>
        Hashtable MainBataBase = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Start.  初始化数据  并且负责限制范围 和 每次启动更新获取的数据项 每次启动 首先加载 
        /// </summary>
        private string StartTxtPath;


        /// <summary>
        /// 选择 当前的输出
        /// </summary>
        private char NowChar=' ';

       /// <summary>
       /// 每压入60000条数据  就重新移动一次指针 内存中的数据负荷过重
       /// </summary>
       private int UrlAddClearCache = 0;

        /// <summary>
        /// 选择项
        /// </summary>
        char[] sn = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };


        /// <summary>
        /// 不用处理的扩展名列表
        /// </summary>
        private string[] FILEEX ={"css",  "js",  "zip",  "avi", "rar",  "exe", "dat",  "png", "jpg", "gif", "mp3","rm","rmvb","doc",
                                   "xsl","pdf","asf","wav" ,"wmv","mpeg","mp4","txt","gz","tar","torrent","swf","ppt","mdb",
                                   "iso","bin","dll","obj","svg","xml","mov","pps","ico","iuc","bak","pps","gz"};


        /// <summary>
        /// 原始的URL
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
        /// 初始化数据
        /// </summary>
        public void  InitClassUrlMain(string StartTxtPathX)
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine("初始化数据");

            if (StartTxtPathX[StartTxtPathX.Length - 1] != '\\')
            {
                StartTxtPathX = StartTxtPathX + "\\";
            }
            StartTxtPath = StartTxtPathX;


            MainBataBase.Clear();

            //加载
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
      
            //加载启动数据Start.txt  到限制器

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
                            if (line.IndexOf("//") != 0)   //不包含注释
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

            //加载启动数据Start.txt  
            LoadOneTxt(StartTxtPath+"Start.txt");


            //移动存储项
           // MoveNowChar();

            //默认选项
            NowChar = 'a';

            //激活系统
            string Tmp = GetOneUrl();

            PutOneUrl(Tmp);

        }

        /// <summary>
        /// 加载一个数据文件
        /// </summary>
        /// <param name="path">数据文件路径</param>
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
        /// 得到URL的MD5名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string getMD5name(string url)
        {
            string strMd5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(url, "md5");
            return strMd5;
        }



        //保存一个数据
        private static void saveOne(string ppath, string dat)
        {

            nSearch.DebugShow.ClassDebugShow.WriteLine("保存一个数据");

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
       /// 保存退出
       /// </summary>
       public void ExitSave()
       {
           nSearch.DebugShow.ClassDebugShow.WriteLine(" 保存退出");

           Hashtable xxxHashtable = (Hashtable)MainBataBase.Clone();
           //保存所有数据到数据缓存文件

           foreach (DictionaryEntry xn in xxxHashtable)
           {
               //存储数据
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
        /// 移动一个当前的存储选项 -> a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9 
        /// </summary>
        private void MoveNowChar()
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine("移动一个当前的存储选项 -> a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9");

                //如果到末尾 则移动到开头
                if (NowChar == '9' | NowChar == ' ')
                {
                    NowChar = 'a';

                }
                else
                {
                    //如果不是末尾 则向后移动一次
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

            //保存所有数据到数据缓存文件

            foreach (DictionaryEntry xn in xxxHashtable)
            { 
                //存储数据
                ArrayList newOne = (ArrayList)xn.Value;
                string nname = xn.Key.ToString();

                if (newOne.Count > 100)  //缓存大于100时保存到文件
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


            //加载当前的数据  读取数据 当前需要处理的URL集合
            string nnamex =StartTxtPath   + NowChar.ToString()+ ".txt";

                 nSearch.DebugShow.ClassDebugShow.WriteLine("移动一个当前的存储选项 -> " + NowChar.ToString());

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
        /// 读取一个URL
        /// </summary>
        /// <returns></returns>
        public string GetOneUrl()
        {

            nSearch.DebugShow.ClassDebugShow.WriteLine("读取一个URL");

            int iTime = 0; //重复36次 则返回 “END_ALL_NULL” 全部结束

            GXXX:

            ArrayList newOne = (ArrayList)MainBataBase[NowChar];

            // 集合为空 则 转移指针  否则 推出第一个
            if (newOne.Count == 0)
            {
                //适当的延时  防止数据一下子读取硬盘速度加快
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
        /// 是否保存该URL  判断是否与集合中的在同一站点
        /// </summary>
        /// <param name="ourl"></param>
        /// <returns></returns>
        private bool isSave(string ourl)
        {

            foreach (string urlT in MainURLBase)
            {
                if (Url2Url(urlT, ourl) == true)
                {
                    return true;  //在限制范围之内
                }         
            }

            return false;


        }

        /// <summary>
        /// 压入1 个URL
        /// </summary>
        /// <returns></returns>
        public void PutOneUrl(string purl)
        {
          //  nSearch.DebugShow.ClassDebugShow.WriteLine("压入1 个URL");

            purl = purl.Trim();

            //不是所需的文件类型
            if (isOKFile(purl) == false)
            {

                return;
            }

            //在限制范围外的数据一概不管
            if (isSave(purl) == false)
            {
                return;
            }



            string Md5X = getMD5name(purl).ToLower();
            ArrayList newOne = (ArrayList)MainBataBase[Md5X[0]];

            //如果不存在 则不管
            if (newOne.Contains(purl) == false)
            {

               


                newOne.Add(purl);
                MainBataBase[Md5X[0]] = newOne;

                //每压入60000 条数据 就移动一次数据指针
                if (UrlAddClearCache > 60000)
                {
                    MoveNowChar();
                }

            }

            newOne = null;
        
        }

        /// <summary>
        /// 再次压入一个URL 重复2次 则自动取消  如果第一次出错 则记录 重新压入url
        /// </summary>
        public void RePutOneUrl(string murl)
        {
            nSearch.DebugShow.ClassDebugShow.WriteLine("再次压入一个URL 重复2次 则自动取消  如果第一次出错 则记录 重新压入url");

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
        /// URL相似度  是否同一站点  
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
                return true; //在同一站点
            }
            else
            {
                return false ;
            }

          
        }



        /// <summary>
        /// 调查文件扩展名是否属于需要的　ccs  js  zip  avi rar  exe dat  png jpg gif mp3 等不抓取
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

            //取出最后４位　如果含有扩展名　则返回　F
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
