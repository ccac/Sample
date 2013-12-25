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
using SimpleGameSystem;

/*
*	A Simple Game System Demonstratoin in C#
*   from shinedraw.com
*/

namespace SimpleGameSystem
{
    public partial class SimpleGameSystem : UserControl
    {
        private const int FPS = 24;

		private static double MOVE_SPEED = 9;
		
		// car direction
        private static int FRONT = 1;
        private static int FRONT_LEFT = 2;
        private static int LEFT = 3;
        private static int BACK_LEFT = 4;
        private static int BACK = 5;
        private static int BACK_RIGHT = 6;
        private static int RIGHT = 7;
        private static int FRONT_RIGHT = 8;

        private Dictionary<int, bool> _pressedKeys = new Dictionary<int,bool>();

        public SimpleGameSystem()
        {
            InitializeComponent();
            
            Loaded += new RoutedEventHandler(SimpleGameSystem_Loaded);
            App.Current.Host.Settings.MaxFrameRate = FPS;
            CompositionTarget.Rendering +=new EventHandler(CompositionTarget_Rendering);
        }


        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////

        void SimpleGameSystem_Loaded(object sender, RoutedEventArgs e)
        {
            App.Current.RootVisual.KeyDown += new KeyEventHandler(RootVisual_KeyDown);
            App.Current.RootVisual.KeyUp += new KeyEventHandler(RootVisual_KeyUp);
            App.Current.RootVisual.LostFocus += new RoutedEventHandler(RootVisual_LostFocus);
        }

        // if silverlight deactivated
        void RootVisual_LostFocus(object sender, RoutedEventArgs e)
        {
            _pressedKeys.Clear();
        }

        // release the pressed key 
        void RootVisual_KeyUp(object sender, KeyEventArgs e)
        {
            _pressedKeys[(int)e.Key] = false;
        }

        // save up the pressed key	
        void RootVisual_KeyDown(object sender, KeyEventArgs e)
        {
            _pressedKeys[(int)e.Key] = true;
        }

        // check the pressed keys and move the car correspondingly
        void  CompositionTarget_Rendering(object sender, EventArgs e)
        {
                double moveSpeedX = 0;
				double moveSpeedY = 0;
				
				if(isDown(Key.Up)){ // move up
					moveSpeedY = -MOVE_SPEED;
					
					// positioning
                    if (isDown(Key.Left))
                    {
						setPosition(BACK_LEFT);		
					}else if(isDown(Key.Right)){
						setPosition(BACK_RIGHT);
					}else{
						setPosition(BACK);	
					}
                }
                else if (isDown(Key.Down))
                { // move down
					moveSpeedY = MOVE_SPEED;
					
					// positioning
                    if (isDown(Key.Left))
                    {
						setPosition(FRONT_LEFT);
                    }
                    else if (isDown(Key.Right))
                    {
						setPosition(FRONT_RIGHT);
					}else{
						setPosition(FRONT);	
					}				
				}

                if (isDown(Key.Left))
                { // move left
					moveSpeedX = -MOVE_SPEED;
					
					// positioning
                    if (!isDown(Key.Up) && !isDown(Key.Down))
                    {
						setPosition(LEFT);		
					}
                }
                else if (isDown(Key.Right))
                { // move right
					moveSpeedX = MOVE_SPEED;
					
					//positioning
                    if (!isDown(Key.Up) && !isDown(Key.Down))
                    {
						setPosition(RIGHT);		
					}					
				} 
				
				// move the car
				move(moveSpeedX, moveSpeedY);
        }

        /////////////////////////////////////////////////////        
        // Private Methods 
        /////////////////////////////////////////////////////

		// check if specific arrow key is pressed
		private bool isDown(Key key) {
            if (_pressedKeys.ContainsKey((int)key))
            {
                return _pressedKeys[(int) key];
            }
            return false;
        }

		// set the positioning direction
		private void setPosition(int position){
            Car.SetPosition(position);
		}   
		
		// update the car position
		private void move(double moveSpeedX, double moveSpeedY){
            double x = (double) Car.GetValue(Canvas.LeftProperty);
            double y = (double) Car.GetValue(Canvas.TopProperty);

			if(moveSpeedX != 0 && x + moveSpeedX < Width && x + moveSpeedX > 0){
				x += moveSpeedX;
                Car.SetValue(Canvas.LeftProperty, x + moveSpeedX);
			}
		
			if(moveSpeedY != 	0 && y + moveSpeedY < Height && y + moveSpeedY > 0){
                Car.SetValue(Canvas.TopProperty, y + moveSpeedY);
			}
		}		     

    }
}
