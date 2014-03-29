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

namespace nSearch.Main
{
    /// <summary>
    /// �������ݴ洢��  http://127.0.0.1:81/s?wd=�й�
    /// </summary>
    public static class ClassST
    {

        /// <summary>
        /// �ִ���
        /// </summary>
        public  static Lucene.Net.Analysis.Analyzer OneAnalyzer ;

        /// <summary>
        /// ��������б�
        /// </summary>
        public static Hashtable nMainType = new Hashtable();

        /// <summary>
        /// ��Žڵ��б�
        /// </summary>
        public static ArrayList vbn = new ArrayList();

        /// <summary>
        /// ������б��ļ�  �����.main
        /// </summary>
        private  static ArrayList GetMainTypeList = new ArrayList();


        /// <summary>
        /// ģ���ļ� &lt;--HTTP://ZD4004.BLOG.163.COM--&gt;
        /// </summary>
        public static string ModelHTM;

        /// <summary>
        /// �趨�ڵ��б�
        /// </summary>
        /// <param name="xxx"></param>
        /// <param name="TypeRS">������ԵĴ��·��</param>
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


            // ��������б�
            Hashtable nTypeRSLIST = new Hashtable();
            nTypeRSLIST.Clear();

            // 1 �õ�Ŀ¼�µ��ļ�
            DirectoryInfo dir = new DirectoryInfo(TypeRS);

            // 2 �����ļ�������ȡ����ѹ��
            foreach (FileInfo f in dir.GetFiles("*.txt"))   //���������xmlΪ��չ�����ļ�   
            {
                String name = f.FullName;         //nameΪ���ļ����µ��ļ����ƣ���f.FullNameΪȫ��  

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

            string ListRT = getFileData(TypeRS + "�����.main");
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
        /// �õ���ѡ�б�
        /// </summary>
        /// <param name="nnmm">��ѡ�б�ԭʼ����</param>
        /// <param name="ST1">��ѡ�б�һԤ��ֵ</param>
        /// <param name="ST2">��ѡ�б��Ԥ��ֵ</param>
        /// <returns></returns>
        public  static string GetBoxListDat1(string KWORD, string ST1, string ST2)
        {


            if (nMainType.Contains(KWORD) == false)
            {
                //û���ҵ������
                return "";
            }

            ArrayList xxbb = (ArrayList)nMainType[KWORD];

            string A1_d = "<select name=\"selectKCA1\">\r\n" + " <option value=\"" + ST1 + "\">" + ST1 + "</option>\r\n";
            string A2_d = "<select name=\"selectKCA2\">\r\n" + " <option value=\"" + ST2 + "\">" + ST2 + "</option>\r\n";

            //����
            ArrayList xkioST12 = new ArrayList();
            xkioST12.Add(ST1);
            xkioST12.Add(ST2);

            foreach (string xsa in xxbb)
            {
                string[] xkio = xsa.Split('\t');

                if (xkioST12.Contains(xkio[1]) == false)  //��ֹһ�����Ա��ظ�ѹ��  ���� ���һ �������ظ�����
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
        /// �õ���ѡ�б�
        /// </summary>
        /// <param name="nnmm">��ѡ�б�ԭʼ����</param>
        /// <param name="ST1">��ѡ�б�һԤ��ֵ</param>
        /// <param name="ST2">��ѡ�б��Ԥ��ֵ</param>
        /// <returns></returns>
        public static string GetBoxListDat2(string KWORD, string ST1, string ST2)
        {


            if (nMainType.Contains(KWORD) == false)
            {
                //û���ҵ������
                return "";
            }

            ArrayList xxbb = (ArrayList)nMainType[KWORD];

            string A1_d = "<select name=\"selectKCA1\" id=\"selectKCA1\">\r\n" + " <option value=\"" + ST1 + "\">" + ST1 + "</option>\r\n";
            string A2_d = "<select name=\"selectKCA2\"  id=\"selectKCA2\">\r\n" + " <option value=\"" + ST2 + "\">" + ST2 + "</option>\r\n";

            //����
            ArrayList xkioST12 = new ArrayList();
            xkioST12.Add(ST1);
            xkioST12.Add(ST2);

            foreach (string xsa in xxbb)
            {
                string[] xkio = xsa.Split('\t');

                if (xkioST12.Contains(xkio[1]) == false)  //��ֹһ�����Ա��ظ�ѹ��  ���� ���һ �������ظ�����
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
        /// �õ�������б�
        /// </summary>
        /// <param name="SELTxt">Ԥ��ѡ��</param>
        /// <param name="SWord">Ԥ���</param>
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
              <option  value = "Ц��">Ц��</option>
              <option  value = "����">����</option>
              <option  value = "��Ƹ">��Ƹ</option>
              <option  value = "��Ʊ">��Ʊ</option>
              <option  value = "����">����</option>
              <option  value = "����">����</option>
              <option  value = "����">����</option>
              <option  value = "����">����</option>
              <option  value = "����">����</option>
              <option  value = "����">����</option>
            </select>
            <input name="wdd" type="text" id="wdd" value="����" />
            */


        }



        /// <summary>
        /// ���ļ�
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
        /// д�ļ�
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
        /// �õ�ҳ��ʹ�õ�HTML
        ///  
        /// ÿҳ10�� ÿ10ҳ ��ʾһ��
        /// </summary>
        /// <param name="url">
        /// "/s?wd=TYPE|title|body|type1|type2&  ws=0&wl=10";
        /// </param>
        /// <param name="allNum">�ܵ���Ŀ</param>
        /// <param name="sNum">��ʼ��Ŀ</param>
        /// <param name="lNum">����  һ��Ϊ10</param>
        /// <returns></returns>
        public static string GetPageNumHTML(string url, int allNum, int sNum, int lNum)
        {

            //�仯�õ�ҳ��

            //��ҳ��
            int AllPage =0;
            if(allNum%10==0)
            {
               AllPage = allNum/10;
            }
            else
            {
               AllPage = allNum/10+1;
            }

            //��ǰҳ
            int NowPage =0;

           if((sNum+1)%10==0)
            {
               NowPage = (sNum+1)/10;
            }
            else
            {
               NowPage = (sNum+1)/10+1;
            }

            //����
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
            <a href="56" target="_self">��һҳ</a> 
<a href="56" target="_self">22</a>
 <a href="54" target="_self">2</a>
 <a href="stgsg" target="_self">��һҳ</a>

            */

            return CShow;
        }

    }
}
