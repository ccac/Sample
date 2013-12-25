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

using System.Windows.Browser;

namespace Silverlight20.WebPage
{
    public partial class DOMDemo : UserControl
    {
        public DOMDemo()
        {
            InitializeComponent();

            Demo();
        }

        void Demo()
        {
            // 获取当前页面的 id 为 hello 的DOM，并设置其样式
            HtmlElement container = HtmlPage.Document.GetElementById("hello");
            container.SetStyleAttribute("display", "block");

            // 创建一个 ul 
            HtmlElement ul = HtmlPage.Document.CreateElement("ul");

            for (int i = 0; i < 10; i++)
            {
                // 创建一个 li ，并设置其显示的内容
                HtmlElement li = HtmlPage.Document.CreateElement("li");
                li.SetAttribute("innerText", "hi: DOM");

                // 将 li 添加到 ul 内
                ul.AppendChild(li);
            }

            // 将 ul 添加到 id 为 hello 的 DOM 内
            container.AppendChild(ul);


            // 创建一个页面的 button ，并设置其 value 属性和 onclick 事件
            HtmlElement button = HtmlPage.Document.CreateElement("button");
            button.SetProperty("value", "hi: Silverlight");
            button.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(HelloClick));

            // 将 button 添加到 id 为 hello 的 DOM 内
            container.AppendChild(button);
        }

        void HelloClick(object sender, HtmlEventArgs e)
        {
            // 页面的 button 单击后所需执行的逻辑
            txtMsg.Text += "hi: Silverlight\r\n";
        }
    }
}
