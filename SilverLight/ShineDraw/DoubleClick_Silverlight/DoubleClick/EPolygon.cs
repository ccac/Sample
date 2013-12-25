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
using System.Windows.Threading;

/*
*   Title: A Double Click Demonstratoin in C#
*   Author: Terence Tsang	
*   http://www.shinedraw.com
*   Your Flash and Silverlight Repositry
*/

namespace DoubleClick
{
    public class EPolygon 
    {
		private static double SPRINGNESS = 0.2;	// Control the return Speed
        private static double DECAY = 0.8;		// Control the bounce Speed
        private static double LIMIT = 0.005;	// Save CPU time. Smaller value means smoother animation

		private double _temp = 0;				// Temporary Variable for calculating spring effect	
        Storyboard _storyBoard;                 // on enter frame simulator
        private int _fps = 24;                  // fps of the on enter frame event
        private ScaleTransform _scaleTransform = new ScaleTransform();

        public Polygon Polygon;                // Polygon

        public EPolygon()
        {
            Polygon = new Polygon();

            // start the enter frame event
            _storyBoard = new Storyboard();
            _storyBoard.Duration = TimeSpan.FromMilliseconds(1000 / _fps);
            _storyBoard.Completed += new EventHandler(_storyBoard_Completed);
        }

        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////	

        void _storyBoard_Completed(object sender, EventArgs e)
        {
            _temp = _temp * DECAY + (1 - _scaleTransform.ScaleX) * SPRINGNESS;
            if (Math.Abs(_temp) < LIMIT) // Save CPU time
            { 
                _storyBoard.Stop();
            }
            else
            {
                _scaleTransform.ScaleX += _temp;
                _scaleTransform.ScaleY += _temp;
                Polygon.RenderTransform = _scaleTransform;
                _storyBoard.Begin();
            }
        }


        /////////////////////////////////////////////////////        
		// Private Methods 
		/////////////////////////////////////////////////////	
		
		/////////////////////////////////////////////////////        
		// Public Methods 
		/////////////////////////////////////////////////////	
		
		// start the animation and enter frame event
		public void startAnimation(){
            _scaleTransform.ScaleX = 0;
            _scaleTransform.ScaleY = 0;
            Polygon.RenderTransform = _scaleTransform;

            _storyBoard.Begin();
		}
		
		// creat the Polygon
		public void draw(int corner, double radius , Color color)  {
			Polygon.Points.Clear();

            Polygon.Fill = new SolidColorBrush(color);
			for(int i = 0; i < corner; i++){
				double angle = 2 *  Math.PI/ corner * (i + 1);
				double lineX = Math.Cos(angle) * radius;
				double lineY = - Math.Sin(angle) * radius;
                Point point = new Point(lineX, lineY);
				
                Polygon.Points.Add(point);
			}
		}
    }
}
