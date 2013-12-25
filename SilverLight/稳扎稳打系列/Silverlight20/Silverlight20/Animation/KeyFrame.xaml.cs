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

namespace Silverlight20.Animation
{
    public partial class KeyFrame : UserControl
    {
        public KeyFrame()
        {
            InitializeComponent();
        }

        private void caRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            caStoryboard.Begin();
        }

        private void daRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            daStoryboard.Begin();
        }

        private void paPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            paStoryboard.Begin();
        }
    }
}
