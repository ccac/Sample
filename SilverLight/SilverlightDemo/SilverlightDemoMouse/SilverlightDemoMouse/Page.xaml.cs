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

namespace SilverlightDemoMouse
{
    public partial class Page : UserControl
    {
        bool trackingMouseMove = false;
        Point mousePosition;

        public Page()
        {
            InitializeComponent();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            mousePosition = e.GetPosition(null);
            trackingMouseMove = true;

            if (element != null)
            {
                element.CaptureMouse();
                element.Cursor = Cursors.Hand;
            }
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            if (element != null)
            {
                element.ReleaseMouseCapture();
                element.Cursor = null;
            }
            mousePosition.X = 0;
            mousePosition.Y = 0;
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (trackingMouseMove)
            {
                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;
                double newTop = deltaV + (double)element.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)element.GetValue(Canvas.LeftProperty);

                element.SetValue(Canvas.TopProperty, newTop);
                element.SetValue(Canvas.LeftProperty, newLeft);

                mousePosition = e.GetPosition(null);
            }
        }
    }
}
