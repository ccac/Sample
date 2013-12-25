using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightDemoLiveImage
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private int index = 5;
        private int MIN = 1;
        private int MAX = 8;

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;
            img.Opacity = 0.6;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;
            img.Opacity = 1.0;
        }

        private void leftImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            index = index == MIN ? MAX : index - 1;
            Play();
            myStoryboard.Begin();
        }

        private void rightImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            index = index == MAX ? MIN : index + 1;
            Play();
            myStoryboard.Begin();
        }

        void Play()
        {
            currentImg.Source = new BitmapImage(new Uri(index.ToString() + ".png", UriKind.Relative));

            int left = index == MIN ? MAX : index - 1;
            leftImg.Source = new BitmapImage(new Uri(left.ToString() + ".png", UriKind.Relative));

            int right = index == MAX ? MIN : index + 1;
            rightImg.Source = new BitmapImage(new Uri(right.ToString() + ".png", UriKind.Relative));
        }
    }
}
