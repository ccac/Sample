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

namespace Car
{
    public partial class Car : UserControl
    {
        private Image _selected ;

        public Car()
        {
            InitializeComponent();
            _selected = car1;
        }

        /////////////////////////////////////////////////////        
        // Public Methods 
        /////////////////////////////////////////////////////

        public void SetPosition(int value){
            Image image = FindName("car" + value) as Image;

            if (image != _selected)
            {
                _selected.Visibility = Visibility.Collapsed;
                image.Visibility = Visibility.Visible;
                _selected = image;
            }
        }
    }
}
