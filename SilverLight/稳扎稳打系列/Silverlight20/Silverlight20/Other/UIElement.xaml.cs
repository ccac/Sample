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

namespace Silverlight20.Other
{
    public partial class UIElement : UserControl
    {
        public UIElement()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            // 如果设置了 UIElement 的 IsHitTestVisible="False"，则不会响应此句

            HtmlWindow html = HtmlPage.Window;
            html.Alert("webabcd");
        }
    }
}
