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
