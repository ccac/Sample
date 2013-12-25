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
*	A Kaleidoscope Demonstratoin in C#
*   from shinedraw.com
*/
namespace Kaleidoscope
{
    public partial class Kaleidoscope : UserControl
    {
        private static double RADIUS = 200;                 // Radius of the Kaleidoscope Item
        private static double IMAGE_WIDTH = 400;            // Image Width
        private static double IMAGE_HEIGHT = 400;           // Image Height
        private static string IMAGE = "images/image1.jpg";  // Image from the xap library
        private static int NUMBER_ITEM = 24;                // Number of Kaleidoscope Item

        private static int FPS = 24;                              // fps of the on enter frame event
        private DispatcherTimer _timer = new DispatcherTimer(); // on enter frame simulator

        private List<KaleidoscopeItem> _items = new List<KaleidoscopeItem>();   // Store all the Kaleidoscope item
        public Kaleidoscope()
        {
            InitializeComponent();

            // add the kaleidoscope item
            addItems();


        }

        // Translate the Kaleidoscope image from it's mouse position
        void Kaleidoscope_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                KaleidoscopeItem item = _items[i];
                double offsetY = -e.GetPosition(this).Y/ Height * IMAGE_HEIGHT / 2 ;
                double offsetX = (e.GetPosition(this).X - Width / 2)/ Width * IMAGE_WIDTH / 2;
                item.Translate(offsetX, offsetY);
            }
        }


        /////////////////////////////////////////////////////        
        // Handlers
        /////////////////////////////////////////////////////	

        // Rotate the Kaleidoscope items
        void _timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                KaleidoscopeItem item = _items[i];
                item.Rotate();
            }
        }

        /////////////////////////////////////////////////////        
        // Public Methods
        /////////////////////////////////////////////////////	

        public void Start()
        {
            // start the enter frame event
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / FPS);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();

            // capture mouse move event
            this.MouseMove += new MouseEventHandler(Kaleidoscope_MouseMove);
        }

        /////////////////////////////////////////////////////        
        // Private Methods
        /////////////////////////////////////////////////////	

        private void addItems()
        {
            double angle = 360.0 / (double)NUMBER_ITEM;
            for (int i = 0; i < NUMBER_ITEM; i++)
            {
                // create Kaleidoscope Item
                KaleidoscopeItem item = new KaleidoscopeItem();
                item.SetValue(Canvas.LeftProperty, (Width - IMAGE_WIDTH)/2);
                item.SetValue(Canvas.TopProperty, 0.0);
                item.Init(angle, IMAGE, IMAGE_WIDTH, IMAGE_HEIGHT, RADIUS);


                TransformGroup group = new TransformGroup();

                RotateTransform rotate = new RotateTransform();
                rotate.Angle = angle * i;
                rotate.CenterX = IMAGE_WIDTH / 2;
                rotate.CenterY = IMAGE_HEIGHT / 2;

                // flip the even item
                if (i % 2 == 0)
                {
                    ScaleTransform scale = new ScaleTransform();
                    scale.ScaleX = -1;
                    scale.ScaleY = 1;
                    scale.CenterX = 200;
                    group.Children.Add(scale);
                }

                group.Children.Add(rotate);
                item.RenderTransform = group;

                // Add to the stage
                LayoutRoot.Children.Add(item);
                _items.Add(item);
            }
         }

        /////////////////////////////////////////////////////        
        // Properties
        /////////////////////////////////////////////////////	

    }
}
