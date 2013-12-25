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
    public partial class ToggleButton : UserControl
    {
        public ToggleButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Alert(string.Format("tbtn1: {0}\r\ntbtn2: {1}\r\ntbtn3: {2}\r\ntbtn4: {3}",
                tbtn1.IsChecked, tbtn2.IsChecked, tbtn3.IsChecked, tbtn4.IsChecked));
        }
    }
}
