using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace SilverlightDemoFullScreen
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
        }

        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Content contentObject = Application.Current.Host.Content;
            if (contentObject.IsFullScreen)
            {
                button1.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                button1.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Content contentObject = Application.Current.Host.Content;
            contentObject.IsFullScreen = !contentObject.IsFullScreen;
        }
    }
}
