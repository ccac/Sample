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
*	A Recruison Tree Demonstratoin in C#
*   from shinedraw.com
*/

namespace RecursionTree
{
    public partial class RecursionTree : UserControl
    {
        private static int DEPTH = 8;                       // Maximum Depth
        private static double ANGLE_BASE = 16;              // Split Base Angle
        private static double ANGLE_RANGE = 8;              // Split Angle Variation
        private static double TREE_HEIGHT = 40;             // Tree Height
        private static double TREE_HEIGHT_RANGE = 15;       // Tree Height Variation
        private static double LEAVE_SHOW_DEPTH = 6;         // Depth to show the leaves

        private RotateTransform _rotateTransform;           // Rotation
        private int _depth = 0;                             // Current Depth
        
        private static int FPS = 24;                // fps of the on enter frame event
        private DispatcherTimer _timer = new DispatcherTimer(); // on enter frame simulator

        public RecursionTree(int depth)
        {
            InitializeComponent();

            // assign values
            _depth = depth;
            _rotateTransform = new RotateTransform();
            this.RenderTransform = _rotateTransform;

            // if depth is higher than the target depth, show the leave
            Leave2.Visibility = Leave1.Visibility = (_depth > LEAVE_SHOW_DEPTH) ? Visibility.Visible : Visibility.Collapsed;
           
        }

        /////////////////////////////////////////////////////        
        // Handlers
        /////////////////////////////////////////////////////	

        void _timer_Tick(object sender, EventArgs e)
        {
            // Angle increment
            if (Math.Abs(_rotateTransform.Angle) < Math.Abs(TargetAngle))
            {
                _rotateTransform.Angle +=  (TargetAngle > 0) ? 1 : -1;
            }
            else
            {
                // if finished moving
                _timer.Stop();
                int seed = (int)DateTime.Now.Ticks;
                Random r = new Random();

                if (_depth < DEPTH)
                {
                    // create two branches
                    RecursionTree tree1 = new RecursionTree(_depth + 1);
                    tree1.TargetAngle = ANGLE_BASE + ANGLE_RANGE - r.NextDouble() * ANGLE_RANGE * 2; ;
                    tree1.TreeHeight = TREE_HEIGHT + TREE_HEIGHT_RANGE - r.NextDouble() * TREE_HEIGHT_RANGE * 2; ;
                    tree1.SetValue(Canvas.TopProperty, -TreeHeight);
                    LayoutRoot.Children.Add(tree1);
                    tree1.Start();

                    RecursionTree tree2 = new RecursionTree(_depth + 1);
                    tree2.TargetAngle = -ANGLE_BASE + ANGLE_RANGE - r.NextDouble() * ANGLE_RANGE * 2; ;
                    tree2.TreeHeight = TREE_HEIGHT + TREE_HEIGHT_RANGE - r.NextDouble() * TREE_HEIGHT_RANGE * 2; ;
                    tree2.SetValue(Canvas.TopProperty, -TreeHeight);
                    LayoutRoot.Children.Add(tree2);
                    tree2.Start();
                }
            }
        }

        /////////////////////////////////////////////////////        
        // Private Methods
        /////////////////////////////////////////////////////	



        /////////////////////////////////////////////////////        
        // Public Methods
        /////////////////////////////////////////////////////	

        // start the animation
        public void Start()
        {
            // start the enter frame event
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }

        /////////////////////////////////////////////////////        
        // Properties
        /////////////////////////////////////////////////////	

        public double TargetAngle { get; set; }

        public double TreeHeight
        {
            get
            {
                return Stick.Height;
            }
            set
            {
                Stick.Height = value;
                Stick.SetValue(Canvas.TopProperty, -value);
            }
        }
    }
}
