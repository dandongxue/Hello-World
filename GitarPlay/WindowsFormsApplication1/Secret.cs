using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Secret
    {
        public string GetCpuID()
        {
            try
            {
                string CpuID = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    CpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                moc = null;
                mc = null;
                return CpuID;
            }
            catch
            {
                return "unknow";
            }
        }
  
        public Boolean CheckCpuIdentit(String CpuId)
        {
            String workDir = new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.FullName;
            String filePath = "Secret";
            if (File.Exists(filePath) == false)
            {
                FileStream fs = new FileStream("Secret", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(CpuId);
                sw.Close();
                MessageBox.Show("系统注册成功，欢迎使用！");
                return true;
            }
            else { 
                try  
                {  
                    StreamReader sr = new StreamReader(filePath, System.Text.Encoding.GetEncoding("utf-8"));  
                    string content = sr.ReadToEnd().ToString();  
                    sr.Close();

                    if (content == CpuId)
                        return 
                            true;
                    else 
                        return false;
                    //return content;  
                }  
                catch (Exception)
                    {
                        MessageBox.Show("文件获取失败，请注册该系统！");
                    }
            }

            return false;
        }
    }
}
