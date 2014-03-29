using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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

namespace nSearch.SUrlEdit
{
    public partial class FormSUrlEdit : Form
    {
        public FormSUrlEdit()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            string aaa = getFileData(textBox1.Text);

            if (aaa.Length == 0)
            {
                return;
            }

            char[] vv = { '\r', '\n' };

            string[] nd = aaa.Split(vv);



            foreach (string a in nd)
            {
                if (a.Length > 0)
                {
                    listBox1.Items.Add(a);
                }

            }

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
        private void SaveFileData(string filename, string data)
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

        private void button1_Click(object sender, EventArgs e)
        {


            if (textBox2.Text.ToLower().IndexOf("http://") == -1)
            {
                MessageBox.Show("不能使用默认也　必需具体到一个有名字的页面   " + textBox2.Text);
                return;
            }

            textBox2.Text = textBox2.Text.Trim();

            string attt = textBox2.Text.ToLower().Replace("http://","");

            int xlt = attt.LastIndexOf('/');

            if (xlt <6)
            {
                MessageBox.Show("不能使用默认也　必需具体到一个有名字的页面   " + textBox2.Text);
                return;
            }

            int xlt2 = attt.IndexOf('.',xlt+1);

            if (xlt2 < 6)
            {
                MessageBox.Show("不能使用默认也　必需具体到一个有名字的页面   " + textBox2.Text);
                return;
            }

            if (textBox2.Text.Length > 0)
            {

                foreach (string aa in listBox1.Items)
                {
                    if (textBox2.Text.Trim() == aa)
                    {

                        MessageBox.Show("该值已存在");
                        return;
                    }
                    else
                    {



                    }
                }

                listBox1.Items.Add(textBox2.Text);
                textBox2.Text = "http://";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            textBox2.Text = textBox2.Text.Trim();

            if (textBox2.Text.ToLower().IndexOf("http://") == -1)
            {
                return;
            }


            if (textBox2.Text.Length == 0)
            {
                return;
            }

            if (textBox2.Text.Length > 0)
            {

                foreach (string aa in listBox1.Items)
                {
                    if (textBox2.Text.Trim() == aa)
                    {

                        goto CXX;
                    }
                    else
                    {



                    }
                }
                MessageBox.Show("该值不存在");

                textBox2.Text = "http://";
                return;
            }

        CXX: ;

            listBox1.Items.Remove(textBox2.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string AAA = "";

            foreach (string aa in listBox1.Items)
            {
                AAA = AAA + aa + "\r\n";
            }

            SaveFileData(textBox1.Text, AAA);

            MessageBox.Show("保存成功！！！");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = listBox1.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormAbout xAbout = new FormAbout();
            xAbout.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void FormSUrlEdit_Load(object sender, EventArgs e)
        {
            textBox1.Text = nSearch.ConfigX.ClassConfig.path_StartTxt;
        }
    }
}