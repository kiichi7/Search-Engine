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

namespace nSearch.Run
{
    public partial class FormRun : Form
    {
        public FormRun()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.SUrlEdit.exe","");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory+  "nSearch.Spider.exe", "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.TheSame.exe", "");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.ModelBuild.exe", "");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.Index.exe", "");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.XWORDEDIT.exe", "");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.Main.exe", "");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.SearchOne.exe", "");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", AppDomain.CurrentDomain.BaseDirectory + "Txt.txt");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.Tedit.exe", "");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("notepad.exe", AppDomain.CurrentDomain.BaseDirectory + "Config.txt");

            nSearch.ConfigX.FormConfig vv = new nSearch.ConfigX.FormConfig();
            vv.ShowDialog();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.nProperties.exe", "");
    
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.nProperties.exe", "");
    
        }

        private void button12_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.UrlMain.exe", "");
    
        }

        private void button13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "nSearch.xOcx.exe", "");
    
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

        private void button14_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}