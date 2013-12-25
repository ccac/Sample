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
*	A Simple Drawing Demonstratoin in C#
*   from shinedraw.com
*/

namespace SimpleDrawing
{
    public partial class Page : UserControl
    {
        private SimpleDrawing _simpleDrawing;
        public Page()
        {
            InitializeComponent();

            _simpleDrawing = new SimpleDrawing();
            LayoutRoot.Children.Add(_simpleDrawing);

            KeyDown += new KeyEventHandler(Page_KeyDown);

            /// add the cover screen before starting the application
            //Cover.MouseLeftButtonDown += new MouseButtonEventHandler(Page_MouseLeftButtonDown);
        }

        void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // hide the cover
            LayoutRoot.Children.Remove(Cover);
            _simpleDrawing = new SimpleDrawing();
            LayoutRoot.Children.Add(_simpleDrawing);

            
        }

        // captue the event 
        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                if (_simpleDrawing != null)
                {
                    _simpleDrawing.redraw();
                }
            }
        }
    }
}
