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

namespace SilverlightDemoWebClient
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Uri endpoint = new Uri(String.Format("http://localhost:1283/BookHandler.ashx?No={0}", Books.SelectedIndex));

            WebClient webclient = new WebClient();

            webclient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webclient_DownloadStringCompleted);

            webclient.DownloadStringAsync(endpoint);
        }

        void webclient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                lblPrice.Text = "价格：" + e.Result;
            }
            else
            {
                lblPrice.Text = e.Error.Message;
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
