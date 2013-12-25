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

using System.IO;
using System.Windows.Resources;
using System.ComponentModel;
using System.Windows.Browser;

namespace Silverlight20.Communication
{
    public partial class WebClientUpload : UserControl
    {
        // 用于演示以字符串的形式上传数据
        string _urlString = "http://localhost:3036/REST.svc/UploadString/?fileName=";

        // 用于演示以流的形式上传数据
        string _urlStream = "http://localhost:3036/REST.svc/UploadStream/?fileName=";

        public WebClientUpload()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 演示字符串式上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnString_Click(object sender, RoutedEventArgs e)
        {
            string data = "";

            /*
             * OpenFileDialog - 文件对话框
             *     ShowDialog() - 显示文件对话框。在文件对话框中单击“确定”则返回true，反之则返回false
             *     File - 所选文件的 FileInfo 对象
             */

            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                using (FileStream fs = dialog.File.OpenRead())
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    // 将指定的 byte[] 转换为字符串（使用Base64编码）
                    data = Convert.ToBase64String(buffer);
                }

                /*
                 * WebClient - 将数据发送到指定的 URI，或者从指定的 URI 接收数据的类
                 *     UploadStringCompleted - 上传数据完毕后（包括取消操作及有错误发生时）所触发的事件
                 *     UploadProgressChanged - 上传数据过程中所触发的事件。正在上传或上传完全部数据后会触发
                 *     Headers - 与请求相关的的标头的 key/value 对集合
                 *     UploadStringAsync(Uri address, string data) - 以字符串的形式上传数据到指定的 URI。所使用的 HTTP 方法默认为 POST
                 *         Uri address - 接收上传数据的 URI
                 *         string data - 需要上传的数据
                 */

                System.Net.WebClient clientUploadString = new System.Net.WebClient();

                clientUploadString.UploadStringCompleted += new UploadStringCompletedEventHandler(clientUploadString_UploadStringCompleted);
                clientUploadString.UploadProgressChanged += new UploadProgressChangedEventHandler(clientUploadString_UploadProgressChanged);

                Uri uri = new Uri(_urlString + dialog.File.Name, UriKind.Absolute);
                clientUploadString.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                clientUploadString.UploadStringAsync(uri, data);
            }
        }

        void clientUploadString_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            /*
             * UploadProgressChangedEventArgs.ProgressPercentage - 上传完成的百分比
             * UploadProgressChangedEventArgs.BytesSent - 当前发送的字节数
             * UploadProgressChangedEventArgs.TotalBytesToSend - 总共需要发送的字节数
             * UploadProgressChangedEventArgs.BytesReceived - 当前接收的字节数
             * UploadProgressChangedEventArgs.TotalBytesToReceive - 总共需要接收的字节数
             * UploadProgressChangedEventArgs.UserState - 用户标识
             */

            lblMsgString.Text = string.Format("上传完成的百分比：{0}\r\n当前发送的字节数：{1}\r\n总共需要发送的字节数：{2}\r\n当前接收的字节数：{3}\r\n总共需要接收的字节数：{4}\r\n",
                e.ProgressPercentage.ToString(),
                e.BytesSent.ToString(),
                e.TotalBytesToSend.ToString(),
                e.BytesReceived.ToString(),
                e.TotalBytesToReceive.ToString());

            progressBarString.Value = (double)e.ProgressPercentage;
        }

        void clientUploadString_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            /*
             * UploadStringCompletedEventArgs.Error - 该异步操作期间是否发生了错误
             * UploadStringCompletedEventArgs.Cancelled - 该异步操作是否已被取消
             * UploadStringCompletedEventArgs.Result - 服务端返回的数据（字符串类型）
             * UploadStringCompletedEventArgs.UserState - 用户标识
             */

            if (e.Error != null)
            {
                lblMsgString.Text += e.Error.ToString();
                return;
            }

            if (e.Cancelled != true)
            {
                var jsonObject = System.Json.JsonObject.Parse(e.Result);
                
                lblMsgString.Text += string.Format("是否上传成功：{0}",
                    (bool)jsonObject);
            }
        }



        /// <summary>
        /// 演示流式上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStream_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = null;

            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                fs = dialog.File.OpenRead();

                /*
                 * WebClient - 将数据发送到指定的 URI，或者从指定的 URI 接收数据的类
                 *     OpenWriteCompleted - 在打开用于上传的流完成时（包括取消操作及有错误发生时）所触发的事件
                 *     WriteStreamClosed - 在写入数据流的异步操作完成时（包括取消操作及有错误发生时）所触发的事件
                 *     UploadProgressChanged - 上传数据过程中所触发的事件。如果调用 OpenWriteAsync() 则不会触发此事件
                 *     Headers - 与请求相关的的标头的 key/value 对集合
                 *     OpenWriteAsync(Uri address, string method, Object userToken) - 打开流以使用指定的方法向指定的 URI 写入数据
                 *         Uri address - 接收上传数据的 URI
                 *         string method - 所使用的 HTTP 方法（POST 或 GET）
                 *         Object userToken - 需要上传的数据流
                 */

                System.Net.WebClient clientUploadStream = new System.Net.WebClient();

                clientUploadStream.OpenWriteCompleted += new OpenWriteCompletedEventHandler(clientUploadStream_OpenWriteCompleted);
                clientUploadStream.UploadProgressChanged += new UploadProgressChangedEventHandler(clientUploadStream_UploadProgressChanged);
                clientUploadStream.WriteStreamClosed += new WriteStreamClosedEventHandler(clientUploadStream_WriteStreamClosed);

                Uri uri = new Uri(_urlStream + dialog.File.Name, UriKind.Absolute);
                clientUploadStream.Headers["Content-Type"] = "multipart/form-data";

                clientUploadStream.OpenWriteAsync(uri, "POST", fs);
            }
        }

        void clientUploadStream_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            // 因为是调用 OpenWriteAsync()，所以不会触发 UploadProgressChanged 事件

            lblMsgString.Text = string.Format("上传完成的百分比：{0}\r\n当前发送的字节数：{1}\r\n总共需要发送的字节数：{2}\r\n当前接收的字节数：{3}\r\n总共需要接收的字节数：{4}\r\n",
                e.ProgressPercentage.ToString(),
                e.BytesSent.ToString(),
                e.TotalBytesToSend.ToString(),
                e.BytesReceived.ToString(),
                e.TotalBytesToReceive.ToString());

            progressBarStream.Value = (double)e.ProgressPercentage;
        }

        void clientUploadStream_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            System.Net.WebClient client = sender as System.Net.WebClient;

            if (e.Error != null)
            {
                lblMsgStream.Text += e.Error.ToString();
                return;
            }

            if (e.Cancelled != true)
            {
                // e.UserState - 需要上传的流（客户端流）
                Stream clientStream = e.UserState as Stream;

                // e.Result - 目标地址的流（服务端流）
                Stream serverStream = e.Result;

                byte[] buffer = new byte[4096];
                int count = 0;

                // clientStream.Read - 将需要上传的流读取到指定的字节数组中
                while ((count = clientStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // serverStream.Write - 将指定的字节数组写入到目标地址的流
                    serverStream.Write(buffer, 0, count);
                }

                serverStream.Close();
                clientStream.Close();
            }
        }

        void clientUploadStream_WriteStreamClosed(object sender, WriteStreamClosedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMsgStream.Text += e.Error.ToString();
                return;
            }
            else
            {
                lblMsgStream.Text += "上传完成";
            }
        }
    }
}

/*
 * 其他：
 * 1、WebClient 对象一次只能启动一个请求。如果在一个请求完成（包括出错和取消）前，即IsBusy为true时，进行第二个请求，则第二个请求将会抛出 NotSupportedException 类型的异常
 * 2、如果 WebClient 对象的 BaseAddress 属性不为空，则 BaseAddress 与 URI（相对地址） 组合在一起构成绝对 URI
 * 3、WebClient 类的 AllowReadStreamBuffering 属性：是否对从 Internet 资源接收的数据做缓冲处理。默认值为true，将数据缓存在客户端内存中，以便随时被应用程序读取
 */
