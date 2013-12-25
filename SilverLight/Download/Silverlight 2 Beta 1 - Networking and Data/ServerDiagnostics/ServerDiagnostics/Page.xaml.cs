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
using ServerDiagnostics.ModulesService;


namespace ServerDiagnostics
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            Binding binding = new BasicHttpBinding();
            EndpointAddress endPoint = new EndpointAddress("http://localhost:8082/ServerDiagnostics_Web/Modules.svc");
            ModulesClient client = new ModulesClient(binding, endPoint);
            client.GetModulesCompleted += new EventHandler<GetModulesCompletedEventArgs>(client_GetModulesCompleted);
            client.GetModulesAsync();
        }

        void client_GetModulesCompleted(object sender, GetModulesCompletedEventArgs e)
        {
            _modulesItems.ItemsSource = e.Result;
        }
    }
}
