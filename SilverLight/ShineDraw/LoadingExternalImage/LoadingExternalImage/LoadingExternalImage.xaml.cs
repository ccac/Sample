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

/*
*	A Loading External Image Demonstratoin in C#
*   from shinedraw.com
*/

namespace LoadingExternalImage
{
    public partial class LoadingExternalImage : UserControl
    {
        private const double IMAGE_WIDTH = 550;    // Image Width
        private const double IMAGE_HEIGHT = 360;   // Image Height

        private BitmapImage _bitmapImage;

        public LoadingExternalImage()
        {
            InitializeComponent();

            init();
        }

        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////	
        void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // reset propertes
            ProgressBar.Visibility = Visibility.Visible;
            Label.Visibility = Visibility.Visible;
            ProgressBar.Value = 0;
            Label.Text = "Loading...";
            Image.Children.Clear();

            if (_bitmapImage != null)
            {
                _bitmapImage.DownloadProgress -= new EventHandler<DownloadProgressEventArgs>(bitmapImage_DownloadProgress);
            }

            _bitmapImage = new BitmapImage();
            _bitmapImage.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(bitmapImage_DownloadProgress);
            _bitmapImage.UriSource = new Uri(UrlText.Text, UriKind.Absolute);
            Image newImage = new Image() { Source = _bitmapImage };
            newImage.Stretch = Stretch.UniformToFill;
            newImage.Width = IMAGE_WIDTH;
            newImage.Height = IMAGE_HEIGHT;
            Image.Children.Add(newImage);
        }

        void bitmapImage_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            ProgressBar.Value = e.Progress;
            Label.Text = "Loading: " + e.Progress + "%";

            if (e.Progress == 100)
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                Label.Visibility = Visibility.Collapsed;
            }
        }


        /////////////////////////////////////////////////////        
        // Private Methods 
        /////////////////////////////////////////////////////	

        private void init()
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            Label.Visibility = Visibility.Collapsed;

            // handlers
            LoadButton.Click += new RoutedEventHandler(LoadButton_Click);
        }


    }

}
