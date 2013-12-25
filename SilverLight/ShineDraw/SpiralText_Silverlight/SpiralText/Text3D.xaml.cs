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
    public partial class Text3D : UserControl
    {
        public Point3D point3D = new Point3D();

        public Text3D()
        {
            InitializeComponent();
        }

        public double x {
            set {
                this.SetValue(Canvas.LeftProperty, value);
            }
            get
            {
                return (double)this.GetValue(Canvas.LeftProperty);
            }
        }

        public double y
        {
            set
            {
                this.SetValue(Canvas.TopProperty, value);
            }
            get
            {
                return (double)this.GetValue(Canvas.TopProperty);
            }
        }
    }
}
