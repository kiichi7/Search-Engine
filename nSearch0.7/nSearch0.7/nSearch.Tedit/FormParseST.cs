using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
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

namespace nSearch.Tedit
{
    public partial class FormParseST : Form
    {

        /// <summary>
        /// 模板匹配类
        /// </summary>
        private nSearch.ClassLibraryStruct.ClassUserModel mxWeb = new nSearch.ClassLibraryStruct.ClassUserModel();

        //加载到的模版数据
        ArrayList n;

        
            //文件ID对照表
        ArrayList mID = new ArrayList();

        public FormParseST()
        {
            InitializeComponent();

            mID.Clear();
        }

        private void FormParseST_Load(object sender, EventArgs e)
        {
            textBox14.Text = nSearch.ConfigX.ClassConfig.path_Model;
            textBox1.Text = nSearch.ConfigX.ClassConfig.path_XLFS;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            xxClear();

            GH();


            if (mID.Count > 0)
            { }
            else
            {

                //文件系统初始化
                nSearch.FS.ClassFSMD.Init(textBox1.Text);
                nSearch.DebugShow.ClassDebugShow.WriteLineF("文件系统初始化");

                //文件ID对照表
                  mID = nSearch.FS.ClassFSMD.GetUrlList();

            }


        }


        private void GH()
        { 
        
        
            button5.Enabled = false;
            int num = mxWeb.init(textBox14.Text);

            n = mxWeb.n;

            listBox1.Items.Clear();
            for (int i = 0; i < n.Count; i++)
            {
                nSearch.ClassLibraryStruct.auto2dat dfg = (nSearch.ClassLibraryStruct.auto2dat)n[i];
                listBox1.Items.Add(dfg.TmpName);
            }

            MessageBox.Show("共有模版数量： " + n.Count.ToString());
            groupBox1.Enabled = true;


        
        }



        



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            xxClear();
            if (listBox1.Text == null)
            {
                return;
            }

            if (listBox1.Text == "")
            {
                return;
            }

             

           string hhhh = listBox1.Text;

            nSearch.ClassLibraryStruct.auto2dat dfg;

            for (int i = 0; i < n.Count; i++)
            {
                dfg = (nSearch.ClassLibraryStruct.auto2dat)n[i];



                if (hhhh == dfg.TmpName)
                {
                    goto VGF;
                
                }
            }

            MessageBox.Show("ERR");

            return;
        VGF: ;


            nSearch.ClassLibraryStruct.auto2dat one = dfg;

            textBox_a.Text = one.A;
            textBox_b.Text = one.B;
            textBox_c.Text = one.C;
            textBox_d.Text = one.D;
            textBox_e.Text = one.E;
            textBox_h.Text = one.H;
            textBox_t.Text = one.T;
            textBox_m.Text = one.M;

            textBox_TA1.Text = one.A_TYPE_1;
            textBox_TA2.Text = one.A_TYPE_2;



            label1.Text = one.TmpName;

          //  label5.Text = " * = " + one.isXnum.ToString();

             textBox_f.Text = one.F;

            comboBox_f.Items.Clear();

            char[] xxff = {'\r','\n' };

            string[] cccc = one.F.Split(xxff);

            foreach (string yui in cccc)
            {

                comboBox_f.Items.Add(yui);

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

         
            /*
            textBox12.Text = k["t"].ToString();
            textBox8.Text = k["a"].ToString();
            textBox7.Text = k["b"].ToString();
            textBox6.Text = k["c"].ToString();
            */

            /*
            textBox12.Text = k.T;
            textBox_m.Text = k.a;
            textBox7.Text = k.b;
            textBox_e1.Text = k.c;
            textBox16.Text = k.h;
            textBox_c.Text = k.s;
            */
        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            
            button5.Enabled = true;

            groupBox1.Enabled = false;
            

            //保存

            string aPath = textBox14.Text;

            if (aPath[aPath.Length - 1] == '\\')
            { }
            else
            {
                aPath = aPath + "\\";
            }

            aPath = aPath + label1.Text;


            SaveFileData(aPath+".a", textBox_a.Text);
            SaveFileData(aPath + ".b", textBox_b.Text);
            SaveFileData(aPath + ".c", textBox_c.Text);
            SaveFileData(aPath + ".d", textBox_d.Text);
            SaveFileData(aPath + ".e", textBox_e.Text);
            SaveFileData(aPath + ".f", textBox_f.Text);
            SaveFileData(aPath + ".h", textBox_h.Text);
            SaveFileData(aPath + ".m", textBox_m.Text);
            SaveFileData(aPath + ".t", textBox_t.Text);

            SaveFileData(aPath + ".a1", textBox_TA1.Text );
            SaveFileData(aPath + ".a2", textBox_TA2.Text );

            GH();
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        private void SaveFileData(string filename, string data)
        {


            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("gb2312"));
                writer.Write(data);
                writer.Close();
            }
            catch (IOException e)
            {
                nSearch.DebugShow.ClassDebugShow.WriteLineF(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
           


            button5.Enabled = true;

            groupBox1.Enabled = false;

              string aPath = textBox14.Text;

            if (aPath[aPath.Length - 1] == '\\')
            { }
            else
            {
                aPath = aPath + "\\";
            }

            aPath = aPath + label1.Text;

            if(System.IO.File.Exists(aPath+".a"))
            {
                System.IO.File.Delete(aPath + ".a");
            }



            if (System.IO.File.Exists(aPath + ".b"))
            {
                System.IO.File.Delete(aPath + ".b");
            }


            if (System.IO.File.Exists(aPath + ".c"))
            {
                System.IO.File.Delete(aPath + ".c");
            }


            if (System.IO.File.Exists(aPath + ".d"))
            {
                System.IO.File.Delete(aPath + ".d");
            }



            if (System.IO.File.Exists(aPath + ".e"))
            {
                System.IO.File.Delete(aPath + ".e");
            }



            if (System.IO.File.Exists(aPath + ".f"))
            {
                System.IO.File.Delete(aPath + ".f");
            }


            if (System.IO.File.Exists(aPath + ".h"))
            {
                System.IO.File.Delete(aPath + ".h");
            }


            if (System.IO.File.Exists(aPath + ".m"))
            {
                System.IO.File.Delete(aPath + ".m");
            }



            if (System.IO.File.Exists(aPath + ".t"))
            {
                System.IO.File.Delete(aPath + ".t");
            }

            if (System.IO.File.Exists(aPath + ".a1"))
            {
                System.IO.File.Delete(aPath + ".a1");
            }



            if (System.IO.File.Exists(aPath + ".a2"))
            {
                System.IO.File.Delete(aPath + ".a2");
            }

            MessageBox.Show("删除成功");



            label1.Text = "";
            xxClear();
            GH();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_f_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (comboBox_f.Text.Length > 0)
            {

              string   aurl  = comboBox_f.Text;

                int id = -1;

                for (int uiy = 0; uiy < mID.Count; uiy++)
                {
                    if (aurl == mID[uiy].ToString())
                    {
                        id = uiy;
                        break;
                    }
                }

                if (id > -1)
                {
                    string newDat = nSearch.FS.ClassFSMD.GetOneDat(id).HtmDat;

                    nSearch.ClassLibraryHTML.ClassHTML myTag = new nSearch.ClassLibraryHTML.ClassHTML();

                    nSearch.DebugShow.onePage mmm = (nSearch.DebugShow.onePage)myTag.GetOnePage(newDat, "");

                    textBox7.Text = mmm.Body;

                }
            
            
            
            
            }



        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            nSearch.ClassLibraryHTML.ClassHTML dd = new nSearch.ClassLibraryHTML.ClassHTML();

            //建立滤波类 
            nSearch.ClassLibraryStruct.ClassUserModel m = new nSearch.ClassLibraryStruct.ClassUserModel();

            //压入测试模板
            m.TestModeL(textBox_a.Text, textBox_b.Text, textBox_c.Text, textBox_d.Text, textBox_e.Text, textBox_t.Text, textBox_h.Text, "", textBox_TA1.Text , textBox_TA2.Text );

            nSearch.ClassLibraryStruct.auto2dat k = m.getTagAndData(textBox7.Text);

            Hashtable p = m.modelOneList;


            
            listBox2.Items.Clear();

            foreach (System.Collections.DictionaryEntry de in p)
            {

                listBox2.Items.Add(de.Key.ToString() + '\t' + de.Value.ToString());
            }


            textBox_a1.Text = k.A;
            textBox_b1.Text = k.B;
            textBox_e1.Text = dd.GetClearCode(   k.E,true);

            textBox_c1.Text = k.C;

            textBox_d1.Text = k.D;

            textBox_TAc1.Text = k.A_TYPE_1;
            textBox_TAc2.Text = k.A_TYPE_2;


        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(listBox2.Text.Length==0)
            {
                return;
            }

            string dfd = listBox2.Text;

          //  int xx = Int32.Parse(dfd);

            textBox13.Text = dfd; // n[xx].ToString();





        }


        /// <summary>
        /// 清理
        /// </summary>
        private void xxClear()
        {

            label1.Text = "";

            textBox_a.Text = "";
            textBox_a1.Text = "";
            textBox_b.Text = "";
            textBox_b1.Text = "";
            textBox_c.Text = "";
            textBox_c1.Text = "";
            textBox_d.Text = "";
            textBox_d1.Text = "";
            textBox_e.Text = "";
            textBox_e1.Text = "";
            textBox_f.Text = "";
            textBox_h.Text = "";
            textBox_m.Text = "";
            textBox_t.Text = "";
            textBox_TA1.Text = "";
            textBox_TA2.Text = "";
            textBox_TAc2.Text = "";
            textBox_TAc1.Text = "";
            textBox13.Text = "";
            textBox7.Text = "";
            comboBox_f.Items.Clear();
            comboBox_f.Text = "";
        
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