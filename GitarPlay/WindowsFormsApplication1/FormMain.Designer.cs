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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.trviewGitar = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.picboxMain = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tboxSearch = new System.Windows.Forms.TextBox();
            this.lb3 = new System.Windows.Forms.Label();
            this.lbNum = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.trViewSearch = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.trViewCollect = new System.Windows.Forms.TreeView();
            this.lb2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picboxMain)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // trviewGitar
            // 
            this.trviewGitar.BackColor = System.Drawing.Color.White;
            this.trviewGitar.ImageIndex = 1;
            this.trviewGitar.ImageList = this.imageList1;
            this.trviewGitar.Location = new System.Drawing.Point(1, 0);
            this.trviewGitar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trviewGitar.Name = "trviewGitar";
            this.trviewGitar.SelectedImageIndex = 0;
            this.trviewGitar.Size = new System.Drawing.Size(270, 524);
            this.trviewGitar.TabIndex = 0;
            this.trviewGitar.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trviewGitar_NodeMouseClick);
            this.trviewGitar.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trviewGitar_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "guitar_128px_1184944_easyicon.net.ico");
            this.imageList1.Images.SetKeyName(1, "Guitar_128px_1188461_easyicon.net.ico");
            // 
            // picboxMain
            // 
            this.picboxMain.BackColor = System.Drawing.Color.Transparent;
            this.picboxMain.InitialImage = null;
            this.picboxMain.Location = new System.Drawing.Point(10, 0);
            this.picboxMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picboxMain.Name = "picboxMain";
            this.picboxMain.Size = new System.Drawing.Size(457, 658);
            this.picboxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picboxMain.TabIndex = 1;
            this.picboxMain.TabStop = false;
            this.picboxMain.Click += new System.EventHandler(this.picboxMain_Click);
            this.picboxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picboxMain_MouseDown);
            this.picboxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picboxMain_MouseMove);
            this.picboxMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picboxMain_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.tboxSearch);
            this.panel1.Location = new System.Drawing.Point(3, 8);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 62);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WindowsFormsApplication1.Properties.Resources.search_128px_1202802_easyicon_net;
            this.pictureBox1.Location = new System.Drawing.Point(10, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // tboxSearch
            // 
            this.tboxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tboxSearch.Location = new System.Drawing.Point(87, 18);
            this.tboxSearch.Name = "tboxSearch";
            this.tboxSearch.Size = new System.Drawing.Size(179, 23);
            this.tboxSearch.TabIndex = 4;
            this.tboxSearch.Text = "斑马斑马";
            this.tboxSearch.TextChanged += new System.EventHandler(this.tboxSearch_TextChanged);
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.BackColor = System.Drawing.Color.Transparent;
            this.lb3.ForeColor = System.Drawing.Color.Gainsboro;
            this.lb3.Location = new System.Drawing.Point(154, 649);
            this.lb3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(44, 17);
            this.lb3.TabIndex = 7;
            this.lb3.Text = "首曲子";
            // 
            // lbNum
            // 
            this.lbNum.AutoSize = true;
            this.lbNum.BackColor = System.Drawing.Color.Transparent;
            this.lbNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNum.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbNum.Location = new System.Drawing.Point(105, 647);
            this.lbNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNum.Name = "lbNum";
            this.lbNum.Size = new System.Drawing.Size(30, 22);
            this.lbNum.TabIndex = 6;
            this.lbNum.Text = "55";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Location = new System.Drawing.Point(770, 95);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 45);
            this.button1.TabIndex = 5;
            this.button1.Text = "上一页";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(770, 499);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 45);
            this.button2.TabIndex = 6;
            this.button2.Text = "下一页";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(4, 89);
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
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.trViewSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(267, 522);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "搜索";
            // 
            // trViewSearch
            // 
            this.trViewSearch.BackColor = System.Drawing.Color.White;
            this.trViewSearch.Location = new System.Drawing.Point(-2, -1);
            this.trViewSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trViewSearch.Name = "trViewSearch";
            this.trViewSearch.Size = new System.Drawing.Size(273, 527);
            this.trViewSearch.TabIndex = 1;
            this.trViewSearch.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trViewSearch_NodeMouseClick);
            this.trViewSearch.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trViewSearch_NodeMouseDoubleClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.trViewCollect);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(267, 522);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "我的收藏";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // trViewCollect
            // 
            this.trViewCollect.ImageIndex = 1;
            this.trViewCollect.ImageList = this.imageList1;
            this.trViewCollect.Location = new System.Drawing.Point(0, 3);
            this.trViewCollect.Name = "trViewCollect";
            this.trViewCollect.SelectedImageIndex = 0;
            this.trViewCollect.Size = new System.Drawing.Size(268, 519);
            this.trViewCollect.TabIndex = 0;
            this.trViewCollect.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trViewCollect_NodeMouseClick);
            this.trViewCollect.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trViewCollect_NodeMouseDoubleClick);
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.BackColor = System.Drawing.Color.Transparent;
            this.lb2.ForeColor = System.Drawing.Color.Gainsboro;
            this.lb2.Location = new System.Drawing.Point(35, 649);
            this.lb2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(60, 17);
            this.lb2.TabIndex = 5;
            this.lb2.Text = "统计 共：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(761, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 22);
            this.label1.TabIndex = 10;
            this.label1.Text = "软件说明";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.picboxMain);
            this.panel2.Location = new System.Drawing.Point(285, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(478, 661);
            this.panel2.TabIndex = 11;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::WindowsFormsApplication1.Properties.Resources._112;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(864, 672);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lb3);
            this.Controls.Add(this.lbNum);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lb2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(880, 710);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DD Guitar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Click += new System.EventHandler(this.FormMain_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picboxMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trviewGitar;
        private System.Windows.Forms.PictureBox picboxMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tboxSearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lb3;
        private System.Windows.Forms.Label lbNum;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView trViewSearch;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.TreeView trViewCollect;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
    }
}

