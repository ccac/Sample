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

using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Silverlight20.Control
{
    public partial class Image : UserControl
    {
        public Image()
        {
            InitializeComponent();

            // 后台方式设置Image的Source
            img.Source = new BitmapImage(new Uri("/Silverlight20;component/Images/Logo.jpg", UriKind.Relative));

            StreamResourceInfo sri = Application.GetResourceStream(
                new Uri("/Silverlight20;component/Images/Logo.jpg", UriKind.Relative));
            BitmapImage imageSource = new BitmapImage();
            imageSource.SetSource(sri.Stream);
            img2.Source = imageSource;
        }
    }
}
