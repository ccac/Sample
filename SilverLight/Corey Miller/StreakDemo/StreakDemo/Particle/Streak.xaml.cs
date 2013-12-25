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

namespace StreakDemo.Particle
{
    public partial class Streak : UserControl
    {
        
        private const double _friction = 0.96;
        private double _tension;

        private double _x = 0;
        private double _y = 0;

        private double velX = 0;
        private double velY = 0;

        private double oldX = 0;
        private double oldY = 0;

        private bool start = false;

        public Streak(int indexVariable)
        {
            InitializeComponent();
            _tension = .0008 + ((double)indexVariable) / 80000;
        }

        public void StartMove(double anchorX, double anchorY)
        {
            _x = anchorX;
            _y = anchorY;
            oldX = _x;
            oldY = _y;
            start = true;
        }

        public void Move(double anchorX, double anchorY)
        {
            if (start)
            {

                double dx = _x - anchorX;
                double dy = _y - anchorY;

                //get the acceleration vector
                Point accel = accelerate(dx, dy);

                //accelerate
                velX += accel.X;
                velY += accel.Y;

                //dampen the speed
                velX *= _friction;
                velY *= _friction;

                //move to new position
                _x += velX;
                _y += velY;

                //move point and streak
                _streak.SetValue(Canvas.TopProperty, _y);
                _streak.SetValue(Canvas.LeftProperty, _x);
                _point.SetValue(Canvas.TopProperty, _y);
                _point.SetValue(Canvas.LeftProperty, _x);

                //by changing the scale I am creating a line to both points
                streakStretch.ScaleX = (oldX - _x) / 50.0;
                streakStretch.ScaleY = (oldY - _y) / 50.0;

                _streak.SetValue(Canvas.LeftProperty, _x);
                _streak.SetValue(Canvas.TopProperty, _y);

                //store the old x and y values
                oldX = _x;
                oldY = _y;
            }
        }

        //acceleration method
        private Point accelerate(double x, double y)
        {
            return new Point(
                -100.0 * _tension * x,
                -100.0 * _tension * y);
        }
    }
}
