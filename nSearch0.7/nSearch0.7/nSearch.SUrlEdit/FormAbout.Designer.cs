namespace nSearch.SUrlEdit
{
    partial class FormAbout
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.aboutControl1 = new nSearch.about.AboutControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // aboutControl1
            // 
            this.aboutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.aboutControl1.Body = "配置蜘蛛的抓取范围和起点";
            this.aboutControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.aboutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutControl1.Location = new System.Drawing.Point(0, 0);
            this.aboutControl1.Name = "aboutControl1";
            this.aboutControl1.Size = new System.Drawing.Size(582, 362);
            this.aboutControl1.TabIndex = 2;
            this.aboutControl1.Title = "蜘蛛配置模块";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(495, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "退出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 362);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.aboutControl1);
            this.MaximizeBox = false;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于--迅龙中文搜索 模块";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private nSearch.about.AboutControl aboutControl1;
        private System.Windows.Forms.Button button1;
    }
}