using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.ClassLibraryStruct
{

    /// <summary>
    /// 模板使用库
    /// </summary>
    public class ClassUserModel
    {

        private nSearch.ClassLibraryHTML.ClassHTML xxxx = new nSearch.ClassLibraryHTML.ClassHTML();

        /// <summary>
        /// 是否是全部显示
        /// </summary>
        private bool isShowmodelOneList = false;

        /// <summary>
        /// 全部列表数据
        /// </summary>
        public Hashtable modelOneList = new Hashtable();

        /// <summary>
        /// 模板数据集
        /// </summary>
        public ArrayList n = new ArrayList();

        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>返回模版数量</returns>
        public int init(string Xdir)
        {
            // 1 得到目录下的文件
            DirectoryInfo dir = new DirectoryInfo(Xdir);

            nSearch.ClassLibraryStruct.auto2dat new_it = new nSearch.ClassLibraryStruct.auto2dat();

            isShowmodelOneList = false;

            modelOneList.Clear();


            ArrayList n22 = new ArrayList();
            n22.Clear();

            n.Clear();

            //模板数目计数
            int i = 0;

            // 2 遍历文件　　读取数据压入
            foreach (FileInfo f in dir.GetFiles("*.a"))   //遍历获得以xml为扩展名的文件   
            {
                String name = f.FullName;         //name为该文件夹下的文件名称，如f.FullName为全名  
                name = name.Substring(0, name.Length - 2);

                string nameT = f.Name;
                nameT = nameT.Substring(0, nameT.Length - 2);
                new_it.TmpName = nameT;

                new_it.A = getFileData(name + ".a");
                new_it.B = getFileData(name + ".b");
                new_it.C = getFileData(name + ".c");
                new_it.D = getFileData(name + ".d");
                new_it.E = getFileData(name + ".e");
                new_it.F = getFileData(name + ".f");
                new_it.H = getFileData(name + ".h");
                new_it.M = getFileData(name + ".m");
                new_it.T = getFileData(name + ".t");

                new_it.A_TYPE_1 = getFileData(name + ".a1");
                new_it.A_TYPE_2 = getFileData(name + ".a2");

                new_it.isSORTIT = false;
                new_it.isXnum = CharNum(new_it.H);

                i = i + 1;

                nSearch.DebugShow.ClassDebugShow.WriteLineF("加载模板 " + name);

                if (n22.Contains(new_it) == false)
                {
                    n22.Add(new_it);
                }

            }

            n = sortIt(n22);

            nSearch.DebugShow.ClassDebugShow.WriteLineF("模板加载完成  总计：　" + i.ToString() + " 个模板");

            return i; //返回模版数量
        }


        /// <summary>
        /// 排序  按照含有的＊多少
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        private ArrayList sortIt(ArrayList ui)
        {
            int ui_Count = ui.Count;

            nSearch.ClassLibraryStruct.auto2dat[] One = new nSearch.ClassLibraryStruct.auto2dat[ui_Count];

            for (int i = 0; i < ui_Count; i++)
            {
                One[i] = (nSearch.ClassLibraryStruct.auto2dat)ui[i];
            }

            for (int i = 0; i < ui_Count; i++)
            {

                for (int j = i; j < ui_Count; j++)
                {

                    if (One[i].isXnum < One[j].isXnum)
                    {
                        nSearch.ClassLibraryStruct.auto2dat OneT = new nSearch.ClassLibraryStruct.auto2dat();
                        OneT = One[i];
                        One[i] = One[j];
                        One[j] = OneT;
                    }
                }

            }

            ArrayList s = new ArrayList();
            s.Clear();

            for (int i = 0; i < ui_Count; i++)
            {
                s.Add(One[i]);
            }

            return s;

        }

        /// <summary>
        /// 得到＊个数
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private int CharNum(string dat)
        {

            return dat.Length - dat.Replace("*", "").Length;

        }


        /// <summary>
        /// 压入测试模板
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="t"></param>
        /// <param name="h"></param>
        /// <param name="m"></param>
        public void TestModeL(string a, string b, string c, string d, string e, string t, string h, string m,string a1,string a2)
        {
            isShowmodelOneList = true;

            n.Clear();

            modelOneList.Clear();

            nSearch.ClassLibraryStruct.auto2dat new_it = new nSearch.ClassLibraryStruct.auto2dat();

            new_it.A = a;
            new_it.B = b;
            new_it.C = c;
            new_it.D = d;
            new_it.E = e;
            new_it.T = t;
            new_it.H = h;
            new_it.M = m;

            new_it.A_TYPE_1 = a1;
            new_it.A_TYPE_2  = a2;

            n.Add(new_it);

        }


        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string getFileData(string filename)
        {

            StreamReader reader = null;
            string data = string.Empty;
            try
            {
                reader = new StreamReader(filename, System.Text.Encoding.GetEncoding("gb2312"));

                data = reader.ReadToEnd();

                reader.Close();

                if (data == null)
                {
                    data = "";
                }

                return data;
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

            return "";


        }


        /// <summary>
        /// 匹配模板 得到数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public nSearch.ClassLibraryStruct.auto2dat getTagAndData(string data)
        {
            nSearch.ClassLibraryStruct.auto2dat myTagData = new nSearch.ClassLibraryStruct.auto2dat();



            string NewData = data;

            // TagAndData myTagData = new TagAndData();
            // 1  查找 是否 可以找到匹配模块  找不到则 返回 cnull 

            // 2  取出匹配数据

            // 3  根据匹配模板得到新的标签和内容数据

            int a1 = 0, a2 = 0, a1Len = 0, a2Len = 0;
            string[] myBack = new string[2000];

            //遍历匹配模版
            //for (int i = 0; i <= TagModelNum; i++)
            //{
            foreach (nSearch.ClassLibraryStruct.auto2dat xain in n)
            {
                data = NewData;

                int myBackLen = 0;

                nSearch.ClassLibraryStruct.auto2dat aX = (nSearch.ClassLibraryStruct.auto2dat)xain;

                if (aX.H.Length < 2)
                {
                    goto NewFindX;
                }

                aX.H = " " + aX.H;

                //匹配数据分解
                string[] myTmpDB = aX.H.Split('*');

                for (int j = 1; j < myTmpDB.Length; j++)
                {
                    if (myTmpDB[j].Length > 0)
                    {
                        a1 = data.IndexOf(myTmpDB[j - 1]);
                        a1Len = myTmpDB[j - 1].Length;

                        a2 = data.IndexOf(myTmpDB[j], a1 + a1Len - 1);

                        a2Len = myTmpDB[j].Length;
                        if (a1 == -1 || a2 == -1)
                        {
                            goto NewFindX;
                        }

                        string mybackone = data.Substring(a1 + a1Len, a2 - a1 - a1Len);

                        data = data.Substring(a2, data.Length - a2);

                        if (mybackone != null)
                        {
                            myBack[myBackLen] = GetTXT(mybackone);
                            myBackLen = myBackLen + 1;
                        }
                        else
                        {
                            mybackone = " ";
                            myBack[myBackLen] = GetTXT(mybackone);
                            myBackLen = myBackLen + 1;
                        }
                    }
                }

                string axa = aX.A;
                string axb = aX.B;
                string axc = aX.C;
                string axd = aX.D;
                string axe = aX.E;
                string axt = aX.T;
                string axm = aX.M;

                string A_TY1 = aX.A_TYPE_1;
                string A_TY2 = aX.A_TYPE_2;

                //替换标签
                if (isShowmodelOneList == true)
                {
                    modelOneList.Clear();
                }

                for (int h = 0; h < myBackLen; h++)
                {

                    axa = axa.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", myBack[h]);

                    axb = axb.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", CCxmlTag(myBack[h]));

                    axc = axc.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", CCxmlTag(myBack[h]));

                    axd = axd.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", CCxmlTag(myBack[h]));


                    axe = axe.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", myBack[h]);


                    axt = axt.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", myBack[h]);

                    A_TY1 = A_TY1.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", myBack[h]);
                    A_TY2 = A_TY2.Replace("<TAGDATA INDEX=" + h.ToString() + "/>", myBack[h]);

                    if (isShowmodelOneList == true)
                    {
                        modelOneList.Add(h, myBack[h]);
                    }

                }

                myTagData.A = axa;
                myTagData.B = axb;
                myTagData.C = axc;
                myTagData.D = axd;
                myTagData.E = axe;
                myTagData.T = axt;
                myTagData.M = axm;

                //暂时不使用自动类聚 所以采用固定的地方
                myTagData.A_TYPE_1 =A_TY1;
                myTagData.A_TYPE_2 =A_TY2;

                myTagData.isOK = true;
              

                return myTagData;


            NewFindX: ;

                for (int xi = 0; xi < 2000; xi++)
                { myBack[xi] = ""; }
                a1 = 0; a2 = 0; a1Len = 0; a2Len = 0;
               


            }

            //取出  <ZD**></ZD***>
            return GetNullXZDHtml(data);  //不能匹配模板时自动按照网页数据取得


        }

        /// <summary>
        /// 当全部不能匹配时 主类别为 其它 得到数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private nSearch.ClassLibraryStruct.auto2dat GetNullXZDHtml(string datas)
        {

            nSearch.ClassLibraryStruct.auto2dat mm = new nSearch.ClassLibraryStruct.auto2dat();
            mm.isOK = false;


            string nh1 =xxxx.GetClearCode(datas,true);
            string nh2 = xxxx.GetClearCode(datas,false);  // GetTXT(datas);


            mm.A = "HTM";// ParseHtml(datas);
            mm.B = "WEB";//"<XL主类别>HTM</XL主类别>";
            mm.C = "C";
            mm.D = nh2;// GetTitle(datas);
            mm.E = nh1;// GetBODY(datas);
            mm.T = "";//GetBODY(datas);
            mm.M = "XXX";
            mm.F = "";

            mm.A_TYPE_1 = "HTM";
            mm.A_TYPE_2 = "HTM";

            mm.isOK =false;

            return mm;

            /*
              oneDoc.Add(new Field("A", k.A, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 标题A
              oneDoc.Add(new Field("B", k.B, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 类聚B
              oneDoc.Add(new Field("C", k.C, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 简要显示C
              oneDoc.Add(new Field("D", k.D, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 快照显示D
              oneDoc.Add(new Field("E", k.E, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.YES));// 索引用的数据E
              oneDoc.Add(new Field("T", k.T, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 单独的标题数据 
              oneDoc.Add(new Field("M", k.M, Field.Store.YES, Field.Index.NO, Field.TermVector.NO));// 主类别

             */
        }

        private string CCxmlTag(string data)
        {
            // 把数据变为符合XML规范的数据
            data = data.Replace("<", "〈");
            data = data.Replace(">", "〉");
            data = data.Trim();

            return data;
        
        }
    


        /// <summary>
        /// 得到中文
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private  string GetTXT(string dat)
        {

            string data = Regex.Replace(dat, "<[^>]*>", " ");

            data = data.Replace("&ldquo;", " ");
            data = data.Replace("&nbsp;", " ");
            data = data.Replace("&rdquo;", " ");
            data = data.Replace("&lt;", " ");
            data = data.Replace("\b", " ");
            data = data.Replace("\f", " ");
            data = data.Replace("\n", " ");
            data = data.Replace("\r", " ");
            data = data.Replace("&nbsp;", " ");
            data = data.Replace("&gt;", " ");
            data = data.Replace("&quot;", " ");
            data = data.Replace("&nbsp;", " ");
            data = data.Replace("\t", " ");
            data = data.Replace("\v", " ");
            data = data.Replace("   ", " ");
            data = data.Replace("  ", " ");
          
            // 把数据变为符合XML规范的数据
            data = data.Replace("<", "〈");
            data = data.Replace(">", "〉");       
            data = data.Trim();
           
            return data;

        }



    }
}


/*
 
     /// <summary>
        /// 把读取的文件中的所有的html标记去掉，把&nbsp;替换成空格
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string ParseHtml(string html)
        {
            string temp = Regex.Replace(html, "<[^>]*>", "");
            return temp.Replace("&nbsp;", " ");
        }
        /// <summary>
        /// 获得读取的html文挡的标题
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetTitle(string html)
        {
            Match m = Regex.Match(html, "<ZDKC0>(.*)</ZDKC0>");
            if (m.Groups.Count == 2)
                return m.Groups[1].Value;
            return "此文挡标题未知";
        }


        /// <summary>
        /// 获得读取的html文挡的内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetBODY(string html)
        {
            Match m = Regex.Match(html, "<ZDbody>(.*)</ZDbody>");
            if (m.Groups.Count == 2)
                return m.Groups[1].Value;
            return "此文挡内容未知";
        }
 
 
 */