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

namespace Silverlight20.Tip
{
    public partial class InitParams : UserControl
    {
        public InitParams()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(InitParams_Loaded);
        }

        void InitParams_Loaded(object sender, RoutedEventArgs e)
        {
            // 以编码的方式读取应用程序级别的资源
            lblMsg.Text += App.Current.Resources["key1"];
        }
    }
}
