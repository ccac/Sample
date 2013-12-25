/*
关于使用 JavaScript 操作 Cookie 参看
http://msdn.microsoft.com/en-us/library/ms533693(VS.85).aspx 
*/
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
using System.Text.RegularExpressions;

namespace Silverlight20.Tip
{
    public partial class Cookie : UserControl
    {
        public Cookie()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置 Cookie
        /// </summary>
        private void btnSetCookie_Click(object sender, RoutedEventArgs e)
        {
            if (txtKey.Text.Trim() != "" && txtValue.Text.Trim() != "")
            {
                string expire = DateTime.Now.AddDays(1).ToString("R"); // RFC1123Pattern 日期格式
                string cookie = string.Format("{0}={1};expires={2}",
                    txtKey.Text.Trim(),
                    txtValue.Text.Trim(),
                    expire);

                // 通过 JavaScript 设置 Cookie
                // 如下语句等于在 JavaScript 中给 document.cookie 赋值
                HtmlPage.Document.SetProperty("cookie", cookie);
            }
        }

        /// <summary>
        /// 获取 Cookie
        /// </summary>
        private void btnGetCookie_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "";

            // 通过 JavaScript 获取 Cookie
            // HtmlPage.Document.Cookies 就是 JavaScript 中的 document.cookie
            string[] cookies = Regex.Split(HtmlPage.Document.Cookies, "; ");

            foreach (var cookie in cookies)
            {
                string[] keyValue = cookie.Split('=');

                if (keyValue.Length == 2)
                {
                    txtResult.Text += keyValue[0] + " : " + keyValue[1];
                    txtResult.Text += "\n";
                }
            }
        }

        /// <summary>
        /// 删除 Cookie
        /// </summary>
        private void btnDeleteCookie_Click(object sender, RoutedEventArgs e)
        {
            string[] cookies = Regex.Split(HtmlPage.Document.Cookies, "; ");

            foreach (var cookie in cookies)
            {
                string[] keyValue = cookie.Split('=');

                if (keyValue.Length == 2)
                {
                    HtmlPage.Document.SetProperty("cookie", keyValue[0] + "=;" + DateTime.Now.AddDays(-1).ToString("R"));
                }
            }
        }
    }
}