using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */


namespace nSearch.nProperties
{
    /// <summary>
    /// 
    ///  �Զ���ȡһ��������  ������Ҫ�Զ�������ɸ�������
    /// 
    /// �õ�ÿ������� �� ���ڵ�ѡ�� ѡ��ֵ  ȡ��ʵʱ���Զ����
    /// 
    /// </summary>
    public partial class FormZD : Form
    {
        //������
        ClassGP xxpp = new ClassGP();

        Thread TT;

        public FormZD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            nSearch.DebugShow.ClassDebugShow.WriteLineF("--->>");

            xxpp.Init(textBox1.Text, textBox2.Text);
            nSearch.DebugShow.ClassDebugShow.WriteLineF("--->>Index " + textBox1.Text );
            nSearch.DebugShow.ClassDebugShow.WriteLineF("--->> Data " + textBox2.Text);

            TT = new Thread(new ThreadStart(xxpp.SearchGet));
            TT.Start();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 200;
            timer1.Enabled = checkBox2.Checked;
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

        private void FormZD_Load(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//�ָ��� 

            textBox1.Text = nSearch.ConfigX.ClassConfig.path_Index;
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_TypeData;

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