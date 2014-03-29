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

namespace nSearch.Main
{
    /// <summary>
    /// 公共数据存储类  http://127.0.0.1:81/s?wd=中国
    /// </summary>
    public static class ClassST
    {

        /// <summary>
        /// 分词类
        /// </summary>
        public  static Lucene.Net.Analysis.Analyzer OneAnalyzer ;

        /// <summary>
        /// 属性类聚列表
        /// </summary>
        public static Hashtable nMainType = new Hashtable();

        /// <summary>
        /// 存放节点列表
        /// </summary>
        public static ArrayList vbn = new ArrayList();

        /// <summary>
        /// 主类别列表文件  主类别.main
        /// </summary>
        private  static ArrayList GetMainTypeList = new ArrayList();


        /// <summary>
        /// 模板文件 &lt;--HTTP://ZD4004.BLOG.163.COM--&gt;
        /// </summary>
        public static string ModelHTM;

        /// <summary>
        /// 设定节点列表
        /// </summary>
        /// <param name="xxx"></param>
        /// <param name="TypeRS">类聚属性的存放路径</param>
        public static void SetVbn(ArrayList xxx, string ModelHtmWeb, string TypeRS)
        {

            if (TypeRS[TypeRS.Length - 1] == '\\')
            {

            }
            else
            {
                TypeRS = TypeRS + "\\";
            }

            vbn = (ArrayList)xxx.Clone();

            OneAnalyzer = new Lucene.Net.Analysis.XunLongX.XunLongAnalyzer();


            ModelHTM = getFileData(ModelHtmWeb);


            // 属性类聚列表
            Hashtable nTypeRSLIST = new Hashtable();
            nTypeRSLIST.Clear();

            // 1 得到目录下的文件
            DirectoryInfo dir = new DirectoryInfo(TypeRS);

            // 2 遍历文件　　读取数据压入
            foreach (FileInfo f in dir.GetFiles("*.txt"))   //遍历获得以xml为扩展名的文件   
            {
                String name = f.FullName;         //name为该文件夹下的文件名称，如f.FullName为全名  

                string a11 = getFileData(name);

                string CC1 =f.Name;
                string CC2 = f.Extension;

                string xxNmae = CC1.Replace(CC2, "");

                char[] dwes = { '\r', '\n' };

                string[] xcd = a11.Split(dwes);

                foreach (string xcdOne in xcd)
                {
                    if (xcdOne.IndexOf('\t') > 0)
                    {
                        string[] vvv = xcdOne.Split('\t');

                        if (nTypeRSLIST.Contains(xxNmae) == false)
                        {
                            ArrayList xPP = new ArrayList();
                            xPP.Clear();

                            xPP.Add(xcdOne.Trim());

                            nTypeRSLIST.Add(xxNmae, xPP);
                            xPP = null;
                        }
                        else
                        {
                            ArrayList xPP = (ArrayList)nTypeRSLIST[xxNmae];
                            if (xPP.Contains(xcdOne) == false)
                            {
                                xPP.Add(xcdOne);
                                nTypeRSLIST[xxNmae] = xPP;
                            }
                            xPP = null;
                        }
                    }
                }







            }


            nMainType = (Hashtable)nTypeRSLIST.Clone();

            string ListRT = getFileData(TypeRS + "主类别.main");
            char[] GH = {'\r','\n' };
            string[] KLGH = ListRT.Split(GH);

            GetMainTypeList.Clear();

            foreach (string DT in KLGH)
            {
                if (DT.Length > 0)
                {
                    if (GetMainTypeList.Contains(DT) == false)
                    {
                        GetMainTypeList.Add(DT);
                    }
                
                }
            
            }


        }

       


        /// <summary>
        /// 得到可选列表
        /// </summary>
        /// <param name="nnmm">可选列表原始参数</param>
        /// <param name="ST1">可选列表一预置值</param>
        /// <param name="ST2">可选列表二预置值</param>
        /// <returns></returns>
        public  static string GetBoxListDat1(string KWORD, string ST1, string ST2)
        {


            if (nMainType.Contains(KWORD) == false)
            {
                //没有找到主类别
                return "";
            }

            ArrayList xxbb = (ArrayList)nMainType[KWORD];

            string A1_d = "<select name=\"selectKCA1\">\r\n" + " <option value=\"" + ST1 + "\">" + ST1 + "</option>\r\n";
            string A2_d = "<select name=\"selectKCA2\">\r\n" + " <option value=\"" + ST2 + "\">" + ST2 + "</option>\r\n";

            //消重
            ArrayList xkioST12 = new ArrayList();
            xkioST12.Add(ST1);
            xkioST12.Add(ST2);

            foreach (string xsa in xxbb)
            {
                string[] xkio = xsa.Split('\t');

                if (xkioST12.Contains(xkio[1]) == false)  //防止一个属性被重复压入  和在 类别一 类别二中重复出现
                {
                    xkioST12.Add(xkio[1]);

                    if (xkio[0] == "a1")
                    {
                        A1_d = A1_d + " <option value=\"" + xkio[1] + "\">" + xkio[1] + "</option>\r\n";
                    }

                    if (xkio[0] == "a2")
                    {
                        A2_d = A2_d + " <option value=\"" + xkio[1] + "\">" + xkio[1] + "</option>\r\n";
                    }
                }


            }

            A1_d = A1_d + "</select>";
            A2_d = A2_d + "</select>";

            string CFDF = A1_d;
            /*
            CFDF = CFDF + " <table width=\"80%\" height=\"20\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n";
            CFDF = CFDF + "<tr>\r\n";
            CFDF = CFDF + " <th scope=\"col\">" + A1_d  + "</th>\r\n";
            CFDF = CFDF + "</tr>\r\n";
            CFDF = CFDF + "</table>\r\n";
            */


            xkioST12 = null;

            return CFDF;


        }

        /// <summary>
        /// 得到可选列表
        /// </summary>
        /// <param name="nnmm">可选列表原始参数</param>
        /// <param name="ST1">可选列表一预置值</param>
        /// <param name="ST2">可选列表二预置值</param>
        /// <returns></returns>
        public static string GetBoxListDat2(string KWORD, string ST1, string ST2)
        {


            if (nMainType.Contains(KWORD) == false)
            {
                //没有找到主类别
                return "";
            }

            ArrayList xxbb = (ArrayList)nMainType[KWORD];

            string A1_d = "<select name=\"selectKCA1\" id=\"selectKCA1\">\r\n" + " <option value=\"" + ST1 + "\">" + ST1 + "</option>\r\n";
            string A2_d = "<select name=\"selectKCA2\"  id=\"selectKCA2\">\r\n" + " <option value=\"" + ST2 + "\">" + ST2 + "</option>\r\n";

            //消重
            ArrayList xkioST12 = new ArrayList();
            xkioST12.Add(ST1);
            xkioST12.Add(ST2);

            foreach (string xsa in xxbb)
            {
                string[] xkio = xsa.Split('\t');

                if (xkioST12.Contains(xkio[1]) == false)  //防止一个属性被重复压入  和在 类别一 类别二中重复出现
                {
                    xkioST12.Add(xkio[1]);

                    if (xkio[0] == "a1")
                    {
                        A1_d = A1_d + " <option value=\"" + xkio[1] + "\">" + xkio[1] + "</option>\r\n";
                    }

                    if (xkio[0] == "a2")
                    {
                        A2_d = A2_d + " <option value=\"" + xkio[1] + "\">" + xkio[1] + "</option>\r\n";
                    }
                }


            }

            A1_d = A1_d + "</select>";
            A2_d = A2_d + "</select>";

            string CFDF = A2_d;// "";
            /*
            CFDF = CFDF + " <table width=\"80%\" height=\"20\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n";
            CFDF = CFDF + "<tr>\r\n";
            CFDF = CFDF + " <td scope=\"col\">"  + A2_d + "</td>\r\n";
            CFDF = CFDF + "</tr>\r\n";
            CFDF = CFDF + "</table>\r\n";

            */

            xkioST12 = null;

            return CFDF;


        }


        /// <summary>
        /// 得到主类别列表
        /// </summary>
        /// <param name="SELTxt">预设选项</param>
        /// <param name="SWord">预设词</param>
        /// <returns></returns>
        public  static string  GetMainTypeListHtm( string SELTxt,string SWord)
        {
            SELTxt=SELTxt.Trim();
            string GGG = "<select name=\"select0\" id=\"select0\"  onchange=\"javascript:changeselect(select0.selectedIndex);\">";

           
            if (SELTxt.Length > 0)
            { 
               GGG = GGG +"    <option  value = \""+ SELTxt +"\">"+  SELTxt+"</option>\r\n";
            }

            foreach (string TT in GetMainTypeList)
            {
                if (TT == SELTxt)
                { }
                else
                {
                    GGG = GGG + "    <option  value = \"" + TT + "\">" + TT + "</option>\r\n";     
                }
            
            }

            GGG = GGG + "</select>\r\n";

               GGG = GGG + "<input name=\"wdd\" type=\"text\" id=\"wdd\" value=\""+SWord +"\" />";

               return GGG;
            /*
            <select name="select0" id="select0"  onchange="javascript:changeselect(select0.selectedIndex);">
              <option  value = "笑话">笑话</option>
              <option  value = "房产">房产</option>
              <option  value = "招聘">招聘</option>
              <option  value = "火车票">火车票</option>
              <option  value = "交友">交友</option>
              <option  value = "经验">经验</option>
              <option  value = "新闻">新闻</option>
              <option  value = "旅游">旅游</option>
              <option  value = "交流">交流</option>
              <option  value = "下载">下载</option>
            </select>
            <input name="wdd" type="text" id="wdd" value="北京" />
            */


        }



        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string getFileData(string filename)
        {

            StreamReader reader = null;
            string data = string.Empty;
            try
            {
                reader = new StreamReader(filename, System.Text.Encoding.GetEncoding("gb2312"));

                data = reader.ReadToEnd();

                reader.Close();
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
        /// 写文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        private static void SaveFileData(string filename, string data)
        {


            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("gb2312"));
                writer.Write(data);
                writer.Close();
            }
            catch (IOException e)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }


        /// <summary>
        /// 得到页码使用的HTML
        ///  
        /// 每页10个 每10页 显示一屏
        /// </summary>
        /// <param name="url">
        /// "/s?wd=TYPE|title|body|type1|type2&  ws=0&wl=10";
        /// </param>
        /// <param name="allNum">总的数目</param>
        /// <param name="sNum">开始数目</param>
        /// <param name="lNum">长度  一般为10</param>
        /// <returns></returns>
        public static string GetPageNumHTML(string url, int allNum, int sNum, int lNum)
        {

            //变化得到页数

            //总页数
            int AllPage =0;
            if(allNum%10==0)
            {
               AllPage = allNum/10;
            }
            else
            {
               AllPage = allNum/10+1;
            }

            //当前页
            int NowPage =0;

           if((sNum+1)%10==0)
            {
               NowPage = (sNum+1)/10;
            }
            else
            {
               NowPage = (sNum+1)/10+1;
            }

            //限制
            int x_x = 0;

            if( allNum>  (NowPage+9)*10)
            {
               x_x = (NowPage+9)*10;
            }
            else
            {
              x_x = allNum;
            }

            ArrayList vg = new ArrayList();

            string CShow = "";

            for (int i = (NowPage-1)*10; i < x_x; i++)
            {
                int TNowPage = 0;
                if ((i + 1) % 10 == 0)
                {
                    TNowPage = (i + 1) / 10;
                }
                else
                {
                    TNowPage = (i + 1) / 10 + 1;
                }

                if (vg.Contains(TNowPage) == false)
                {

                    vg.Add(TNowPage);
                    //ws=0&wl=10
                    if (TNowPage == NowPage)
                    {
                        CShow = CShow + TNowPage + " &nbsp;&nbsp; ";
                    }
                    else
                    {
                        CShow = CShow + "<a href=\"" + url + "ws=" + i + "&wl=10" + "\" target=\"_self\">" + TNowPage + "</a> &nbsp;&nbsp;  ";
                    }
                }
            }


            /*
            <a href="56" target="_self">上一页</a> 
<a href="56" target="_self">22</a>
 <a href="54" target="_self">2</a>
 <a href="stgsg" target="_self">下一页</a>

            */

            return CShow;
        }

    }
}
