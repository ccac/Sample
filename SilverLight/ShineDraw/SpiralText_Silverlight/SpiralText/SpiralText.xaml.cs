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
using System.Threading;
using System.Windows.Threading;

/*
*	A Spiral Text Demonstratoin in C#
*   from shinedraw.com
*/

namespace SpiralText
{
    public partial class SpiralText : UserControl
    {

		private static String TEXT = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		private static double SPACE_LENGTH = 1000; // dimension of the 3d space
		private static int ANGLE_INC = 5;		// angle incremenet
		private static double DEPTH_INC = 15;	// Depth increment
		private static double SPACE_WIDTH = 200;	// Text maximum x
		private static double SPACE_HEIGHT = 200;	// Text maximum y
		private static double SPACE_MOVEMENT_WIDTH = 200;	// Space Movement Effect Width
		private static double X_MUL = 10; 		// springness toward x axis
		private static double Y_MUL = 10; 		// springness toward y axis
		private static double Z_MUL = 10;		// springness toward z axis
		private static double Z_MOVEMENT = 80;	// speed toward z axis	
		private static int MAX_CHILDREN = 300;	// maximum number of text

        // on enter frame simulator
        private DispatcherTimer _timer;
        private int _fps = 24;

        private Canvas _holder = new Canvas();
        public Point3D camera = new Point3D(); // camera
        public Point3D destination = new Point3D(0, 0, -500); // move desination

		private int _spaceAngle  = 0;		// making space effect
		private int _angle = 0;				// angle
		private double _depth = -20;		// depth

        public SpiralText()
        {
            InitializeComponent();

            // add the holder to the canvas
            _holder.SetValue(Canvas.LeftProperty, Width/ 2);
            _holder.SetValue(Canvas.TopProperty, Height / 2);
            LayoutRoot.Children.Add(_holder);

            RotateTransform rotate = new RotateTransform();
            rotate.CenterX = Width / 2;
            rotate.CenterY = Height / 2;
            LayoutRoot.RenderTransform = rotate;
        }

		/////////////////////////////////////////////////////        
		// Handlers
		/////////////////////////////////////////////////////	

        void  _timer_Tick(object sender, EventArgs e)
        {
			// move the camera automatically
			destination.z += Z_MOVEMENT;
			
			camera.z += (destination.z - camera.z) / Z_MUL;
			camera.x += (destination.x - camera.x) / X_MUL;
			camera.y += (destination.y - camera.y) / Y_MUL;
			
			// rearrange the position of the texts
           postTexts();

           // add text
           addText();

           // rotate the movieclip
           RotateTransform rotate = (RotateTransform)LayoutRoot.RenderTransform;
           rotate.Angle++;
           LayoutRoot.RenderTransform = rotate;
        }

		/////////////////////////////////////////////////////        
		// Private Methods
		/////////////////////////////////////////////////////	

        private void addText(){
            int seed = (int)DateTime.Now.Ticks;

            while(_holder.Children.Count < MAX_CHILDREN){

				// calculate the x and y
				double radian = (double)_angle/ 180.0 * Math.PI;
                double offsetX = SPACE_WIDTH * Math.Cos(radian) + Math.Cos((double) _spaceAngle / 180.0 * Math.PI) * SPACE_MOVEMENT_WIDTH;
                double offsetY = SPACE_HEIGHT * Math.Sin(radian);
				
				if(_angle % 30 == 0){
						_spaceAngle ++;
				}
				

                seed += (int)DateTime.Now.Ticks;
                Random r = new Random(seed);

                String text = TEXT.Substring((int)(r.NextDouble() * TEXT.Length), 1);
				Text3D text3D = new Text3D();
				text3D.text.Text = text;
                text3D.Opacity = 0;
                text3D.x = text3D.point3D.x = offsetX;
                text3D.y = text3D.point3D.y = offsetY;
                text3D.point3D.z = _depth;
                _holder.Children.Add(text3D);

                // increase values
                _angle = (_angle + ANGLE_INC) % 360;
                _depth += DEPTH_INC;
            }
        }



        private void postTexts(){
			for( int i =  _holder.Children.Count - 1; i >= 0; i--){
                Text3D text3D = _holder.Children[i] as Text3D;
				
				double zActual = SPACE_LENGTH + (text3D.point3D.z - camera.z);
                double scale = SPACE_LENGTH / zActual;

				if(scale > 0){
					text3D.x = (text3D.point3D.x - camera.x) * scale;
					text3D.y = (text3D.point3D.y - camera.y) * scale;

                    ScaleTransform scaleTransform = new ScaleTransform();
                    scaleTransform.ScaleX = scale;
                    scaleTransform.ScaleY = scale;
                    text3D.RenderTransform = scaleTransform;

                    text3D.Opacity = 1  -0.99 * zActual / SPACE_LENGTH * 0.2;
				}else{
                    // remove if the text is too large
                    _holder.Children.Remove(text3D);
				}			
			}
        }

        /////////////////////////////////////////////////////        
        // Public Methods
        /////////////////////////////////////////////////////	

        public void Start()
        {
            // start the enter frame event
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / _fps);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }
    }
}
