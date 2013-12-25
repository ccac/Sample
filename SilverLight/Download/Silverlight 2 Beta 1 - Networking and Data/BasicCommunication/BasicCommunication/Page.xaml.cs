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

namespace BasicCommunication
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            Uri endpoint = new Uri("http://localhost:8080/BasicCommunication_Web/ServerTime.ashx");
            WebRequest request = WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(new AsyncCallback(RequestReady), request);
            //request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
            
            /*WebClient client = new WebClient();
            Uri endpoint = new Uri("http://localhost:8080/BasicCommunication_Web/ServerTime.ashx");
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(endpoint);*/
        }

        void RequestReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;
            this.Dispatcher.BeginInvoke(delegate(){ Stream requestStream = request.EndGetRequestStream(asyncResult);
            StreamWriter writer = new StreamWriter(requestStream);
            writer.Write("NewTime=11:58 AM");
            writer.Flush();
            });
            request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }

        void ResponseReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;
            using(WebResponse response = request.EndGetResponse(asyncResult))
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                _serverTimeText.Text = reader.ReadToEnd();
            }
        }
        /*void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            _serverTimeText.Text = e.Result;
        }*/


    }
}
