using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
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
    public class Point3D
    {
        public double x;
        public double y;
        public double z;

        public Point3D()
        {

        }

        public Point3D(double x, double y, double z){
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
