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
*   Title: A Driving Game Demonstratoin in C#
*   Author: Terence Tsang	
*   http://www.shinedraw.com
*   Your Flash and Silverlight Repositry
*/

namespace DrivingGame
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            DrivingGame drivingGame = new DrivingGame();
            LayoutRoot.Children.Add(drivingGame);
        }
    }
}
