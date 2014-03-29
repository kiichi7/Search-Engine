using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

//
//   LGPLЭ�鷢��   ����  ���Ĵ�ѧ �Ŷ�  2006.12.29  zd4004@163.com
//
/*
      '       Ѹ�����ķ����������� v0.7  nSearch�� 
      '
      '        LGPL  ��ɷ���
      '
      '       ���Ĵ�ѧ  �Ŷ�   zd4004@163.com
      ' 
      '        ���� http://blog.163.com/zd4004/
 */



namespace XL.XWORDEDIT
{
    /// <summary>
    /// һ���ʵĵ�λ
    /// </summary>
    struct oneXNU
    {
        /// <summary>
        /// Ƶ��
        /// </summary>
        public int a_PL ;

        /// <summary>
        /// �ʳ�
        /// </summary>
        public int a_CC ;

        /// <summary>
        /// ����
        /// </summary>
        public int a_CX ;

        /// <summary>
        /// ����
        /// </summary>
        public string a_Word;
    
    }


    public partial class FormWordEdit : Form
    {
        /// <summary>
        /// ���ÿ���ֵĽṹ����ͨ����ŵõ�����
        /// </summary>
        ArrayList nDict = new ArrayList(6768);

        ArrayList NWORD6768 = new ArrayList(6768);

        /// <summary>
        /// ����·��
        /// </summary>
        public string mm_path = "";

        public FormWordEdit()
        {
            InitializeComponent();


      
        }

        /// <summary>
        /// �б�
        /// </summary>
        private void ItList()
        {

            comboBox1.Items.Clear();

            for (int i = 0; i < 6768; i++)
            {

                string WordCN = GetWordCN(i);

                comboBox1.Items.Add(WordCN);

            }

            comboBox1.Text = GetWordCN(0);

        }

        private void FormWordEdit_Load(object sender, EventArgs e)
        {
            GetTable();
            //dict.dat  bigramtest.dat
          

        }

        /// <summary>
        /// �õ���ֵ
        /// </summary>
        /// <param name="nn"></param>
        /// <returns></returns>
        private int GetIntIt(byte[] nn)
        {
            try
            {
                byte[] m = nn; // �ȶ�ȡ�������õ�����Ĵ�������   //������λ���õ������������ƵĴ�            
                int x_0 = m[0]; int x_1 = m[1]; int x_2 = m[2]; int x_3 = m[3];
                string xx_0 = x_0.ToString("X"); string xx_1 = x_1.ToString("X");
                string xx_2 = x_2.ToString("X"); string xx_3 = x_3.ToString("X");

                if (xx_0.Length == 1)
                {
                    xx_0 = "0" + xx_0;
                }

                if (xx_1.Length == 1)
                {
                    xx_1 = "0" + xx_1;
                }

                if (xx_2.Length == 1)
                {
                    xx_2 = "0" + xx_2;
                }

                if (xx_3.Length == 1)
                {
                    xx_3 = "0" + xx_3;
                }

                string new16 = xx_3 + xx_2 + xx_1 + xx_0;
                int T_xx = System.Convert.ToInt32(new16, 16); //�õ��˱����������

                return T_xx;
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// �õ�����
        /// </summary>
        /// <param name="nn"></param>
        /// <returns></returns>
        private byte[] GetByteIt(int nn)
        {

            byte[] xxx = new byte[4];

            string xx_NN = nn.ToString("X");

            //���
            xx_NN = xx_NN.PadLeft(8, '0');

            string X1 = xx_NN.Substring(0, 2);
            string X2 = xx_NN.Substring(2, 2);
            string X3 = xx_NN.Substring(4, 2);
            string X4 = xx_NN.Substring(6, 2);

            int T_X1 = System.Convert.ToInt32(X1, 16);
            int T_X2 = System.Convert.ToInt32(X2, 16);
            int T_X3 = System.Convert.ToInt32(X3, 16);
            int T_X4 = System.Convert.ToInt32(X4, 16);

            xxx[0] = (byte)T_X4;
            xxx[1] = (byte)T_X3;
            xxx[2] = (byte)T_X2;
            xxx[3] = (byte)T_X1;

            return xxx;
            /*
            try
            {
                byte[] m ; // �ȶ�ȡ�������õ�����Ĵ�������   //������λ���õ������������ƵĴ�            
                int x_0 = m[0]; int x_1 = m[1]; int x_2 = m[2]; int x_3 = m[3];
                string xx_0 = x_0.ToString("X"); string xx_1 = x_1.ToString("X");
                string xx_2 = x_2.ToString("X"); string xx_3 = x_3.ToString("X");

                if (xx_0.Length == 1)
                {
                    xx_0 = "0" + xx_0;
                }

                if (xx_1.Length == 1)
                {
                    xx_1 = "0" + xx_1;
                }

                if (xx_2.Length == 1)
                {
                    xx_2 = "0" + xx_2;
                }

                if (xx_3.Length == 1)
                {
                    xx_3 = "0" + xx_3;
                }

                string new16 = xx_3 + xx_2 + xx_1 + xx_0;
                int T_xx = System.Convert.ToInt32(new16, 16); //�õ��˱����������

                return T_xx;
            }
            catch
            {
                return 0;
            }
            */
        }


        /// <summary>
        /// �õ��ַ�
        /// </summary>
        /// <param name="nn"></param>
        /// <returns></returns>
        private string GetStrIt(byte[] nn)
        {
            //��ȡGB2312����ҳ����
            Encoding gb = Encoding.GetEncoding("gb2312");

            string aaa = gb.GetString(nn);

            return aaa;

        }

        /// <summary>
        /// ������š��õ�����
        /// </summary>
        /// <param name="nn"></param>
        /// <returns></returns>
        private string GetWordCN(int nn)
        {

            return NWORD6768[nn].ToString();

            /*
            //B0A0
            int GBXC = System.Convert.ToInt32("B0A0", 16);
            int newGBXC = GBXC + nn + 1;
            string newGBXDSTR = newGBXC.ToString("X");

            if (newGBXDSTR.Length == 1)
            {
                newGBXDSTR = "000" + newGBXDSTR;
            }

            if (newGBXDSTR.Length == 2)
            {
                newGBXDSTR = "00" + newGBXDSTR;
            }

            if (newGBXDSTR.Length == 3)
            {
                newGBXDSTR = "0" + newGBXDSTR;
            }

            string new_1 = newGBXDSTR.Substring(0, 2);
            string new_2 = newGBXDSTR.Substring(2, 2);
            int new_w_1 = System.Convert.ToInt32(new_1, 16);
            int new_w_2 = System.Convert.ToInt32(new_2, 16);
            byte[] newww = new byte[2];
            newww[0] = (byte)new_w_1;
            newww[1] = (byte)new_w_2;

            Encoding gb = Encoding.GetEncoding("gb2312");

            string newGZ = gb.GetString(newww);

            return newGZ;
            */
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 6)
            {

                return;
            }

            string[] newIu = textBox1.Text.Split('\t');

            string FR = newIu[0];
            string PL = newIu[1];
            string CC = newIu[2];
            string CX = newIu[3];
            string Word = newIu[4];

            oneXNU io = new oneXNU();

            io.a_CC = Int32.Parse(CC);
            io.a_CX = Int32.Parse(CX);
            io.a_PL = Int32.Parse(PL);
            io.a_Word = Word;

            int xNum = GetWordNum(FR);

            ArrayList nn_T = (ArrayList)nDict[xNum];

            if (nn_T.Contains(io) == true)
            {

                nn_T.Remove(io);

                nDict[xNum] = nn_T;

                comboBox1.Text = "";
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedIndex = 1;
                comboBox1.Text = FR;



            }
            else
            {
                MessageBox.Show("������!");
            
            }



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text =comboBox1.Text +'\t'+ listBox1.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" | nDict.Count==0 | NWORD6768.Count==0 )
            {
                return;
            }

            int x = comboBox1.SelectedIndex;

            string newWCN = GetWordCN(x);

            ArrayList mix = (ArrayList)nDict[x];

            listBox1.Items.Clear();

            foreach (oneXNU one in mix)
            {
                listBox1.Items.Add(one.a_PL.ToString()+"\t"+one.a_CC.ToString()+"\t"+ one.a_CX.ToString()+"\t"+  one.a_Word);
            
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox_word.Text.Length < 2)
            {
                MessageBox.Show("�������!");
                return;
            }

            //Ƶ��
            int PL = Int32.Parse(textBox_PL.Text);

            //����
            int CX = Int32.Parse(textBox_CX.Text);

            //��
            string nne = textBox_word.Text;

            string aF = nne.Substring(0, 1);
            string aB = nne.Substring(1, nne.Length - 1);

            //�ʳ�
            Encoding gb = Encoding.GetEncoding("gb2312");

            byte[] xx = gb.GetBytes(aB);

            oneXNU one = new oneXNU();

            one.a_CC = xx.Length;
            one.a_PL = PL;
            one.a_CX = CX;
            one.a_Word = aB;

            //���� aF �õ����

            int oneX = GetWordNum(aF);

            string nWord = GetWordCN(oneX);

            ArrayList tmp = (ArrayList)nDict[oneX];

            if (tmp.Contains(one) == false)
            {

                tmp.Add(one);

                nDict[oneX] = tmp;
            }

            comboBox1.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndex = 1;
            comboBox1.Text = aF;

        }

        /// <summary>
        /// �õ�������Ŷ��ձ�
        /// </summary>
        private void GetTable()
        { 
             //����GB2312 �ֿ�

            string[] Top_1 ={"A","B","C","D","E","F" };
            string[] Top_2 ={"0","1","2","3","4","5","6","7","8","9", "A", "B", "C", "D", "E", "F" };
            string[] Top_3 ={ "A", "B", "C", "D", "E", "F" };
            string[] Top_4 ={ "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

            bool StratADD = false; //��ʼ��¼

            ArrayList v = new ArrayList();
            v.Clear();
            foreach (string t_1 in Top_1)
            {
                foreach (string t_2 in Top_2)
                {
                    foreach (string t_3 in Top_3)
                    {
                        foreach (string t_4 in Top_4)
                        {

                            if ((t_3 == "A" & t_4 == "0") | (t_3 == "F" & t_4 == "F") |(t_1=="A" & t_2 =="0"))
                            {

                            }
                            else
                            {

                                string all_1 = t_1 + t_2;
                                string all_2 = t_3 + t_4;


                                int GBXC1 = System.Convert.ToInt32(all_1, 16);
                                int GBXC2 = System.Convert.ToInt32(all_2, 16);

                                byte[] newCN = new byte[2];

                                newCN[0] = (byte)GBXC1;
                                newCN[1] = (byte)GBXC2;

                                Encoding gb = Encoding.GetEncoding("gb2312");

                                string CCNN = gb.GetString(newCN);

                                if (CCNN == "��")
                                {
                                    StratADD = true;
                                }

                                if (StratADD == true)
                                {
                                    v.Add(CCNN);
                                }

                                if (CCNN == "��")
                                {
                                    StratADD = false;
                                }

                            }
                        }
                   }
                }
            
            
            }

            //�õ�����
            NWORD6768 = v;

        }

        /// <summary>
        /// ���ݺ��ֵõ�����
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private int GetWordNum(string dat)
        {

            int x = 0;

            for(int i=0;i<NWORD6768.Count;i++)
            {
              string  aaa = NWORD6768[i].ToString();

              if (aaa == dat)
              {
                  return i;
              }
            
            
            
            }

            MessageBox.Show("ERROR! GetWordNum@");

            return 0;
        
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "";

            openFileDialog1.ShowDialog();

            string mF = openFileDialog1.FileName;

            if (System.IO.File.Exists(mF) == false)
            {
                return;
            }


           // nDict.Clear();

           // NWORD6768.Clear();

            mm_path = mF;

            OpenDict(mF);



        }


        private void OpenDict(string mpath)
        {
            FileStream cfs = new FileStream(mpath, FileMode.Open, FileAccess.Read);
            BinaryReader cr = new BinaryReader(cfs, Encoding.GetEncoding("gb2312"));

            int PPP = 0;

            int Num_all = 0;

            //6768������
            for (int i = 0; i < 6768; i++)
            {

                string WordCN = GetWordCN(i);

                //�õ��˱����������
                byte[] m1 = cr.ReadBytes(4);
                PPP = PPP + 4;

                int G_1 = GetIntIt(m1);
                ArrayList newOneIt = new ArrayList(G_1);

                if (G_1 == 0)
                {
                    goto NEWXXXX;
                }

                for (int ii = 0; ii < G_1; ii++)
                {
                    //Ƶ��
                    byte[] m2 = cr.ReadBytes(4);
                    int G_2 = GetIntIt(m2);

                    PPP = PPP + 4;

                    //�ʳ�
                    byte[] m3 = cr.ReadBytes(4);
                    int G_3 = GetIntIt(m3);

                    PPP = PPP + 4;

                    //����
                    byte[] m4 = cr.ReadBytes(4);
                    int G_4 = GetIntIt(m4);

                    PPP = PPP + 4;

                    byte[] mWord = cr.ReadBytes(G_3);

                    string G_STR = GetStrIt(mWord);

                    PPP = PPP + G_3;

                    oneXNU newOneXNU = new oneXNU();
                    newOneXNU.a_CC = G_3;
                    newOneXNU.a_CX = G_4;
                    newOneXNU.a_PL = G_2;
                    newOneXNU.a_Word = G_STR;

                    // ii
                    newOneIt.Add(newOneXNU);
                    Num_all = Num_all + 1;
                }

            NEWXXXX: ;

                nDict.Add(newOneIt);

            }

            cr.Close();
            ItList();

            toolStripStatusLabel1.Text = "�������ش��� > " + Num_all.ToString();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            saveFileDialog1.FileName = mm_path;

            saveFileDialog1.ShowDialog();

            string nppt = saveFileDialog1.FileName;


            //if (System.IO.File.Exists(nppt) == false)
            //{

              //  MessageBox.Show("����ʧ�ܣ�");
              //  return;
            //}

            //������
            SaveIt(nppt);
            MessageBox.Show("������ɣ�");
        }

        /// <summary>
        /// ����ʵ�
        /// </summary>
        /// <param name="path"></param>
        private void SaveIt(string path)
        {
            Encoding gb = Encoding.GetEncoding("gb2312");

            FileStream cfs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter cr = new BinaryWriter(cfs, Encoding.GetEncoding("gb2312"));

            //6768������
            for (int i = 0; i < 6768; i++)
            {
                //�õ������ֵĽṹ����
                ArrayList nU = (ArrayList)nDict[i];

                //�õ���ѡ��  ����ĸ���
                int F_T_1 = nU.Count;
                byte[] a_F_T_1 = GetByteIt(F_T_1);
                cr.Write(a_F_T_1);

                foreach(oneXNU a in nU)
                {
                //Ƶ��
                    int F_T_2 = (int)a.a_PL;
                    byte[] a_F_T_2 = GetByteIt(F_T_2);
                    cr.Write(a_F_T_2);
                //�ʳ�
                    int F_T_3 = (int)a.a_CC;
                    byte[] a_F_T_3 = GetByteIt(F_T_3);
                    cr.Write(a_F_T_3);
                //����
                    int F_T_4 = (int)a.a_CX;
                    byte[] a_F_T_4 = GetByteIt(F_T_4);
                    cr.Write(a_F_T_4);
                //��
                    string F_T_5 = a.a_Word;

                    byte[] a_F_T_5 = gb.GetBytes(F_T_5);

                    if (F_T_3 != a_F_T_5.Length)
                    {
                        MessageBox.Show("�������̳���");
                    }
                    cr.Write(a_F_T_5);

                }            
            }

            cr.Close();
        
        
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            nSearch.XWORDEDIT.FormAbout xAbout = new nSearch.XWORDEDIT.FormAbout();  
            xAbout.ShowDialog();

            

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        } 


    }
}