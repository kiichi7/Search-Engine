using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
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

namespace nSearch.ModelBuild
{
    public partial class FormModelBuild : Form
    {

        nSearch.ModelBuild.BuildZXL.ClassAuto nn = new nSearch.ModelBuild.BuildZXL.ClassAuto();

        Thread TT;

        public FormModelBuild()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            timer1.Interval = 200;
            timer1.Enabled = true;

            nn.SetPath(textBox3.Text, textBox1.Text, textBox2.Text);

           // nn.StartRun();

           TT = new Thread(new ThreadStart(nn.StartRun));
            TT.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            string xxx = nSearch.DebugShow.ClassDebugShow.showf();

            if (xxx.Length > 0)
            {
                textBox4.AppendText(xxx);

                if (textBox4.Text.Length > 1024 * 128)
                {
                    textBox4.Text = "";
                }
                // .Items.Add(xxx);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                nn.IsStop = true;

                TT.Abort();
            }
            catch
            { }

            Application.Exit();

        }

        private void FormModelBuild_Load(object sender, EventArgs e)
        {
            textBox3.Text = nSearch.ConfigX.ClassConfig.path_XLFS;
            textBox1.Text = nSearch.ConfigX.ClassConfig.path_T;
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_Model;
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