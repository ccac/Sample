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

namespace SilverlightDemoWebRequest
{
    public partial class Page : UserControl
    {
        private string bookNo;

        public Page()
        {
            InitializeComponent();
        }

        private void Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bookNo = Books.SelectedIndex.ToString();

            Uri endpoint = new Uri("http://localhost:1399/BookHandler.ashx");

            WebRequest request = WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(new AsyncCallback(RequestReady), request);

        }

        void RequestReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;

            Stream requestStream = request.EndGetRequestStream(asyncResult);

            using (StreamWriter writer = new StreamWriter(requestStream))
            {
                writer.Write(String.Format("No={0}", bookNo));
                writer.Flush();
            }
            request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }

        void ResponseReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;

            WebResponse response = request.EndGetResponse(asyncResult);

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string str = reader.ReadToEnd();
                this.Dispatcher.BeginInvoke(() => { lblPrice.Text = "价格：" + str; });
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<Book> list = new List<Book>()
            {
                new Book("Professional ASP.NET 3.5"),
                new Book("ASP.NET AJAX In Action"),
                new Book("Silverlight In Action"),
                new Book("ASP.NET 3.5 Unleashed"),
                new Book("Introducing Microsoft ASP.NET AJAX")
            };

            Books.ItemsSource = list;
        }
    }
}
