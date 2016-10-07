namespace WindowsFormsApplication1
{
    partial class FormMain
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
            this.trviewGitar = new System.Windows.Forms.TreeView();
            this.picboxMain = new System.Windows.Forms.PictureBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tboxSearch = new System.Windows.Forms.TextBox();
            this.lb1 = new System.Windows.Forms.Label();
            this.lb3 = new System.Windows.Forms.Label();
            this.lbNum = new System.Windows.Forms.Label();
            this.lb2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.trViewSearch = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.picboxMain)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // trviewGitar
            // 
            this.trviewGitar.Location = new System.Drawing.Point(1, 0);
            this.trviewGitar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trviewGitar.Name = "trviewGitar";
            this.trviewGitar.Size = new System.Drawing.Size(270, 524);
            this.trviewGitar.TabIndex = 0;
            this.trviewGitar.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trviewGitar_NodeMouseClick);
            // 
            // picboxMain
            // 
            this.picboxMain.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.picboxMain.Location = new System.Drawing.Point(275, 1);
            this.picboxMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picboxMain.Name = "picboxMain";
            this.picboxMain.Size = new System.Drawing.Size(492, 670);
            this.picboxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picboxMain.TabIndex = 1;
            this.picboxMain.TabStop = false;
            this.picboxMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picboxMain_MouseClick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(184, 36);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(69, 29);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.tboxSearch);
            this.panel1.Controls.Add(this.lb1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 78);
            this.panel1.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(66, 36);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(71, 29);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tboxSearch
            // 
            this.tboxSearch.Location = new System.Drawing.Point(93, 5);
            this.tboxSearch.Name = "tboxSearch";
            this.tboxSearch.Size = new System.Drawing.Size(136, 23);
            this.tboxSearch.TabIndex = 4;
            this.tboxSearch.Text = "斑马斑马";
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(2, 8);
            this.lb1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(80, 17);
            this.lb1.TabIndex = 1;
            this.lb1.Text = "吉他谱搜索：";
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.Location = new System.Drawing.Point(151, 636);
            this.lb3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(44, 17);
            this.lb3.TabIndex = 7;
            this.lb3.Text = "首曲子";
            // 
            // lbNum
            // 
            this.lbNum.AutoSize = true;
            this.lbNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNum.Location = new System.Drawing.Point(102, 634);
            this.lbNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNum.Name = "lbNum";
            this.lbNum.Size = new System.Drawing.Size(30, 22);
            this.lbNum.TabIndex = 6;
            this.lbNum.Text = "55";
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.Location = new System.Drawing.Point(32, 636);
            this.lb2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(60, 17);
            this.lb2.TabIndex = 5;
            this.lb2.Text = "统计 共：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(769, 108);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 72);
            this.button1.TabIndex = 5;
            this.button1.Text = "上一页";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(769, 432);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 72);
            this.button2.TabIndex = 6;
            this.button2.Text = "下一页";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(-2, 79);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(275, 552);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.trviewGitar);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(267, 522);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "主页";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.trViewSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(267, 522);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "搜索";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // trViewSearch
            // 
            this.trViewSearch.Location = new System.Drawing.Point(-2, -1);
            this.trViewSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trViewSearch.Name = "trViewSearch";
            this.trViewSearch.Size = new System.Drawing.Size(270, 524);
            this.trViewSearch.TabIndex = 1;
            this.trViewSearch.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trViewSearch_NodeMouseClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(267, 522);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "我的收藏";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(845, 672);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lb3);
            this.Controls.Add(this.lbNum);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lb2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.picboxMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormMain";
            this.Text = "吉他show";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picboxMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trviewGitar;
        private System.Windows.Forms.PictureBox picboxMain;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox tboxSearch;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lb3;
        private System.Windows.Forms.Label lbNum;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView trViewSearch;
    }
}

