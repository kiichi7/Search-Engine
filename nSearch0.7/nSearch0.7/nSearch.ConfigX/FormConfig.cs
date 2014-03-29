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

namespace nSearch.ConfigX
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string aa = "";

            openFileDialog1.ShowDialog();

            aa = openFileDialog1.FileName;

            if (aa.Length < 5)
            {
                return;
            }

            textBox10.Text = aa;

            nSearch.ConfigX.ClassConfig.InitConfigData(aa);

            
            textBox5.Text = ClassConfig.path_AIDStart;
            textBox4.Text = ClassConfig.path_Index;
            textBox6.Text = ClassConfig.path_mHTML;
            textBox1.Text = ClassConfig.path_Model;
            textBox7.Text = ClassConfig.path_T;
            textBox3.Text = ClassConfig.path_TypeData;
            textBox2.Text = ClassConfig.path_XLFS;
            textBox8.Text = ClassConfig.path_UrlCent;
            textBox9.Text = ClassConfig.path_StartTxt;
            button2.Enabled = true;
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            ClassConfig.path_AIDStart = textBox5.Text;
            ClassConfig.path_Index = textBox4.Text;
            ClassConfig.path_mHTML = textBox6.Text;
            ClassConfig.path_Model = textBox1.Text;
            ClassConfig.path_T = textBox7.Text;
            ClassConfig.path_TypeData = textBox3.Text;
            ClassConfig.path_XLFS = textBox2.Text;
            ClassConfig.path_UrlCent = textBox8.Text;
        ClassConfig.path_StartTxt  =  textBox9.Text;  


            if (ClassConfig.path_AIDStart.Trim().Length == 0)
            { 
               MessageBox.Show("参数为空！");
               return;
            }

            if (ClassConfig.path_TypeData.Trim().Length == 0)
            {
                MessageBox.Show("参数为空！");
                return;
            }


            if (ClassConfig.path_XLFS.Trim().Length == 0)
            {
                MessageBox.Show("参数为空！");
                return;
            }

            if (ClassConfig.path_Index.Trim().Length == 0)
            {
                MessageBox.Show("参数为空！");
                return;
            }

            System.Threading.Thread.Sleep(2000);

            button2.Enabled = true;

            nSearch.ConfigX.ClassConfig.SaveConfigData(textBox10.Text );
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