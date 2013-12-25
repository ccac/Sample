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
*	A Spiral Text Demonstratoin in C#
*   from shinedraw.com
*/

namespace SpiralText
{
    public partial class Page : UserControl
    {
        private SpiralText _spiralText;
        public Page()
        {
            InitializeComponent();

            _spiralText = new SpiralText();
            LayoutRoot.Children.Insert(0, _spiralText);

            Cover.MouseLeftButtonDown += new MouseButtonEventHandler(Cover_MouseLeftButtonDown);
        }

        void Cover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(Cover);
            _spiralText.Start();
        }
    }
}
