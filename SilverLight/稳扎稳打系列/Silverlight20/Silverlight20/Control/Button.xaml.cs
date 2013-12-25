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

namespace Silverlight20.Control
{
    public partial class Button : UserControl
    {
        public Button()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Alert(((System.Windows.Controls.Button)sender).Tag.ToString() + " 被单击了");
        }
    }
}
