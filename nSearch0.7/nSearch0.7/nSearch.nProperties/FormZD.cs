using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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


namespace nSearch.nProperties
{
    /// <summary>
    /// 
    ///  自动获取一个特征表  避免需要自动类聚生成该特征表
    /// 
    /// 得到每个主类别 和 对于的选项 选项值  取消实时的自动类聚
    /// 
    /// </summary>
    public partial class FormZD : Form
    {
        //处理类
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
            this.Cursor = System.Windows.Forms.Cursors.Arrow;//恢复！ 

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