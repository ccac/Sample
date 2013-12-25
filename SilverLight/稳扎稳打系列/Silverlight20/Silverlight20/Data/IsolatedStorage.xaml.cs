using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.IO.IsolatedStorage;
using System.IO;

namespace Silverlight20.Data
{
    public partial class IsolatedStorage : UserControl
    {
        public IsolatedStorage()
        {
            InitializeComponent();

            // 演示 IsolatedStorageFile
            Demo();

            // 演示 IsolatedStorageSettings
            Demo2();
        }

        /// <summary>
        /// 演示 IsolatedStorageFile
        /// </summary>
        void Demo()
        {
            // Isolated Storage - 独立存储。一个虚拟文件系统

            // IsolatedStorageFile - 操作 独立存储 的类
            //     IsolatedStorageFile.GetUserStoreForSite() - 按站点获取用户的独立存储
            //     IsolatedStorageFile.GetUserStoreForApplication() - 按应用程序获取用户的独立存储
            
            // using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // DirectoryExists(path) - 指定的路径是否存在
                // CreateDirectory(path) - 创建指定的路径
                // FileExists(path) - 指定的文件是否存在
                // CreateFile(path) - 创建指定的文件
                // GetDirectoryNames() - 获取根目录下的目录名数组
                // GetFileNames()() - 获取根目录下的文件名数组
                // GetDirectoryNames(path) - 获取指定目录下的目录名数组
                // GetFileNames(path) - 获取指定目录下的文件名数组
                // OpenFile() - 打开指定的文件。具体参数参看文档
                // DeleteFile(path) - 删除指定的文件
                // DeleteDirectory(path) - 删除指定的目录（要求目录存在，且目录内无内容）
                // Remove() - 关闭 IsolatedStorageFile 对象并移除独立存储内的全部内容


                // 在根目录下创建指定的目录
                if (!isf.DirectoryExists("Directory01"))
                    isf.CreateDirectory("Directory01");
                if (!isf.DirectoryExists("Directory02"))
                    isf.CreateDirectory("Directory02");

                // 创建指定的子目录
                string subDirectory01 = System.IO.Path.Combine("Directory01", "SubDirectory01");
                string subDirectory02 = System.IO.Path.Combine("Directory01", "SubDirectory02");
                if (!isf.DirectoryExists(subDirectory01))
                    isf.CreateDirectory(subDirectory01);
                if (!isf.DirectoryExists(subDirectory02))
                    isf.CreateDirectory(subDirectory02);
                

                // 根目录下创建指定的文件
                if (!isf.FileExists("RootFile.txt"))
                {
                    IsolatedStorageFileStream isfs = isf.CreateFile("RootFile01.txt");
                    isfs.Close();
                }

                // 在指定的目录下创建指定的文件
                string file01 = System.IO.Path.Combine(subDirectory01, "File01.txt");
                string file02 = System.IO.Path.Combine(subDirectory01, "File02.txt");
                string file03 = System.IO.Path.Combine(subDirectory01, "File03.xml");
                if (!isf.FileExists(file01))
                {
                    // IsolatedStorageFileStream - 独立存储内的文件流。继承自 FileStream
                    IsolatedStorageFileStream isfs = isf.CreateFile(file01);
                    isfs.Close();
                }
                if (!isf.FileExists(file02))
                {
                    IsolatedStorageFileStream isfs = isf.CreateFile(file02);
                    isfs.Close();
                }
                if (!isf.FileExists(file03))
                {
                    IsolatedStorageFileStream isfs = isf.CreateFile(file03);
                    isfs.Close();
                }


                txtMsg.Text += "根目录下的目录列表：\r\n";
                // 获取根目录下的目录名数组
                foreach (string directoryName in isf.GetDirectoryNames())
                {
                    txtMsg.Text += directoryName + "\r\n";
                }

                txtMsg.Text += "根目录下的文件列表：\r\n";
                // 获取根目录下的文件名数组
                foreach (string fileName in isf.GetFileNames())
                {
                    txtMsg.Text += fileName + "\r\n";
                }

                txtMsg.Text += "目录 Directory01 下的目录列表：\r\n";
                // 获取 Directory01 目录下的目录名数组
                foreach (string directoryName in isf.GetDirectoryNames(subDirectory01))
                {
                    txtMsg.Text += directoryName + "\r\n";
                }

                txtMsg.Text += "目录 Directory01/SubDirectory01 下的*.txt文件列表：\r\n";
                // 获取 Directory01/SubDirectory01 目录下的后缀名为 txt 的文件名数组
                foreach (string fileName in isf.GetFileNames(System.IO.Path.Combine(subDirectory01, "*.txt")))
                {
                    txtMsg.Text += fileName + "\r\n";
                }


                if (isf.FileExists(file01))
                {
                    // 在文件 file01 中写入内容
                    IsolatedStorageFileStream streamWrite = isf.OpenFile(file01, FileMode.Open, FileAccess.Write);
                    using (StreamWriter sw = new StreamWriter(streamWrite))
                    {
                        sw.WriteLine("我是：webabcd");
                        sw.WriteLine("我专注于asp.net, Silverlight");
                    }

                    txtMsg.Text += "文件 File01.txt 的内容：\r\n";
                    // 读取文件 file01 中的内容
                    IsolatedStorageFileStream streamRead = isf.OpenFile(file01, FileMode.Open, FileAccess.Read);
                    using (StreamReader sr = new StreamReader(streamRead))
                    {
                        txtMsg.Text += sr.ReadToEnd();
                    }
                }


                // 删除文件 file01
                if (isf.FileExists(file01))
                {
                    isf.DeleteFile(file01);
                }

                try
                {
                    // 删除目录 subDirectory01
                    isf.DeleteDirectory(subDirectory01);
                }
                catch (IsolatedStorageException ex)
                {
                    // IsolatedStorageException - 操作临时存储失败时抛出的异常

                    // 因为 subDirectory01 目录内还有文件，所以会抛异常
                    txtMsg.Text += ex.Message;
                }
            }
        }

        /// <summary>
        /// 演示 IsolatedStorageSettings
        /// </summary>
        void Demo2()
        {
            // IsolatedStorageSettings - 在独立存储中保存的 key-value 字典表
            //     IsolatedStorageSettings.SiteSettings - 按站点保存的 key-value 字典表
            //     IsolatedStorageSettings.ApplicationSettings - 按应用程序保存的 key-value 字典表

            IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;

            // Add(key, value) - 添加一对 key-value
            iss.Add("key", "value");
            txtMsg2.Text += (string)iss["key"] + "\r\n";

            // 修改指定的 key 的 value
            iss["key"] = "value2";
            txtMsg2.Text += (string)iss["key"] + "\r\n";

            // Remove(key) - 移除指定的 key
            // Count - 字典表内的总的 key-value 数
            iss.Remove("key");
            txtMsg2.Text += iss.Count;
        }
       
        private void increase_Click(object sender, RoutedEventArgs e)
        {
            // 演示独立存储的配额的相关操作
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Quota - 当前配额（KB）
                // IncreaseQuotaTo(newQuotaSize) - 增加到指定的配额
                // AvailableFreeSpace - 当前的可用配额

                isf.IncreaseQuotaTo(isf.Quota + 1 * 1024 * 1024);

                System.Windows.Browser.HtmlPage.Window.Alert(
                    string.Format("当前配额：{0}；可用配额：{1}", isf.Quota, isf.AvailableFreeSpace));
            }
        } 
    }
}
