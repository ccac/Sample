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
using System.Windows.Threading;

/*
*   Title: A Double Click Demonstratoin in C#
*   Author: Terence Tsang	
*   http://www.shinedraw.com
*   Your Flash and Silverlight Repositry
*/

namespace DoubleClick
{
    public partial class DoubleClick : UserControl
    {
        private static double ALPHA_MIN = 64;		// Minimum alpha 
        private static double ALPHA_MAX = 196;		// Maximum alpha
        private static double CORNER_MIN = 3;		// Minimum corner
        private static double CORNER_MAX = 10;		// Maximum corner
        private static double RADIUS_MIN = 20;		// Minimum radius
        private static double RADIUS_MAX = 100;		// Maximum radius
        private static int INTERVAL = 200;          // Double Click Detect Interval
        DispatcherTimer _timer;                     // Detect Double Click

        public DoubleClick()
        {
            InitializeComponent();

            // create a timer to detect double click
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, INTERVAL);
            _timer.Tick += new EventHandler(_timer_Tick);

            // add mouse click handler to layout root
            LayoutRoot.MouseLeftButtonDown += new MouseButtonEventHandler(LayoutRoot_MouseLeftButtonDown);
        }

        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////	

        // stop the timer after an interval
        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
        }

        void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // if second click comes before the timer end, it's an double click
            if (_timer.IsEnabled)
            {
                // stop the timer
                _timer.Stop();

                // create a random polygon
                int seed = (int)DateTime.Now.Ticks;
                Random r = new Random(seed);

                // construct the polygon properties
                byte alpha = (byte)(ALPHA_MIN + (ALPHA_MAX - ALPHA_MIN) * r.NextDouble());
                int corner = (int)(CORNER_MIN + Math.Round(CORNER_MAX - CORNER_MIN) * r.NextDouble());
                double radius = RADIUS_MIN + (RADIUS_MAX - RADIUS_MIN) * r.NextDouble();
                double offsetX = Width * r.NextDouble();
                double offsetY = Height * r.NextDouble();
                byte red = (byte)(255 * r.NextDouble());
                byte green = (byte)(255 * r.NextDouble());
                byte blue = (byte)(255 * r.NextDouble());

                // create the polygon
                EPolygon ePolygon = new EPolygon();
                ePolygon.draw(corner, radius, Color.FromArgb(alpha, red, green, blue));
                ePolygon.Polygon.Fill = new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));

                // set the position
                Canvas.SetLeft(ePolygon.Polygon, e.GetPosition(LayoutRoot).X);
                Canvas.SetTop(ePolygon.Polygon, e.GetPosition(LayoutRoot).Y);

                // start the animation
                LayoutRoot.Children.Add(ePolygon.Polygon);
                ePolygon.startAnimation();
            }
            else
            {
                // if not double click, start the timer
                _timer.Start();
            }
        }
    }
}
