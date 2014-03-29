using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

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
    public partial class FormMain : Form
    {

        /// <summary>
        /// TTTT
        /// </summary>
        ClassMainS nSTV = new ClassMainS();

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            if (button1.Text == "开始")
            {

                button1.Text = "结束";
            }
            else
            {
                button1.Text = "开始";

            }
            */
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            linkLabel1.Text = textBox5.Text;

            char[] ji = {'\r','\n' };
            string[] xH = textBox1.Text.Split(ji);

            ArrayList cgt = new ArrayList();

            cgt.Clear();



            foreach (string uy in xH)
            {
                if (uy.Length > 7 & uy.IndexOf(':') > 5)
                {
                    cgt.Add(uy);
                }
            }

            ClassST.SetVbn((ArrayList)cgt.Clone(),textBox3.Text,textBox4.Text );


            if (checkBox2.Checked == true )
            {
                nSTV.Set_IP = textBox6.Text;
                nSTV.Set_Port = Int32.Parse(textBox7.Text);
                nSTV.StartRun();

                nSearch.DebugShow.ClassDebugShow.WriteLine("开始监听....");

                linkLabel1.Text = textBox5.Text;//   "http://" + textBox6.Text + ":" + textBox7.Text + "/s?wd=下载|title|电子|type1|type2&ws=0&wl=10";

            }
            else
            {
                nSTV.StopRun();
                nSearch.DebugShow.ClassDebugShow.WriteLine("停止监听");
            }








        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 200;
            timer1.Enabled = checkBox1.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            nSTV.StopRun();


            Application.Exit();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
    string xxx = nSearch.DebugShow.ClassDebugShow.showf();

            if (xxx.Length > 0)
            {
                textBox2.AppendText(xxx);

                if (textBox2.Text.Length > 1024 * 128)
                {
                    textBox2.Text = "";
                }
                // .Items.Add(xxx);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.Text = textBox5.Text;
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            textBox3.Text = nSearch.ConfigX.ClassConfig.path_mHTML;
            textBox4.Text = nSearch.ConfigX.ClassConfig.path_TypeData;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormAbout xAbout = new FormAbout();
            xAbout.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            linkLabel1.Text = textBox5.Text;
        }
    }
}