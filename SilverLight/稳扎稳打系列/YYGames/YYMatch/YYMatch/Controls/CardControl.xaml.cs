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

using System.Windows.Resources;
using System.Windows.Media.Imaging;
using YYMatch.Models;

namespace YYMatch.Controls
{
    public partial class CardControl : UserControl
    {
        private readonly string baseAddress = "/Resources/";

        public CardControl()
        {
            InitializeComponent();
        }

        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        public static readonly DependencyProperty ImageNameProperty = DependencyProperty.Register(
            "ImageName",
            typeof(string),
            typeof(CardControl),
            new PropertyMetadata("", ImageNamePropertyChanged));

        private static void ImageNamePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as CardControl).OnImageNamePropertyChanged((string)e.NewValue);
        }

        private void OnImageNamePropertyChanged(string imageName)
        {
            if (imageName == Global.EmptyImageName)
            {
                IsHitTestVisible = false;
                ani.Begin();
            }
            else
            {
                Uri uri = new Uri(string.Format("{0}{1}.png", baseAddress, imageName), UriKind.Relative);
                BitmapImage imageSource = new BitmapImage(uri);

                img.Source = imageSource;
            }
        }

        public void Checked()
        {
            container.Width = 34;
            container.Height = 34;
            border.BorderThickness = new Thickness(1);
        }

        public void Unchecked()
        {
            container.Width = 38;
            container.Height = 38;
            border.BorderThickness = new Thickness(0);
        }
    }
}
