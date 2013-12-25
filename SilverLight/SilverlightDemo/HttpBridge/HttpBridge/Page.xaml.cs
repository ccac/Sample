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

namespace HttpBridge
{
    public partial class Page : UserControl
    {
        HtmlDocument _doc;   // requires using System.Windows.Browser; 
        int _cnt = 0;

            
        public Page()
        {
            InitializeComponent();

            _doc = HtmlPage.Document;

            // Return for all pages except those ending in TestPage.html
            if (_doc.DocumentUri.AbsoluteUri.EndsWith("TestPage.html") != true)
                return;

            _doc.GetElementById("txtInput").SetProperty("disabled", false);
            _doc.GetElementById("txtInput").SetAttribute("value", "This text from the Silverlight Side");

            HtmlElement btnUC = _doc.GetElementById("btnUCtxt");

            btnUC.AttachEvent("onclick",new EventHandler<HtmlEventArgs>(this.OnUCtextClicked));

            setUpPropBtn();

        }

        void OnUCtextClicked(object sender, HtmlEventArgs e)
        {
            HtmlElement input = _doc.GetElementById("txtInput");
            HtmlElement output = _doc.GetElementById("txtOutput");
            output.SetAttribute("value", input.GetAttribute("value").ToUpper());
        }

        void setUpPropBtn()
        {

            HtmlElement btnPropGet = _doc.GetElementById("btnGetProperties");
            btnPropGet.AttachEvent("onclick",new EventHandler<HtmlEventArgs>(this.OnGetPropClicked));
        }

        void OnGetPropClicked(object sender, HtmlEventArgs e)
        {
            string outputText = "";
            _cnt++;
            switch (_cnt % 3)
            {
                case 0:
                    outputText = "DocumentUri.AbsolutePath: "
                    + HtmlPage.Document.DocumentUri.AbsolutePath;
                    break;

                case 1:
                    outputText = "Cookies Enabled: " +
                        HtmlPage.BrowserInformation.CookiesEnabled.ToString();
                    break;

                case 2:
                    outputText = "Port: " +
                        HtmlPage.Document.DocumentUri.Port.ToString();
                    break;
            }
            _doc.GetElementById("txtOutputProperties").SetAttribute("value", outputText);
        }


    }
}
