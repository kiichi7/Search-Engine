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


namespace nSearch.SearchOne
{
    public partial class FormTest : Form
    {
        

        /// <summary>
        /// 数据监听类
        /// </summary>
        nSearch.SearchOne.StartClass nSTV = new StartClass();


        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //   ListViewItem item = new ListViewItem(new string[] { null, filename, di.Name, one_tmp.FileSize , hits.Score(i).ToString()});

            ClassSearch nSearchTmp = (ClassSearch)nSearch.SearchOne.ClassST.mSearch;

            nSearch.SearchOne.RSK xRs = nSearchTmp.GetRS(textBox1.Text, 0, 0);

            textBox3.Text = xRs.ALLNum.ToString();
            textBox4.Text = xRs.ANum.ToString();
            textBox5.Text = xRs.BNum.ToString();

            listViewResults.Items.Clear();

            //遍历结果
            foreach (nSearch.SearchOne.OneRs one in xRs.rs)
            {

                ListViewItem item = new ListViewItem(new string[] { null, one.url, one.D, one.B, one.Score.ToString() });

                listViewResults.Items.Add(item);
            }


        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            nSearch.SearchOne.ClassST.Init();

            ClassSearch nSearchTmp = (ClassSearch)nSearch.SearchOne.ClassST.mSearch;

            nSearchTmp.Init(textBox2.Text);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true )
            {
                nSTV.Set_IP = textBox6.Text;
                nSTV.Set_Port = Int32.Parse(textBox7.Text);
                nSTV.StartRun();
            }
            else
            {
                nSTV.StopRun();
            }

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
                textBox8.AppendText(xxx);

                if (textBox8.Text.Length > 1024 * 128)
                {
                    textBox8.Text = "";
                }
                // .Items.Add(xxx);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            nSTV.StopRun();

            Application.Exit();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            textBox2.Text = nSearch.ConfigX.ClassConfig.path_Index;
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