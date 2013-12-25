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

using System.Windows.Browser;

using SilverlightDemoLiveSearch.LiveSearchWS;

namespace SilverlightDemoLiveSearch
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LiveSearchWebServiceSoapClient client = new LiveSearchWebServiceSoapClient();

            client.DoSearchCompleted += new EventHandler<DoSearchCompletedEventArgs>(client_DoSearchCompleted);
            client.DoSearchAsync(txtQuery.Text);
        }

        void client_DoSearchCompleted(object sender, DoSearchCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                SearchResultItem[] items = e.Result as SearchResultItem[];

                HtmlElement result = HtmlPage.Document.GetElementById("result");

                foreach (SearchResultItem item in items)
                {
                    HtmlElement itemElement = HtmlPage.Document.CreateElement("div");
                    itemElement.CssClass = "itemstyle";

                    HtmlElement titleElement = HtmlPage.Document.CreateElement("a");
                    titleElement.SetAttribute("innerText", item.Title);
                    titleElement.SetAttribute("href", item.Url);

                    HtmlElement descElement = HtmlPage.Document.CreateElement("div");
                    descElement.SetAttribute("innerText", item.Description);

                    HtmlElement urlElement = HtmlPage.Document.CreateElement("span");
                    urlElement.SetAttribute("innerText", item.Url);
                    urlElement.CssClass = "urlstyle";

                    itemElement.AppendChild(titleElement);
                    itemElement.AppendChild(descElement);
                    itemElement.AppendChild(urlElement);

                    result.AppendChild(itemElement);
                }
            }
        }
    }
}
