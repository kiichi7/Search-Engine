using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lucene.Net.Documents;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */


namespace nSearch.Index
{
    public partial class FormIndex : Form
    {

        /// <summary>
        /// ������
        /// </summary>
        ClassIndex nIndex = new ClassIndex();


        public FormIndex()
        {
            InitializeComponent();
        }

        private void FormIndex_Load(object sender, EventArgs e)
        {
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_XLFS;
            textBox1.Text = nSearch.ConfigX.ClassConfig.path_Model;
            textBox4.Text = nSearch.ConfigX.ClassConfig.path_Index;
            textBox5.Text = nSearch.ConfigX.ClassConfig.path_AIDStart;
        }


        /// <summary>
        /// ���� ѭ������ 
        /// </summary>
        private void StartRun()
        {


            nIndex.InitRum(textBox2.Text, textBox1.Text,textBox4.Text,textBox5.Text );
            


        }


     

        

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Enabled = false ;


            if (button1.Text == "��ʼ")
            {
                StartRun();
                button1.Text = "����";
            }
            else
            {
                button1.Text = "��ʼ";
                nIndex.Stop();
                Application.Exit();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 200;
            timer1.Enabled = checkBox1.Checked;

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