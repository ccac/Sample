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

namespace SilverlightDemoCallJS
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            //方法一
            //ScriptObject hello = HtmlPage.Window.GetProperty("Hello") as ScriptObject;
            //hello.InvokeSelf(input.Text);

            //方法二
            //ScriptObject script = HtmlPage.Window.CreateInstance("myHello", input.Text);
            //object result = script.Invoke("Display");

            //方法三
            HtmlElement element = HtmlPage.Window.Eval("document.getElementById('result')") as HtmlElement;
            string message = element.GetAttribute("innerText");
            HtmlPage.Window.Alert(message);
        }
    }
}
