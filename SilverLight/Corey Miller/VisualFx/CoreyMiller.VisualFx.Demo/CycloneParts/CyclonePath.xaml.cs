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
    public partial class CyclonePath : UserControl
    {
        public CycloneParticle Particle;

        double _radius = 0;

        double _rotation;
        int _speed;
        double _velRot;

        Storyboard _timer = new Storyboard();

        public CyclonePath(double radius, double scale, int speed)
        {
            InitializeComponent();

            Particle = new CycloneParticle(scale);

            _timer.Duration = new TimeSpan(0, 0, 0, 0, 5);
            _timer.Completed += new EventHandler(_timer_Completed);

            _radius = radius;

            //scale
            LayoutRoot.Children.Add(Particle);
            Particle.SetValue(Canvas.LeftProperty, radius);

            Random random = new Random();

            _rotation = Math.Floor(random.NextDouble() * 360);

            LayoutRotate.Angle = _rotation;

            _speed = speed;

            _velRot = _speed / _rotation;
            _timer.Begin();
        }

        void _timer_Completed(object sender, EventArgs e)
        {
            this.Live();
            _timer.Begin();
        }

        public void Live()
        {
            _rotation += _velRot;

            if (double.IsInfinity(_rotation))
            {
                _rotation = 0;
            }

            LayoutRotate.Angle = _rotation;
        }
    }
}
