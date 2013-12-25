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
using System.Xml;
using System.Xml.Linq;

namespace StudentDataServices
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            Uri allStudentsUri = new Uri("http://localhost:8083/StudentDataServices_Web/StudentDataService.svc/Students");
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(allStudentsUri);
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            XmlReader r = XmlReader.Create(e.Result);
            XDocument students = XDocument.Load(r);
            XNamespace xmlns = "http://www.w3.org/2005/Atom";
            XNamespace ads = "http://schemas.microsoft.com/ado/2007/08/dataweb";
            _studentList.DataContext =
            from x in students.Descendants(xmlns + "entry")
            select new Student
            {
                Name = x.Descendants(ads + "Name").First().Value,
                Age = int.Parse(
                       x.Descendants(ads + "Age").First().Value)
            };

        }
    }
}
