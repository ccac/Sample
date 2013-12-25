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

using System.Xml.Linq;
using System.IO;

namespace Silverlight20.Communication
{
    public partial class Xml : UserControl
    {
        public Xml()
        {
            InitializeComponent();

            // 演示如何处理 XML（对象）
            XmlDemo();

            // 演示如何处理 XML（集合）
            XmlDemo2();
        }

        /// <summary>
        /// 演示如何处理 XML（对象）
        /// </summary>
        void XmlDemo()
        {
            // REST 服务的 URL
            Uri uri = new Uri("http://localhost:3036/REST.svc/User/webabcd/xml", UriKind.Absolute);

            // 实例化 WebClient
            System.Net.WebClient client = new System.Net.WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(xml_DownloadStringCompleted);
            client.DownloadStringAsync(uri);

            txtMsgXml.Text = "读取 XML（对象） 数据中。。。";
        }

        void xml_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // 发生错误的话，则打印出来
                txtMsgXml.Text = e.Error.ToString();
                return;
            }

            // 将获得的字符串转换为 XML（对象）
            var buffer = System.Text.Encoding.UTF8.GetBytes(e.Result);
            var ms = new MemoryStream(buffer);

            XElement xmlObject = XElement.Load(ms);

            txtMsgXml.Text = e.Result + "\r\n";
            XNamespace ns = "http://webabcd.cnblogs.com/";
            txtMsgXml.Text += string.Format("姓名: {0}, 生日: {1}",
                (string)xmlObject.Element(ns + "Name"),
                ((DateTime)xmlObject.Element(ns + "DayOfBirth")).ToString("yyyy-MM-dd"));

            /* 
             * 总结：
             * XElement - 表示一个 XML 元素
             *     XElement.Element - XML 元素内的 XML 元素
             *     XElement.Attribute - XML 元素内的 XML 属性
             *     XElement.Load(Stream) - 使用指定流创建一个 XElement 对象
             *     XElement.Parse(String) - 解析指定的 XML 字符串为一个 XElement 对象
             * XAttribute - 表示一个 XML 属性
             */
        }

        /// <summary>
        /// 演示如何处理 XML（集合）
        /// </summary>
        void XmlDemo2()
        {
            // REST 服务的 URL
            Uri uri = new Uri("http://localhost:3036/REST.svc/Users/xml", UriKind.Absolute);

            // 实例化 WebClient
            System.Net.WebClient client = new System.Net.WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(xml2_DownloadStringCompleted);
            client.DownloadStringAsync(uri);

            txtMsgXml2.Text = "读取 XML（集合） 数据中。。。";
        }

        void xml2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // 发生错误的话，则打印出来
                txtMsgXml2.Text = e.Error.ToString();
                return;
            }

            // 将获得的字符串转换为 XML（集合）
            XDocument xmlObject = XDocument.Parse(e.Result);

            txtMsgXml2.Text = e.Result + "\r\n";
            XNamespace ns = "http://webabcd.cnblogs.com/";
            var obj = from p in xmlObject.Elements(ns + "ArrayOfUser").Elements(ns + "User")
                      where p.Element(ns + "Name").Value == "webabcd02"
                      select new { Name = (string)p.Element(ns + "Name"), DayOfBirth = (DateTime)p.Element(ns + "DayOfBirth") };
            
            txtMsgXml2.Text += string.Format("姓名: {0}, 生日: {1}",
                obj.First().Name,
                obj.First().DayOfBirth.ToString("yyyy-MM-dd"));


            /* 
             * 总结：
             * LINQ to XML 相当的方便
             */
        }
    }
}
