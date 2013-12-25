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
using System.ServiceModel.Channels;

namespace CustomerClient
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            Binding binding = new BasicHttpBinding();

            EndpointAddress endpoint = new EndpointAddress("http://localhost:1552/Service1.svc");

            ServiceReference.Service1Client proxy = new CustomerClient.ServiceReference.Service1Client(binding, endpoint);

            proxy.CountUsersCompleted += new EventHandler<CustomerClient.ServiceReference.CountUsersCompletedEventArgs>(proxy_CountUsersCompleted);
            proxy.CountUsersAsync();


            proxy.GetUserCompleted += new EventHandler<CustomerClient.ServiceReference.GetUserCompletedEventArgs>(proxy_GetUserCompleted);
            proxy.GetUserAsync(1);
        }

        void proxy_CountUsersCompleted(object sender, CustomerClient.ServiceReference.CountUsersCompletedEventArgs e)
        {
            userCountResult.Text = "Number of users: " + e.Result;
        }

        void proxy_GetUserCompleted(object sender, CustomerClient.ServiceReference.GetUserCompletedEventArgs e)
        {
            getUserResult.Text = "User name: " + e.Result.Name + ", age: " + e.Result.Age + ", is member: " + e.Result.IsMember;
        }
    }
}
