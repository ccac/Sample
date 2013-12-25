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
using System.Windows.Media.Imaging;

/*
*	A Kaleidoscope Demonstratoin in C#
*   from shinedraw.com
*/


namespace Kaleidoscope
{
    public class KaleidoscopeItem: Canvas
    {

        private double _imageWidth;         // Image Width
        private double _imageHeight;        // Image Height
        private Rectangle _graphics;        // Store the image brush
        private double _angle = 0;          // Rotated angle
        private double _angleInc = 1;       // Rotation increment

        public KaleidoscopeItem()
        {

        }

        /////////////////////////////////////////////////////        
        // Handlers
        /////////////////////////////////////////////////////	


        /////////////////////////////////////////////////////        
        // Public Methods
        /////////////////////////////////////////////////////	

        // create a Kaleidoscope item
        public void Init(double angle, string imagePath, double imageWidth, double imageHeight, double radius){
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;

            double centerX = _imageWidth / 2;
            double centerY = _imageHeight / 2;

            // create a retancle to store the image graphics
            _graphics = new Rectangle();
            _graphics.Width = _imageWidth;
            _graphics.Height = _imageHeight;

            // create Image brush
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            imageBrush.Stretch = Stretch.None;
            _graphics.Fill = imageBrush;


            // Create a polynomial geometry
            PathGeometry p = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.IsClosed = true;
            pathFigure.StartPoint = new Point(centerX, centerY);

            // create a triangle according to the angle
            double theta = angle / 180 * Math.PI / 2;
            double lineX = Math.Sin(theta) * radius;
            double lineY = Math.Cos(theta) * radius;

            // add the triangle points
            PolyLineSegment polyLineSegment = new PolyLineSegment();
            polyLineSegment.Points.Add(new Point(centerX + lineX, centerY - lineY));
            polyLineSegment.Points.Add(new Point(centerX - lineX, centerY - lineY));
            polyLineSegment.Points.Add(new Point(centerX, centerY));

            pathFigure.Segments.Add(polyLineSegment);
            p.Figures.Add(pathFigure);

            // clip the canvas
            this.Clip = p;

            // add the rectanlge image to the stage
            this.Children.Add(_graphics);
        }

        // Rotate the Kaleidoscope Item
        public void Rotate()
        {
            RotateTransform rotate = new RotateTransform();
            rotate.Angle = _angle;
            rotate.CenterX = _imageWidth / 2;
            rotate.CenterY = _imageHeight / 2;

            _graphics.RenderTransform = rotate;

            _angle += _angleInc;
        }

        // move the rectangle image
        public void Translate(double x, double y)
        {
            _graphics.SetValue(Canvas.LeftProperty, x);
            _graphics.SetValue(Canvas.TopProperty, y);
        }

        /////////////////////////////////////////////////////        
        // Private Methods
        /////////////////////////////////////////////////////	

        /////////////////////////////////////////////////////        
        // Properties
        /////////////////////////////////////////////////////	
    }
}
