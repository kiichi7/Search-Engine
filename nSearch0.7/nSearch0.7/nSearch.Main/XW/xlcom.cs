using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace nSearch.xOcx
{
    public partial class xlcom : Form
    {
        public xlcom()
        {
            InitializeComponent();
        }





        public string GetXword(string dat)
        {


            if (dat.Length == 0)
            {
                return "";
            }

            //是否全部为数字
            if (isNum(dat) == true )
            {

                return dat + "/n";

            }


            //判断句子中是否含有中文

            if (WordsIScn(dat) == false)
            {

                return dat + "/n";

            }





            int KJG = dat.Replace(" ", "").Length;
            int lensDat = dat.Length;

            //含有空格的字符序列
            if (lensDat - KJG > lensDat / 10 & lensDat > 80)
            {


                //空格太多 不进行分词了 
                return dat;
            }

            //含有空格的字符序列
            if (lensDat - KJG > lensDat / 3)
            {


                //空格太多 不进行分词了 
                return dat;
            }

            int CC = 0;
            foreach (char oneTT in dat)
            {

                if (oneTT < (char)0 | oneTT > (char)255)
                {
                    CC = CC + 1;
                }

            }

            if (CC < 3 | CC <= dat.Length * 0.3)  //其它字符个数很少
            {
                return dat + "/n";
            }



        NEXTTRY2:








            string s1 = dat;

            Encoding e1 = Encoding.GetEncoding(936);
            byte[] bb1 = e1.GetBytes(s1);


            string aaa = "";
            for (int i = 0; i < bb1.Length; i++)
            {
                byte one = bb1[i];
                string one_str = one.ToString();

                if (one_str.Length == 3)
                {
                    aaa = aaa + one_str;
                }
                else
                {
                    if (one_str.Length == 2)
                    {
                        aaa = aaa + "0" + one_str;
                    }
                    else
                    {
                        if (one_str.Length == 1)
                        {
                            aaa = aaa + "00" + one_str;
                        }
                        else
                        {

                        }
                    }
                }



            }


            aaa = aaa.Replace("0", "aa");
            aaa = aaa.Replace("1", "ab");
            aaa = aaa.Replace("2", "ac");
            aaa = aaa.Replace("3", "ad");
            aaa = aaa.Replace("4", "ae");
            aaa = aaa.Replace("5", "af");
            aaa = aaa.Replace("6", "ag");
            aaa = aaa.Replace("7", "ah");
            aaa = aaa.Replace("8", "ai");
            aaa = aaa.Replace("9", "aj");

            aaa = aaa + "zz"; //标志结束符
            /*aa慡0
ab扡1
ac捡2
ad摡3
ae敡4
af晡5
ag条6
ah桡7
ai楡8
aj橡9
             * zz 空
             * */
            //确定 代表0-255 之间的数的 变量

            try
            {
                string tText = axaa_xl_word1.aa_X_word_ocx(aaa);

                string d = tText.Trim();
                //// 合并名称参数
                string d2 = comNameTag(d);


                return d2;
            }
            catch
            {
                return aaa;
            }
        }

        private void xlcom_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 合并名称参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string comNameTag(string data)
        {
            data = data.Replace("   ", " ");
            data = data.Replace("  ", " ");

            string[] myStr = data.Split(' ');

            for (int i = 1; i < myStr.Length; i++)
            {
                if ((myStr[i - 1].IndexOf("/nr") > -1) && (myStr[i].IndexOf("/nr") > -1))
                {
                    string[] my1 = myStr[i - 1].Split('/');
                    string[] my2 = myStr[i].Split('/');
                    myStr[i - 1] = my1[0] + my2[0] + "/nr";
                    myStr[i] = "";
                }
            }

            string myback = "";

            for (int i = 0; i < myStr.Length; i++)
            {
                if (myStr[i].Length > 0)
                {
                    myback = myback + myStr[i] + " ";
                }
            }

            myback = myback.Trim();

            return myback;

        }

        /// <summary>
        ///	判断句子中是否含有中文
        /// </summary>
        /// <param name="words">字符串</param> 
        private bool WordsIScn(string words)
        {
            string TmmP;

            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);

                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);

                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 是否全为数字组成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
            private bool isNum(string data)
        {
            if (data == null | data.Length == 0)
            {
                return false;
            }

            foreach (char a in data)
            {
                if (a < '0' | a > '9' | a!=' ')
                {
                    return false;
                }
            }

            return true;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetXword(textBox1.Text);
        }



    }
}