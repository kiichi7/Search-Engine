using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.TheSame
{
    public partial class FormTheSame : Form
    {
        public FormTheSame()
        {
            InitializeComponent();
        }


        ClassTheSame cc = new ClassTheSame();

        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            
            timer1.Interval = 200;
            timer1.Enabled = true;

            cc.fspath = textBox1.Text;
            cc.tspath = textBox2.Text;

            Thread TT = new Thread(new ThreadStart(cc.StartX));
            TT.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string xxx = nSearch.DebugShow.ClassDebugShow.showf();

            if (xxx.Length > 0)
            {
                textBox3.AppendText(xxx);
              // .Items.Add(xxx);
            }
        }

        private void FormTheSame_Load(object sender, EventArgs e)
        {
            textBox1.Text = nSearch.ConfigX.ClassConfig.path_XLFS;
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_T;
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