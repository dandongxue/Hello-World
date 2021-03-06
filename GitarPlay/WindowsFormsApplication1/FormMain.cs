﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Collections;


namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
        private PublicClass.AutoSizeForm autoform = new PublicClass.AutoSizeForm();
        private String workDir = new DirectoryInfo(Application.StartupPath).FullName;
        //private String workDir = new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.FullName;
        private IPEndPoint ipLocalPoint,remoteIpend;
        private IPAddress lip, remoteIp;
        private int localPort = 5526;
        private Socket mySocket;
        private TcpClient tcp;
        private bool RunningFlag = false;
        private TreeNode curNode=null;
        private int xPos, yPos;
        private bool mvFlag = false;
        private Secret secret = new Secret();
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
            trView.Nodes.Clear();
            XElement xe = XElement.Load(fileName);
            IEnumerable<XElement> elements = from ele in xe.Elements("Guitar") select ele;
            ShowInfoByElements(elements);

        }
        private ArrayList arrTnode = new ArrayList();
        private void Form1_Load(object sender, EventArgs e)
        {
            String cpuId = secret.GetCpuID();
            if (secret.CheckCpuIdentit(cpuId) == false)
            {
                MessageBox.Show("认证失败！");
                this.Close();
                return;
            }
            //MessageBox.Show(cpuId.ToString());
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            this.UpdateStyles();
            trviewGitar.Nodes.Clear();
            this.MouseWheel += Form1_MouseWheel;
            if (Directory.Exists(workDir+"\\Gitar") == false)//如果不存在就创建file文件夹     
            {
                MessageBox.Show(workDir + "\\Gitar");
                Directory.CreateDirectory(workDir + "\\Gitar");     
            }
            InitCollectTree(trViewCollect, workDir + "\\collect.xml");
            InitTreeView(trviewGitar, workDir + "\\Gitar");
            lbNum.Text = trviewGitar.Nodes.Count.ToString();

            //MessageBox.Show(getIPAddress());
            /*IPAddress.TryParse(getIPAddress(), out lip);
            ipLocalPoint = new IPEndPoint(lip, localPort);
            IPAddress.TryParse("120.210.207.197", out remoteIp);//"120.210.207.197"
            /*remoteIpend = new IPEndPoint(remoteIp, 28585);
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mySocket.Bind(ipLocalPoint);*/

            foreach (TreeNode tnode in trviewGitar.Nodes)
            {
                arrTnode.Add(tnode);
            }
            
        }
        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            System.Drawing.Point pInit = picboxMain.Location;
            if (e.Delta > 0) //放大图片
            {
                picboxMain.Size = new Size(picboxMain.Width + 40, picboxMain.Height + 40);
            }
            else
            {  //缩小图片
                picboxMain.Size = new Size(picboxMain.Width - 40, picboxMain.Height - 40);
            }
            //设置图片在窗体居中
            double len = 2.6 * (button1.Width);
            //picboxMain.Location = new Point((this.Width - picboxMain.Width + Convert.ToInt16(len)) / 2, (this.Height - picboxMain.Height) / 2);

           /* System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);//画刷
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.FillEllipse(myBrush, new Rectangle(pInit.X, pInit.Y, 50, 50));//画实心椭圆*/
        }


        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            autoform.controlAutoSize(this);
        }

        private void trviewGitar_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1 && (e.Button == MouseButtons.Left))
            {
                ShowFile(e.Node.Tag.ToString(), picboxMain);
                curNode = e.Node;
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            trViewSearch.Nodes.Clear();
            String ans = tboxSearch.Text.Trim();
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
            byte[] data = Encoding.Default.GetBytes(ans);
            mySocket.SendTo(data, data.Length, SocketFlags.None, remoteIpend); 
        }
        private void trViewSearch_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1 && (e.Button == MouseButtons.Left))
            {
                ShowFile(e.Node.Tag.ToString(), picboxMain);
                curNode = e.Node;
            }
        }
        void ShowFile(String filePath,PictureBox pbox)
        {
            if (File.Exists(filePath))
            {
                pbox.Image = Image.FromFile(filePath);
            }
            else
            {
                MessageBox.Show("文件不存在！");
            }
        }
        private void trViewCollect_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1 && (e.Button == MouseButtons.Left))
            {
                ShowFile(e.Node.Tag.ToString(), picboxMain);
                curNode = e.Node;
            }
        }

        private void trViewSearch_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0 && (e.Button == MouseButtons.Left))
            {
                String songName = e.Node.Text;
                XmlDocument doc = new XmlDocument();　  
                string strFileName = workDir + "\\collect.xml";
                doc.Load(strFileName);
                XmlNode root = doc.SelectSingleNode("collectstore");
                XmlNodeList nodes = root.ChildNodes;
                XmlNodeList nodeds = root.SelectNodes("Guitar");
                for (int i = 0; i < nodeds.Count; i++)
                {
                    XmlElement xe = (XmlElement)nodeds[i];

                    if (xe.GetAttribute("Name").Equals(songName))
                    {
                        MessageBox.Show("对不起，您已收藏!！");
                        return;
                    }
                }
                XmlElement xe1 = doc.CreateElement("Guitar");//创建一个<Node>节点 
                xe1.SetAttribute("Name", songName);//设置该节点genre属性 
                xe1.SetAttribute("ISBN", "7-111-19149-2");//设置该节点ISBN属性
                xe1.SetAttribute("PicNum", e.Node.Nodes.Count.ToString());//设置该节点genre属性 
                int num=1;
                foreach (TreeNode tr in e.Node.Nodes)
                {
                    XmlElement xesub1 = doc.CreateElement("path"+(num++).ToString());
                    xesub1.InnerText = tr.Tag.ToString();//设置文本节点 
                    xe1.AppendChild(xesub1);
                }
                root.AppendChild(xe1);
                try
                {
                    //保存上面的修改　　  
                    doc.Save(strFileName);
                    MessageBox.Show("已成功收藏曲目《"+songName+"》");
                    InitCollectTree(trViewCollect, workDir + "\\collect.xml");
                    
                }
                catch (Exception e3)
                {
                    throw e3;
                }  
            }
        }
       private string getIPAddress()  
       {   
            IPAddress[] AddressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;  
            if (AddressList.Length < 1)  
            {  
                 return "";  
             }  
            return AddressList[0].ToString();  
        }
        private void trviewGitar_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0 && (e.Button == MouseButtons.Left))
            {
                String songName = e.Node.Text;
                XmlDocument doc = new XmlDocument();　  
                string strFileName = workDir + "\\collect.xml";
                doc.Load(strFileName);
                XmlNode root = doc.SelectSingleNode("collectstore");
                XmlNodeList nodes = root.ChildNodes;
                XmlNodeList nodeds = root.SelectNodes("Guitar");
                for (int i = 0; i < nodeds.Count; i++)
                {
                    XmlElement xe = (XmlElement)nodeds[i];

                    if (xe.GetAttribute("Name").Equals(songName))
                    {
                        MessageBox.Show("对不起，您已收藏!！");
                        return;
                    }
                }
                XmlElement xe1 = doc.CreateElement("Guitar");//创建一个<Node>节点 
                xe1.SetAttribute("Name", songName);//设置该节点genre属性 
                xe1.SetAttribute("ISBN", "7-111-19149-2");//设置该节点ISBN属性
                xe1.SetAttribute("PicNum", e.Node.Nodes.Count.ToString());//设置该节点genre属性 
                int num=1;
                foreach (TreeNode tr in e.Node.Nodes)
                {
                    XmlElement xesub1 = doc.CreateElement("path"+(num++).ToString());
                    xesub1.InnerText = tr.Tag.ToString();//设置文本节点 
                    xe1.AppendChild(xesub1);
                }
                root.AppendChild(xe1);
                try
                {
                    //保存上面的修改　　  
                    doc.Save(strFileName);
                    MessageBox.Show("已成功收藏曲目《"+songName+"》");
                    InitCollectTree(trViewCollect, workDir + "\\collect.xml");
                }
                catch (Exception e3)
                {
                    throw e3;
                }  
            }
        
        }

        private void tboxSearch_TextChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            trViewSearch.Nodes.Clear();
            String ans = tboxSearch.Text.Trim();
            if (ans.Trim() == "")
                return;

            for (int a = 0; a < arrTnode.Count; a++)
            {
                TreeNode tnode = arrTnode[a] as TreeNode;
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

        private void button2_Click(object sender, EventArgs e)
        {
            NextPage();
        }
        void PrePage()
        {
            if (curNode == null || curNode.PrevNode == null)
                return;
            curNode = curNode.PrevNode;
            if (curNode != null)
            {
                String picPath = curNode.Tag.ToString();
                picboxMain.Image = Image.FromFile(picPath);
            }
        }
        void NextPage()
        {
            if (curNode == null || curNode.NextNode == null)
                return;
            curNode = curNode.NextNode;
            if (curNode != null)
            {
                String picPath = curNode.Tag.ToString();
                picboxMain.Image = Image.FromFile(picPath);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PrePage();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(tcp!=null)
               tcp.Close();

        }

        private void picBoxAuthor_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void label1_Click(object sender, EventArgs e)
        {
            frmIntro frm = new frmIntro();
            frm.Show();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
            {
                NextPage();
            }
            else  if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
            {
                PrePage();
            }
        }

        private void trViewCollect_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0 && (e.Button == MouseButtons.Left))
            {
                String songName = e.Node.Text;
                XmlDocument doc = new XmlDocument();
                string strFileName = workDir + "\\collect.xml";
                doc.Load(strFileName);
                XmlNodeList nodeList = doc.SelectSingleNode("collectstore").ChildNodes; //查找节点 
                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn;
                    if(xe.GetAttribute("Name") == songName)
                    {
                        xn.ParentNode.RemoveChild(xe);
                        MessageBox.Show(songName + "  已删除！");
                        break;
                    }
                }
                doc.Save(strFileName);
            }
            InitCollectTree(trViewCollect, workDir + "\\collect.xml");
        }

        private void picboxMain_MouseDown(object sender, MouseEventArgs e)
        {
            mvFlag = true;
            xPos = e.X;
            yPos = e.Y;
        }

        private void picboxMain_MouseUp(object sender, MouseEventArgs e)
        {
            mvFlag = false;
        }

        private void picboxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if(mvFlag)
            {
                picboxMain.Left += Convert.ToInt16(e.X - xPos);
                picboxMain.Top += Convert.ToInt16(e.Y - yPos);
            }
        }

        private void FormMain_Click(object sender, EventArgs e)
        {
            picboxMain.Focus();
        }

        private void picboxMain_Click(object sender, EventArgs e)
        {
            picboxMain.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tboxSearch_TextChanged(sender, e);
        }


     
    }
}
