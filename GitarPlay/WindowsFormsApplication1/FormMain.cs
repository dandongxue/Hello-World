using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
        private PublicClass.AutoSizeForm autoform = new PublicClass.AutoSizeForm();
        private String workDir = new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.FullName;
        public FormMain()
        {
            InitializeComponent();
        }
        void InitTreeView(TreeView trView,String workDir)
        {
            DirectoryInfo info = new DirectoryInfo(workDir);
            DirectoryInfo[] dirList = info.GetDirectories();
            for (int i = 0; i < dirList.Length; i++)
            {
                string name = dirList[i].Name;
                string pathName = dirList[i].FullName;
                {
                    TreeNode subNode = trView.Nodes.Add(name);
                    subNode.Tag = dirList[i].FullName;
                    DirectoryInfo folder = new DirectoryInfo(pathName);
                    foreach (FileInfo file in folder.GetFiles())
                    {
                        TreeNode nodePic = new TreeNode(file.Name);
                        nodePic.Tag = file.FullName;
                        nodePic.ImageIndex = 0;
                        subNode.Nodes.Add(nodePic);
                    }
                }
            }

        }
        private void ShowInfoByElements(IEnumerable<XElement> elements)
           {
               foreach (var ele in elements)
              {
                   String GuitarName = ele.Attribute("Name").Value.ToString();
                   int PicNum = Convert.ToInt16(ele.Attribute("PicNum").Value);
                   TreeNode subNode =  trViewCollect.Nodes.Add(GuitarName);
                   for (int i = 1; i <= PicNum; i++)
                   {
                       String Gitpath = ele.Element("path"+i.ToString()).Value.ToString();
                       DirectoryInfo info = new DirectoryInfo(Gitpath);
                       TreeNode nodePic = new TreeNode(info.Name);
                       nodePic.Tag = Gitpath;
                       nodePic.ImageIndex = 0;
                       subNode.Nodes.Add(nodePic);
                   }
              }
             
         }
        void InitCollectTree(TreeView trView, String fileName)
        {
            XElement xe = XElement.Load(fileName);
            IEnumerable<XElement> elements = from ele in xe.Elements("Guitar") select ele;
            ShowInfoByElements(elements);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            trviewGitar.Nodes.Clear();
            this.MouseWheel += Form1_MouseWheel;
            if (Directory.Exists(workDir+"\\Gitar") == false)//如果不存在就创建file文件夹     
            {
                Directory.CreateDirectory(workDir + "\\Gitar");     
            }
            InitCollectTree(trViewCollect, workDir + "\\collect.xml");
            InitTreeView(trviewGitar, workDir + "\\Gitar");
            lbNum.Text = trviewGitar.Nodes.Count.ToString();
        }
        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            System.Drawing.Point pInit = picboxMain.Location;
            if (e.Delta > 0) //放大图片
            {
                picboxMain.Size = new Size(picboxMain.Width + 50, picboxMain.Height + 50);
            }
            else
            {  //缩小图片
                picboxMain.Size = new Size(picboxMain.Width - 50, picboxMain.Height - 50);
            }
            //设置图片在窗体居中
            double len = 2.6 * (button1.Width);
            picboxMain.Location = new Point((this.Width - picboxMain.Width + Convert.ToInt16(len)) / 2, (this.Height - picboxMain.Height) / 2);

           /* System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);//画刷
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.FillEllipse(myBrush, new Rectangle(pInit.X, pInit.Y, 50, 50));//画实心椭圆*/
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(workDir + "\\collect.xml");
           
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            autoform.controlAutoSize(this);
        }

        private void trviewGitar_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1 && (e.Button == MouseButtons.Left))
            {
                String picPath = e.Node.Tag.ToString();
                picboxMain.Image = Image.FromFile(picPath);
               // picboxMain.Size = Image.FromFile(picPath).Size;
            }
        }

        private void picboxMain_MouseClick(object sender, MouseEventArgs e)
        {
            System.Drawing.Point pInit = picboxMain.Location;
            picboxMain.Size = new Size(picboxMain.Width + 50, picboxMain.Height + 50);
            double len = 2.6 * (button1.Width);
            picboxMain.Location = new Point((this.Width - picboxMain.Width + Convert.ToInt16(len)) / 2, (this.Height - picboxMain.Height) / 2);
            /* System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);//画刷
             System.Drawing.Graphics formGraphics = this.CreateGraphics();
             formGraphics.FillEllipse(myBrush, new Rectangle(pInit.X, pInit.Y, 50, 50));//画实心椭圆*/
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            trViewSearch.Nodes.Clear();
            String ans = tboxSearch.Text.Trim();
            //trviewGitar.Nodes[5].Checked = true;
            foreach (TreeNode tnode in trviewGitar.Nodes)
            {
                if (tnode.Text.Contains(ans))
                {
                    DirectoryInfo info = new DirectoryInfo(tnode.Tag.ToString());
                    TreeNode subNode = trViewSearch.Nodes.Add(info.Name);
                    //subNode.Tag = dirList[i].FullName;
                    DirectoryInfo folder = new DirectoryInfo(tnode.Tag.ToString());
                    foreach (FileInfo file in folder.GetFiles())
                    {
                        TreeNode nodePic = new TreeNode(file.Name);
                        nodePic.Tag = file.FullName;
                        subNode.Nodes.Add(nodePic);
                    }    
                }
            }
        }
        private void trViewSearch_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1 && (e.Button == MouseButtons.Left))
            {
                String picPath = e.Node.Tag.ToString();
                picboxMain.Image = Image.FromFile(picPath);
            }
        }

        private void trViewCollect_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1 && (e.Button == MouseButtons.Left))
            {
                String picPath = e.Node.Tag.ToString();
                picboxMain.Image = Image.FromFile(picPath);
            }
        }
    }
}
