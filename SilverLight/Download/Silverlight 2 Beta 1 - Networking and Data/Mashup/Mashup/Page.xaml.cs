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
using System.IO;
using System.Xml;
using System.ServiceModel.Syndication;


namespace Mashup
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            
            
            WebClient client = new WebClient();
            Uri endpoint = new Uri("http://localhost:8081/Mashup_Web/Mashup.ashx");
            //client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            //client.OpenReadAsync(endpoint);
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(endpoint);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            XmlReader r = XmlReader.Create(e.Result);
            SyndicationFeed feed = SyndicationFeed.Load(r);
            _itemTitles.ItemsSource = feed.Items;
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            XmlReader r = XmlReader.Create(e.Result);
            SyndicationFeed feed = SyndicationFeed.Load(r);
            _itemTitles.ItemsSource = feed.Items;
        }

    }
}
