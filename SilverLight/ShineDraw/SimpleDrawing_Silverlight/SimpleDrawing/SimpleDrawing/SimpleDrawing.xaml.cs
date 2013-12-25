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
*	A Simple Drawing Demonstratoin in C#
*   from shinedraw.com
*/

namespace SimpleDrawing
{
    public partial class SimpleDrawing : UserControl
    {


        private static double BEGIN_WIDTH = 18;			// Start up draw width
		private static double END_WIDTH  = 1;			// Minimum draw width
		private static Color LINE_COLOR  = Colors.Black;	// Line color
		private static double WIDTH_DEC = 0.4;			// Width decrement

		private double _currentWidth ;					// Store the current width
		private Boolean _mouseDown  = false;			// flag indicate if the mouse is down
		private Line _line;							// Shape object
		// Store the drawing data, with default drawing
		private String _drawData = "#125,54,121,54,113,55,101,58,87,62,72,69,58,75,45,82,35,88,30,95,28,99,28,101,31,105,38,108,49,112,59,116,75,121,89,126,97,130,99,133,99,135,99,138,96,143,92,152,85,160,76,168,66,174,55,181,43,188,34,193,30,196,32,196,34,193,35,191,38,187#162,68,162,75,159,84,157,96,152,112,147,134,139,158,134,175,130,185,130,189,130,190,132,188,136,184,141,176,148,166,156,158,166,149,179,139,189,132,196,129,198,129,199,134,201,141,203,150,204,160,205,169,205,177,205,184,205,189,204,194,204,196,204,197,204,194#246,126,247,126,249,125,252,124,255,122,258,120,258,118,259,116,260,115,260,113,260,112,261,111#249,152,249,158,249,166,249,175,248,183,246,190,246,192,246,191,247,189,247,187,250,185#286,138,287,144,287,154,287,166,287,178,287,188,284,196,284,201,284,198,284,192,287,181,291,167,297,155,305,148,315,143,322,140,329,140,336,140,340,140,345,141,347,146,350,153,351,161,351,168,351,173,351,178,351,183,350,190,349,195,348,199,348,200,348,197,348,194#389,164,396,166,404,167,412,168,423,168,432,166,439,163,443,159,445,153,445,147,444,142,442,137,440,136,437,136,434,136,429,137,423,141,413,148,404,156,397,163,393,169,389,175,388,179,388,184,388,188,392,190,394,192,395,192,397,192,401,193,406,194,413,194,425,194,439,194,451,194,461,194,465,192,466,191,465,189#53,260,62,256,74,251,92,246,114,245,136,245,155,245,167,247,174,252,175,260,175,269,169,280,159,289,146,298,123,307,96,315,74,321,60,325,52,329,49,330,49,329#92,270,93,274,94,283,94,293,95,302,96,308,97,310,98,308,99,305#194,271,194,274,194,284,194,293,194,302,193,311,193,316,193,319,193,320,193,317,193,312,197,303,203,293,212,285,221,279,229,277,234,276,240,276,244,276,248,276,253,277,255,278,256,278#278,305,279,305,279,303,282,301,286,296,293,291,297,285,298,279,298,276,298,275,296,275,295,275,291,275,285,276,277,282,268,288,263,296,261,301,261,307,261,311,261,314,261,315,261,316,263,317,266,317,270,317,273,317,275,317,277,316,280,314,287,309,292,305,295,303,296,302,297,302,297,301,297,302,299,304,300,305,303,306,307,308,312,310,322,312,331,314,337,314,339,314#354,279,353,286,353,292,352,298,352,303,352,308,352,311,353,313,354,314,357,315,359,315,362,315,365,314,369,309,375,303,381,295,389,289,393,284,393,283,392,285,390,289,389,293,388,299,388,303,388,307,388,311,388,314,390,317,392,317,393,317,395,317,398,317,403,314,406,310,413,304,424,293,435,281,443,271,444,266,444,263,443,263,441,263,439,263";					
		
		// redraw properties
		private int _lineCounter = 0;					// The current # of lines
        private Point _lastPoint;
		private int _pointCounter = 0;					// The current # of points
		private String [] _allLines ;						// Store all the Lines
        private String[] _points;						// Store all the points
		private Boolean _replay  = false;				// Replay flag

        private static int FPS = 24;                // fps of the on enter frame event
        private DispatcherTimer _timer = new DispatcherTimer(); // on enter frame simulator

        public SimpleDrawing()
        {
            InitializeComponent();

			// add the mouse event
            this.MouseLeftButtonDown +=new MouseButtonEventHandler(on_mouse_down);
            this.MouseLeftButtonUp +=new MouseButtonEventHandler(on_mouse_up);
            this.MouseMove +=new MouseEventHandler(on_mouse_move);
            KeyUp += new KeyEventHandler(on_key_down);
            this.KeyDown += new KeyEventHandler(SimpleDrawing_KeyDown);

            // start the enter frame event
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();

            // redraw the default string
            redraw();
		}

        void SimpleDrawing_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
		
		/////////////////////////////////////////////////////        
		// Handlers 
		/////////////////////////////////////////////////////	
		
		// replay function
		 void _timer_Tick(object sender, EventArgs e)
        {

            if(_replay){
                if (_lineCounter < _allLines.Length)
                {
					
                    // create a new Line 
                    if(_pointCounter == 0){
                        string pointData = _allLines[_lineCounter];
                        _points = pointData.Split(new Char[] { ',' });	
                        _currentWidth = BEGIN_WIDTH;
                    }
					
                    if(_pointCounter + 1< _points.Length){
                        if(_pointCounter < 2){
                            // move to the point first
                            _lastPoint  = new Point(Convert.ToDouble(_points[0]), Convert.ToDouble(_points[1]));
                        }else{
                                // create a new Line
                                _line = new Line();
                                _line.StrokeEndLineCap = PenLineCap.Round;
                                _line.StrokeStartLineCap = PenLineCap.Round;
                                _line.Stroke = new SolidColorBrush(LINE_COLOR);
                                _line.StrokeThickness = _currentWidth;
                                _line.X1 = _lastPoint.X;
                                _line.Y1 = _lastPoint.Y;
                                _line.X2 = Convert.ToDouble(_points[_pointCounter]);
                                _line.Y2 = Convert.ToDouble(_points[_pointCounter + 1]);
                                LayoutRoot.Children.Add(_line);

                                _currentWidth = Math.Max(END_WIDTH, _currentWidth - WIDTH_DEC);
                                _lastPoint = new Point(Convert.ToDouble(_points[_pointCounter]), Convert.ToDouble(_points[_pointCounter + 1]));
                        }
                        _pointCounter += 2;
                    }else{
                        _pointCounter = 0;
                        _lineCounter ++;	
                    }
                }else{
                    // finish and reset
                    _replay = false;	
                    _line = null;
                }
				
            }
		}
		
		// detect key down, reply after 'r' is pressed
         private void on_key_down(object sender, KeyEventArgs e){
             if(e.Key == Key.R){
                 redraw();
             }
         }

		// mouse down event
		private void on_mouse_down(object sender, MouseButtonEventArgs e)
        {
            if (_replay) return;		// do nothing during drawing

            _mouseDown = true;
            _currentWidth = BEGIN_WIDTH;

            // save the last click point
            _lastPoint = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);

            // save the draw data
            _drawData += "#" + e.GetPosition(this).X + "," + e.GetPosition(this).Y;
		}
			
		private void on_mouse_up(object sender, MouseButtonEventArgs e)
        {
            if(_replay) return;	// do nothing during drawing
			
             //reset 
            _mouseDown = false;
            _lastPoint = new Point(0, 0);	
		}
		
		private void on_mouse_move(object sender, MouseEventArgs e)
        {
            if (_replay) return;	// do nothing during drawing


            PolyLineSegment polyLineSegment = new PolyLineSegment();

            PathFigure pathFigure = new PathFigure();
            pathFigure.Segments.Add(polyLineSegment);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            Path path = new Path();
            path.Stroke = new SolidColorBrush(Colors.Black);
            path.StrokeThickness = 10;
            path.Data = pathGeometry;
            
            

            if (_mouseDown)
            {
                if (_lastPoint.X != 0 && _lastPoint.Y != 0)
                {
                    _lineCounter += 10;

                    // create a new Line
                    _line = new Line();
                    _line.StrokeEndLineCap = PenLineCap.Round;
                    _line.StrokeStartLineCap = PenLineCap.Round;
                    _line.Stroke = new SolidColorBrush(LINE_COLOR);
                    _line.StrokeThickness = _currentWidth;
                    _line.X1 = _lastPoint.X;
                    _line.Y1 = _lastPoint.Y;
                    _line.X2 = e.GetPosition(this).X;
                    _line.Y2 = e.GetPosition(this).Y;
                    LayoutRoot.Children.Add(_line);

                    _currentWidth = Math.Max(END_WIDTH, _currentWidth - WIDTH_DEC);
                    _lastPoint = new Point(e.GetPosition(this).X, e.GetPosition(this).Y); 

                    // save the draw data
                    _drawData += ("," + e.GetPosition(this).X + "," + e.GetPosition(this).Y);
                }
            }
		}					
			
		/////////////////////////////////////////////////////        
		// Private Methods 
		/////////////////////////////////////////////////////	

        // clear the screen
        private void clearScreen()
        {
            while (LayoutRoot.Children.Count > 0)
            {
                LayoutRoot.Children.RemoveAt(0);
            }
        }


        /////////////////////////////////////////////////////        
        // Private Methods 
        /////////////////////////////////////////////////////	

		
		public void redraw(){

			if(_replay) return;	// do nothing during drawing
			
			// clear the screen
			clearScreen();
			
			// reset properties
			_pointCounter = 0;
			_lineCounter = 0;
            _allLines = _drawData.Split(new Char[] { '#' });
			_replay = true;
		}
		

    }
}
