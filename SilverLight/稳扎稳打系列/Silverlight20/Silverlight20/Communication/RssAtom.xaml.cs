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

using System.Xml;
using System.IO;
using System.ServiceModel.Syndication;

namespace Silverlight20.Communication
{
    public partial class RssAtom : UserControl
    {
        public RssAtom()
        {
            InitializeComponent();

            // 演示如何处理 Rss/Atom
            RssDemo();
        }

        /// <summary>
        /// 演示如何处理 Rss/Atom
        /// </summary>
        void RssDemo()
        {
            // 让一个代理页面去请求相关的 Rss/Atom（如果用Silverlight直接去请求，则需要在目标域的根目录下配置策略文件）
            Uri uri = new Uri("http://localhost:3036/Proxy.aspx?url=http://webabcd.cnblogs.com/rss", UriKind.Absolute);

            // 实例化 WebClient
            System.Net.WebClient client = new System.Net.WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(rss_DownloadStringCompleted);
            client.DownloadStringAsync(uri);

            txtMsgRss.Text = "读取 RSS 数据中。。。";
        }

        void rss_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // 发生错误的话，则打印出来
                txtMsgRss.Text = e.Error.ToString();
                return;
            }

            // 将获得的字符串转换为 XmlReader
            var buffer = System.Text.Encoding.UTF8.GetBytes(e.Result);
            var ms = new MemoryStream(buffer);
            XmlReader reader = XmlReader.Create(ms);

            // 从指定的 XmlReader 中加载，以生成 SyndicationFeed
            SyndicationFeed feed = SyndicationFeed.Load(reader);

            // 设置 list 的数据源为 Rss/Atom 的项集合（SyndicationFeed.Items）
            list.ItemsSource = feed.Items;
            txtMsgRss.Text = e.Result + "\r\n";
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 设置 detail 的数据上下文为 Rss/Atom 的指定项（SyndicationItem）
            detail.DataContext = list.SelectedItem as SyndicationItem;
        }
    }
}
