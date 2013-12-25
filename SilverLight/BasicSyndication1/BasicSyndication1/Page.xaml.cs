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

using System.ServiceModel.Syndication;
using System.Net;
using System.Xml;

namespace BasicSyndication1
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri("http://silverlight.net/blogs/microsoft/rss.aspx"));

            request.BeginGetResponse(new AsyncCallback(responseHandler), request);
        }

        public void responseHandler(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);

            XmlReader reader = XmlReader.Create(response.GetResponseStream());
            SyndicationFeed feed = SyndicationFeed.Load(reader);

            foreach (SyndicationItem item in feed.Items)
            {
                feedContent.Text += "* " + item.Title.Text + Environment.NewLine;
                feedContent.Text += "  Published on: " + item.PublishDate + Environment.NewLine;
            }
        }
    }
}
