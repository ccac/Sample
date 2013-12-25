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

namespace DynamicAnimationsDemo1
{
    public partial class Page : UserControl
    {
        List<Point> points = new List<Point>();
        int index = 0;
        double opacity = 0;

        System.Windows.Threading.DispatcherTimer _slideShowTimer = new System.Windows.Threading.DispatcherTimer();

        Storyboard _anotherTimer = new Storyboard();

        public Page()
        {
            InitializeComponent();

            _slideShowTimer.Interval = new TimeSpan(0, 0, 2);
            _slideShowTimer.Stop();
            _slideShowTimer.Tick += new EventHandler(_slideShowTimer_Tick);

            _anotherTimer.Duration = TimeSpan.FromMilliseconds(100);
            _anotherTimer.Completed += new EventHandler(_anotherTimer_Completed);
            
            points.Add(new Point(300, 200));
            points.Add(new Point(600, 300));
            points.Add(new Point(300, 500));
            points.Add(new Point(450, 350));
            points.Add(new Point(250, 400));

            Animate();
        }

        void _anotherTimer_Completed(object sender, EventArgs e)
        {
            _pic.Opacity = opacity;

            if (opacity >= 1)
            {
                opacity = 0;

                _slideShowTimer.Start();
            }
            else
            {
                opacity += .1;
                _anotherTimer.Begin();
            }
        }

        void _slideShowTimer_Tick(object sender, EventArgs e)
        {
            Animate();
        }

        private void Animate()
        {
            dynamicHeight.Value = points[index].Y + 20;
            dynamicWidth.Value = points[index].X + 20;

            DynamicAnimation.Begin();
        }

        private void DynamicAnimation_Completed(object sender, EventArgs e)
        {
            _pic.Width = points[index].X;
            _pic.Height = points[index].Y;

            _anotherTimer.Begin();

            index++;

            if (index == points.Count)
            {
                index = 0;
            }
        }
    }
}
