using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */

namespace nSearch.UrlMain
{
    public partial class FormUrlMain : Form
    {
        public FormUrlMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = ClassSTURL.GetTongji();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            button2.Enabled = false;
            ClassSTURL.Init(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassSTURL.PutOneUrl(textBox3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
          textBox4.Text =    ClassSTURL.GetOneUrl();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClassSTURL.RePutOneUrl(textBox5.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClassSTURL.SaveExit();
            this.Close();
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