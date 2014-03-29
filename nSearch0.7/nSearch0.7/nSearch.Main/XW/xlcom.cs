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

            //�Ƿ�ȫ��Ϊ����
            if (isNum(dat) == true )
            {

                return dat + "/n";

            }


            //�жϾ������Ƿ�������

            if (WordsIScn(dat) == false)
            {

                return dat + "/n";

            }





            int KJG = dat.Replace(" ", "").Length;
            int lensDat = dat.Length;

            //���пո���ַ�����
            if (lensDat - KJG > lensDat / 10 & lensDat > 80)
            {


                //�ո�̫�� �����зִ��� 
                return dat;
            }

            //���пո���ַ�����
            if (lensDat - KJG > lensDat / 3)
            {


                //�ո�̫�� �����зִ��� 
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

            if (CC < 3 | CC <= dat.Length * 0.3)  //�����ַ���������
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

            aaa = aaa + "zz"; //��־������
            /*aa�S0
ab�L1
ac��2
ad��3
ae��4
af��5
ag��6
ah��7
ai�@8
aj��9
             * zz ��
             * */
            //ȷ�� ����0-255 ֮������� ����

            try
            {
                string tText = axaa_xl_word1.aa_X_word_ocx(aaa);

                string d = tText.Trim();
                //// �ϲ����Ʋ���
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
        /// �ϲ����Ʋ���
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
        ///	�жϾ������Ƿ�������
        /// </summary>
        /// <param name="words">�ַ���</param> 
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
        /// �Ƿ�ȫΪ�������
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