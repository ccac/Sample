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

namespace SilverlightDemoCallByJS
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.RegisterScriptableObject("Calculator", this);

            HtmlPage.RegisterCreateableType("cal", typeof(Calculator));
        }

        [ScriptableMember]
        public void Add(int x, int y)
        {
            int z = x + y;
            this.result.Text = string.Format("{0} + {1} = {2}", x, y, z);
        }
    }
}
