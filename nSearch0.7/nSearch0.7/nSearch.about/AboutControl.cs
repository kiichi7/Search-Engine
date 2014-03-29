using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace nSearch.about
{
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 模块标题
        /// </summary>
        public string Title
        {
            get
            {
               // throw new System.NotImplementedException();
              
                return label7.Text;

            }
            set
            {


               
                label7.Text = value;              
                
            }
        }

        /// <summary>
        /// 模块介绍
        /// </summary>
        public string Body
        {
            get
            {
               // throw new System.NotImplementedException();
                return textBox1.Text;
            }
            set
            {
                
                textBox1.Text = value;
                
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void AboutControl_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Silver;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}