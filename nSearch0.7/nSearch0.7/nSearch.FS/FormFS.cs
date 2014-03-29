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

namespace nSearch.FS
{
    public partial class FormFS : Form
    {


        ArrayList xxx = new ArrayList();

        public FormFS()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            ClassFSMD.Init(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassFSMD.PutOneDat(textBox1.Text, textBox4.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            xxx = ClassFSMD.GetUrlList();

            listBox1.Items.Clear();

            for (int i = 0; i < xxx.Count; i++)
            {
                listBox1.Items.Add(i.ToString());
            
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox3.Text = 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClassFSMD.SaveExit();

            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Text.Length == 0)
            {
                return;
            }

            int xxxxxx = Int32.Parse(listBox1.Text);

            textBox3.Text = xxx[xxxxxx].ToString();


            textBox5.Text = ClassFSMD.GetOneDat(xxxxxx).HtmDat;

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int xxxxxx = Int32.Parse(textBox6.Text);

           


            textBox5.Text = ClassFSMD.GetOneDat(xxxxxx).HtmDat;

            nSearch.ClassLibraryHTML.ClassHTML dd = new nSearch.ClassLibraryHTML.ClassHTML();
            textBox7.Text = dd.GetClearCode(textBox5.Text , true);


        }

        private void FormFS_Load(object sender, EventArgs e)
        {
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_XLFS;
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