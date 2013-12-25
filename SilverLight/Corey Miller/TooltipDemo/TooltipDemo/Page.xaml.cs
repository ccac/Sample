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

namespace TooltipDemo
{
    public partial class Page : UserControl
    {

        System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();

        public Page()
        {
            InitializeComponent();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Stop();
            _timer.Tick += new EventHandler(_timer_Tick);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            tip.IsOpen = true;
            _timer.Stop();
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            _timer.Start();
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            _timer.Stop();
            tip.IsOpen = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            tip.HorizontalOffset = e.GetPosition(null).X + 10;
            tip.VerticalOffset = e.GetPosition(null).Y + 10;
        }

        

    }
}
