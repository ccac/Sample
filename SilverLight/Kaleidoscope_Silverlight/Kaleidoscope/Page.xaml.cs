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
*	A Kaleidoscope Demonstratoin in C#
*   from shinedraw.com
*/

namespace Kaleidoscope
{
    public partial class Page : UserControl
    {
        private Kaleidoscope _kaleidoscope;
        public Page()
        {
            InitializeComponent();

            _kaleidoscope = new Kaleidoscope();
            LayoutRoot.Children.Insert(0, _kaleidoscope);

            Cover.MouseLeftButtonDown += new MouseButtonEventHandler(Cover_MouseLeftButtonDown);
        }

        void Cover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(Cover);
            _kaleidoscope.Start();
        }
    }
}
