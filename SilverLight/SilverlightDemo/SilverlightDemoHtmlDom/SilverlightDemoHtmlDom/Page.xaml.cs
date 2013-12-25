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

namespace SilverlightDemoHtmlDom
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void displayButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement element = HtmlPage.Document.GetElementById(this.input.Text.Trim());

            if (element != null)
            {
                result.Text = element.GetAttribute("innerText");

                element.SetAttribute("innerText", this.change.Text);

                element.SetStyleAttribute("height", "100px");
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement parent = HtmlPage.Document.GetElementById("list");

            HtmlElement child = HtmlPage.Document.CreateElement("li");
            child.SetAttribute("innerText", this.input1.Text);

            parent.AppendChild(child);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement parent = HtmlPage.Document.GetElementById("list");

            //HtmlElement child = HtmlPage.Document.GetElementById(this.input1.Text);

            //parent.r

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlElement parent = HtmlPage.Document.GetElementById("parent");

            HtmlElement button = HtmlPage.Document.CreateElement("a");
            button.SetAttribute("innerText", "改变Silverlight中的颜色");
            button.SetAttribute("href", "#");
            button.CssClass = "newstyle";

            parent.AppendChild(button);

            button.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(button_Click));
        }

        void button_Click(object sender, HtmlEventArgs e)
        {
            result1.Stroke = new SolidColorBrush(Colors.Black);
            result1.Fill = new SolidColorBrush(Colors.Green);
            result1.StrokeThickness = 2;
        }
    }
}
