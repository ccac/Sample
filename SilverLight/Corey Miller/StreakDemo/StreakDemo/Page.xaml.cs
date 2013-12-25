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

namespace StreakDemo
{
    public partial class Page : UserControl
    {
        double _lastX = 0;
        double _lastY = 0;

        List<Particle.Streak> streaks = new List<StreakDemo.Particle.Streak>();
        Storyboard timer = new Storyboard();

        bool start = false;

        public Page()
        {
            InitializeComponent();

            for (int i=1; i <= 64; i+=2)
            {
                Particle.Streak streak = new StreakDemo.Particle.Streak(i);
                LayoutRoot.Children.Add(streak);
                streaks.Add(streak);
            }

            timer.Duration = TimeSpan.FromMilliseconds(30);
            timer.Completed += new EventHandler(timer_Completed);
            timer.Begin();
        }

        void timer_Completed(object sender, EventArgs e)
        {
            //move all streaks
            foreach (Particle.Streak streak in streaks)
            {
                streak.Move(_lastX, _lastY);
            }

            timer.Begin();
        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            _lastX = e.GetPosition(null).X;
            _lastY = e.GetPosition(null).Y;

            if (!start)
            {
                foreach (Particle.Streak streak in streaks)
                {
                    streak.StartMove(_lastX, _lastY);
                }
                start = true;
            }
        }
    }
}
