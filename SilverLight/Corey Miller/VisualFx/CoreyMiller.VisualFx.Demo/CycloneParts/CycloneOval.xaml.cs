using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CoreyMiller.VisualFx.Demo.CycloneParts
{
    public partial class CycloneOval : UserControl
    {
        public CyclonePath Path;

        double _stiffness;

        public CycloneOval(int index, double stiffness, double radius, double scale, int speed)
        {
            InitializeComponent();

            Path = new CyclonePath(radius, scale, speed);

            LayoutRoot.Children.Add(Path);

            _stiffness = stiffness;
        }

        public void Sidewind(double _x)
        {
            double currentX = (double)this.GetValue(Canvas.LeftProperty);
            double distance = _x - currentX;

            // move toward neighbour
            if (Math.Abs(distance) > .01)
            {
                this.SetValue(Canvas.LeftProperty, currentX + distance * _stiffness);
            }
        }
    }
}
