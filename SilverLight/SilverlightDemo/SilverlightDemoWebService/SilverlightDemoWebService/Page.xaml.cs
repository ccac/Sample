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

namespace SilverlightDemoWebService
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BlogService.BlogServiceSoapClient client = new SilverlightDemoWebService.BlogService.BlogServiceSoapClient();

            client.GetPostsCompleted += new EventHandler<SilverlightDemoWebService.BlogService.GetPostsCompletedEventArgs>(client_GetPostsCompleted);
            client.GetPostsAsync();
        }

        void client_GetPostsCompleted(object sender, SilverlightDemoWebService.BlogService.GetPostsCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                Posts.ItemsSource = e.Result;
            }
        }
    }
}
