using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace TNMediaViewer
{
    public partial class Form1 : Form
    {
        Bitmap _CurrentPic = null;
        string _DefaultPhotoExts = ".jpg|.jpeg|.gif|.bmp|.tif";
        string _DefaultWMExts = ".asf|.wma|.avi|.mp3|.mp2|.mpa|.mid|.midi|.rmi|.aif|.aifc|.aiff|.au|.snd|.wav|.cda|.wmv|.wm|.dvr-ms|.mpe|.mpeg|.mpg|.m1v|.vob";
        string _CurrentFileExt;
        string _FavFolderPath = "";
        ListBox _CutFiles = new ListBox();
        TreeNode _CutNode = null;
        Thread _CreateThumbListThread = null;
        List<ThumbnailItem> _ThumbList = new List<ThumbnailItem>();
        int _MaxThumbPerThreadRun = 10;
        string _CurrentPassword = "";
        MemoryStream _MSErrorImage = new MemoryStream();

        public Form1()
        {
            InitializeComponent();

            Thread th = new Thread(new ThreadStart(DoSplash));
            th.Start();

            PopulateTreeView();
            if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width >= 1024)
            {
                this.Width = 1024;
                this.Height = 768;
            }
            else
            {
                this.Width = 800;
                this.Height = 600;
            }
            splitContainer2.SplitterDistance = splitContainer2.Height - 45;
            
            System.Drawing.Bitmap bitmap1 = TNMediaViewer.Properties.Resources.error_triangle;
            bitmap1.Save(_MSErrorImage, System.Drawing.Imaging.ImageFormat.Bmp);

            // Retrieve Favorite
            _FavFolderPath = GetRegistryKey("FavFolderPath");
            if (_FavFolderPath != "")
            {
                OpenFavoriteMenuItem.ToolTipText = _FavFolderPath;
            }
            else
            {
                OpenFavoriteMenuItem.Enabled = false;
            }
            string PhotoExts = GetRegistryKey("PhotoExts");
            if (PhotoExts == "") PhotoExts = _DefaultPhotoExts;
            PopulatePhotoExtsList(PhotoExts);
            string MediaExts = GetRegistryKey("MediaExts");
            if (MediaExts == "") MediaExts = _DefaultWMExts;
            PopulateMediaExtsList(MediaExts);
            
            LimitViewableMenuItem.Image = TNMediaViewer.Properties.Resources.icon_check;

            this.Closed += new EventHandler(Form1_Closed); 


            System.Threading.Thread.Sleep(1000);
            th.Abort();
            System.Threading.Thread.Sleep(1000); // give system enough time to dispose splash
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
            FileInfo[] delFiles = dir.GetFiles("0000.*");

            try
            {
                foreach (FileInfo fo in delFiles)
                {
                    File.Delete(fo.FullName);
                } 
            }
            catch
            { 
            
            }
        } 

        private string GetRegistryKey(string KeyName)
        {
            string KeyValue = "";
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\TNMediaPlayer");
            if (key != null)
            {
                if (key.GetValue(KeyName) != null)
                {
                    KeyValue = key.GetValue(KeyName).ToString();
                }
            }
            return KeyValue;
        }

        private bool GetCurrentConfigEncryptionKey(bool PromptErr)
        {
            string strPassword = PasswordTextBox.Text.Trim();
            if (strPassword == "" || strPassword.Length != 8)
            {
                if (PromptErr)
                {
                    MessageBox.Show("Encryption key either not set or invalid format.");
                    tabControl1.SelectedTab = tabPage3;
                }
                _CurrentPassword = "";

                return false;
            }

            _CurrentPassword = strPassword;
            return true;
        }

        private void SetRegistryKey(string KeyName, string KeyValue)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\TNMediaPlayer");
            key.SetValue(KeyName, KeyValue);
        }

        private void DoSplash()
        {
            Splash sp = new Splash();
            sp.ShowDialog();
        }
        
        private void PopulateTreeView()
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            TreeNode rootNode;
            DirectoryInfo dirInfo;
            foreach (DriveInfo drive in driveInfo)
            {
                if (drive.DriveType == DriveType.Fixed) {
                    dirInfo = new DirectoryInfo(drive.Name);
                    if (dirInfo.Exists)
                    {
                        rootNode = new TreeNode(dirInfo.Name, 0, 0);
                        rootNode.Tag = dirInfo;
                        GetDirectories(dirInfo.GetDirectories(), rootNode, 0);
                        rootNode.Expand();
                        treeView1.Nodes.Add(rootNode);
                    }
                }
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo, int nodeLevel)
        {
            if (nodeLevel > 1) return; // load 2 levels max

            TreeNode aNode;
            Color TextColor;
            if (nodeLevel == 0)
            {
                TextColor = Color.Black;
            }
            else 
            {
                TextColor = Color.Teal;
            }
            
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.ForeColor = TextColor;
                aNode.Tag = subDir;

                try
                {
                    GetDirectories(subDir.GetDirectories(), aNode, nodeLevel + 1);
                    nodeToAddTo.Nodes.Add(aNode);
                }
                catch 
                {
                    // do nothing
                }
            }
            if (nodeToAddTo.Level > 1) nodeToAddTo.Expand();
        }

        // *** treeView1 supporting methods ***
        
        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1_SelectNode(e.Node);
        }

        void treeView1_SelectNode(TreeNode newSelected)
        {
            //reset
            listView1.Items.Clear();
            listView1.Columns.Clear();
            thumbImageList.Images.Clear();
            _ThumbList.Clear();

            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            if (_CreateThumbListThread != null && _CreateThumbListThread.IsAlive)
                _CreateThumbListThread.Abort();

            if (ThumbOnlyMenuItem.Image != null)
            {
                listView1.View = View.Tile;
            }
            else 
            {
                listView1.View = View.Details;
                listView1.Columns.Add("File", 156);
                listView1.Columns.Add("Size", 65);
                listView1.Columns.Add("Last Modified", 114);
            }
            
            // clicked on node is of level 2+ and no child nodes (subfolders) then load child nodes if exist
            if (newSelected.ForeColor != Color.Black)
            {
                GetDirectories(nodeDirInfo.GetDirectories(), newSelected, 1);
                newSelected.ForeColor = Color.Black;
            }

            bool limitToViewable = (LimitViewableMenuItem.Image != null);
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                if (!limitToViewable || (limitToViewable && (IsPhoto(file.Extension) || IsAV(file.Extension))))
                {
                    item = new ListViewItem(file.Name);
                    item.Tag = file;
                    subItems = new ListViewItem.ListViewSubItem[]
                        {   
                            new ListViewItem.ListViewSubItem(item, GetDisplaySize(file.Length)),
                            new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString() + " " + file.LastAccessTime.ToShortTimeString()) 
                        };
                    item.SubItems.AddRange(subItems);
                    listView1.Items.Add(item);
                    listView1.Items[listView1.Items.Count - 1].ImageIndex = listView1.Items.Count - 1;

                    if (listView1.View != View.Details)
                    {
                        if (IsPhoto(file.Extension))
                        {
                            _ThumbList.Add(new ThumbnailItem(listView1.Items.Count - 1, file.FullName, file.Extension));
                            thumbImageList.Images.Add((Bitmap)TNMediaViewer.Properties.Resources.loading);
                        }
                        else if (IsAV(file.Extension))
                        {
                            thumbImageList.Images.Add((Bitmap)TNMediaViewer.Properties.Resources.video);
                        }
                        else
                        {
                            thumbImageList.Images.Add((Bitmap)TNMediaViewer.Properties.Resources.other);
                        }                                    
                    }
                }
            }

            if (listView1.View != View.Details)
            {
                RunThumbGeneratorThread();
            }
            
            lbListCount.Text = "(" + listView1.Items.Count.ToString() + " files)";
        }

        private void RunThumbGeneratorThread()
        {
            if (_ThumbList.Count > 0)
            {
                _CreateThumbListThread = new Thread(new ThreadStart(CreateThumbList));
                _CreateThumbListThread.Start();
            }
        }
        
        private void CreateThumbList()
        {
            
            int NumItemsToProcess = (_MaxThumbPerThreadRun < _ThumbList.Count ? _MaxThumbPerThreadRun : _ThumbList.Count);
            for (int i = 0; i < NumItemsToProcess; i++)
            {
                if (IsPhoto(_ThumbList[i].FileExtension)) 
                {
                    Bitmap orgImage;

                    if (_ThumbList[i].FileFullName.IndexOf("_Enc@@") != -1) // encrypted file -> decrypt first
                    {
                        GetCurrentConfigEncryptionKey(false);
                        orgImage = (Bitmap)Bitmap.FromStream(DecryptFile(_ThumbList[i].FileFullName));
                    }
                    else
                    {
                        orgImage = (Bitmap)Bitmap.FromFile(_ThumbList[i].FileFullName);
                        
                    }
                    _ThumbList[i].ThumbnailImage = CreateThumbnail(orgImage);
                }
            }

            UpdateLargeIcons(NumItemsToProcess);
        }

        private delegate void UpdateLargeIconsDelegate(int ItemsProcessed);

        private void UpdateLargeIcons(int ItemsProcessed)
        {
            if (listView1.InvokeRequired)
            {
                UpdateLargeIconsDelegate d = new UpdateLargeIconsDelegate(UpdateLargeIcons);
                listView1.BeginInvoke(d, new object[] { ItemsProcessed });
            }
            else
            {
                for (int i = 0; i < ItemsProcessed; i++)
                {
                    thumbImageList.Images[_ThumbList[i].ThumbListIndex] = _ThumbList[i].ThumbnailImage;
                }
                
                listView1.Refresh();
                
                // remove processed thumbs
                for (int j = 0; j < ItemsProcessed; j++)
                {
                    _ThumbList.RemoveAt(0);
                }

                // still has unprocessed thumbs => run generator again
                if (_ThumbList.Count > 0)
                {
                    RunThumbGeneratorThread();
                }
            }
        }

        private string GetDisplaySize(long fileSize)
        {
            string retSize = fileSize.ToString();
            if (fileSize > 1048576)
            {
                retSize = ((double)fileSize / 1048576).ToString("n1") + " MB";
            }
            else if (fileSize > 1024)
            {
                retSize = ((double)fileSize / 1024).ToString("n1") + " KB";
            }
            
            return retSize;
        }

        // ** treeView1 ContextMenu **
        private string GetNonDuplicateFileName(string dirName, string fileName)
        {
            if (!File.Exists(dirName + fileName)) // no duplicate, return as is
            {
                return fileName;
            }
            else // duplicate
            {
                string fileNameWOExt = "", fileExt = "";
                if (fileName.IndexOf(".") >= 0) // has extension
                {
                    string[] fileParts = fileName.Split('.');
                    fileExt = "." + fileParts[fileParts.Length - 1]; // last item is extension
                    fileNameWOExt = fileName.Substring(0, fileName.Length - fileExt.Length);
                }
                else
                {
                    fileNameWOExt = fileName;
                }
                int startIndex = 1;
                while (File.Exists(dirName + fileNameWOExt + "(" + startIndex + ")" + fileExt))
                {
                    startIndex++;
                }

                return fileNameWOExt + "(" + startIndex.ToString() + ")" + fileExt;
            }
        }
        
        private void contextMenuTreeView1_Opening(object sender, CancelEventArgs e)
        {
            toolStripPasteFilesToFolder.Enabled = (_CutFiles.Items.Count > 0);
            toolStripRenameFolder.Enabled = (treeView1.SelectedNode.Level > 0);
            toolStripPasteFolder.Enabled = (_CutNode != null);
        }
        
        private void toolStripPasteFilesToFolder_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;
            string moveToDir = dirInfo.FullName + "\\";

            for (int i = 0; i < _CutFiles.Items.Count; i++)
            {
                string[] fileParts = _CutFiles.Items[i].ToString().Split('\\');
                if (File.Exists(_CutFiles.Items[i].ToString()))
                {
                    File.Move(_CutFiles.Items[i].ToString(), moveToDir + GetNonDuplicateFileName(moveToDir, fileParts[fileParts.Length - 1]));
                }
            }

            treeView1_SelectNode(treeView1.SelectedNode); //refresh file list
            _CutFiles.Items.Clear();
        }

        private void toolStripDeleteFolder_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0) return;

            DirectoryInfo dirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;

            if (DeleteFileOrFolderVB(dirInfo.FullName, 2))
            {
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                listView1.Items.Clear();
                treeView1_SelectNode(treeView1.SelectedNode); //refresh file list
            }
        }

        private void toolStripSetFav_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                DirectoryInfo nodeDirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;
                _FavFolderPath = nodeDirInfo.FullName;
                SetRegistryKey("FavFolderPath", _FavFolderPath);
                OpenFavoriteMenuItem.Enabled = true;
                OpenFavoriteMenuItem.ToolTipText = _FavFolderPath;
            }
        }

        private void toolStripRenameFolder_MouseHover(object sender, EventArgs e)
        {
            toolStripTextBoxFolder.Text = treeView1.SelectedNode.Text;
        }

        private void toolStripTextBoxFolder_LostFocus(object sender, EventArgs e)
        {
            string newFolderName = toolStripTextBoxFolder.Text.Trim();
            if (newFolderName != "" && newFolderName != treeView1.SelectedNode.Text)
            {
                try
                {
                    DirectoryInfo nodeDirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;
                    Directory.Move(nodeDirInfo.FullName, nodeDirInfo.Parent.FullName + "\\" + newFolderName);

                    // update current treeview item
                    treeView1.SelectedNode.Text = newFolderName;
                    DirectoryInfo newDir = new DirectoryInfo(nodeDirInfo.Parent.FullName + "\\" + newFolderName);
                    treeView1.SelectedNode.Tag = newDir;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("toolStripTextBoxFolder_LostFocus: " + ex.Message);
                }
            }
        }

        private void toolStripCutFolder_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0) return; // no cutting of C, D drive
            _CutNode = treeView1.SelectedNode;
        }

        private void toolStripPasteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo orgDirInfo = (DirectoryInfo)_CutNode.Tag;
                DirectoryInfo destRootDirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;
                DirectoryInfo newDirInfo = new DirectoryInfo(destRootDirInfo.FullName + "\\" + orgDirInfo.Name);
                Directory.Move(orgDirInfo.FullName, newDirInfo.FullName);
                TreeNode aNode = new TreeNode(newDirInfo.Name, 0, 0);
                aNode.ForeColor = Color.Black;
                aNode.Tag = newDirInfo;

                GetDirectories(newDirInfo.GetDirectories(), aNode, 1);
                treeView1.SelectedNode.Nodes.Add(aNode);

                treeView1.Nodes.Remove(_CutNode);
                _CutNode = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("toolStripPasteFolder_Click: " + ex.Message);
            }
        }

        private void refreshChildFolderList()
        {
            treeView1.SelectedNode.Nodes.Clear();
            treeView1.SelectedNode.ForeColor = Color.Teal;
            treeView1_SelectNode(treeView1.SelectedNode);
        }

        private void toolStripRefreshFolder_Click(object sender, EventArgs e)
        {
            refreshChildFolderList();
        }

        private void toolStripTextBoxNewFolder_LostFocus(object sender, EventArgs e)
        {
            string newFolderName = toolStripTextBoxNewFolder.Text.Trim();
            if (newFolderName != "")
            {
                try
                {
                    DirectoryInfo nodeDirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;
                    nodeDirInfo.CreateSubdirectory(newFolderName);
                    refreshChildFolderList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("toolStripTextBoxNewFolder_LostFocus: " + ex.Message);
                }
            }
        }

        private void toolStripEncryptAllAndDelete_Click(object sender, EventArgs e)
        {
            EncryptListViewFiles(2, true);
        }

        private void toolStripEncryptAll_Click(object sender, EventArgs e)
        {
            EncryptListViewFiles(2, false);
        }

        private void toolstripDecryptAllAndDelete_Click(object sender, EventArgs e)
        {
            DecryptListViewFiles(2, true);
        }

        private void toolStripDecryptAll_Click(object sender, EventArgs e)
        {
            DecryptListViewFiles(2, false);
        }


        // *** listView1 supporting methods ****
        
        string getFullFilePath(ListViewItem lvItem)
        {
            FileInfo fo = (FileInfo)lvItem.Tag;
            return fo.FullName;
        }

        void listView_ItemSelected(object sender, ListViewItemSelectionChangedEventArgs e) 
        {
            // verifying file   
            if (listView1.SelectedItems.Count == 0) return; 
            FileInfo fo = (FileInfo)listView1.SelectedItems[0].Tag;
            if (!File.Exists(fo.FullName) || fo.Length == 0) return;

            _CurrentFileExt = fo.Extension;

            if (IsPhoto(_CurrentFileExt))
            {
                loadPhoto(fo.FullName);
            }
            else if (IsAV(_CurrentFileExt))
            {
                tabControl1.SelectedTab = tabPage2;
                if (fo.FullName.IndexOf("_Enc@@") != -1) // encrypted, must decrypt first
                {
                    if (!GetCurrentConfigEncryptionKey(true)) return;

                    string tempFileName = System.Environment.CurrentDirectory + "\\0000" + fo.Extension;
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    if (DecryptFile(fo.FullName, tempFileName))
                    {
                        axWindowsMediaPlayer1.URL = tempFileName;
                    }
                    else
                    {
                        MessageBox.Show("Decrypting video file failed.");
                    }
                }
                else
                {
                    axWindowsMediaPlayer1.URL = fo.FullName;
                }
            }
        }

        void listView1_KeyPress(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete) // delete key pressed
            {
                deleteListViewSelectedItem();
                e.Handled = true;
            }
        
        }

        private void deleteListViewSelectedItem()
        {
            if (listView1.SelectedItems.Count == 0) return;

            if (IsPhoto(_CurrentFileExt))
            {
                clearPhoto();
            }
            else if (IsAV(_CurrentFileExt))
            {
                clearVideo();
            }

            int firstDelItemIndex = listView1.SelectedItems[0].Index;
            for (int i = listView1.SelectedItems.Count - 1; i >= 0; i--)
            {
                string DelFile = getFullFilePath(listView1.SelectedItems[i]);
                if (DeleteFileOrFolderVB(DelFile, 1))
                {
                    // delete successfull - remove deleted item from list
                    listView1.Items.Remove(listView1.SelectedItems[i]);
                }
                else
                {
                    break;
                }
            }

            // auto select next item
            if (listView1.Items.Count > firstDelItemIndex)
            {
                listView1.Items[firstDelItemIndex].Selected = true;
            }
            else if (listView1.Items.Count > 0)
            {
                listView1.Items[firstDelItemIndex - 1].Selected = true;
            }

            lbListCount.Text = "(" + listView1.Items.Count.ToString() + " files)";
        }


        private void LimitViewableMenuItem_Click(object sender, EventArgs e)
        {
            if (LimitViewableMenuItem.Image != null)
                LimitViewableMenuItem.Image = null;
            else
                LimitViewableMenuItem.Image = TNMediaViewer.Properties.Resources.icon_check; 
            // refresh file list
            if (treeView1.SelectedNode != null)
                treeView1_SelectNode(treeView1.SelectedNode);
        }

        private void ThumbOnlyMenuItem_Click(object sender, EventArgs e)
        {
            if (ThumbOnlyMenuItem.Image != null)
                ThumbOnlyMenuItem.Image = null;
            else
                ThumbOnlyMenuItem.Image = TNMediaViewer.Properties.Resources.icon_check;
            // refresh file list
            if (treeView1.SelectedNode != null)
                treeView1_SelectNode(treeView1.SelectedNode);
        }

        private void OpenFavoriteMenuItem_Click(object sender, EventArgs e)
        {
            if (_FavFolderPath != "")
            {
                string[] arrPath = _FavFolderPath.Split('\\');
                arrPath[0] += "\\";
                TreeNodeCollection nodeList = treeView1.Nodes;
                TreeNode SelectedNode = null;
                DirectoryInfo DirInfo;
                bool nodeFound;

                for (int i = 0; i < arrPath.Length; i++)
                {
                    nodeFound = false;
                    for (int j = 0; j < nodeList.Count; j++)
                    {
                        DirInfo = (DirectoryInfo)nodeList[j].Tag;
                        if (DirInfo.Name == arrPath[i])
                        {
                            SelectedNode = nodeList[j];
                            SelectedNode.Expand();
                            nodeFound = true;
                            if (SelectedNode.ForeColor != Color.Black)
                            {
                                GetDirectories(DirInfo.GetDirectories(), SelectedNode, 1);
                                SelectedNode.ForeColor = Color.Black;
                            }
                            break;
                        }
                        else
                        {
                            nodeList[j].Collapse();
                        }
                    }

                    if (!nodeFound || SelectedNode.Nodes.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        nodeList = SelectedNode.Nodes;
                    }
                }
                if (SelectedNode != null)
                {
                    SelectedNode.Expand();
                    treeView1.SelectedNode = SelectedNode;
                    treeView1_SelectNode(SelectedNode);
                }
            }

        }

        private void ClearScreensMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.SelectedItems.Clear();
            }
            clearPhoto();
            clearVideo();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string ToggleCheckLabel(string CurrentText)
        { 
            string[] TextParts = CurrentText.Split('[');
            if (TextParts[1] == "x]")
                return TextParts[0] + "[ ]";
            else
                return TextParts[0] + "[x]";
        }
        
        // ** listView1 ContextMenu **
        private void toolStripDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            deleteListViewSelectedItem();
        }

        private void contextMenuListView1_Opening(object sender, CancelEventArgs e)
        {
            bool enableItems = (listView1.SelectedItems.Count > 0);
            toolStripTextBox1.Enabled = enableItems;
            toolStripListViewDelete.Enabled = enableItems;
            toolStripOpenUsingSystemDefault.Enabled = enableItems;
            toolStripOpenWithIE.Enabled = enableItems;
            toolStripOpenWithVLC.Enabled = enableItems;
            toolStripOpenWithWM.Enabled = enableItems;
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }

        private void toolStripRename_MouseHover(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            string filename = listView1.SelectedItems[0].Text;
            filename = filename.Substring(0, filename.Length - _CurrentFileExt.Length);
            toolStripTextBox1.Text = filename;
        }

        private void toolStripTextBox1_LostFocus(object sender, EventArgs e)
        {
            string newFileName = toolStripTextBox1.Text.Trim();
            if (newFileName == "") return;

            newFileName += _CurrentFileExt;
            if (newFileName != listView1.SelectedItems[0].Text)
            {
                try
                {
                    FileInfo fo = (FileInfo)listView1.SelectedItems[0].Tag;
                    string destFile = fo.DirectoryName + "\\" + newFileName;
                    File.Move(fo.FullName, destFile);
                    
                    // update current listview item
                    listView1.SelectedItems[0].Text = newFileName;
                    FileInfo newFo = new FileInfo(destFile);
                    listView1.SelectedItems[0].Tag = newFo;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("toolStripTextBox1_LostFocus: " + ex.Message);
                }
            }
        }

        private void toolStripCutItems_Click(object sender, EventArgs e)
        {
            _CutFiles.Items.Clear(); //clear previous selections
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                _CutFiles.Items.Add(getFullFilePath(listView1.SelectedItems[i]));
            }
        }

        private void EncryptListViewFiles(int SelectionType, bool DeleteOriginal)
        {
            if (!GetCurrentConfigEncryptionKey(true)) return;

            if (SelectionType == 1) // selected items
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    FileInfo fo = (FileInfo)listView1.SelectedItems[i].Tag;
                    // media type and not already encrypted
                    if ((IsPhoto(fo.Extension) || IsAV(fo.Extension)) && fo.Name.IndexOf("_Enc@@") == -1)
                    {
                        string EncryptedfileName = fo.Name;
                        EncryptedfileName = EncryptedfileName.Substring(0, EncryptedfileName.Length - fo.Extension.Length) + "_Enc@@" + fo.Extension;
                        bool EncryptSuccess = EncryptFile(fo.FullName, fo.DirectoryName + "\\" + EncryptedfileName);
                        if (EncryptSuccess && DeleteOriginal)
                        {
                            File.Delete(fo.FullName);
                        }
                    }
                }

            }
            else // all
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    FileInfo fo = (FileInfo)listView1.Items[i].Tag;
                    // photo type and not already encrypted
                    if ((IsPhoto(fo.Extension) || IsAV(fo.Extension)) && fo.Name.IndexOf("_Enc@@") == -1)
                    {
                        string EncryptedfileName = fo.Name;
                        EncryptedfileName = EncryptedfileName.Substring(0, EncryptedfileName.Length - fo.Extension.Length) + "_Enc@@" + fo.Extension;
                        bool EncryptSuccess = EncryptFile(fo.FullName, fo.DirectoryName + "\\" + EncryptedfileName);
                        if (EncryptSuccess && DeleteOriginal)
                        {
                            File.Delete(fo.FullName);
                        }
                    }
                }
            }
            
            treeView1_SelectNode(treeView1.SelectedNode); //refresh file list
        }

        private void DecryptListViewFiles(int SelectionType, bool DeleteOriginal)
        {
            if (!GetCurrentConfigEncryptionKey(true)) return;

            if (SelectionType == 1) // selected items
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    FileInfo fo = (FileInfo)listView1.SelectedItems[i].Tag;
                    // encrypted file
                    if (fo.Name.IndexOf("_Enc@@") != -1)
                    {
                        string DecryptedfileName = fo.Name;
                        DecryptedfileName = DecryptedfileName.Substring(0, DecryptedfileName.Length - (fo.Extension.Length + 6)) + fo.Extension;
                        bool DecryptSuccess = DecryptFile(fo.FullName, fo.DirectoryName + "\\" + DecryptedfileName);
                        if (DecryptSuccess && DeleteOriginal)
                        {
                            DeleteFileOrFolderVB(fo.FullName, 1);
                        }
                    }
                }

            }
            else // all
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    FileInfo fo = (FileInfo)listView1.Items[i].Tag;
                    // encrypted file
                    if (fo.Name.IndexOf("_Enc@@") != -1)
                    {
                        string DecryptedfileName = fo.Name;
                        DecryptedfileName = DecryptedfileName.Substring(0, DecryptedfileName.Length - (fo.Extension.Length + 6)) + fo.Extension;
                        bool DecryptSuccess = DecryptFile(fo.FullName, fo.DirectoryName + "\\" + DecryptedfileName);
                        if (DecryptSuccess && DeleteOriginal)
                        {
                            DeleteFileOrFolderVB(fo.FullName, 1);
                        }
                    }
                }
            }
            
            treeView1_SelectNode(treeView1.SelectedNode); //refresh file list
        }

        private void ToolStripEncryptSelectedAndDelete_Click(object sender, EventArgs e)
        {
            EncryptListViewFiles(1, true);
        }

        private void ToolStripEncryptSelected_Click(object sender, EventArgs e)
        {
            EncryptListViewFiles(1, false);
        }

        private void ToolStripDeleteSelectedAndDelete_Click(object sender, EventArgs e)
        {
            DecryptListViewFiles(1, true);
        }

        private void ToolStripDecryptSelected_Click(object sender, EventArgs e)
        {
            DecryptListViewFiles(1, false);
        }

        private void toolStripOpenUsingSystemDefault_Click(object sender, EventArgs e)
        {
            ShellExecute(IntPtr.Zero, "open", getFullFilePath(listView1.SelectedItems[0]), "", "", ShowCommands.SW_SHOWNOACTIVATE);
        }

        private void toolStripOpenWithWM_Click(object sender, EventArgs e)
        {
            openSelectedWith("C:\\Program Files\\Windows Media Player\\wmplayer.exe", true);
        }

        private void toolStripOpenWithVLC_Click(object sender, EventArgs e)
        {
            openSelectedWith("C:\\Program Files\\VideoLAN\\VLC\\vlc.exe", true);
        }

        private void toolStripOpenWithIE_Click(object sender, EventArgs e)
        {
            openSelectedWith("IExplore.exe", false);
        }

        private void openSelectedWith(string programPath, bool UseDOSFileName)
        {
            string openFilePath = getFullFilePath(listView1.SelectedItems[0]);
            if (UseDOSFileName)
            {
                StringBuilder shortFilePath = new StringBuilder(1024);
                GetShortPathName(openFilePath, shortFilePath, shortFilePath.Capacity);
                openFilePath = shortFilePath.ToString();
            }
            Process.Start(programPath, openFilePath);
        }
        
 
        // **** Media processing supporting functions ***
        
        private void loadPhoto(string fullFilename)
        {
            //re-focus
            tabControl1.SelectedTab = tabPage1;

            if (fullFilename != "") // load new image
            {
                // pause wm player if currently playing something
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                }
                
                try
                {
                    Bitmap tempBitmap;
                    if (fullFilename.IndexOf("_Enc@@") != -1) // encrypted file -> decrypt first
                    {
                        if (!GetCurrentConfigEncryptionKey(true)) return;

                        tempBitmap = (Bitmap)Bitmap.FromStream(DecryptFile(fullFilename));
                    }
                    else
                    {
                        tempBitmap = (Bitmap)Bitmap.FromFile(fullFilename);
                    }
                    
                    _CurrentPic = new Bitmap(tempBitmap);
                    tempBitmap.Dispose();
                    trackBar2.Value = 50; //reset
                }
                catch (Exception ex)
                {
                    MessageBox.Show("loadPhoto: " + ex.Message);
                    _CurrentPic = null;
                    return;
                }

            }
            else if (_CurrentPic == null)
            {
                // refresh current view by Fit to screen click - use current if avail or do nothing
                 return; 
            }

            if (cbFitToScreen.Checked)
            {
                pictureBox1.Dock = DockStyle.Fill;

                int iZoomPerX = (int)((double)pictureBox1.Width / (double)_CurrentPic.Width * 100);
                int iZoomPerY = (int)((double)pictureBox1.Height / (double)_CurrentPic.Height * 100);
                int iZoomPer = (iZoomPerX < iZoomPerY ? iZoomPerX : iZoomPerY);

                trackBar1.Enabled = true;

                if (iZoomPer < trackBar1.Minimum)
                {
                    trackBar1.Value = trackBar1.Minimum;
                }
                else if (iZoomPer > trackBar1.Maximum)
                {
                    trackBar1.Value = trackBar1.Maximum;
                }
                else
                {
                    trackBar1.Value = iZoomPer;
                }


                lblZoomPercentage.Text = iZoomPer.ToString() + "%";
                pictureBox1.Image = AlterBrightness(ScaleByPercent(_CurrentPic, iZoomPer), trackBar2.Value);
            }
            else
            {
                pictureBox1.Dock = DockStyle.None;
                trackBar1.Value = 100;
                lblZoomPercentage.Text = "100%";
                pictureBox1.Width = _CurrentPic.Width;
                pictureBox1.Height = _CurrentPic.Height;
                pictureBox1.Image = AlterBrightness(_CurrentPic, trackBar2.Value);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            scaleByTrackBar1();
        }

        private void scaleByTrackBar1()
        {
            if (_CurrentPic != null)
            {
                pictureBox1.Dock = DockStyle.None;
                lblZoomPercentage.Text = trackBar1.Value.ToString() + "%";
                Image scaledbmp = ScaleByPercent(_CurrentPic, trackBar1.Value);
                pictureBox1.Width = scaledbmp.Width;
                pictureBox1.Height = scaledbmp.Height;
                pictureBox1.Image = AlterBrightness(scaledbmp, trackBar2.Value) ;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (_CurrentPic != null)
            {
                loadPhoto("");
            }
            lblBrightnessPercentage.Text = trackBar2.Value.ToString();
        }

        static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(0, 0, destWidth, destHeight),
                new Rectangle(0, 0, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        static Image CreateThumbnail(Image imgPhoto)
        {
            Bitmap thumbBmp = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(thumbBmp);
            g.FillRectangle(Brushes.White, 0, 0, 100, 100);
            
            //Adjust settings to make this as high-quality as possible
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality; 

            int thumbWidth, thumbHeight;
            if (imgPhoto.Width >= imgPhoto.Height) // reduce width to 100 then height proportionally
            {
                thumbWidth = 100;
                thumbHeight = (int)((double)imgPhoto.Height * (100.0 / imgPhoto.Width));
            }
            else // reduce height to 100 then width proportionally
            {
                thumbHeight = 100;
                thumbWidth = (int)((double)imgPhoto.Width * (100.0 / imgPhoto.Height));
            }

            // draw the original Image onto the empty Bitmap, dont use GetThumbnailImage() from original
            // because it returns poor quality thumb
            int top = (100 - thumbHeight) / 2;
            int left = (100 - thumbWidth) / 2;

            g.DrawImage(imgPhoto, new Rectangle(left, top, thumbWidth, thumbHeight),
                new Rectangle(0, 0, imgPhoto.Width, imgPhoto.Height),
                GraphicsUnit.Pixel);

            g.Dispose();

            return thumbBmp;
        }

 
        private static Image AlterBrightness(Image bmp, int level)
        {
            if (level == 50)
            {
                // do nothing
                return bmp;
            }
            
            Graphics graphics = Graphics.FromImage(bmp);
            if (level < 50)
            {
                // make it darker
                // Work out how much darker
                int conversion = 250 - (5 * level);
                Pen pDark = new Pen(Color.FromArgb(conversion, 0, 0, 0), bmp.Width * 2);
                graphics.DrawLine(pDark, 0, 0, bmp.Width, bmp.Height);
            }
            else if (level > 50)
            {
                // mark it lighter
                // Work out how much lighter.
                int conversion = (5 * (level - 50));
                Pen pLight = new Pen(Color.FromArgb(conversion, 255, 255, 255), bmp.Width * 2);
                graphics.DrawLine(pLight, 0, 0, bmp.Width, bmp.Height);
            }
            graphics.Save();
            graphics.Dispose();
            return bmp;
        } 

        private void clearPhoto()
        {
            if (_CurrentPic != null)
            {
                _CurrentPic.Dispose(); // release handle to current file to prevent delete file in-use error
                _CurrentPic = null;
                pictureBox1.Image = null;

            }
        }

        private void clearVideo()
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            axWindowsMediaPlayer1.URL = "";
        }
        
        private bool IsPhoto(string fileExt)
        {
            return ((PhotoExtlistBox.Items.IndexOf(fileExt.ToLower()) != -1));
        }

        private bool IsAV(string fileExt)
        {
            return ((MediaExtlistBox.Items.IndexOf(fileExt.ToLower()) != -1));
        }

        private void cbFitToScreen_CheckedChanged(object sender, EventArgs e)
        {
            loadPhoto("");
        }

        private bool EncryptFile(string inputFile, string outputFile)
        {
            bool bSuccess = false;

            if (_CurrentPassword == "") return bSuccess;

            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@_CurrentPassword);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

                bSuccess = true;
            }
            catch(Exception)
            {
                // nothing    
            }
            
            return bSuccess;
        }

        private MemoryStream DecryptFile(string inputFile)
        {
            if (_CurrentPassword == "") return _MSErrorImage;

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            try
            {
                MemoryStream msOut = new MemoryStream();
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@_CurrentPassword);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    msOut.WriteByte((byte)data);
                }
                cs.Close();
                fsCrypt.Close();

                return msOut;
            }
            catch 
            {
                fsCrypt.Close(); // release file lock
                return _MSErrorImage;
            }
        }

        private bool DecryptFile(string inputFile, string outputFile)
        {
            bool bSuccess = false;
            if (_CurrentPassword == "") return bSuccess;

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            try
            {
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@_CurrentPassword);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

                int data;
                while ((data = cs.ReadByte()) != -1)
                {
                    fsOut.WriteByte((byte)data);
                }
                cs.Close();
                fsCrypt.Close();
                fsOut.Close();
                bSuccess = true;
            }
            catch
            {
                fsCrypt.Close(); // release file lock
            }
            
            return bSuccess;
        }
        //**** Other ****
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);

      [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
      public static extern int GetShortPathName(
        [MarshalAs(UnmanagedType.LPTStr)] string path,
        [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
        int shortPathLength);


      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
      public struct SHFILEOPSTRUCT
      {
          public IntPtr hwnd;
          [MarshalAs(UnmanagedType.U4)]
          public int wFunc;
          public string pFrom;
          public string pTo;
          public short fFlags;
          [MarshalAs(UnmanagedType.Bool)]
          public bool fAnyOperationsAborted;
          public IntPtr hNameMappings;
          public string lpszProgressTitle;
      }
     
      private bool DeleteFileOrFolderVB(string fullPath, int delType)
      {
          try
          {
              if (delType == 1) // file
              {
                  Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                    fullPath,
                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin
                  );
              }
              else // folder
              {
                  Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(
                    fullPath,
                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin
                  );
              }
              
              return true;          
          }
          catch (Exception)
          {
              return false;          
          }
      }


      // ************ Configuration functions ****************
      private string GetConcatListItems(ListBox listB)
      {
          string RetValue = "";
          foreach (string value in listB.Items)
          {
              RetValue += value + "|";
          }
          if (RetValue != "") RetValue = RetValue.Substring(0, RetValue.Length - 1);

          return RetValue;
      }

      private void BtnDelExt_Click(object sender, EventArgs e)
      {
          if (PhotoExtlistBox.SelectedItem != null)
          {
              PhotoExtlistBox.Items.Remove(PhotoExtlistBox.SelectedItem);
              SetRegistryKey("PhotoExts", GetConcatListItems(PhotoExtlistBox));
          }
      }

      private void BtnNewExt_Click(object sender, EventArgs e)
      {
          string newExt = NewPhotoExttextBox.Text.Trim();
          if (newExt != "")
          {
              if (newExt.Substring(0, 1) != ".") newExt = "." + newExt;
              PhotoExtlistBox.Items.Add(newExt.ToLower());
              SetRegistryKey("PhotoExts", GetConcatListItems(PhotoExtlistBox));
          }
          NewPhotoExttextBox.Text = "";
      }

      private void BtnDelMediaExt_Click(object sender, EventArgs e)
      {
          if (MediaExtlistBox.SelectedItem != null)
          {
              MediaExtlistBox.Items.Remove(MediaExtlistBox.SelectedItem);
              SetRegistryKey("MediaExts", GetConcatListItems(MediaExtlistBox));
          }
      }

      private void BtnAddMediaExt_Click(object sender, EventArgs e)
      {
          string newExt = NewMediaExttextBox.Text.Trim();
          if (newExt != "")
          {
              if (newExt.Substring(0, 1) != ".") newExt = "." + newExt;
              MediaExtlistBox.Items.Add(newExt.ToLower());
              SetRegistryKey("MediaExts", GetConcatListItems(MediaExtlistBox));
          }
          NewMediaExttextBox.Text = "";
      }

      private void PopulatePhotoExtsList(string exts)
      { 
          PhotoExtlistBox.Items.AddRange(exts.Split('|'));
      }

      private void PopulateMediaExtsList(string exts)
      {
          MediaExtlistBox.Items.AddRange(exts.Split('|'));
      }

      
      private void LnkRestorePhotoExtsDefault_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          PhotoExtlistBox.Items.Clear();
          PopulatePhotoExtsList(_DefaultPhotoExts);
          SetRegistryKey("PhotoExts", "");
      }

      private void LnkRestoreMediaExtsDefault_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          MediaExtlistBox.Items.Clear();
          PopulateMediaExtsList(_DefaultWMExts);
          SetRegistryKey("MediaExts", "");
      }
   }

    public class ThumbnailItem
    {
        Image _ThumbImage = null;

        public ThumbnailItem(int ThumbListIndex, string FileFullName, string FileExtension)
        {
            this.ThumbListIndex = ThumbListIndex;
            this.FileFullName = FileFullName;
            this.FileExtension = FileExtension;
        }
        public int ThumbListIndex
        {
            get;
            set;
        }
        public string FileFullName
        {
            get;
            set;
        }
        public string FileExtension
        {
            get;
            set;
        }
        public Image ThumbnailImage
        {
            get { return _ThumbImage; }
            set { _ThumbImage = value;  }
        }
    }
}
