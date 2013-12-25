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

using System.Net;
using System.Xml;
using System.IO;

namespace ShowPoxQuickStart
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url1 = "http://services.digg.com/galleryphotos?count=" +
                    numTextBox.Text +
                    "&appkey=http%3A%2F%2Fwww.silverlight.net";

            WebClient client = new WebClient();

            client.DownloadStringCompleted +=
                new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(url1));
            
        }

        void client_DownloadStringCompleted(object sender,DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                resultBlock.Text = e.Result;
            }

            else
                resultBlock.Text = e.Error.Message;
        }
    }
}
