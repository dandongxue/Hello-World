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
        }
    }
}
