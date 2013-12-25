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

using System.Xml;
using System.Net;
using System.ServiceModel.Syndication;
using System.Windows.Interop;
using System.IO;

namespace SilverlightDemoRssReader
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void displayButton_Click(object sender, RoutedEventArgs e)
        {
            Uri url = new Uri(feedAddress.Text);

            WebRequest request = WebRequest.Create(url);
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.BeginGetRequestStream(new AsyncCallback(RequestReady), request); 

            Dispatcher.BeginInvoke(delegate() { request.BeginGetResponse(new AsyncCallback(responseReady), request); });
            //request.BeginGetResponse(new AsyncCallback(responseReady), request);
        }

        void RequestReady(IAsyncResult asyncResult)
        {

            WebRequest request = asyncResult.AsyncState as WebRequest;
            Stream requestStream = request.EndGetRequestStream(asyncResult);  //seems to die here!

            //StreamWriter writer = new StreamWriter(requestStream); writer.Write("NewTime=11:58 AM");
            //writer.Flush();

            Dispatcher.BeginInvoke(delegate() { request.BeginGetResponse(new AsyncCallback(responseReady), request); });
        }


        void responseReady(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;
            Dispatcher.BeginInvoke(delegate()
            {
                WebResponse response = (WebResponse)request.EndGetResponse(asyncResult);

                XmlReader reader = XmlReader.Create(response.GetResponseStream());
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                PostsList.ItemsSource = feed.Items;
            });
        }

        private void PostsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SyndicationItem item = PostsList.SelectedItem as SyndicationItem;
            Detail.DataContext = item;
        }

        private void fullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            Content contentObject = Application.Current.Host.Content;
            contentObject.IsFullScreen = !contentObject.IsFullScreen;
        }
    }
}
