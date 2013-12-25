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

/*
*	A Loading External Image Demonstratoin in C#
*   from shinedraw.com
*/

namespace LoadingExternalImage
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            LoadingExternalImage loadingExternalImage = new LoadingExternalImage();
            LayoutRoot.Children.Add(loadingExternalImage);
        }
    }
}
