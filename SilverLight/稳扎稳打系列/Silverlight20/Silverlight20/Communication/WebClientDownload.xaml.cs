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

namespace Silverlight20.Communication
{
    public partial class WebClientDownload : UserControl
    {
        // 用于演示以字符串的形式下载数据
        string _urlString = "http://wanglei-pc/UrichADFile/logo.bmp";

        // 用于演示以流的形式下载数据
        string _urlStream = "http://wanglei-pc/UrichADFile/logo.png";

        public WebClientDownload()
        {
            InitializeComponent();

            // 演示字符串式下载
            DownloadStringDemo();

            // 演示流式下载
            DownloadStreamDemo();
        }

        /// <summary>
        /// 演示字符串式下载
        /// </summary>
        void DownloadStringDemo()
        {
            Uri uri = new Uri(_urlString, UriKind.Absolute);

            /* 
             * WebClient - 将数据发送到指定的 URI，或者从指定的 URI 接收数据的类
             *     DownloadStringCompleted - 下载数据完毕后（包括取消操作及有错误发生时）所触发的事件
             *     DownloadProgressChanged - 下载数据过程中所触发的事件。正在下载或下载完全部数据后会触发
             *     DownloadStringAsync(Uri address, Object userToken) - 以字符串的形式下载指定的 URI 的资源
             *         Uri address - 需要下载的资源地址
             *         Object userToken - 用户标识
             */

            System.Net.WebClient clientDownloadString = new System.Net.WebClient();

            clientDownloadString.DownloadStringCompleted += new DownloadStringCompletedEventHandler(clientDownloadString_DownloadStringCompleted);
            clientDownloadString.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientDownloadString_DownloadProgressChanged);
            clientDownloadString.DownloadStringAsync(uri, "userToken");
        }

        void clientDownloadString_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            /*
             * DownloadProgressChangedEventArgs.ProgressPercentage - 下载完成的百分比
             * DownloadProgressChangedEventArgs.BytesReceived - 当前收到的字节数
             * DownloadProgressChangedEventArgs.TotalBytesToReceive - 总共需要下载的字节数
             * DownloadProgressChangedEventArgs.UserState - 用户标识
             */

            lblMsgString.Text = string.Format("下载完成的百分比：{0}\r\n当前收到的字节数：{1}\r\n总共需要下载的字节数：{2}\r\n",
                e.ProgressPercentage.ToString() + "%",
                e.BytesReceived.ToString(),
                e.TotalBytesToReceive.ToString());

            progressBarString.Value = (double)e.ProgressPercentage;
        }

        void clientDownloadString_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            /*
             * DownloadStringCompletedEventArgs.Error - 该异步操作期间是否发生了错误
             * DownloadStringCompletedEventArgs.Cancelled - 该异步操作是否已被取消
             * DownloadStringCompletedEventArgs.Result - 下载后的字符串类型的数据
             * DownloadStringCompletedEventArgs.UserState - 用户标识
             */

            if (e.Error != null)
            {
                lblMsgString.Text += e.Error.ToString();
                return;
            }

            if (e.Cancelled != true)
            {
                lblMsgString.Text += string.Format("用户标识：{0}", e.UserState.ToString());
            }
        }



        /// <summary>
        /// 演示流式下载
        /// </summary>
        void DownloadStreamDemo()
        {
            Uri uri = new Uri(_urlStream, UriKind.Absolute);

            /* 
             * WebClient - 将数据发送到指定的 URI，或者从指定的 URI 接收数据的类
             *     IsBusy - 指定的web请求是否正在进行中
             *     CancelAsync() - 取消指定的异步操作 
             *     OpenReadCompleted - 数据读取完毕后（包括取消操作及有错误发生时）所触发的事件。流的方式
             *     DownloadProgressChanged - 下载数据过程中所触发的事件。正在下载或下载完全部数据后会触发
             *     OpenReadAsync(Uri address, Object userToken) - 以流的形式下载指定的 URI 的资源
             *         Uri address - 需要下载的资源地址
             *         Object userToken - 用户标识
             */

            System.Net.WebClient clientDownloadStream = new System.Net.WebClient();

            if (clientDownloadStream.IsBusy)
                clientDownloadStream.CancelAsync();

            clientDownloadStream.OpenReadCompleted += new OpenReadCompletedEventHandler(clientDownloadStream_OpenReadCompleted);
            clientDownloadStream.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientDownloadStream_DownloadProgressChanged);
            clientDownloadStream.OpenReadAsync(uri);
        }

        void clientDownloadStream_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            /*
             * DownloadProgressChangedEventArgs.ProgressPercentage - 下载完成的百分比
             * DownloadProgressChangedEventArgs.BytesReceived - 当前收到的字节数
             * DownloadProgressChangedEventArgs.TotalBytesToReceive - 总共需要下载的字节数
             * DownloadProgressChangedEventArgs.UserState - 用户标识
             */

            lblMsgString.Text = string.Format("下载完成的百分比：{0}\r\n当前收到的字节数：{1}\r\n总共需要下载的字节数：{2}\r\n",
                e.ProgressPercentage.ToString() + "%",
                e.BytesReceived.ToString(),
                e.TotalBytesToReceive.ToString());

            progressBarStream.Value = (double)e.ProgressPercentage;
        }

        void clientDownloadStream_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            /*
             * OpenReadCompletedEventArgs.Error - 该异步操作期间是否发生了错误
             * OpenReadCompletedEventArgs.Cancelled - 该异步操作是否已被取消
             * OpenReadCompletedEventArgs.Result - 下载后的 Stream 类型的数据
             * OpenReadCompletedEventArgs.UserState - 用户标识
             */

            if (e.Error != null)
            {
                lblMsgStream.Text += e.Error.ToString();
                return;
            }

            if (e.Cancelled != true)
            {
                System.Windows.Media.Imaging.BitmapImage imageSource = new System.Windows.Media.Imaging.BitmapImage();
                imageSource.SetSource(e.Result);
                img.Source = imageSource;
            }
        }
    }
}