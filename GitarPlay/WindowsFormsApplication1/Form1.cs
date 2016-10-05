using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private PublicClass.AutoSizeForm autoform = new PublicClass.AutoSizeForm();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trviewGitar.Nodes.Clear();
            TreeNode node = trviewGitar.Nodes.Add("玫瑰");
            node.Tag = -1;//机房节点统一为  -1
            for (int i = 0; i < 3; i++)
            {
                TreeNode NodePic = new TreeNode("Pic"+i.ToString());
                node.Nodes.Add(NodePic);
            }
            this.MouseWheel += Form1_MouseWheel;
        }
        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) //放大图片
            {
                picboxMain.Size = new Size(picboxMain.Width + 50, picboxMain.Height + 50);
            }
            else
            {  //缩小图片
                picboxMain.Size = new Size(picboxMain.Width - 50, picboxMain.Height - 50);
            }
            //设置图片在窗体居中
            picboxMain.Location = new Point(( picboxMain.Width) / 2, (picboxMain.Height) / 2);
            //picboxMain.Location = new Point((this.Width - picboxMain.Width) / 2, (this.Height - picboxMain.Height) / 2);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            picboxMain.Image = Image.FromFile("E:\\Bayes\\GitResp\\Gitar-Play\\GitarPlay\\WindowsFormsApplication1\\1.jpg");
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            autoform.controlAutoSize(this);
        }
    }
}
