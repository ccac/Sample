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

namespace Silverlight20.Control
{
    public partial class Slider : UserControl
    {
        public Slider()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // RoutedPropertyChangedEventArgs<double>.OldValue - Slider控件的原值
            // RoutedPropertyChangedEventArgs<double>.NewValue - Slider控件的新值

            lblMsg.Text = string.Format("原值：{0}\r\n新值：{1}", e.OldValue.ToString(), e.NewValue.ToString());
        }
    }
}
