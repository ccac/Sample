using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

/*
*	A Simple Game System Demonstratoin in C#
*   from shinedraw.com
*/

namespace SimpleGameSystem
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            SimpleGameSystem simpleGameSystem = new SimpleGameSystem();
            LayoutRoot.Children.Add(simpleGameSystem);
        }
    }
}
