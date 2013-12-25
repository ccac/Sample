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
using System.Diagnostics;

/*
*   Title: A Driving Game Demonstratoin in C#
*   Author: Terence Tsang	
*   http://www.shinedraw.com
*   Your Flash and Silverlight Repositry
*/

namespace DrivingGame
{
    public partial class DrivingGame : UserControl
    {
        private const int FPS = 24;
        private static double MOVE_SPEED = 9;   // Moving Speed
        private static double MARGIN = 80;      // Moving Margin from the top and bottom
        private static int CAR_BOUND = 30;      // Car Area to check for hit testing
        private static int TOTAL_FOOD = 20;     // Total Food to generate
        private static int MAX_SPAN = 1000;     // Food Maximum Position

        // car direction
        private static int FRONT = 1;
        private static int FRONT_LEFT = 2;
        private static int LEFT = 3;
        private static int BACK_LEFT = 4;
        private static int BACK = 5;
        private static int BACK_RIGHT = 6;
        private static int RIGHT = 7;
        private static int FRONT_RIGHT = 8;

        private long index = 0;

        private Dictionary<int, bool> _pressedKeys = new Dictionary<int, bool>();    // Pressed Keys

        public DrivingGame()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(SimpleGameSystem_Loaded);
            App.Current.Host.Settings.MaxFrameRate = FPS;
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        /////////////////////////////////////////////////////        
        // Handlers 
        /////////////////////////////////////////////////////

        void SimpleGameSystem_Loaded(object sender, RoutedEventArgs e)
        {
            App.Current.RootVisual.KeyDown += new KeyEventHandler(RootVisual_KeyDown);
            App.Current.RootVisual.KeyUp += new KeyEventHandler(RootVisual_KeyUp);
            App.Current.RootVisual.LostFocus += new RoutedEventHandler(RootVisual_LostFocus);

            // add random food to the stage
            addRandomFood();
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
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            index++;
            Debug.WriteLine(index.ToString());
            double moveSpeedX = 0;
            double moveSpeedY = 0;

            if (isDown(Key.Up))
            { // move up
                moveSpeedY = -MOVE_SPEED;

                // positioning
                if (isDown(Key.Left))
                {
                    setPosition(BACK_LEFT);
                }
                else if (isDown(Key.Right))
                {
                    setPosition(BACK_RIGHT);
                }
                else
                {
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
                }
                else
                {
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
        private bool isDown(Key key)
        {
            if (_pressedKeys.ContainsKey((int)key))
            {
                return _pressedKeys[(int)key];
            }
            return false;
        }

        // set the positioning direction
        private void setPosition(int position)
        {
            Car.SetPosition(position);
        }


        // add some random food to the stage
        private void addRandomFood()
        {
            Random r = new Random();

            for (int i = 0; i < TOTAL_FOOD; i++)
            {
                Food food = new Food();
                food.SetValue(Canvas.TopProperty, MAX_SPAN / 2 - r.NextDouble() * MAX_SPAN);
                food.SetValue(Canvas.LeftProperty, r.NextDouble() * Width);
                FoodContainer.Children.Add(food);
            }

        }

        // update the car position
        private void move(double moveSpeedX, double moveSpeedY)
        {
            double x = (double)Car.GetValue(Canvas.LeftProperty);
            double y = (double)Car.GetValue(Canvas.TopProperty);

            // Horizontal Direction
            if (moveSpeedX != 0 && x + moveSpeedX < Width && x + moveSpeedX > 0)
            {
                x += moveSpeedX;
                Car.SetValue(Canvas.LeftProperty, x + moveSpeedX);
            }

            // Vertical Direction
            if ((y < MARGIN && moveSpeedY < 0) || (y > Height - MARGIN && moveSpeedY > 0))
            {
                double offsetY = (double)Road.GetValue(Canvas.TopProperty);
                double foodContainerY = (double)FoodContainer.GetValue(Canvas.TopProperty);

                if (offsetY - moveSpeedY > 0) offsetY -= Height;
                else if (offsetY - moveSpeedY < -Height) offsetY += Height;

                // update the road background
                Road.SetValue(Canvas.TopProperty, offsetY - moveSpeedY);

                // update the food container
                FoodContainer.SetValue(Canvas.TopProperty, foodContainerY - moveSpeedY);
            }
            else
            {
                // if not moving background, update the car position
                Car.SetValue(Canvas.TopProperty, y + moveSpeedY);
            }

            // food hit testing
            hitTestFood();
        }


        private void hitTestFood()
        {
            // get the car position
            int carX = Convert.ToInt32((double)Car.GetValue(Canvas.LeftProperty));
            int carY = Convert.ToInt32((double)Car.GetValue(Canvas.TopProperty));
            Point ptCheck = new Point();

            // check all the points around the car
            for (int x = carX - CAR_BOUND; x < carX + CAR_BOUND; x += 5)
            {
                for (int y = carY - CAR_BOUND; y < carY + CAR_BOUND; y += 5)
                {
                    ptCheck.X = x;
                    ptCheck.Y = y;

                    // get all the elements on that points
                    List<UIElement> hits = System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(ptCheck, LayoutRoot) as List<UIElement>;

                    // check throught all the food
                    for (int i = 0; i < FoodContainer.Children.Count; i++)
                    {
                        Food food = FoodContainer.Children[i] as Food;
                        if (hits.Contains(food))
                        {
                            // remove the food if there is any collision
                            FoodContainer.Children.Remove(food);
                        }
                    }
                }
            }
        }
    }
}
