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
    public partial class CheckBox : UserControl
    {
        public CheckBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Alert(string.Format("chk1: {0}\r\nchk2: {1}\r\nchk3: {2}\r\nchk4: {3}", 
                chk1.IsChecked, chk2.IsChecked, chk3.IsChecked, chk4.IsChecked));
        }
    }
}
