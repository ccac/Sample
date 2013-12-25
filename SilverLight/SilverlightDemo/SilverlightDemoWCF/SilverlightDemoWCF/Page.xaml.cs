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

using System.ServiceModel;

namespace SilverlightDemoWCF
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            EndpointAddress address = new EndpointAddress("http://localhost:1593/Blog.svc");

            BlogService.BlogClient client = new SilverlightDemoWCF.BlogService.BlogClient(binding,address);

            client.GetPostsCompleted += new EventHandler<SilverlightDemoWCF.BlogService.GetPostsCompletedEventArgs>(client_GetPostsCompleted);
            client.GetPostsAsync();
        }

        void client_GetPostsCompleted(object sender, SilverlightDemoWCF.BlogService.GetPostsCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                Posts.ItemsSource = e.Result;
            }
        }
    }
}
