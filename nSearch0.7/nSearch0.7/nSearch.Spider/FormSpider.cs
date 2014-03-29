using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.Spider
{
    public partial class FormSpider : Form
    {

        ClassSpider nSpider = new ClassSpider();

        public FormSpider()
        {
         

            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = nSpider.GetShow();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (button1.Text == "开始")
            {

                button1.Text = "结束";

                comboBox1.Enabled = false;

            timer1.Interval = 200;
            timer1.Enabled = true;
            
            button1.Enabled = false;
            nSpider.Init( textBox1.Text, textBox2.Text);

            string DSD = comboBox1.Text;

            int ss = Int32.Parse(DSD);

            nSpider.StartRun(ss);

            }
            else
            {
                button1.Text = "开始";
                comboBox1.Enabled = true;
                timer1.Enabled = false;

                nSpider.StopRun();
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button1.Enabled == false)
            {
                button3.Enabled = false;
                nSpider.StopRun();
            }
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            string xxx = nSearch.DebugShow.ClassDebugShow.showf();

            if (xxx.Length > 0)
            {
                textBox3.AppendText(xxx);

                if (textBox3.Text.Length > 1024 * 128)
                {
                    textBox3.Text = "";
                }
                // .Items.Add(xxx);
            }
        }

        private void FormSpider_Load(object sender, EventArgs e)
        {
            textBox1.Text = nSearch.ConfigX.ClassConfig.path_XLFS;
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_UrlCent;
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
    }
}