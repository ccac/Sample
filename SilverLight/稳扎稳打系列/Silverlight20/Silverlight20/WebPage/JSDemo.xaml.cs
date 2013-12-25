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
    public partial class JSDemo : UserControl
    {
        public JSDemo()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(JSDemo_Loaded);
        }

        private void invokeJS_Click(object sender, RoutedEventArgs e)
        {
            // 调用页面的 JavaScript 方法
            HtmlPage.Window.Invoke("silverlightInvokeJS", "webabcd");

            // 执行任意 JavaScript 语句
            HtmlPage.Window.Eval("silverlightInvokeJS('webabcd2')");
        }

        void JSDemo_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.Document.GetElementById("btnHello").SetStyleAttribute("display", "inline");

            // 将此对象注册到客户端中，所对应的客户端的对象名为 silverlightObject
            HtmlPage.RegisterScriptableObject("silverlightObject", this);
        }

        /// <summary>
        /// Hello 方法
        /// 暴露给页面的方法，调用后在 Silverlight 中显示结果
        /// </summary>
        /// <param name="name">名字</param>
        [ScriptableMember] // 脚本化此方法
        public void hello(string name)
        {
            txtMsg.Text += string.Format("Hello: {0}\r\n", name);
        }
    }
}
