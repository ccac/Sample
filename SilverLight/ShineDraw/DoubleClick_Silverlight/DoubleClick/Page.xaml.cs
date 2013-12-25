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
*   Title: A Double Click Demonstratoin in C#
*   Author: Terence Tsang	
*   http://www.shinedraw.com
*   Your Flash and Silverlight Repositry
*/

namespace DoubleClick
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            DoubleClick doubleClick = new DoubleClick();
            LayoutRoot.Children.Add(doubleClick);
        }
    }
}
